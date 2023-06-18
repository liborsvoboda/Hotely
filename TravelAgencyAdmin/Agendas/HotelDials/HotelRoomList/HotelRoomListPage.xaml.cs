using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.Classes;
using TravelAgencyAdmin.GlobalOperations;
using TravelAgencyAdmin.GlobalStyles;

namespace TravelAgencyAdmin.Pages {

    public partial class HotelRoomListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static HotelRoomList selectedRecord = new HotelRoomList();

        private List<UserList> adminUserList = new List<UserList>();
        private List<HotelList> hotelList = new List<HotelList>();
        private List<HotelRoomTypeList> hotelRoomTypeList = new List<HotelRoomTypeList>();
        private List<CurrencyList> currencyList = new List<CurrencyList>();

        public HotelRoomListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            try {
                lbl_id.Content = Resources["id"].ToString();
                lbl_hotelId.Content = Resources["accommodation"].ToString();
                lbl_roomTypeId.Content = Resources["roomType"].ToString();
                lbl_name.Content = Resources["fname"].ToString();
                lbl_descriptionCz.Content = Resources["descriptionCz"].ToString();
                lbl_descriptionEn.Content = Resources["descriptionEn"].ToString();
                lbl_price.Content = Resources["price"].ToString();
                lbl_maxCapacity.Content = Resources["maxCapacity"].ToString();
                lbl_extraBed.Content = Resources["extraBed"].ToString();
                lbl_owner.Content = Resources["owner"].ToString();
                lbl_roomsCount.Content = Resources["roomsCount"].ToString();
                lbl_approveRequest.Content = Resources["approveRequest"].ToString();
                lbl_approved.Content = Resources["approved"].ToString();
                lbl_image.Content = Resources["image"].ToString();

                btn_browse.Content = Resources["browse"].ToString();
                btn_save.Content = Resources["btn_save"].ToString();
                btn_cancel.Content = Resources["btn_cancel"].ToString();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            _ = LoadDataList();
            SetRecord(false);
        }

