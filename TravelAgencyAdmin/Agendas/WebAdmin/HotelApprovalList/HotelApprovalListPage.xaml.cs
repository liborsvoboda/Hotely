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
using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.Classes;
using TravelAgencyAdmin.GlobalOperations;
using TravelAgencyAdmin.GlobalStyles;

namespace TravelAgencyAdmin.Pages {

    public partial class HotelApprovalListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static HotelApprovalList selectedRecord = new HotelApprovalList();

        private List<UserList> adminUserList = new List<UserList>();
        private List<CountryList> countryList = new List<CountryList>();
        private List<CityList> cityList = new List<CityList>();
        private List<CurrencyList> currencyList = new List<CurrencyList>();
        private HotelApprovalList hotelList = new HotelApprovalList();
        private List<HotelRoomTypeList> hotelRoomTypeList = new List<HotelRoomTypeList>();
        private HotelRoomList selectedRoom = new HotelRoomList();
        private List<PropertyOrServiceTypeList> propertyOrServiceTypeList = new List<PropertyOrServiceTypeList>();
        private List<PropertyOrServiceUnitList> propertyOrServiceUnitList = new List<PropertyOrServiceUnitList>();
        private HotelPropertyAndServiceList selectedProperty = new HotelPropertyAndServiceList();

        public HotelApprovalListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            try {
                lbl_approvingProcess1.Content = lbl_approvingProcess2.Content = Resources["approvingProcess"].ToString();
                lbl_cityId.Content = Resources["city"].ToString();
                lbl_countryId.Content = Resources["country"].ToString();
                lbl_name.Content = Resources["fname"].ToString();
                lbl_descriptionCz.Content = Resources["descriptionCz"].ToString();
                lbl_descriptionEn.Content = Resources["descriptionEn"].ToString();
                lbl_currencyId.Content = Resources["currency"].ToString();

                lbl_owner.Content = lbl_owner2.Content = lbl_owner3.Content = Resources["owner"].ToString();
                lbl_approveRequest.Content = lbl_approveRequest2.Content = lbl_approveRequest3.Content = Resources["approveRequest"].ToString();
                lbl_approved.Content = lbl_approved2.Content = lbl_approved3.Content = Resources["approved"].ToString();
                lbl_advertised.Content = Resources["advertised"].ToString();

                btn_save1.Content = btn_save2.Content = btn_save3.Content = Resources["btn_save"].ToString();
                btn_next1.Content = btn_next2.Content = Resources["next"].ToString();
                btn_cancel1.Content = btn_cancel2.Content = btn_cancel3.Content = Resources["close"].ToString();

                //RoomList
                lbl_rooms.Content = Resources["roomList"].ToString();
                lbl_roomTypeId.Content = Resources["roomType"].ToString();
                lbl_name2.Content = Resources["fname"].ToString();
                lbl_descriptionCz2.Content = Resources["descriptionCz"].ToString();
                lbl_descriptionEn2.Content = Resources["descriptionEn"].ToString();
                lbl_price.Content = Resources["price"].ToString();
                lbl_maxCapacity.Content = Resources["maxCapacity"].ToString();
                lbl_extraBed.Content = Resources["extraBed"].ToString();
                lbl_roomsCount.Content = Resources["roomsCount"].ToString();

                //PropertyList
                lbl_propertyOrServiceId.Content = Resources["propertyOrService"].ToString();
                lbl_isAvailable.Content = Resources["isAvailable"].ToString();
                lbl_value.Content = Resources["value"].ToString();
                lbl_valueRangeMin.Content = Resources["valueRangeMin"].ToString();
                lbl_valueRangeMax.Content = Resources["valueRangeMax"].ToString();
                lbl_fee.Content = Resources["fee"].ToString();
                lbl_feeValue.Content = Resources["feeValue"].ToString();
                lbl_feeValueRangeMin.Content = Resources["feeValueRangeMin"].ToString();
                lbl_feeValueRangeMax.Content = Resources["feeValueRangeMax"].ToString();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            _ = LoadDataList();
            SetRecord(false);
        }

