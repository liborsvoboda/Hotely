using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UbytkacAdmin.Api;
using UbytkacAdmin.Classes;
using UbytkacAdmin.GlobalOperations;
using UbytkacAdmin.GlobalStyles;

namespace UbytkacAdmin.Pages {

    public partial class MessageTypeListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static MessageTypeList selectedRecord = new MessageTypeList();

        private List<MessageTypeList> MessageTypeList = new List<MessageTypeList>();

        public MessageTypeListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            try {
                lbl_id.Content = Resources["id"].ToString();
                lbl_name.Content = Resources["fname"].ToString();
                lbl_variables.Content = Resources["variables"].ToString();
                lbl_description.Content = Resources["description"].ToString();
                lbl_answerAllowed.Content = Resources["answerAllowed"].ToString();
                lbl_isSystemOnly.Content = Resources["isSystemOnly"].ToString();

                btn_save.Content = Resources["btn_save"].ToString();
                btn_cancel.Content = Resources["btn_cancel"].ToString();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            _ = LoadDataList();
            SetRecord(false);
        }

        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                MessageTypeList = await ApiCommunication.GetApiRequest<List<MessageTypeList>>(ApiUrls.MessageTypeList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);

                MessageTypeList.ForEach(async messageType => { messageType.Translation = await DBOperations.DBTranslation(messageType.Name); });
                    DgListView.ItemsSource = MessageTypeList;
                    DgListView.Items.Refresh();


            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        private async void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(async e => {
                    string headername = e.Header.ToString().ToLower();
                    if (headername == "name") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 1; }
                    else if (headername == "translation".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); ; e.DisplayIndex = 2; }
                    else if (headername == "variables".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); ; e.DisplayIndex = 3; }
                    else if (headername == "description".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); ; e.DisplayIndex = 4; }
                    else if (headername == "AnswerAllowed".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); ; e.DisplayIndex = 5; }
                    else if (headername == "isSystemOnly".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); ; e.DisplayIndex = 6; }

                    else if (headername == "timestamp".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); ; e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; } 
                    
                    else if (headername == "id".ToLower()) e.DisplayIndex = 0;
                    else if (headername == "userid".ToLower()) e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    MessageTypeList filterColumns = e as MessageTypeList;
                    return filterColumns.Name.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(filterColumns.Variables) && filterColumns.Variables.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(filterColumns.Description) && filterColumns.Description.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void NewRecord() {
            selectedRecord = new MessageTypeList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (MessageTypeList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (MessageTypeList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await ApiCommunication.DeleteApiRequest(ApiUrls.MessageTypeList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (MessageTypeList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (MessageTypeList)DgListView.SelectedItem; } else { selectedRecord = new MessageTypeList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.Name = txt_name.Text;
                selectedRecord.Variables = txt_variables.Text;
                selectedRecord.Description = txt_description.Text;
                selectedRecord.AnswerAllowed = (bool)chb_answerAllowed.IsChecked;
                selectedRecord.IsSystemOnly = (bool)chb_isSystemOnly.IsChecked;

                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.TimeStamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await ApiCommunication.PutApiRequest(ApiUrls.MessageTypeList, httpContent, null, App.UserData.Authentification.Token);
                } else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.MessageTypeList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.RecordCount > 0) {
                    selectedRecord = new MessageTypeList();
                    await LoadDataList();
                    SetRecord(false);
                } else { await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (MessageTypeList)DgListView.SelectedItem : new MessageTypeList();
            SetRecord(false);
        }

        private void SetRecord(bool showForm, bool copy = false) {
            try { 
                txt_id.Value = (copy) ? 0 : selectedRecord.Id;
                txt_name.Text = selectedRecord.Name;
                txt_variables.Text = selectedRecord.Variables;
                txt_description.Text = selectedRecord.Description;
                chb_answerAllowed.IsChecked = selectedRecord.AnswerAllowed;
                chb_isSystemOnly.IsChecked = selectedRecord.IsSystemOnly;

            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            if (showForm) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            } else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
            }
        }
    }
}