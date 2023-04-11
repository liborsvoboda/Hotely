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
    public partial class HotelListPage : UserControl
    {

        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static HotelList selectedRecord = new HotelList();

        private List<UserList> adminUserList = new List<UserList>();
        private List<CountryList> countryList = new List<CountryList>();
        private List<CityList> cityList = new List<CityList>();
        private List<CurrencyList> currencyList = new List<CurrencyList>();

        public HotelListPage()
        {
            InitializeComponent();
            _ = MediaFunctions.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            try
            {
                lbl_id.Content = Resources["id"].ToString();
                lbl_cityId.Content = Resources["city"].ToString();
                lbl_countryId.Content = Resources["country"].ToString();
                lbl_name.Content = Resources["fname"].ToString();
                lbl_descriptionCz.Content = Resources["descriptionCz"].ToString();
                lbl_descriptionEn.Content = Resources["descriptionEn"].ToString();
                lbl_currencyId.Content = Resources["currency"].ToString();

                lbl_owner.Content = Resources["owner"].ToString();
                lbl_approveRequest.Content = Resources["approveRequest"].ToString();
                lbl_approved.Content = Resources["approved"].ToString();
                lbl_advertised.Content = Resources["advertised"].ToString();

                btn_save.Content = Resources["btn_save"].ToString();
                btn_cancel.Content = Resources["btn_cancel"].ToString();
            } catch (Exception autoEx) {SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(autoEx));}

            _ = LoadDataList();
            SetRecord(false);
        }


        public async Task<bool> LoadDataList()
        {
            List<HotelList> hotelList = new List<HotelList>();
            MainWindow.ProgressRing = Visibility.Visible;
            try
            {
                hotelList = await ApiCommunication.GetApiRequest<List<HotelList>>(ApiUrls.HotelList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                cityList = await ApiCommunication.GetApiRequest<List<CityList>>(ApiUrls.CityList, null, App.UserData.Authentification.Token);
                countryList = await ApiCommunication.GetApiRequest<List<CountryList>>(ApiUrls.CountryList, null, App.UserData.Authentification.Token);
                currencyList = await ApiCommunication.GetApiRequest<List<CurrencyList>>(ApiUrls.CurrencyList, null, App.UserData.Authentification.Token);

                cityList.ForEach(city => { city.Translation = SystemFunctions.DBTranslation(countryList.FirstOrDefault(a => a.Id == city.CountryId).SystemName); });
                countryList.ForEach(country => { country.Translation = SystemFunctions.DBTranslation(country.SystemName); });

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin")
                {
                    cb_owner.ItemsSource = adminUserList = await ApiCommunication.GetApiRequest<List<UserList>>(ApiUrls.UserList, null, App.UserData.Authentification.Token);
                    lbl_owner.Visibility = cb_owner.Visibility = Visibility.Visible;
                }

                hotelList.ForEach(hotel => {
                    hotel.Country = countryList.First(a => a.Id == hotel.CountryId).Translation;
                    hotel.City = cityList.First(a => a.Id == hotel.CityId).City;
                    hotel.Currency = currencyList.First(a => a.Id == hotel.DefaultCurrencyId).Name;
                });
                DgListView.ItemsSource = hotelList;
                DgListView.Items.Refresh();

                cb_cityId.ItemsSource = cityList;
                cb_countryId.ItemsSource = countryList;
                cb_currencyId.ItemsSource = currencyList;

            }
            catch (Exception autoEx) {SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(autoEx));}
            MainWindow.ProgressRing = Visibility.Hidden;return true;
        }

        private void DgListView_Translate(object sender, EventArgs ex)
        {
            try { 
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "Name") { e.Header = Resources["fname"].ToString(); e.DisplayIndex = 3; }
                    else if (headername == "Country") { e.Header = Resources["country"].ToString(); e.DisplayIndex = 1; }
                    else if (headername == "City") { e.Header = Resources["city"].ToString(); e.DisplayIndex = 2; }
                    else if (headername == "Currency") { e.Header = Resources["currency"].ToString(); e.DisplayIndex = 4; }
                    else if (headername == "Advertised") { e.Header = Resources["advertised"].ToString(); e.DisplayIndex = 5; }
                    else if (headername == "ApproveRequest") e.Header = Resources["approveRequest"].ToString();
                    else if (headername == "Approved") e.Header = Resources["approved"].ToString();
                    else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }

                    else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                    else if (headername == "CityId") e.Visibility = Visibility.Hidden;
                    else if (headername == "CountryId") e.Visibility = Visibility.Hidden;
                    else if (headername == "DescriptionCz") e.Visibility = Visibility.Hidden;
                    else if (headername == "DescriptionEn") e.Visibility = Visibility.Hidden;
                    else if (headername == "DefaultCurrencyId") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) {SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(autoEx));}
        }

        public void Filter(string filter)
        {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    HotelList user = e as HotelList;
                    return user.Name.ToLower().Contains(filter.ToLower())
                    || user.Country.ToLower().Contains(filter.ToLower())
                    || user.City.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.DescriptionCz) && user.DescriptionCz.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.DescriptionEn) && user.DescriptionEn.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) {SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(autoEx));}
        }


        public void NewRecord()
        {
            selectedRecord = new HotelList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }


        public void EditRecord(bool copy)
        {
            selectedRecord = (HotelList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }


        public async void DeleteRecord()
        {
            selectedRecord = (HotelList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessage(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative)
            {
                DBResultMessage dBResult = await ApiCommunication.DeleteApiRequest(ApiUrls.HotelList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.recordCount == 0) await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }


        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (HotelList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }


        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgListView.SelectedItems.Count > 0)
            { selectedRecord = (HotelList)DgListView.SelectedItem; }
            else { selectedRecord = new HotelList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }


        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.CountryId = ((cb_countryId.SelectedItem != null) ? (int?)((CountryList)cb_countryId.SelectedItem).Id : null);
                selectedRecord.CityId = (cb_cityId.SelectedItem != null) ? (int?)((CityList)cb_cityId.SelectedItem).Id : null;
                selectedRecord.Name = !string.IsNullOrWhiteSpace(txt_name.Text) ? txt_name.Text : null;
                selectedRecord.DescriptionCz = txt_descriptionCz.Text;
                selectedRecord.DescriptionEn = txt_descriptionEn.Text;
                selectedRecord.DefaultCurrencyId = (cb_currencyId.SelectedItem != null) ? (int?)((CurrencyList)cb_currencyId.SelectedItem).Id : null;
                selectedRecord.ApproveRequest = (bool)chb_approveRequest.IsChecked;
                selectedRecord.Approved = (bool)chb_approved.IsChecked;
                selectedRecord.Advertised = (bool)chb_advertised.IsChecked;
                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.Timestamp = DateTimeOffset.Now.DateTime;


                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin")
                    selectedRecord.UserId = ((UserList)cb_owner.SelectedItem).Id;

                selectedRecord.Country = selectedRecord.City = selectedRecord.Currency = null;
                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0)
                {
                    dBResult = await ApiCommunication.PutApiRequest(ApiUrls.HotelList, httpContent, null, App.UserData.Authentification.Token);
                }
                else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.HotelList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.recordCount > 0)
                {
                    selectedRecord = new HotelList();
                    await LoadDataList();
                    SetRecord(false);
                }
                else { await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage); }
            }
            catch (Exception autoEx) {SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(autoEx));}
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (HotelList)DgListView.SelectedItem : new HotelList();
            SetRecord(false);
        }


        private void SetRecord(bool showForm, bool copy = false)
        {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;
            cb_cityId.SelectedItem = cityList.FirstOrDefault(a => a.Id == selectedRecord.CityId);
            cb_countryId.SelectedItem = countryList.FirstOrDefault(a => a.Id == selectedRecord.CountryId);
            txt_name.Text = selectedRecord.Name;
            txt_descriptionCz.Text = selectedRecord.DescriptionCz;
            txt_descriptionEn.Text = selectedRecord.DescriptionEn;
            cb_currencyId.SelectedItem = currencyList.FirstOrDefault(a => a.Id == selectedRecord.DefaultCurrencyId);

            chb_approveRequest.IsChecked = selectedRecord.ApproveRequest;
            chb_approved.IsChecked = false;
            chb_advertised.IsChecked = selectedRecord.Advertised;

            //Only for Admin: Owner/UserId Selection
            if (App.UserData.Authentification.Role == "Admin")
                cb_owner.Text = adminUserList.Where(a => a.Id == selectedRecord.UserId).Select(a => a.UserName).FirstOrDefault();

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


        private void CitySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_cityId.SelectedItem != null)
                cb_countryId.Text = ((CityList)cb_cityId.SelectedItem).Translation;
        }
    }
}