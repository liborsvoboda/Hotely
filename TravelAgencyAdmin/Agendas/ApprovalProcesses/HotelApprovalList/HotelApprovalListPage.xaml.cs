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

namespace UbytkacAdmin.Pages {

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
        List<HotelRoomList> hotelRoomList = new List<HotelRoomList>();

        public HotelApprovalListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            try {
                lbl_approvingProcess1.Content = lbl_approvingProcess2.Content = Resources["approvingProcess"].ToString();
                lbl_cityId.Content = Resources["city"].ToString();
                lbl_countryId.Content = Resources["country"].ToString();
                lbl_name.Content = Resources["fname"].ToString();
                lbl_descriptionCz.Content = Resources["descriptionCz"].ToString();
                lbl_currencyId.Content = Resources["currency"].ToString();
                lbl_enabledCommDaysBeforeStart.Content = Resources["enabledCommDaysBeforeStart"].ToString();

                lbl_stornoDaysCountBeforeStart.Content = Resources["stornoDaysCountBeforeStart"].ToString();
                lbl_guestStornoEnabled.Content = Resources["guestStornoEnabled"].ToString();

                lbl_owner.Content = lbl_owner1.Content = Resources["owner"].ToString();
                lbl_approveRequest.Content = Resources["approveRequest"].ToString();
                lbl_approved.Content = Resources["approved"].ToString();
                lbl_advertised.Content = Resources["advertised"].ToString();


                btn_save1.Content = btn_save2.Content = Resources["btn_save"].ToString();
                btn_next1.Content = Resources["next"].ToString();
                btn_cancel1.Content = btn_cancel2.Content = Resources["close"].ToString();

                //RoomList
                lbl_rooms.Content = Resources["roomList"].ToString();
                lbl_roomTypeId.Content = Resources["roomType"].ToString();
                lbl_name2.Content = Resources["fname"].ToString();
                lbl_descriptionCz2.Content = Resources["descriptionCz"].ToString();
                //lbl_descriptionEn2.Content = Resources["descriptionEn"].ToString();
                lbl_price.Content = Resources["price"].ToString();
                lbl_maxCapacity.Content = Resources["maxCapacity"].ToString();
                lbl_extraBed.Content = Resources["extraBed"].ToString();
                lbl_roomsCount.Content = Resources["roomsCount"].ToString();

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
                if (App.UserData.Authentification.Role == "admin") {
                    cb_owner.ItemsSource = adminUserList = await ApiCommunication.GetApiRequest<List<UserList>>(ApiUrls.UserList, null, App.UserData.Authentification.Token);
                    lbl_owner.Visibility = cb_owner.Visibility = Visibility.Visible;
                }

                HotelApprovalList.ForEach( async hotel => {
                    hotel.UserName = adminUserList.First(a => a.Id == hotel.UserId).UserName;
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
            MainWindow.ProgressRing = Visibility.Hidden; 
            return true;
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
                    else if (headername == "ApproveRequest") { e.Header = Resources["approveRequest"].ToString(); e.DisplayIndex = 8; }
                    else if (headername == "EnabledCommDaysBeforeStart") { e.Header = Resources["enabledCommDaysBeforeStart"].ToString(); e.DisplayIndex = 9; }

                    else if (headername == "StornoDaysCountBeforeStart") { e.Header = Resources["stornoDaysCountBeforeStart"].ToString(); e.DisplayIndex = 10; }
                    else if (headername == "GuestStornoEnabled") { e.Header = Resources["guestStornoEnabled"].ToString(); e.DisplayIndex = 11; }

                    else if (headername == "UserName") { e.Header = Resources["userName"].ToString(); e.DisplayIndex = 12; }
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
                    || user.UserName.ToLower().Contains(filter.ToLower())
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
                selectedRecord.DescriptionEn = "";
                selectedRecord.DefaultCurrencyId = (cb_currencyId.SelectedItem != null) ? (int?)((CurrencyList)cb_currencyId.SelectedItem).Id : null;

                selectedRecord.EnabledCommDaysBeforeStart = (int)txt_enabledCommDaysBeforeStart.Value;

                selectedRecord.StornoDaysCountBeforeStart = (int)txt_stornoDaysCountBeforeStart.Value;
                selectedRecord.GuestStornoEnabled = (bool)chb_guestStornoEnabled.IsChecked;

                selectedRecord.ApproveRequest = (bool)chb_approveRequest.IsChecked;
                selectedRecord.Approved = (bool)chb_approved.IsChecked;
                selectedRecord.Advertised = (bool)chb_advertised.IsChecked;
                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.Timestamp = DateTimeOffset.Now.DateTime;

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "admin")
                    selectedRecord.UserId = ((UserList)cb_owner.SelectedItem).Id;

                selectedRecord.CountryTranslation = selectedRecord.Currency = null; selectedRecord.City = null;
                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                dBResult = await ApiCommunication.PostApiRequest(ApiUrls.HotelList, httpContent, null, App.UserData.Authentification.Token);
                if (dBResult.RecordCount > 0) { await MainWindow.ShowMessageOnMainWindow(false, Resources["changesSaved"].ToString()); } else { await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private async void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (HotelApprovalList)DgListView.SelectedItem : new HotelApprovalList();
            await LoadDataList();
            SetRecord(false);
        }

        private async void SetRecord(bool showForm, bool copy = false) {

            cb_cityId.SelectedItem = cityList.FirstOrDefault(a => a.Id == selectedRecord.CityId);
            cb_countryId.SelectedItem = countryList.FirstOrDefault(a => a.Id == selectedRecord.CountryId);
            txt_name.Text = selectedRecord.Name;
            txt_descriptionCz.Text = selectedRecord.DescriptionCz;
            cb_currencyId.SelectedItem = currencyList.FirstOrDefault(a => a.Id == selectedRecord.DefaultCurrencyId);

            txt_enabledCommDaysBeforeStart.Value = selectedRecord.EnabledCommDaysBeforeStart;

            txt_stornoDaysCountBeforeStart.Value = selectedRecord.StornoDaysCountBeforeStart;
            chb_guestStornoEnabled.IsChecked = selectedRecord.GuestStornoEnabled;

            chb_approveRequest.IsChecked = false;
            btn_save1.IsEnabled = selectedRecord.ApproveRequest;
            chb_approved.IsChecked = selectedRecord.Approved;
            chb_advertised.IsChecked = selectedRecord.Advertised;

            hotelList = selectedRecord;

            //Only for Admin: Owner/UserId Selection
            if (App.UserData.Authentification.Role == "admin")
                cb_owner.Text = selectedRecord.Id == 0 ? App.UserData.UserName : adminUserList.Where(a => a.Id == selectedRecord.UserId).Select(a => a.UserName).FirstOrDefault();

            if (showForm) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = ListForm2.Visibility = Visibility.Hidden; ListForm1.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            } else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm1.Visibility = ListForm2.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
            }
            await LoadRoomDataList();
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
        }