        public async Task<bool> LoadDataList() {
            List<HotelApprovalList> HotelApprovalList = new List<HotelApprovalList>();
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                HotelApprovalList = await ApiCommunication.GetApiRequest<List<HotelApprovalList>>(ApiUrls.HotelApprovalList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                countryList = await ApiCommunication.GetApiRequest<List<CountryList>>(ApiUrls.CountryList, null, App.UserData.Authentification.Token);
                currencyList = await ApiCommunication.GetApiRequest<List<CurrencyList>>(ApiUrls.CurrencyList, null, App.UserData.Authentification.Token);

                countryList.ForEach(async country => { country.CountryTranslation = await DBOperations.DBTranslation(country.SystemName); });

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin") {
                    cb_owner.ItemsSource = adminUserList = await ApiCommunication.GetApiRequest<List<UserList>>(ApiUrls.UserList, null, App.UserData.Authentification.Token);
                    lbl_owner.Visibility = cb_owner.Visibility = Visibility.Visible;
                }

                HotelApprovalList.ForEach( async hotel => {
                    hotel.CountryTranslation = countryList.First(a => a.Id == hotel.CountryId).CountryTranslation;
                    hotel.CityTranslation = await DBOperations.DBTranslation(hotel.City);
                    hotel.Currency = currencyList.First(a => a.Id == hotel.DefaultCurrencyId).Name;
                });
                DgListView.ItemsSource = HotelApprovalList;
                DgListView.Items.Refresh();

                cb_cityId.ItemsSource = cityList;
                cb_countryId.ItemsSource = countryList;
                cb_currencyId.ItemsSource = currencyList;
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        private void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "Name") { e.Header = Resources["fname"].ToString(); e.DisplayIndex = 4; }
                    else if (headername == "Approved") { e.Header = Resources["approved"].ToString(); e.DisplayIndex = 1; }
                    else if (headername == "Advertised") { e.Header = Resources["advertised"].ToString(); e.DisplayIndex = 2; }
                    else if (headername == "CountryTranslation") { e.Header = Resources["country"].ToString(); e.DisplayIndex = 3; } 
                    else if (headername == "CityTranslation") { e.Header = Resources["city"].ToString(); e.DisplayIndex = 5; } 
                    else if (headername == "Currency") { e.Header = Resources["currency"].ToString(); e.DisplayIndex = 6; } 
                    else if (headername == "TotalCapacity") { e.Header = Resources["totalCapacity"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = 7; } 
                    else if (headername == "ApproveRequest") { e.Header = Resources["approveRequest"].ToString(); e.DisplayIndex = 7; } 
                    else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; } 
                    
