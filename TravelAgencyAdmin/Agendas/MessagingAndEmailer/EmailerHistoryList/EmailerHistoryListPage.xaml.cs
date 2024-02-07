using GlobalClasses;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UbytkacAdmin.Api;
using UbytkacAdmin.Classes;
using UbytkacAdmin.GlobalOperations;
using UbytkacAdmin.GlobalStyles;


namespace UbytkacAdmin.Pages {

    public partial class EmailerHistoryListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static EmailerHistoryList selectedRecord = new EmailerHistoryList();


        private List<EmailerHistoryList> EmailerHistoryList = new List<EmailerHistoryList>();
        private bool changeEditorView = false;

        public EmailerHistoryListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            try {
                lbl_id.Content = Resources["id"].ToString();
                lbl_recipients.Content = Resources["recipients"].ToString();
                lbl_subject.Content = Resources["subject"].ToString();
                lbl_status.Content = Resources["status"].ToString();

                btn_save.Content = Resources["btn_save"].ToString();
                btn_cancel.Content = Resources["btn_cancel"].ToString();

                //html_htmlMessage.HtmlContentDisableInitialChange = true;
                //html_htmlMessage.Toolbar.SetSourceMode(true);
                //html_htmlMessage.Browser.ToggleSourceEditor(html_htmlMessage.Toolbar, true);

            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            _ = LoadDataList();
            SetRecord(false);
        }

        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {

                EmailerHistoryList = await ApiCommunication.GetApiRequest<List<EmailerHistoryList>>(ApiUrls.EmailerHistoryList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);

                DgListView.ItemsSource = EmailerHistoryList;
                DgListView.Items.Refresh();
                
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }


            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        private async void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(async e => {
                    string headername = e.Header.ToString().ToLower();
                    if (headername == "Recipient".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 1; }
                    else if (headername == "Subject".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 2; }
                    else if (headername == "Status".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 3; }

                    else if (headername == "TimeStamp".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }

                    else if (headername == "Id".ToLower()) e.DisplayIndex = 0;
                    else if (headername == "Email".ToLower()) e.Visibility = Visibility.Hidden;

                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    EmailerHistoryList filterColumns = e as EmailerHistoryList;
                    return filterColumns.Subject.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(filterColumns.Recipient) && filterColumns.Recipient.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(filterColumns.Email) && filterColumns.Email.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void NewRecord() {
            selectedRecord = new EmailerHistoryList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (EmailerHistoryList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (EmailerHistoryList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await ApiCommunication.DeleteApiRequest(ApiUrls.EmailerHistoryList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (EmailerHistoryList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (EmailerHistoryList)DgListView.SelectedItem; } else { selectedRecord = new EmailerHistoryList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                if (!html_htmlMessage.Browser.IsInSourceEditMode) { html_htmlMessage.Browser.ToggleSourceEditor(html_htmlMessage.Toolbar, true); changeEditorView = true; } else { changeEditorView = false; }

                selectedRecord.TimeStamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await ApiCommunication.PutApiRequest(ApiUrls.EmailerHistoryList, httpContent, null, App.UserData.Authentification.Token);
                } else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.EmailerHistoryList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.RecordCount > 0) {
                    selectedRecord = new EmailerHistoryList();
                    await LoadDataList();
                    SetRecord(false);
                } else { await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            if (changeEditorView) { html_htmlMessage.Browser.ToggleSourceEditor(html_htmlMessage.Toolbar, false);changeEditorView = false; }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (EmailerHistoryList)DgListView.SelectedItem : new EmailerHistoryList();
            SetRecord(false);
        }

        private void SetRecord(bool showForm, bool copy = false) {
            try {
                txt_id.Value = (copy) ? 0 : selectedRecord.Id;

                txt_recipients.Text = selectedRecord.Recipient;
                txt_subject.Text = selectedRecord.Subject;
                html_htmlMessage.Browser.OpenDocument(selectedRecord.Email);
                txt_status.Text = selectedRecord.Status;

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