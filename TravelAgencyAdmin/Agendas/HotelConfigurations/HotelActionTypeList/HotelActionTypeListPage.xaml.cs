using Newtonsoft.Json;
using TravelAgencyAdmin.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;
using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.GlobalStyles;
using TravelAgencyAdmin.GlobalFunctions;
using MahApps.Metro.Controls.Dialogs;
using System.Net;


namespace TravelAgencyAdmin.Pages
{
    public partial class HotelActionTypeListPage : UserControl
    {

        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static HotelActionTypeList selectedRecord = new HotelActionTypeList();


        private List<HotelActionTypeList> HotelActionTypeLists = new List<HotelActionTypeList>();
        public HotelActionTypeListPage()
        {
            InitializeComponent();
            _ = MediaFunctions.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            try
            {
                lbl_id.Content = Resources["id"].ToString();
                lbl_systemName.Content = Resources["systemName"].ToString();

                btn_save.Content = Resources["btn_save"].ToString();
                btn_cancel.Content = Resources["btn_cancel"].ToString();
            } catch (Exception autoEx) {App.ApplicationLogging(autoEx);}

            _ = LoadDataList();
            SetRecord(false);
        }


        public async Task<bool> LoadDataList()
        {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                DgListView.ItemsSource = HotelActionTypeLists = await ApiCommunication.GetApiRequest<List<HotelActionTypeList>>(ApiUrls.HotelActionTypeList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                HotelActionTypeLists.ForEach(async property => { property.Translation = await DBFunctions.DBTranslation(property.SystemName); });

                DgListView.ItemsSource = HotelActionTypeLists;
                DgListView.Items.Refresh();

            } catch (Exception autoEx) {App.ApplicationLogging(autoEx);}
            MainWindow.ProgressRing = Visibility.Hidden;return true;
        }

        private void DgListView_Translate(object sender, EventArgs ex)
        {
            try { 
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "SystemName") { e.Header = Resources["systemName"].ToString(); e.DisplayIndex = 1; }
                    else if (headername == "Translation") { e.Header = Resources["translation"].ToString(); e.DisplayIndex = 2; }
                    else if (headername == "Active") { e.Header = Resources["active"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 2; }
                    else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }

                    else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) {App.ApplicationLogging(autoEx);}
        }

        public void Filter(string filter)
        {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    HotelActionTypeList user = e as HotelActionTypeList;
                    return user.SystemName.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.Translation) && user.Translation.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) {App.ApplicationLogging(autoEx);}
        }


        public void NewRecord()
        {
            selectedRecord = new HotelActionTypeList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }


        public void EditRecord(bool copy)
        {
            selectedRecord = (HotelActionTypeList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }


        public async void DeleteRecord()
        {
            selectedRecord = (HotelActionTypeList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessage(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative)
            {
                DBResultMessage dBResult = await ApiCommunication.DeleteApiRequest(ApiUrls.HotelActionTypeList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.recordCount == 0) await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }


        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (HotelActionTypeList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }


        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgListView.SelectedItems.Count > 0)
            { selectedRecord = (HotelActionTypeList)DgListView.SelectedItem; }
            else { selectedRecord = new HotelActionTypeList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }


        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.SystemName = txt_systemName.Text;
                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.Timestamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0)
                {
                    dBResult = await ApiCommunication.PutApiRequest(ApiUrls.HotelActionTypeList, httpContent, null, App.UserData.Authentification.Token);
                }
                else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.HotelActionTypeList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.recordCount > 0)
                {
                    selectedRecord = new HotelActionTypeList();
                    await LoadDataList();
                    SetRecord(false);
                }
                else { await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage); }
            }
            catch (Exception autoEx) {App.ApplicationLogging(autoEx);}
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (HotelActionTypeList)DgListView.SelectedItem : new HotelActionTypeList();
            SetRecord(false);
        }


        private void SetRecord(bool showForm, bool copy = false)
        {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;
            txt_systemName.Text = selectedRecord.SystemName;


            if (showForm)
            {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            }
            else
            {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
            }
        }

    }
}