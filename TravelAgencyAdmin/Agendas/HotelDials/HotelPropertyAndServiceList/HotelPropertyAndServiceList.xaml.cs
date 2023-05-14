using Newtonsoft.Json;
using TravelAgencyAdmin.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.GlobalStyles;
using TravelAgencyAdmin.GlobalOperations;
using MahApps.Metro.Controls.Dialogs;
using System.Net;


namespace TravelAgencyAdmin.Pages
{
    public partial class HotelPropertyAndServiceListPage : UserControl
    {

        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static HotelPropertyAndServiceList selectedRecord = new HotelPropertyAndServiceList();

        private List<UserList> adminUserList = new List<UserList>();
        private List<HotelList> hotelList = new List<HotelList>();
        private List<CurrencyList> currencyList = new List<CurrencyList>();
        private List<PropertyOrServiceTypeList> propertyOrServiceTypeList = new List<PropertyOrServiceTypeList>();
        private List<PropertyOrServiceUnitList> propertyOrServiceUnitList = new List<PropertyOrServiceUnitList>();

        public HotelPropertyAndServiceListPage()
        {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            try
            {
                lbl_id.Content = Resources["id"].ToString();
                lbl_hotelId.Content = Resources["accommodation"].ToString();
                lbl_propertyOrServiceId.Content = Resources["propertyOrService"].ToString();
                lbl_isAvailable.Content = Resources["isAvailable"].ToString();
                lbl_value.Content = Resources["value"].ToString();
                lbl_valueRangeMin.Content = Resources["valueRangeMin"].ToString();
                lbl_valueRangeMax.Content = Resources["valueRangeMax"].ToString();
                lbl_fee.Content = Resources["fee"].ToString();
                lbl_feeValue.Content = Resources["feeValue"].ToString();
                lbl_feeValueRangeMin.Content = Resources["feeValueRangeMin"].ToString();
                lbl_feeValueRangeMax.Content = Resources["feeValueRangeMax"].ToString();

                lbl_owner.Content = Resources["owner"].ToString();
                lbl_approveRequest.Content = Resources["approveRequest"].ToString();
                lbl_approved.Content = Resources["approved"].ToString();

                btn_save.Content = Resources["btn_save"].ToString();
                btn_cancel.Content = Resources["btn_cancel"].ToString();
            } catch (Exception autoEx) {App.ApplicationLogging(autoEx);}

            _ = LoadDataList();
            SetRecord(false);
        }


        public async Task<bool> LoadDataList()
        {
            List<HotelPropertyAndServiceList> hotelPropertyAndServiceList = new List<HotelPropertyAndServiceList>();
            MainWindow.ProgressRing = Visibility.Visible;
            try
            {
                hotelPropertyAndServiceList = await ApiCommunication.GetApiRequest<List<HotelPropertyAndServiceList>>(ApiUrls.HotelPropertyAndServiceList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                hotelList = await ApiCommunication.GetApiRequest<List<HotelList>>(ApiUrls.HotelList, null, App.UserData.Authentification.Token);
                currencyList = await ApiCommunication.GetApiRequest<List<CurrencyList>>(ApiUrls.CurrencyList, null, App.UserData.Authentification.Token);
                propertyOrServiceTypeList = await ApiCommunication.GetApiRequest<List<PropertyOrServiceTypeList>>(ApiUrls.PropertyOrServiceTypeList, null, App.UserData.Authentification.Token);
                propertyOrServiceUnitList = await ApiCommunication.GetApiRequest<List<PropertyOrServiceUnitList>>(ApiUrls.PropertyOrServiceUnitList, null, App.UserData.Authentification.Token);

                propertyOrServiceTypeList.ForEach(async property => { property.Translation = await DBOperations.DBTranslation(property.SystemName); });
                propertyOrServiceUnitList.ForEach(async propertyUnit => { propertyUnit.Translation = await DBOperations.DBTranslation(propertyUnit.SystemName); });
                hotelList.ForEach(hotel => { hotel.Currency = currencyList.First(a => a.Id == hotel.DefaultCurrencyId).Name; });

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin")
                {
                    cb_owner.ItemsSource = adminUserList = await ApiCommunication.GetApiRequest<List<UserList>>(ApiUrls.UserList, null, App.UserData.Authentification.Token);
                    lbl_owner.Visibility = cb_owner.Visibility = Visibility.Visible;
                }

                hotelPropertyAndServiceList.ForEach(async room => {
                    room.Accommodation = hotelList.First(a => a.Id == room.HotelId).Name;
                    room.PropertyOrService = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == room.PropertyOrServiceId).Translation;
                    room.IsSearchRequired = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == room.PropertyOrServiceId).IsSearchRequired;
                    room.IsService = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == room.PropertyOrServiceId).IsService;
                    room.Fee = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == room.PropertyOrServiceId).IsFeeInfoRequired;
                    room.PropertyUnit = await DBOperations.DBTranslation(propertyOrServiceUnitList.FirstOrDefault(a=>a.Id == propertyOrServiceTypeList.FirstOrDefault(b => b.Id == room.PropertyOrServiceId).PropertyOrServiceUnitTypeId).Translation);
                });
                DgListView.ItemsSource = hotelPropertyAndServiceList;
                DgListView.Items.Refresh();

