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
using EasyITSystemCenter.Api;
using EasyITSystemCenter.Classes;
using EasyITSystemCenter.GlobalOperations;
using EasyITSystemCenter.GlobalStyles;

namespace EasyITSystemCenter.Pages {

    public partial class HotelReservationReviewListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static HotelReservationReviewList selectedRecord = new HotelReservationReviewList();


        private List<HotelList> hotelList = new List<HotelList>();
        private List<HotelReservationList> hotelReservationList = new List<HotelReservationList>();
        private List<GuestList> guestList = new List<GuestList>();

        public HotelReservationReviewListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);

            try {
                lbl_id.Content = Resources["id"].ToString();
                lbl_hotelName.Content = Resources["hotelName"].ToString();
                lbl_reservationNumber.Content = Resources["reservationNumber"].ToString();
                lbl_guestName.Content = Resources["guestName"].ToString();
                lbl_rating.Content = Resources["rating"].ToString();
                lbl_description.Content = Resources["description"].ToString();
                lbl_answer.Content = Resources["answer"].ToString();
                lbl_advertiserShown.Content = Resources["advertiserShown"].ToString();
                lbl_approved.Content = Resources["approved"].ToString();

                btn_save.Content = Resources["btn_save"].ToString();
                btn_cancel.Content = Resources["btn_cancel"].ToString();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            _ = LoadDataList();
            SetRecord(false);
        }

        public async Task<bool> LoadDataList() {
            List<HotelReservationReviewList> hotelReservationReviewList = new List<HotelReservationReviewList>();
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                hotelReservationReviewList = await CommApi.GetApiRequest<List<HotelReservationReviewList>>(ApiUrls.HotelReservationReviewList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                hotelList = await CommApi.GetApiRequest<List<HotelList>>(ApiUrls.HotelList, null, App.UserData.Authentification.Token);
                hotelReservationList = await CommApi.GetApiRequest<List<HotelReservationList>>(ApiUrls.HotelReservationList, null, App.UserData.Authentification.Token);
                guestList = await CommApi.GetApiRequest<List<GuestList>>(ApiUrls.GuestList, null, App.UserData.Authentification.Token);

                hotelReservationReviewList.ForEach(async hotel => {
                    hotel.HotelName = hotelList.First(a=>a.Id == hotel.HotelId).Name; txt_hotelName.Content = hotel.HotelName;
                    hotel.GuestName = guestList.First(a => a.Id == hotel.GuestId).FirstName + " " + guestList.First(a => a.Id == hotel.GuestId).LastName; txt_guestName.Content = hotel.GuestName;
                    hotel.ReservationNumber = hotelReservationList.First(a => a.Id == hotel.ReservationId).ReservationNumber; txt_reservationNumber.Content = hotel.ReservationNumber;
                });

                DgListView.ItemsSource = hotelReservationReviewList;
                DgListView.Items.Refresh();

            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        private void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "Rating") { e.Header = Resources["rating"].ToString(); e.DisplayIndex = 3; } 
                    else if (headername == "HotelName") { e.Header = Resources["hotelName"].ToString(); e.DisplayIndex = 1; } 
                    else if (headername == "ReservationNumber") { e.Header = Resources["reservationNumber"].ToString(); e.DisplayIndex = 2; } 
                    else if (headername == "Approved") { e.Header = Resources["approved"].ToString(); e.DisplayIndex = 4; } 
                    else if (headername == "AdvertiserShown") { e.Header = Resources["advertiserShown"].ToString(); e.DisplayIndex = 5; } 
                    else if (headername == "GuestName") { e.Header = Resources["guestName"].ToString(); e.DisplayIndex = 6; }
                    
                    else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; } 

                    else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "GuestId") e.Visibility = Visibility.Hidden;
                    else if (headername == "HotelId") e.Visibility = Visibility.Hidden;
                    else if (headername == "ReservationId") e.Visibility = Visibility.Hidden;
                    else if (headername == "Answer") e.Visibility = Visibility.Hidden;
                    else if (headername == "Description") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    HotelReservationReviewList user = e as HotelReservationReviewList;
                    return user.HotelName.ToLower().Contains(filter.ToLower())
                    || user.ReservationNumber.ToLower().Contains(filter.ToLower())
                    || user.GuestName.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.Description) && user.Description.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.Answer) && user.Answer.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void NewRecord() {
            selectedRecord = new HotelReservationReviewList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (HotelReservationReviewList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (HotelReservationReviewList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await CommApi.DeleteApiRequest(ApiUrls.HotelReservationReviewList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (HotelReservationReviewList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (HotelReservationReviewList)DgListView.SelectedItem; } else { selectedRecord = new HotelReservationReviewList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);

                selectedRecord.Rating = (int)txt_rating.Value;
                selectedRecord.Description = tb_description.Text;
                selectedRecord.Answer = tb_answer.Text;

                selectedRecord.AdvertiserShown = (bool)chb_advertiserShown.IsChecked;
                selectedRecord.Approved = chb_approved.IsChecked;
                selectedRecord.Timestamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await CommApi.PutApiRequest(ApiUrls.HotelReservationReviewList, httpContent, null, App.UserData.Authentification.Token);
                } else { dBResult = await CommApi.PostApiRequest(ApiUrls.HotelReservationReviewList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.RecordCount > 0) {
                    selectedRecord = new HotelReservationReviewList();
                    await LoadDataList();
                    SetRecord(false);
                } else { await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (HotelReservationReviewList)DgListView.SelectedItem : new HotelReservationReviewList();
            SetRecord(false);
        }

        private void SetRecord(bool showForm, bool copy = false) {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;

            txt_rating.Value = selectedRecord.Rating;
            tb_description.Text = selectedRecord.Description;
            tb_answer.Text = selectedRecord.Answer;

            chb_advertiserShown.IsChecked = selectedRecord.AdvertiserShown;
            chb_approved.IsChecked = selectedRecord.Approved;

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