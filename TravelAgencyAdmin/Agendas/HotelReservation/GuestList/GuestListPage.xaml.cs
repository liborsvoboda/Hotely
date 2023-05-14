using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.Classes;
using TravelAgencyAdmin.GlobalOperations;
using TravelAgencyAdmin.GlobalStyles;

namespace TravelAgencyAdmin.Pages {

    public partial class GuestListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static GuestList selectedRecord = new GuestList();

        private List<UserList> adminUserList = new List<UserList>();

        public GuestListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            //translate fields in detail form
            lbl_id.Content = Resources["id"].ToString();
            lbl_email.Content = Resources["email"].ToString();
            lbl_fullName.Content = Resources["fullName"].ToString();
            lbl_street.Content = Resources["street"].ToString();
            lbl_city.Content = Resources["city"].ToString();
            lbl_postCode.Content = Resources["postCode"].ToString();
            lbl_phone.Content = Resources["phone"].ToString();
            lbl_country.Content = Resources["country"].ToString();

            btn_save.Content = Resources["btn_save"].ToString();
            btn_cancel.Content = Resources["btn_cancel"].ToString();

            _ = LoadDataList();
            SetRecord(false);
        }

        //change datasource
        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                DgListView.ItemsSource = await ApiCommunication.GetApiRequest<List<GuestList>>(ApiUrls.GuestList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", ""), App.UserData.Authentification.Token);
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        // set translate columns in listView
        private void DgListView_Translate(object sender, EventArgs ex) {
            ((DataGrid)sender).Columns.ToList().ForEach(e => {
                string headername = e.Header.ToString();
                if (headername == "Email") e.Header = Resources["email"].ToString();
                else if (headername == "FullName") e.Header = Resources["fullName"].ToString();
                else if (headername == "Street") e.Header = Resources["street"].ToString();
                else if (headername == "City") e.Header = Resources["city"].ToString();
                else if (headername == "ZipCode") { e.Header = Resources["postCode"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; } else if (headername == "Country") e.Header = Resources["country"].ToString();
                else if (headername == "Phone") e.Header = Resources["phone"].ToString();
                else if (headername == "Active") e.Header = Resources["active"].ToString();
                else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; } else if (headername == "Id") e.DisplayIndex = 0;
                else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                else if (headername == "Password") e.Visibility = Visibility.Hidden;
                else if (headername == "UserIdAccount") e.Visibility = Visibility.Hidden;
            });
        }

        //change filter fields
        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    GuestList report = e as GuestList;
                    return report.FullName.ToLower().Contains(filter.ToLower())
                    || report.Street.ToLower().Contains(filter.ToLower())
                    || report.City.ToLower().Contains(filter.ToLower())
                    || report.Country.ToLower().Contains(filter.ToLower())
                    || report.Phone.ToLower().Contains(filter.ToLower())
                    || report.Email.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void NewRecord() {
            selectedRecord = new GuestList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (GuestList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (GuestList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessage(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await ApiCommunication.DeleteApiRequest(ApiUrls.GuestList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.recordCount == 0) await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage);
                _ = LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (GuestList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) {
                selectedRecord = (GuestList)DgListView.SelectedItem;
            } else { selectedRecord = new GuestList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        //change dataset save method
        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.Email = txt_email.Text;
                selectedRecord.FullName = txt_fullName.Text;
                selectedRecord.Street = txt_street.Text;
                selectedRecord.City = txt_city.Text;
                selectedRecord.ZipCode = txt_postCode.Text;
                selectedRecord.Phone = txt_phone.Text;
                selectedRecord.Country = txt_country.Text;
                selectedRecord.Timestamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await ApiCommunication.PutApiRequest(ApiUrls.GuestList, httpContent, null, App.UserData.Authentification.Token);
                } else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.GuestList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.recordCount > 0) {
                    selectedRecord = new GuestList();
                    await LoadDataList();
                    SetRecord(false);
                } else { await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (GuestList)DgListView.SelectedItem : new GuestList();
            SetRecord(false);
        }

        //change dataset prepare for working
        private void SetRecord(bool showForm, bool copy = false) {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;
            txt_email.Text = selectedRecord.Email;
            txt_fullName.Text = selectedRecord.FullName;
            txt_street.Text = selectedRecord.Street;
            txt_city.Text = selectedRecord.City;
            txt_postCode.Text = selectedRecord.ZipCode;
            txt_phone.Text = selectedRecord.Phone;
            txt_country.Text = selectedRecord.Country;

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