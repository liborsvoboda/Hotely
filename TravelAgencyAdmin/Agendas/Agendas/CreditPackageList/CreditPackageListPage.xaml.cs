using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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

// This is Template ListView + UserForm
namespace UbytkacAdmin.Pages {

    public partial class CreditPackageListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static CreditPackageList selectedRecord = new CreditPackageList();

        private List<CurrencyList> currencyList = new List<CurrencyList>();

        public CreditPackageListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            //translate fields in detail form
            try {
                lbl_id.Content = Resources["id"].ToString();
                lbl_sequence.Content = Resources["sequence"].ToString();
                lbl_systemName.Content = Resources["systemName"].ToString();
                lbl_description.Content = Resources["description"].ToString();
                lbl_creditCount.Content = Resources["creditCount"].ToString();
                lbl_creditPrice.Content = Resources["creditPrice"].ToString();

                lbl_currency.Content = Resources["currency"].ToString();
                lbl_active.Content = Resources["active"].ToString();

                btn_save.Content = Resources["btn_save"].ToString();
                btn_cancel.Content = Resources["btn_cancel"].ToString();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            _ = LoadDataList();
            SetRecord(false);
        }

        //change datasource
        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            List<CreditPackageList> creditPackageList = new List<CreditPackageList>();

            try {
                creditPackageList = await ApiCommunication.GetApiRequest<List<CreditPackageList>>(ApiUrls.CreditPackageList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                currencyList = await ApiCommunication.GetApiRequest<List<CurrencyList>>(ApiUrls.CurrencyList, null, App.UserData.Authentification.Token);
                
                creditPackageList.ForEach(async record => {
                    record.Translation = await DBOperations.DBTranslation(record.SystemName);
                    record.CurencyName = currencyList.First(a => a.Id == record.CurrencyId).Name;
                });

                cb_currency.ItemsSource = currencyList;
                DgListView.ItemsSource = creditPackageList;
                DgListView.Items.Refresh();

            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        // set translate columns in listView
        private void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "Sequence") { e.Header = Resources["sequence"].ToString(); e.DisplayIndex = 1; }
                    else if (headername == "SystemName") { e.Header = Resources["systemName"].ToString(); e.DisplayIndex = 2; }
                    else if (headername == "Translation") { e.Header = Resources["translation"].ToString(); e.DisplayIndex = 3; }
                    else if (headername == "CreditCount") { e.Header = Resources["creditCount"].ToString(); e.DisplayIndex = 4; }
                    else if (headername == "CreditPrice") { e.Header = Resources["creditPrice"].ToString(); e.DisplayIndex = 5; }
                    else if (headername == "CurencyName") { e.Header = Resources["currency"].ToString(); e.DisplayIndex = 6; }

                    else if (headername == "Active") { e.Header = Resources["active"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 2; }
                    else if (headername == "TimeStamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }

                    else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                    else if (headername == "CurrencyId") e.Visibility = Visibility.Hidden;
                    else if (headername == "Description") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        //change filter fields
        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    CreditPackageList user = e as CreditPackageList;
                    return user.SystemName.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.Description) && user.Description.ToLower().Contains(filter.ToLower());
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void NewRecord() {
            selectedRecord = new CreditPackageList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (CreditPackageList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (CreditPackageList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await ApiCommunication.DeleteApiRequest(ApiUrls.CreditPackageList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage);
                _ = LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (CreditPackageList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) {
                selectedRecord = (CreditPackageList)DgListView.SelectedItem;
            } else { selectedRecord = new CreditPackageList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        //change dataset save method
        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);

                selectedRecord.Sequence = (int)txt_sequence.Value;
                selectedRecord.SystemName = txt_systemName.Text;
                selectedRecord.Description = txt_description.Text;
                selectedRecord.CreditCount = (int)txt_creditCount.Value;
                selectedRecord.CreditPrice = (decimal)txt_creditPrice.Value;
                selectedRecord.CurrencyId = ((CurrencyList)cb_currency.SelectedItem).Id;
                selectedRecord.Active = (bool)chb_active.IsChecked;

                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.TimeStamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await ApiCommunication.PutApiRequest(ApiUrls.CreditPackageList, httpContent, null, App.UserData.Authentification.Token);
                } else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.CreditPackageList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.RecordCount > 0) {
                    selectedRecord = new CreditPackageList();
                    await LoadDataList();
                    SetRecord(false);
                } else { await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (CreditPackageList)DgListView.SelectedItem : new CreditPackageList();
            SetRecord(false);
        }

        //change dataset prepare for working
        private void SetRecord(bool showForm, bool copy = false) {

            txt_id.Value = (copy) ? 0 : selectedRecord.Id;

            txt_sequence.Value = selectedRecord.Sequence;
            txt_systemName.Text = selectedRecord.SystemName;
            txt_description.Text = selectedRecord.Description;
            txt_creditCount.Value = selectedRecord.CreditCount;
            txt_creditPrice.Value = (double)selectedRecord.CreditPrice;
            cb_currency.Text = txt_id.Value == 0 ? null : selectedRecord.CurencyName;
            chb_active.IsChecked = selectedRecord.Active;
            


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