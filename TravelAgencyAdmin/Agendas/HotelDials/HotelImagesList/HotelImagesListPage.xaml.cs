using HelixToolkit.Wpf;
using Newtonsoft.Json;
using TravelAgencyAdmin.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using TravelAgencyAdmin.GlobalFunctions;
using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.GlobalStyles;
using System.Net;


// This is Template ListView
namespace TravelAgencyAdmin.Pages
{
    public partial class HotelImagesListPage : UserControl
    {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static HotelImagesList selectedRecord = new HotelImagesList();


        private List<HotelImagesList> hotelImagesList = new List<HotelImagesList>();
        private List<HotelList> hotelList = new List<HotelList>();

        public HotelImagesListPage()
        {
            InitializeComponent();
            _ = MediaFunctions.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            _ = LoadDataList();
            SetRecord();
        }

        //change datasource
        public async Task<bool> LoadDataList()
        {
            MainWindow.ProgressRing = Visibility.Visible;
            try
            {
                hotelImagesList = await ApiCommunication.GetApiRequest<List<HotelImagesList>>(ApiUrls.HotelImagesList, null, App.UserData.Authentification.Token);
                hotelList = await ApiCommunication.GetApiRequest<List<HotelList>>(ApiUrls.HotelList, null, App.UserData.Authentification.Token);

                hotelImagesList.ForEach(item => { item.Hotel = hotelList.First(a=>a.Id == item.HotelId).Name; });


                DgListView.ItemsSource = hotelImagesList;
                DgListView.Items.Refresh();
            }
            catch (Exception autoEx) {App.ApplicationLogging(autoEx);}
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        // set translate columns in listView
        private void DgListView_Translate(object sender, EventArgs ex)
        {
            try { 
                ((DataGrid)sender).Columns.ToList().ForEach(e =>
                {
                    string headername = e.Header.ToString();
                    if (headername == "Active") { e.Header = Resources["active"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 2; }
                    else if (headername == "PartNumber") { e.Header = Resources["partNumber"].ToString(); }
                    else if (headername == "FileName") { e.Header = Resources["fileName"].ToString();}
                    else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }

                    else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) {App.ApplicationLogging(autoEx);}

        }

        //change filter fields
        public void Filter(string filter)
        {
            try
            {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    HotelImagesList user = e as HotelImagesList;
                    return user.TimeStamp.ToShortDateString().ToLower().Contains(filter.ToLower())
                    || user.FileName.ToLower().Contains(filter.ToLower());
                };
            }
            catch (Exception autoEx) {App.ApplicationLogging(autoEx);}
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (HotelImagesList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord();
        }

        private async void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DgListView.SelectedItems.Count > 0)
                {
                    selectedRecord = (HotelImagesList)DgListView.SelectedItem;
                    dataViewSupport.SelectedRecordId = selectedRecord.Id;

                    string filePath = Path.Combine(App.tempFolder, selectedRecord.FileName);
                    FileFunctions.ByteArrayToFile(filePath, (await ApiCommunication.GetApiRequest<HotelImagesList>(ApiUrls.HotelImagesList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token)).Attachment);
                } else { selectedRecord = new HotelImagesList(); }
                dataViewSupport.SelectedRecordId = selectedRecord.Id;
                SetRecord();
            }
            catch (Exception autoEx) {App.ApplicationLogging(autoEx);}
        }

        //change dataset prepare for working
        private void SetRecord()
        {
            MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = false; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
            ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
        }
    }
}