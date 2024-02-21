using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UbytkacAdmin.Api;
using UbytkacAdmin.Classes;
using UbytkacAdmin.GlobalOperations;
using UbytkacAdmin.GlobalStyles;


namespace UbytkacAdmin.Pages {


    public partial class GuestFavoriteListPage : UserControl {


        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static GuestFavoriteList selectedRecord = new GuestFavoriteList();

        private List<GuestFavoriteList> guestFavoriteList = new List<GuestFavoriteList>(); 
        private List<GuestList> guestList = new List<GuestList>();
        private List<HotelList> hotelList = new List<HotelList>();

        public GuestFavoriteListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            _ = LoadDataList();
            SetRecord();
        }


        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try { 

                guestFavoriteList = await ApiCommunication.GetApiRequest<List<GuestFavoriteList>>(ApiUrls.GuestFavoriteList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                guestList = await ApiCommunication.GetApiRequest<List<GuestList>>(ApiUrls.GuestList, null, App.UserData.Authentification.Token);
                hotelList = await ApiCommunication.GetApiRequest<List<HotelList>>(ApiUrls.HotelList, null, App.UserData.Authentification.Token);

                guestFavoriteList.ForEach(favorite => {
                    favorite.GuestName = guestList.First(a => a.Id == favorite.GuestId).FirstName + " " + guestList.First(a => a.Id == favorite.GuestId).LastName;
                    favorite.HotelName = hotelList.First(a => a.Id == favorite.HotelId).Name;
                });

                DgListView.ItemsSource = guestFavoriteList;
                DgListView.Items.Refresh();

            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        
        private void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "HotelName") { e.Header = Resources["fname"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = 1; }
                    else if (headername == "GuestName") { e.Header = Resources["guestName"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = 2; }

                    else if (headername == "TimeStamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; } 
                    
                    else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "GuestId") e.Visibility = Visibility.Hidden;
                    else if (headername == "HotelId") e.Visibility = Visibility.Hidden;
                    else if (headername == "Description") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }


        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    GuestFavoriteList user = e as GuestFavoriteList;
                    return user.HotelName.ToLower().Contains(filter.ToLower())
                    || user.Description.ToLower().Contains(filter.ToLower());
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }


        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (GuestFavoriteList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord();
        }


        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (GuestFavoriteList)DgListView.SelectedItem; } else { selectedRecord = new GuestFavoriteList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord();
        }


        private void SetRecord() {
            MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = false; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
            ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
        }
    }
}