                cb_hotelId.ItemsSource = hotelList;
                cb_propertyOrServiceId.ItemsSource = propertyOrServiceTypeList;
            }
            catch (Exception autoEx) {App.ApplicationLogging(autoEx);}
            MainWindow.ProgressRing = Visibility.Hidden;return true;
        }

        private void DgListView_Translate(object sender, EventArgs ex)
        {
            try { 
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "IsSearchRequired") { e.Header = Resources["searchRequired"].ToString(); e.DisplayIndex = 3; }
                    else if (headername == "Accommodation") { e.Header = Resources["accommodation"].ToString(); e.DisplayIndex = 1; }
                    else if (headername == "PropertyOrService") { e.Header = Resources["propertyOrService"].ToString(); e.DisplayIndex = 2; }
                    else if (headername == "IsService") { e.Header = Resources["service"].ToString(); e.DisplayIndex = 4; }
                    else if (headername == "IsAvailable") e.Header = Resources["isAvailable"].ToString();
                    else if (headername == "Fee") e.Header = Resources["fee"].ToString();
                    else if (headername == "PropertyUnit") e.Header = Resources["unit"].ToString();
                    else if (headername == "RoomsCount") e.Header = Resources["roomsCount"].ToString();
                    else if (headername == "ApproveRequest") e.Header = Resources["approveRequest"].ToString();
                    else if (headername == "Approved") e.Header = Resources["approved"].ToString();
                    else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }

                    else if (headername == "Id") e.DisplayIndex = 0;
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
            } catch (Exception autoEx) {App.ApplicationLogging(autoEx);}
        }

        public void Filter(string filter)
        {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    HotelPropertyAndServiceList user = e as HotelPropertyAndServiceList;
                    return user.Accommodation.ToLower().Contains(filter.ToLower())
                    || user.PropertyOrService.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) {App.ApplicationLogging(autoEx);}
        }


        public void NewRecord()
        {
            SetRecord(false);
        }


        public void EditRecord(bool copy)
        {
            selectedRecord = (HotelPropertyAndServiceList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, false);
        }


        public async void DeleteRecord()
        {
            SetRecord(false);
        }


        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (HotelPropertyAndServiceList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }


        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgListView.SelectedItems.Count > 0)
            { selectedRecord = (HotelPropertyAndServiceList)DgListView.SelectedItem; }
            else { selectedRecord = new HotelPropertyAndServiceList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }


        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.IsAvailable = (bool)chb_isAvailable.IsChecked;
                selectedRecord.Value = txt_value.Value;
                selectedRecord.ValueRangeMin = txt_valueRangeMin.Value;
                selectedRecord.ValueRangeMax = txt_valueRangeMax.Value;
                selectedRecord.Fee = (bool)chb_fee.IsChecked;
                selectedRecord.FeeValue = txt_feeValue.Value;
                selectedRecord.FeeRangeMin = txt_feeValueRangeMin.Value;
                selectedRecord.FeeRangeMax = txt_feeValueRangeMax.Value;

                selectedRecord.ApproveRequest = (bool)chb_approveRequest.IsChecked;
                selectedRecord.Approved = (bool)chb_approved.IsChecked;

                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.Timestamp = DateTimeOffset.Now.DateTime;


                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin")
                    selectedRecord.UserId = ((UserList)cb_owner.SelectedItem).Id;

                //nullable additional fields
                selectedRecord.Accommodation = selectedRecord.PropertyOrService = null;
                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0)
                {
                    dBResult = await ApiCommunication.PutApiRequest(ApiUrls.HotelPropertyAndServiceList, httpContent, null, App.UserData.Authentification.Token);
                }
                else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.HotelPropertyAndServiceList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.recordCount > 0)
                {
                    selectedRecord = new HotelPropertyAndServiceList();
                    await LoadDataList();
                    SetRecord(false);
                }
                else { await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage); }
            }
            catch (Exception autoEx) {App.ApplicationLogging(autoEx);}
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (HotelPropertyAndServiceList)DgListView.SelectedItem : new HotelPropertyAndServiceList();
            SetRecord(false);
        }


        private void SetRecord(bool showForm, bool copy = false)
        {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;
            if (txt_id.Value == 0) { showForm = false;}

            cb_hotelId.SelectedItem = hotelList.FirstOrDefault(a => a.Id == selectedRecord.HotelId);
            cb_propertyOrServiceId.Text = selectedRecord.PropertyOrService;
            chb_isAvailable.IsChecked = selectedRecord.IsAvailable;

            if (propertyOrServiceTypeList.Count > 0 && showForm)
            {
                lbl_propertyUnit.Content = selectedRecord.PropertyUnit;

                txt_value.IsEnabled = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == selectedRecord.PropertyOrServiceId).IsValue && !propertyOrServiceTypeList.FirstOrDefault(a => a.Id == selectedRecord.PropertyOrServiceId).IsRangeValue;
                txt_value.Value = selectedRecord.Value;

                txt_valueRangeMin.IsEnabled = txt_valueRangeMax.IsEnabled = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == selectedRecord.PropertyOrServiceId).IsRangeValue || propertyOrServiceTypeList.FirstOrDefault(a => a.Id == selectedRecord.PropertyOrServiceId).IsValueRangeAllowed;
                txt_valueRangeMin.Value = selectedRecord.ValueRangeMin;
                txt_valueRangeMax.Value = selectedRecord.ValueRangeMax;

                if (propertyOrServiceTypeList.FirstOrDefault(a => a.Id == selectedRecord.PropertyOrServiceId).IsFeeInfoRequired)
                { chb_fee.IsChecked = true; chb_fee.IsEnabled = false; }
                else { chb_fee.IsChecked = selectedRecord.Fee; }

                txt_feeValue.Value = selectedRecord.FeeValue;
                txt_feeValueRangeMin.IsEnabled = txt_feeValueRangeMax.IsEnabled = propertyOrServiceTypeList.FirstOrDefault(a => a.Id == selectedRecord.PropertyOrServiceId).IsFeeRangeAllowed;
                
                txt_feeValueRangeMin.Value = selectedRecord.FeeRangeMin;
                txt_feeValueRangeMax.Value = selectedRecord.FeeRangeMax;
            }

            chb_approveRequest.IsChecked = selectedRecord.ApproveRequest;
            chb_approved.IsChecked = false;

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


        private void FeeStatusClick(object sender, RoutedEventArgs e)
        {
            txt_feeValue.IsEnabled = txt_feeValueRangeMin.IsEnabled = txt_feeValueRangeMax.IsEnabled = (bool)chb_fee.IsChecked;
        }
    }
}