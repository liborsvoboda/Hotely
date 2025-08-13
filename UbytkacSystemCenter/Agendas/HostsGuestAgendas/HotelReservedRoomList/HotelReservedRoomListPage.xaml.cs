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
using EasyITSystemCenter.GlobalClasses;

namespace EasyITSystemCenter.Pages {

    public partial class HotelReservedRoomListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static HotelReservedRoomList selectedRecord = new HotelReservedRoomList();

        private List<HotelReservedRoomList> hotelReservedRoomList =  new List<HotelReservedRoomList>();
        private List<HotelList> hotelList = new List<HotelList>();
        private List<HotelReservationStatusList> hotelReservationStatusList = new List<HotelReservationStatusList>();
        private List<HotelReservationList> hotelReservationList = new List<HotelReservationList>();

        public HotelReservedRoomListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);

            _ = LoadDataList();
            SetRecord();
        }

        //change datasource
        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {

                hotelReservedRoomList = await CommunicationManager.GetApiRequest<List<HotelReservedRoomList>>(ApiUrls.HotelReservedRoomList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                hotelList = await CommunicationManager.GetApiRequest<List<HotelList>>(ApiUrls.HotelList, null, App.UserData.Authentification.Token);
                hotelReservationStatusList = await CommunicationManager.GetApiRequest<List<HotelReservationStatusList>>(ApiUrls.HotelReservationStatusList, null, App.UserData.Authentification.Token);
                hotelReservationList = await CommunicationManager.GetApiRequest<List<HotelReservationList>>(ApiUrls.HotelReservationList, null, App.UserData.Authentification.Token);


                hotelReservedRoomList.ForEach(async reservation => {

                    reservation.HotelName = hotelList.First(a => a.Id == reservation.HotelId).Name;
                    reservation.GuestName = hotelReservationList.First(a => a.Id == reservation.ReservationId).FirstName + " " + hotelReservationList.First(a => a.Id == reservation.ReservationId).LastName;
                    reservation.StatusName = await DBOperations.DBTranslation(hotelReservationStatusList.First(a => a.Id == reservation.StatusId).SystemName);
                    reservation.ReservationNumber = hotelReservationList.First(a => a.Id == reservation.ReservationId).ReservationNumber;

                });
                List<HotelReservedRoomList> shortHotelReservedRoomList = new List<HotelReservedRoomList>();
                shortHotelReservedRoomList.AddRange(hotelReservedRoomList.Where(a=> a.Count > 0));

                DgListView.ItemsSource = shortHotelReservedRoomList;
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
                else if (headername == "Name") { e.Header = Resources["fname"].ToString(); e.DisplayIndex = 2; }
                else if (headername == "Count") { e.Header = Resources["roomsCount"].ToString(); e.DisplayIndex = 3; }
                else if (headername == "ExtraBed") { e.Header = Resources["extraBed"].ToString(); e.DisplayIndex = 4; }
                else if (headername == "StatusName") { e.Header = Resources["status"].ToString(); e.DisplayIndex = 5; }
                else if (headername == "StartDate") { e.Header = Resources["startDate"].ToString(); e.DisplayIndex = 6; }
                else if (headername == "EndDate") { e.Header = Resources["endDate"].ToString(); e.DisplayIndex = 7; }
                else if (headername == "GuestName") { e.Header = Resources["guestName"].ToString(); e.DisplayIndex = 8; }

                else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }

                else if (headername == "Id") e.DisplayIndex = 0;
                else if (headername == "HotelId") e.Visibility = Visibility.Hidden;
                else if (headername == "GuestId") e.Visibility = Visibility.Hidden;
                else if (headername == "StatusId") e.Visibility = Visibility.Hidden;
                else if (headername == "CurrencyId") e.Visibility = Visibility.Hidden;
                else if (headername == "HotelRoomId") e.Visibility = Visibility.Hidden;
                else if (headername == "ReservationId") e.Visibility = Visibility.Hidden;
                else if (headername == "RoomTypeId") e.Visibility = Visibility.Hidden;
            });
        }

        //change filter fields
        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    HotelReservedRoomList user = e as HotelReservedRoomList;
                    if (
                       user.Id.ToString().ToLower().Contains(filter.ToLower())
                    || user.ReservationNumber.ToLower().Contains(filter.ToLower())
                    || user.HotelName.ToLower().Contains(filter.ToLower())
                    || user.GuestName.ToLower().Contains(filter.ToLower())
                    || user.StatusName.ToLower().Contains(filter.ToLower())
                    ) return true;
                    return false;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (HotelReservedRoomList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord();
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (HotelReservedRoomList)DgListView.SelectedItem; } else { selectedRecord = new HotelReservedRoomList(); }
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