        private async void BtnNext_Click(object sender, RoutedEventArgs e) {
            if (ListForm1.Visibility == Visibility.Visible) {
                ListForm1.Visibility = Visibility.Hidden;
                ListForm2.Visibility = Visibility.Visible;
                CleanRoomForm();
            }
        }

        //START RoomApproving
        public async Task<bool> LoadRoomDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                if (hotelList.Id != 0) {
                    hotelRoomList = await ApiCommunication.GetApiRequest<List<HotelRoomList>>(ApiUrls.HotelApprovalList, "Rooms/" + hotelList.Id, App.UserData.Authentification.Token);
                    hotelRoomTypeList = await ApiCommunication.GetApiRequest<List<HotelRoomTypeList>>(ApiUrls.HotelRoomTypeList, null, App.UserData.Authentification.Token);
                    currencyList = await ApiCommunication.GetApiRequest<List<CurrencyList>>(ApiUrls.CurrencyList, null, App.UserData.Authentification.Token);

                    hotelRoomTypeList.ForEach(async roomType => { roomType.Translation = await DBOperations.DBTranslation(roomType.SystemName); });

                    //Only for Admin: Owner/UserId Selection
                    if (App.UserData.Authentification.Role == "admin") {
                        cb_owner1.ItemsSource = adminUserList = await ApiCommunication.GetApiRequest<List<UserList>>(ApiUrls.UserList, null, App.UserData.Authentification.Token);
                        lbl_owner1.Visibility = cb_owner1.Visibility = Visibility.Visible;
                    }

                    hotelRoomList.ForEach(async room => {
                        room.Accommodation = hotelList.Name;
                        room.RoomType = await DBOperations.DBTranslation(hotelRoomTypeList.First(a => a.Id == room.RoomTypeId).SystemName);
                    });
                    DgRoomListView.ItemsSource = hotelRoomList;
                    DgRoomListView.Items.Refresh();

                    cb_roomTypeId.ItemsSource = hotelRoomTypeList;
                }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden;
            return true;
        }

        private void DgRoomListView_Translate(object sender, EventArgs ex) {
            ((DataGrid)sender).Columns.ToList().ForEach(e => {
                string headername = e.Header.ToString();
                if (headername == "Name") { e.Header = Resources["fname"].ToString(); e.DisplayIndex = 3; }
                else if (headername == "Accommodation") { e.Header = Resources["accommodation"].ToString(); e.DisplayIndex = 1; } 
                else if (headername == "RoomType") { e.Header = Resources["roomType"].ToString(); e.DisplayIndex = 2; } 
                else if (headername == "Price") { e.Header = Resources["price"].ToString(); e.DisplayIndex = 4; } 
                else if (headername == "MaxCapacity") e.Header = Resources["maxCapacity"].ToString();
                else if (headername == "ExtraBed") e.Header = Resources["extraBed"].ToString();
                else if (headername == "RoomsCount") e.Header = Resources["roomsCount"].ToString();
                else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; } 
                
                else if (headername == "Id") e.DisplayIndex = 0;
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
            txt_descriptionCz2.Text = null;
            txt_price.Value = txt_maxCapacity.Value = txt_roomsCount.Value = null;
            chb_extraBed.IsChecked = false;


            cb_roomTypeId.IsEnabled = txt_name2.IsEnabled = txt_descriptionCz2.IsEnabled =
                txt_price.IsEnabled = txt_maxCapacity.IsEnabled = txt_roomsCount.IsEnabled = chb_extraBed.IsEnabled =
                btn_save1.IsEnabled = false;
        }

        private void SetRoomRecord() {
            try {
                cb_roomTypeId.Text = hotelRoomTypeList.FirstOrDefault(a => a.Id == selectedRoom.RoomTypeId).Translation;
                txt_name2.Text = selectedRoom.Name;
                txt_descriptionCz2.Text = selectedRoom.DescriptionCz;
                txt_price.Value = selectedRoom.Price;
                txt_maxCapacity.Value = selectedRoom.MaxCapacity;
                chb_extraBed.IsChecked = selectedRoom.ExtraBed;
                txt_roomsCount.Value = selectedRoom.RoomsCount;

                btn_save1.IsEnabled = true;

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "admin")
                    cb_owner1.Text = adminUserList.Where(a => a.Id == selectedRoom.UserId).Select(a => a.UserName).FirstOrDefault();

                cb_roomTypeId.IsEnabled = txt_name2.IsEnabled = txt_descriptionCz2.IsEnabled = 
                    txt_price.IsEnabled = txt_maxCapacity.IsEnabled = txt_roomsCount.IsEnabled = chb_extraBed.IsEnabled = true;
            }
            catch (Exception ex) { }
        }

        private async void BtnSaveRoom_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRoom.RoomTypeId = (cb_roomTypeId.SelectedItem != null) ? (int?)((HotelRoomTypeList)cb_roomTypeId.SelectedItem).Id : null;
                selectedRoom.Name = !string.IsNullOrWhiteSpace(txt_name2.Text) ? txt_name2.Text : null;
                selectedRoom.DescriptionCz = txt_descriptionCz2.Text;
                selectedRoom.DescriptionEn = "";
                selectedRoom.Price = (double)txt_price.Value;
                selectedRoom.MaxCapacity = (int)txt_maxCapacity.Value;
                selectedRoom.ExtraBed = (bool)chb_extraBed.IsChecked;
                selectedRoom.RoomsCount = (int)txt_roomsCount.Value;

                selectedRoom.Timestamp = DateTimeOffset.Now.DateTime;

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "admin")
                    selectedRoom.UserId = ((UserList)cb_owner1.SelectedItem).Id;

                selectedRoom.Accommodation = selectedRoom.RoomType = null;
                string json = JsonConvert.SerializeObject(selectedRoom);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                dBResult = await ApiCommunication.PostApiRequest(ApiUrls.HotelRoomList, httpContent, null, App.UserData.Authentification.Token);

                if (dBResult.RecordCount > 0) { LoadRoomDataList(); } else { await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }
    }
}