                    else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                    else if (headername == "CityId") e.Visibility = Visibility.Hidden;
                    else if (headername == "City") e.Visibility = Visibility.Hidden;
                    else if (headername == "CountryId") e.Visibility = Visibility.Hidden;
                    else if (headername == "DescriptionCz") e.Visibility = Visibility.Hidden;
                    else if (headername == "DescriptionEn") e.Visibility = Visibility.Hidden;
                    else if (headername == "DefaultCurrencyId") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    HotelApprovalList user = e as HotelApprovalList;
                    return user.Name.ToLower().Contains(filter.ToLower())
                    || user.CountryTranslation.ToLower().Contains(filter.ToLower())
                    || user.CityTranslation.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.DescriptionCz) && user.DescriptionCz.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.DescriptionEn) && user.DescriptionEn.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void NewRecord() {
            SetRecord(false);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (HotelApprovalList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, false);
        }

        public async void DeleteRecord() {
            SetRecord(false);
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (HotelApprovalList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (HotelApprovalList)DgListView.SelectedItem; } else { selectedRecord = new HotelApprovalList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        private async void BtnSaveHotel_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
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

                selectedRecord.CountryTranslation = selectedRecord.Currency = null; selectedRecord.City = null;
                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                dBResult = await ApiCommunication.PostApiRequest(ApiUrls.HotelList, httpContent, null, App.UserData.Authentification.Token);
                if (dBResult.RecordCount > 0) { await MainWindow.ShowMessage(false, Resources["changesSaved"].ToString()); } else { await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private async void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (HotelApprovalList)DgListView.SelectedItem : new HotelApprovalList();
            await LoadDataList();
            SetRecord(false);
        }

        private void SetRecord(bool showForm, bool copy = false) {
            cb_cityId.SelectedItem = cityList.FirstOrDefault(a => a.Id == selectedRecord.CityId);
            cb_countryId.SelectedItem = countryList.FirstOrDefault(a => a.Id == selectedRecord.CountryId);
            txt_name.Text = selectedRecord.Name;
            txt_descriptionCz.Text = selectedRecord.DescriptionCz;
            txt_descriptionEn.Text = selectedRecord.DescriptionEn;
            cb_currencyId.SelectedItem = currencyList.FirstOrDefault(a => a.Id == selectedRecord.DefaultCurrencyId);

            chb_approveRequest.IsChecked = false;
            btn_save1.IsEnabled = selectedRecord.ApproveRequest;
            chb_approved.IsChecked = selectedRecord.Approved;
            chb_advertised.IsChecked = selectedRecord.Advertised;

            hotelList = selectedRecord;

            //Only for Admin: Owner/UserId Selection
            if (App.UserData.Authentification.Role == "Admin")
                cb_owner.Text = selectedRecord.Id == 0 ? App.UserData.UserName : adminUserList.Where(a => a.Id == selectedRecord.UserId).Select(a => a.UserName).FirstOrDefault();

            if (showForm) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = ListForm2.Visibility = ListForm3.Visibility = Visibility.Hidden; ListForm1.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            } else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm1.Visibility = ListForm2.Visibility = ListForm3.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
            }
        }

        private async void CountrySelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cb_countryId.SelectedItem != null) {
                MainWindow.ProgressRing = Visibility.Visible;
                cb_cityId.SelectedItem = null;
                cityList = await ApiCommunication.GetApiRequest<List<CityList>>(ApiUrls.CityList, "ByCountry/" + ((CountryList)cb_countryId.SelectedItem).Id.ToString(), App.UserData.Authentification.Token); ;

                cityList.ForEach(async city => {
                    city.CityTranslation = await DBOperations.DBTranslation(city.City, true);
                    city.CountryTranslation = await DBOperations.DBTranslation(countryList.FirstOrDefault(a => a.Id == city.CountryId).SystemName);
                });

                cb_cityId.ItemsSource = cityList;

                if (((CountryList)cb_countryId.SelectedItem).Id == selectedRecord.CountryId) { cb_cityId.SelectedItem = cityList.FirstOrDefault(a => a.Id == selectedRecord.CityId); }
                MainWindow.ProgressRing = Visibility.Hidden;
            }
        }

        private void GoToForm1_Click(object sender, MouseButtonEventArgs e) {
            ListForm1.Visibility = Visibility.Visible;
            ListForm2.Visibility = Visibility.Hidden;
            ListForm3.Visibility = Visibility.Hidden;
        }

        private void GoToForm2_Click(object sender, MouseButtonEventArgs e) {
            ListForm1.Visibility = Visibility.Hidden;
            ListForm2.Visibility = Visibility.Visible;
            ListForm3.Visibility = Visibility.Hidden;
            CleanRoomForm();
            LoadRoomDataList();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e) {
            if (ListForm1.Visibility == Visibility.Visible) {
                ListForm1.Visibility = ListForm3.Visibility = Visibility.Hidden;
                ListForm2.Visibility = Visibility.Visible;
                CleanRoomForm();
                LoadRoomDataList();
            } else if (ListForm2.Visibility == Visibility.Visible) {
                ListForm1.Visibility = ListForm2.Visibility = Visibility.Hidden;
                ListForm3.Visibility = Visibility.Visible;
                CleanPropertyForm();
                LoadPropertyDataList();
            }
        }

        //START RoomApproving
        public async void LoadRoomDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            List<HotelRoomList> hotelRoomList = new List<HotelRoomList>();
            try {
                hotelRoomList = await ApiCommunication.GetApiRequest<List<HotelRoomList>>(ApiUrls.HotelApprovalList, "Rooms/" + hotelList.Id, App.UserData.Authentification.Token);
                hotelRoomTypeList = await ApiCommunication.GetApiRequest<List<HotelRoomTypeList>>(ApiUrls.HotelRoomTypeList, null, App.UserData.Authentification.Token);
                currencyList = await ApiCommunication.GetApiRequest<List<CurrencyList>>(ApiUrls.CurrencyList, null, App.UserData.Authentification.Token);

                hotelRoomTypeList.ForEach(async roomType => { roomType.Translation = await DBOperations.DBTranslation(roomType.SystemName); });

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin") {
                    cb_owner2.ItemsSource = adminUserList = await ApiCommunication.GetApiRequest<List<UserList>>(ApiUrls.UserList, null, App.UserData.Authentification.Token);
                    lbl_owner2.Visibility = cb_owner2.Visibility = Visibility.Visible;
                }

                hotelRoomList.ForEach(async room => {
                    room.Accommodation = hotelList.Name;
                    room.RoomType = await DBOperations.DBTranslation(hotelRoomTypeList.First(a => a.Id == room.RoomTypeId).SystemName);
                });
                DgRoomListView.ItemsSource = hotelRoomList;
                DgRoomListView.Items.Refresh();

                cb_roomTypeId.ItemsSource = hotelRoomTypeList;
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden;
        }

        private void DgRoomListView_Translate(object sender, EventArgs ex) {
            ((DataGrid)sender).Columns.ToList().ForEach(e => {
                string headername = e.Header.ToString();
                if (headername == "Name") { e.Header = Resources["fname"].ToString(); e.DisplayIndex = 5; }
                else if (headername == "ApproveRequest") { e.Header = Resources["approveRequest"].ToString(); e.DisplayIndex = 1; }
                else if (headername == "Approved") { e.Header = Resources["approved"].ToString(); e.DisplayIndex = 2; }
                else if (headername == "Accommodation") { e.Header = Resources["accommodation"].ToString(); e.DisplayIndex = 3; } 
                else if (headername == "RoomType") { e.Header = Resources["roomType"].ToString(); e.DisplayIndex = 4; } 
                else if (headername == "Price") { e.Header = Resources["price"].ToString(); e.DisplayIndex = 6; } 
                else if (headername == "MaxCapacity") e.Header = Resources["maxCapacity"].ToString();
                else if (headername == "ExtraBed") e.Header = Resources["extraBed"].ToString();
                else if (headername == "RoomsCount") e.Header = Resources["roomsCount"].ToString();
                else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; } else if (headername == "Id") e.DisplayIndex = 0;
                else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                else if (headername == "HotelId") e.Visibility = Visibility.Hidden;
                else if (headername == "RoomTypeId") e.Visibility = Visibility.Hidden;
                else if (headername == "DescriptionCz") e.Visibility = Visibility.Hidden;
                else if (headername == "DescriptionEn") e.Visibility = Visibility.Hidden;
            });
        }

        private void DgRoomListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgRoomListView.SelectedItems.Count > 0) {
                selectedRoom = (HotelRoomList)DgRoomListView.SelectedItem;
                SetRoomRecord();
            } else { selectedRoom = new HotelRoomList(); CleanRoomForm(); }
        }

        private void CleanRoomForm() {
            cb_roomTypeId.Text = null;
            txt_name2.Text = null;
            txt_descriptionCz2.Text = txt_descriptionEn2.Text = null;
            txt_price.Value = txt_maxCapacity.Value = txt_roomsCount.Value = null;
            chb_extraBed.IsChecked = false;
            chb_approveRequest2.IsChecked = false;
            chb_approved2.IsChecked = false;

            cb_roomTypeId.IsEnabled = txt_name2.IsEnabled = txt_descriptionCz2.IsEnabled = txt_descriptionEn2.IsEnabled =
                txt_price.IsEnabled = txt_maxCapacity.IsEnabled = txt_roomsCount.IsEnabled = chb_extraBed.IsEnabled =
                chb_approveRequest2.IsEnabled = chb_approved2.IsEnabled = cb_owner2.IsEnabled = btn_save2.IsEnabled = false;
        }

        private void SetRoomRecord() {
            cb_roomTypeId.SelectedItem = hotelRoomTypeList.FirstOrDefault(a => a.Id == selectedRoom.RoomTypeId);
            txt_name2.Text = selectedRoom.Name;
            txt_descriptionCz2.Text = selectedRoom.DescriptionCz;
            txt_descriptionEn2.Text = selectedRoom.DescriptionEn;
            txt_price.Value = selectedRoom.Price;
            txt_maxCapacity.Value = selectedRoom.MaxCapacity;
            chb_extraBed.IsChecked = selectedRoom.ExtraBed;
            txt_roomsCount.Value = selectedRoom.RoomsCount;

            chb_approveRequest2.IsChecked = false;
            btn_save2.IsEnabled = selectedRoom.ApproveRequest;
            chb_approved2.IsChecked = selectedRoom.Approved;

            //Only for Admin: Owner/UserId Selection
            if (App.UserData.Authentification.Role == "Admin")
                cb_owner2.Text = adminUserList.Where(a => a.Id == selectedRoom.UserId).Select(a => a.UserName).FirstOrDefault();

            cb_roomTypeId.IsEnabled = txt_name2.IsEnabled = txt_descriptionCz2.IsEnabled = txt_descriptionEn2.IsEnabled =
                txt_price.IsEnabled = txt_maxCapacity.IsEnabled = txt_roomsCount.IsEnabled = chb_extraBed.IsEnabled =
                chb_approveRequest2.IsEnabled = chb_approved2.IsEnabled = cb_owner2.IsEnabled = true;
        }

        private async void BtnSaveRoom_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRoom.RoomTypeId = (cb_roomTypeId.SelectedItem != null) ? (int?)((HotelRoomTypeList)cb_roomTypeId.SelectedItem).Id : null;
                selectedRoom.Name = !string.IsNullOrWhiteSpace(txt_name2.Text) ? txt_name2.Text : null;
                selectedRoom.DescriptionCz = txt_descriptionCz2.Text;
                selectedRoom.DescriptionEn = txt_descriptionEn2.Text;
                selectedRoom.Price = (double)txt_price.Value;
                selectedRoom.MaxCapacity = (int)txt_maxCapacity.Value;
                selectedRoom.ExtraBed = (bool)chb_extraBed.IsChecked;
                selectedRoom.RoomsCount = (int)txt_roomsCount.Value;

                selectedRoom.ApproveRequest = false;
                selectedRoom.Approved = (bool)chb_approved2.IsChecked;

                selectedRoom.UserId = App.UserData.Authentification.Id;
                selectedRoom.Timestamp = DateTimeOffset.Now.DateTime;

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin")
                    selectedRoom.UserId = ((UserList)cb_owner2.SelectedItem).Id;

                selectedRoom.Accommodation = selectedRoom.RoomType = null;
                string json = JsonConvert.SerializeObject(selectedRoom);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                dBResult = await ApiCommunication.PostApiRequest(ApiUrls.HotelRoomList, httpContent, null, App.UserData.Authentification.Token);

                if (dBResult.RecordCount > 0) { LoadRoomDataList(); } else { await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private async void LoadPropertyDataList() {
            List<HotelPropertyAndServiceList> hotelPropertyAndServiceList = new List<HotelPropertyAndServiceList>();
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                hotelPropertyAndServiceList = await ApiCommunication.GetApiRequest<List<HotelPropertyAndServiceList>>(ApiUrls.HotelApprovalList, "Properties/" + hotelList.Id, App.UserData.Authentification.Token);
                currencyList = await ApiCommunication.GetApiRequest<List<CurrencyList>>(ApiUrls.CurrencyList, null, App.UserData.Authentification.Token);
                propertyOrServiceTypeList = await ApiCommunication.GetApiRequest<List<PropertyOrServiceTypeList>>(ApiUrls.PropertyOrServiceTypeList, null, App.UserData.Authentification.Token);
                propertyOrServiceUnitList = await ApiCommunication.GetApiRequest<List<PropertyOrServiceUnitList>>(ApiUrls.PropertyOrServiceUnitList, null, App.UserData.Authentification.Token);

                propertyOrServiceTypeList.ForEach(async property => { property.Translation = await DBOperations.DBTranslation(property.SystemName); });
                propertyOrServiceUnitList.ForEach(async propertyUnit => { propertyUnit.Translation = await DBOperations.DBTranslation(propertyUnit.SystemName); });

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin") {
                    cb_owner3.ItemsSource = adminUserList = await ApiCommunication.GetApiRequest<List<UserList>>(ApiUrls.UserList, null, App.UserData.Authentification.Token);
                    lbl_owner3.Visibility = cb_owner3.Visibility = Visibility.Visible;
                }

                hotelPropertyAndServiceList.ForEach(async room => {
                    room.Accommodation = hotelList.Name;
                    room.PropertyOrService = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == room.PropertyOrServiceId).Translation;
                    room.IsSearchRequired = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == room.PropertyOrServiceId).IsSearchRequired;
                    room.IsService = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == room.PropertyOrServiceId).IsService;
                    room.Fee = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == room.PropertyOrServiceId).IsFeeInfoRequired;
                    room.PropertyUnit = await DBOperations.DBTranslation(propertyOrServiceUnitList.FirstOrDefault(a => a.Id == propertyOrServiceTypeList.FirstOrDefault(b => b.Id == room.PropertyOrServiceId).PropertyOrServiceUnitTypeId).Translation);
                });
                DgPropertyListView.ItemsSource = hotelPropertyAndServiceList;
                DgPropertyListView.Items.Refresh();
                cb_propertyOrServiceId.ItemsSource = propertyOrServiceTypeList;
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden;
        }

        private void DgPropertyListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "IsSearchRequired") { e.Header = Resources["searchRequired"].ToString(); e.DisplayIndex = 5; }
                    else if (headername == "ApproveRequest") { e.Header = Resources["approveRequest"].ToString(); e.DisplayIndex = 1; }
                    else if (headername == "Approved") { e.Header = Resources["approved"].ToString(); e.DisplayIndex = 2; }
                    else if (headername == "Accommodation") { e.Header = Resources["accommodation"].ToString(); e.DisplayIndex = 3; } 
                    else if (headername == "PropertyOrService") { e.Header = Resources["propertyOrService"].ToString(); e.DisplayIndex = 4; } 
                    else if (headername == "IsService") { e.Header = Resources["service"].ToString(); e.DisplayIndex = 6; } 
                    else if (headername == "IsAvailable") e.Header = Resources["isAvailable"].ToString();
                    else if (headername == "Fee") e.Header = Resources["fee"].ToString();
                    else if (headername == "PropertyUnit") e.Header = Resources["unit"].ToString();
                    else if (headername == "RoomsCount") e.Header = Resources["roomsCount"].ToString();
                    else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; } else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                    else if (headername == "HotelId") e.Visibility = Visibility.Hidden;
                    else if (headername == "PropertyOrServiceId") e.Visibility = Visibility.Hidden;
                    else if (headername == "ValueBit") e.Visibility = Visibility.Hidden;
                    else if (headername == "Value") e.Visibility = Visibility.Hidden;
                    else if (headername == "ValueRangeMin") e.Visibility = Visibility.Hidden;
                    else if (headername == "ValueRangeMax") e.Visibility = Visibility.Hidden;
                    else if (headername == "FeeValue") e.Visibility = Visibility.Hidden;
                    else if (headername == "FeeRangeMin") e.Visibility = Visibility.Hidden;
                    else if (headername == "FeeRangeMax") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void DgPropertyListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgPropertyListView.SelectedItems.Count > 0) {
                selectedProperty = (HotelPropertyAndServiceList)DgPropertyListView.SelectedItem;
                SetPropertyRecord();
            } else { selectedProperty = new HotelPropertyAndServiceList(); CleanPropertyForm(); }
        }

        private void CleanPropertyForm() {
            cb_propertyOrServiceId.Text = null;
            chb_isAvailable.IsChecked = chb_fee.IsChecked = chb_approveRequest3.IsChecked = chb_approved3.IsChecked = false;
            txt_value.Value = txt_valueRangeMin.Value = txt_valueRangeMax.Value = txt_feeValue.Value = txt_feeValueRangeMin.Value = txt_feeValueRangeMax.Value = null;

            cb_propertyOrServiceId.IsEnabled = chb_isAvailable.IsEnabled = txt_value.IsEnabled =
                txt_valueRangeMin.IsEnabled = txt_valueRangeMax.IsEnabled = chb_fee.IsEnabled = txt_feeValue.IsEnabled =
                txt_feeValueRangeMin.IsEnabled = txt_feeValueRangeMax.IsEnabled = chb_approveRequest3.IsEnabled = chb_approved3.IsEnabled = cb_owner3.IsEnabled = btn_save3.IsEnabled = false;
        }

        private void SetPropertyRecord() {
            cb_propertyOrServiceId.Text = selectedProperty.PropertyOrService;
            chb_isAvailable.IsChecked = selectedProperty.IsAvailable;

            chb_isAvailable.IsEnabled = txt_value.IsEnabled =
                txt_valueRangeMin.IsEnabled = txt_valueRangeMax.IsEnabled = chb_fee.IsEnabled = txt_feeValue.IsEnabled =
                txt_feeValueRangeMin.IsEnabled = txt_feeValueRangeMax.IsEnabled = chb_approved3.IsEnabled = cb_owner3.IsEnabled = btn_save3.IsEnabled = true;

            lbl_propertyUnit.Content = selectedProperty.PropertyUnit;

            txt_value.IsEnabled = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == selectedProperty.PropertyOrServiceId).IsValue && !propertyOrServiceTypeList.FirstOrDefault(a => a.Id == selectedProperty.PropertyOrServiceId).IsRangeValue;
            txt_value.Value = selectedProperty.Value;

            txt_valueRangeMin.IsEnabled = txt_valueRangeMax.IsEnabled = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == selectedProperty.PropertyOrServiceId).IsRangeValue || propertyOrServiceTypeList.FirstOrDefault(a => a.Id == selectedProperty.PropertyOrServiceId).IsValueRangeAllowed;
            txt_valueRangeMin.Value = selectedProperty.ValueRangeMin;
            txt_valueRangeMax.Value = selectedProperty.ValueRangeMax;

            if (propertyOrServiceTypeList.FirstOrDefault(a => a.Id == selectedProperty.PropertyOrServiceId).IsFeeInfoRequired) { chb_fee.IsChecked = true; chb_fee.IsEnabled = false; } else { chb_fee.IsChecked = selectedProperty.Fee; }

            txt_feeValue.Value = selectedProperty.FeeValue;
            txt_feeValueRangeMin.IsEnabled = txt_feeValueRangeMax.IsEnabled = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == selectedProperty.PropertyOrServiceId).IsFeeRangeAllowed;

            txt_feeValueRangeMin.Value = selectedProperty.FeeRangeMin;
            txt_feeValueRangeMax.Value = selectedProperty.FeeRangeMax;

            chb_approveRequest.IsChecked = false;
            btn_save3.IsEnabled = selectedProperty.ApproveRequest;
            chb_approved.IsChecked = selectedProperty.Approved;

            //Only for Admin: Owner/UserId Selection
            if (App.UserData.Authentification.Role == "Admin")
                cb_owner3.Text = adminUserList.Where(a => a.Id == selectedProperty.UserId).Select(a => a.UserName).FirstOrDefault();
        }

        private async void BtnSaveProperty_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedProperty.IsAvailable = (bool)chb_isAvailable.IsChecked;
                selectedProperty.Value = txt_value.Value;
                selectedProperty.ValueRangeMin = txt_valueRangeMin.Value;
                selectedProperty.ValueRangeMax = txt_valueRangeMax.Value;
                selectedProperty.Fee = (bool)chb_fee.IsChecked;
                selectedProperty.FeeValue = txt_feeValue.Value;
                selectedProperty.FeeRangeMin = txt_feeValueRangeMin.Value;
                selectedProperty.FeeRangeMax = txt_feeValueRangeMax.Value;

                selectedProperty.ApproveRequest = false;
                selectedProperty.Approved = (bool)chb_approved3.IsChecked;

                selectedProperty.UserId = App.UserData.Authentification.Id;
                selectedProperty.Timestamp = DateTimeOffset.Now.DateTime;

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin")
                    selectedProperty.UserId = ((UserList)cb_owner3.SelectedItem).Id;

                //nullable additional fields
                selectedProperty.Accommodation = selectedProperty.PropertyOrService = null;
                string json = JsonConvert.SerializeObject(selectedProperty);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                dBResult = await ApiCommunication.PostApiRequest(ApiUrls.HotelPropertyAndServiceList, httpContent, null, App.UserData.Authentification.Token);

                if (dBResult.RecordCount > 0) { LoadPropertyDataList(); } else { await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void FeeStatusClick(object sender, RoutedEventArgs e) {
            txt_feeValue.IsEnabled = txt_feeValueRangeMin.IsEnabled = txt_feeValueRangeMax.IsEnabled = (bool)chb_fee.IsChecked;
        }
    }
}