        public async Task<bool> LoadDataList() {
            List<HotelRoomList> hotelRoomList = new List<HotelRoomList>();
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                hotelRoomList = await ApiCommunication.GetApiRequest<List<HotelRoomList>>(ApiUrls.HotelRoomList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                hotelList = await ApiCommunication.GetApiRequest<List<HotelList>>(ApiUrls.HotelList, null, App.UserData.Authentification.Token);
                hotelRoomTypeList = await ApiCommunication.GetApiRequest<List<HotelRoomTypeList>>(ApiUrls.HotelRoomTypeList, null, App.UserData.Authentification.Token);
                currencyList = await ApiCommunication.GetApiRequest<List<CurrencyList>>(ApiUrls.CurrencyList, null, App.UserData.Authentification.Token);

                hotelList.ForEach(hotel => { hotel.Currency = currencyList.First(a => a.Id == hotel.DefaultCurrencyId).Name; });
                hotelRoomTypeList.ForEach(async roomType => { roomType.Translation = await DBOperations.DBTranslation(roomType.SystemName); });

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin") {
                    cb_owner.ItemsSource = adminUserList = await ApiCommunication.GetApiRequest<List<UserList>>(ApiUrls.UserList, null, App.UserData.Authentification.Token);
                    lbl_owner.Visibility = cb_owner.Visibility = Visibility.Visible;
                }

                hotelRoomList.ForEach(async room => {
                    room.Accommodation = hotelList.First(a => a.Id == room.HotelId).Name;
                    room.RoomType = await DBOperations.DBTranslation(hotelRoomTypeList.First(a => a.Id == room.RoomTypeId).SystemName);
                });
                DgListView.ItemsSource = hotelRoomList;
                DgListView.Items.Refresh();

                cb_hotelId.ItemsSource = hotelList;
                cb_roomTypeId.ItemsSource = hotelRoomTypeList;
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        private void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "Name") { e.Header = Resources["fname"].ToString(); e.DisplayIndex = 3; } else if (headername == "Accommodation") { e.Header = Resources["accommodation"].ToString(); e.DisplayIndex = 1; } else if (headername == "RoomType") { e.Header = Resources["roomType"].ToString(); e.DisplayIndex = 2; } else if (headername == "Price") { e.Header = Resources["price"].ToString(); e.DisplayIndex = 4; } else if (headername == "MaxCapacity") e.Header = Resources["maxCapacity"].ToString();
                    else if (headername == "ExtraBed") e.Header = Resources["extraBed"].ToString();
                    else if (headername == "RoomsCount") e.Header = Resources["roomsCount"].ToString();
                    else if (headername == "ApproveRequest") e.Header = Resources["approveRequest"].ToString();
                    else if (headername == "Approved") e.Header = Resources["approved"].ToString();
                    else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; } else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                    else if (headername == "HotelId") e.Visibility = Visibility.Hidden;
                    else if (headername == "RoomTypeId") e.Visibility = Visibility.Hidden;
                    else if (headername == "DescriptionCz") e.Visibility = Visibility.Hidden;
                    else if (headername == "DescriptionEn") e.Visibility = Visibility.Hidden;
                    else if (headername == "Image") e.Visibility = Visibility.Hidden;
                    else if (headername == "ImageName") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    HotelRoomList user = e as HotelRoomList;
                    return user.Name.ToLower().Contains(filter.ToLower())
                    || user.Accommodation.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.DescriptionCz) && user.DescriptionCz.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.DescriptionEn) && user.DescriptionEn.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void NewRecord() {
            selectedRecord = new HotelRoomList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (HotelRoomList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (HotelRoomList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessage(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await ApiCommunication.DeleteApiRequest(ApiUrls.HotelRoomList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.recordCount == 0) await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (HotelRoomList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (HotelRoomList)DgListView.SelectedItem; } else { selectedRecord = new HotelRoomList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.HotelId = ((cb_hotelId.SelectedItem != null) ? (int?)((HotelList)cb_hotelId.SelectedItem).Id : null);
                selectedRecord.RoomTypeId = (cb_roomTypeId.SelectedItem != null) ? (int?)((HotelRoomTypeList)cb_roomTypeId.SelectedItem).Id : null;
                selectedRecord.Name = !string.IsNullOrWhiteSpace(txt_name.Text) ? txt_name.Text : null;
                selectedRecord.DescriptionCz = txt_descriptionCz.Text;
                selectedRecord.DescriptionEn = txt_descriptionEn.Text;
                selectedRecord.Price = (double)txt_price.Value;
                selectedRecord.MaxCapacity = (int)txt_maxCapacity.Value;
                selectedRecord.ExtraBed = (bool)chb_extraBed.IsChecked;
                selectedRecord.RoomsCount = (int)txt_roomsCount.Value;
                selectedRecord.ApproveRequest = (bool)chb_approveRequest.IsChecked;
                selectedRecord.Approved = (bool)chb_approved.IsChecked;

                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.Timestamp = DateTimeOffset.Now.DateTime;

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin")
                    selectedRecord.UserId = ((UserList)cb_owner.SelectedItem).Id;

                selectedRecord.Accommodation = selectedRecord.RoomType = null;
                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await ApiCommunication.PutApiRequest(ApiUrls.HotelRoomList, httpContent, null, App.UserData.Authentification.Token);
                } else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.HotelRoomList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.recordCount > 0) {
                    selectedRecord = new HotelRoomList();
                    await LoadDataList();
                    SetRecord(false);
                } else { await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (HotelRoomList)DgListView.SelectedItem : new HotelRoomList();
            SetRecord(false);
        }

        private void SetRecord(bool showForm, bool copy = false) {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;
            cb_hotelId.SelectedItem = hotelList.FirstOrDefault(a => a.Id == selectedRecord.HotelId);
            cb_roomTypeId.SelectedItem = hotelRoomTypeList.FirstOrDefault(a => a.Id == selectedRecord.RoomTypeId);
            txt_name.Text = selectedRecord.Name;
            txt_descriptionCz.Text = selectedRecord.DescriptionCz;
            txt_descriptionEn.Text = selectedRecord.DescriptionEn;
            txt_price.Value = selectedRecord.Price;
            txt_maxCapacity.Value = selectedRecord.MaxCapacity;
            chb_extraBed.IsChecked = selectedRecord.ExtraBed;
            txt_roomsCount.Value = selectedRecord.RoomsCount;
            chb_approveRequest.IsChecked = selectedRecord.ApproveRequest;
            chb_approved.IsChecked = false;

            img_photoPath.Source = (selectedRecord.Image == null) ? new BitmapImage(new Uri(Path.Combine(App.settingFolder, "no_photo.png"))) : MediaOperations.ArrayToImage(selectedRecord.Image);

            //Only for Admin: Owner/UserId Selection
            if (App.UserData.Authentification.Role == "Admin")
                cb_owner.Text = txt_id.Value == 0 ? App.UserData.UserName : adminUserList.Where(a => a.Id == selectedRecord.UserId).Select(a => a.UserName).FirstOrDefault();

            if (showForm) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            } else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
            }
        }

        private void HotelSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cb_hotelId.SelectedItem != null)
                lbl_defaultPriceUnit.Content = ((HotelList)cb_hotelId.SelectedItem).Currency;
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e) {
            try {
                OpenFileDialog dlg = new OpenFileDialog() { DefaultExt = ".png", Filter = "Image files |*.png;*.jpg;*.jpeg", Title = Resources["fileOpenDescription"].ToString() };
                if (dlg.ShowDialog() == true) {
                    img_photoPath.Source = new BitmapImage(new Uri(dlg.FileName));
                    selectedRecord.ImageName = dlg.SafeFileName;
                    selectedRecord.Image = File.ReadAllBytes(dlg.FileName);
                }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }
    }
}