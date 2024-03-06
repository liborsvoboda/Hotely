using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EasyITSystemCenter.Api;
using EasyITSystemCenter.Classes;
using EasyITSystemCenter.GlobalOperations;
using EasyITSystemCenter.GlobalStyles;

namespace EasyITSystemCenter.Pages {

    public partial class HotelReservationListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static HotelReservationList selectedRecord = new HotelReservationList();

        private List<HotelReservationList> hotelReservationList =  new List<HotelReservationList>();
        private List<HotelList> hotelList = new List<HotelList>();
        private List<BasicCurrencyList> basicCurrencyList = new List<BasicCurrencyList>();
        private List<GuestList> guestList = new List<GuestList>();
        private List<HotelReservationStatusList> hotelReservationStatusList = new List<HotelReservationStatusList>();

        public HotelReservationListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);

            _ = LoadDataList();
            SetRecord();
        }

        //change datasource
        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                
                hotelReservationList = await CommApi.GetApiRequest<List<HotelReservationList>>(ApiUrls.HotelReservationList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                hotelList = await CommApi.GetApiRequest<List<HotelList>>(ApiUrls.HotelList, null, App.UserData.Authentification.Token);
                basicCurrencyList = await CommApi.GetApiRequest<List<BasicCurrencyList>>(ApiUrls.BasicCurrencyList, null, App.UserData.Authentification.Token);
                guestList = await CommApi.GetApiRequest<List<GuestList>>(ApiUrls.GuestList, null, App.UserData.Authentification.Token);
                hotelReservationStatusList = await CommApi.GetApiRequest<List<HotelReservationStatusList>>(ApiUrls.HotelReservationStatusList, null, App.UserData.Authentification.Token);

                hotelReservationList.ForEach(async reservation => {

                    reservation.HotelName = hotelList.First(a => a.Id == reservation.HotelId).Name;
                    reservation.GuestName = guestList.First(a => a.Id == reservation.GuestId).FirstName + " " + guestList.First(a => a.Id == reservation.GuestId).LastName;
                    reservation.StatusName = await DBOperations.DBTranslation(hotelReservationStatusList.First(a => a.Id == reservation.StatusId).SystemName);
                    reservation.CurrencyName = basicCurrencyList.First(a => a.Id == reservation.CurrencyId).Name;

                });

                DgListView.ItemsSource = hotelReservationList;
                DgListView.Items.Refresh();

            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
                MainWindow.ProgressRing = Visibility.Hidden; return true;
            }

        // set translate columns in listView
        private void DgListView_Translate(object sender, EventArgs ex) {
            ((DataGrid)sender).Columns.ToList().ForEach(e => {
                string headername = e.Header.ToString();
                if (headername == "ReservationNumber") { e.Header = Resources["reservationNumber"].ToString(); e.DisplayIndex = 1; }
                else if (headername == "HotelName") { e.Header = Resources["hotelName"].ToString(); e.DisplayIndex = 2; }
                else if (headername == "GuestName") { e.Header = Resources["guestName"].ToString(); e.DisplayIndex = 3; }
                else if (headername == "StatusName") { e.Header = Resources["status"].ToString(); e.DisplayIndex = 4; }
                else if (headername == "TotalPrice") { e.Header = Resources["totalPrice"].ToString(); e.DisplayIndex = 5; }
                else if (headername == "CurrencyName") { e.Header = Resources["currency"].ToString(); e.DisplayIndex = 6; }
                else if (headername == "StartDate") { e.Header = Resources["startDate"].ToString(); e.DisplayIndex = 7; }
                else if (headername == "EndDate") { e.Header = Resources["endDate"].ToString(); e.DisplayIndex = 8; }
                else if (headername == "Adult") { e.Header = Resources["adult"].ToString(); e.DisplayIndex = 9; }
                else if (headername == "Children") { e.Header = Resources["children"].ToString(); e.DisplayIndex = 10; }
                else if (headername == "Street") { e.Header = Resources["street"].ToString(); e.DisplayIndex = 11; }
                else if (headername == "Zipcode") { e.Header = Resources["zipcode"].ToString(); e.DisplayIndex = 12; }
                else if (headername == "City") { e.Header = Resources["city"].ToString(); e.DisplayIndex = 13; }
                else if (headername == "Country") { e.Header = Resources["country"].ToString(); e.DisplayIndex = 14; }
                else if (headername == "Phone") { e.Header = Resources["phone"].ToString(); e.DisplayIndex = 15; }
                else if (headername == "Email") { e.Header = Resources["email"].ToString(); e.DisplayIndex = 16; }


                else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }

                else if (headername == "Id") e.DisplayIndex = 0;
                else if (headername == "HotelId") e.Visibility = Visibility.Hidden;
                else if (headername == "GuestId") e.Visibility = Visibility.Hidden;
                else if (headername == "StatusId") e.Visibility = Visibility.Hidden;
                else if (headername == "CurrencyId") e.Visibility = Visibility.Hidden;
                else if (headername == "HotelAccommodationActionId") e.Visibility = Visibility.Hidden;
                else if (headername == "FirstName") e.Visibility = Visibility.Hidden;
                else if (headername == "LastName") e.Visibility = Visibility.Hidden;
            });
        }

        //change filter fields
        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    HotelReservationList user = e as HotelReservationList;
                    if (
                       user.Id.ToString().ToLower().Contains(filter.ToLower())
                    || user.ReservationNumber.ToLower().Contains(filter.ToLower())
                    || user.HotelName.ToLower().Contains(filter.ToLower())
                    || user.GuestName.ToLower().Contains(filter.ToLower())
                    || user.StatusName.ToLower().Contains(filter.ToLower())
                    || user.Phone.ToLower().Contains(filter.ToLower())
                    || user.Email.ToLower().Contains(filter.ToLower())
                    ) return true;
                    return false;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (HotelReservationList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord();
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (HotelReservationList)DgListView.SelectedItem; } else { selectedRecord = new HotelReservationList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //change dataset prepare for working
        private void SetRecord() {
            MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = false; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
            ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
        }
    }
}