using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

    public partial class PropertyOrServiceTypeListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static PropertyOrServiceTypeList selectedRecord = new PropertyOrServiceTypeList();

        private List<PropertyGroupList> propertyGroupList = new List<PropertyGroupList>();
        private List<PropertyOrServiceTypeList> propertyOrServiceTypeList = new List<PropertyOrServiceTypeList>();
        private List<PropertyOrServiceUnitList> propertyOrServiceUnitList = new List<PropertyOrServiceUnitList>();

        public PropertyOrServiceTypeListPage() {
            dataViewSupport = new DataViewSupport();
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);

            try {
                lbl_id.Content = Resources["id"].ToString();
                lbl_systemName.Content = Resources["systemName"].ToString();
                lbl_sequence.Content = Resources["sequence"].ToString();
                lbl_propertyGroup.Content = Resources["propertyGroup"].ToString();
                lbl_unit.Content = Resources["unit"].ToString();
                gb_valueType.Header = Resources["parameterType"].ToString();

                lbl_service.Content = Resources["service"].ToString();
                lbl_searchRequired.Content = Resources["searchRequired"].ToString();
                lbl_isBit.Content = Resources["isBit"].ToString();
                lbl_isValue.Content = Resources["isValue"].ToString();
                lbl_isValueRangeAllowed.Content = Resources["isValueRangeAllowed"].ToString();
                lbl_isRangeValue.Content = Resources["isRangeValue"].ToString();
                lbl_isRangeTime.Content = Resources["isRangeTime"].ToString();

                gb_searchEngine.Header = Resources["searchEngine"].ToString();
                lbl_defaultBit.Content = Resources["defaultBit"].ToString();
                lbl_defaultValue.Content = Resources["defaultValue"].ToString();
                lbl_defaultMin.Content = Resources["defaultMin"].ToString();
                lbl_defaultMax.Content = Resources["defaultMax"].ToString();
                lbl_feeInfoRequired.Content = Resources["feeInfoRequired"].ToString();
                lbl_feeRangeAllowed.Content = Resources["feeRangeAllowed"].ToString();

                btn_save.Content = Resources["btn_save"].ToString();
                btn_cancel.Content = Resources["btn_cancel"].ToString();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            _ = LoadDataList();
            SetRecord(false);
        }

        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                propertyGroupList = await CommApi.GetApiRequest<List<PropertyGroupList>>(ApiUrls.PropertyGroupList, null, App.UserData.Authentification.Token);
                propertyOrServiceTypeList = await CommApi.GetApiRequest<List<PropertyOrServiceTypeList>>(ApiUrls.PropertyOrServiceTypeList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                propertyOrServiceUnitList = await CommApi.GetApiRequest<List<PropertyOrServiceUnitList>>(ApiUrls.PropertyOrServiceUnitList, null, App.UserData.Authentification.Token);

                propertyGroupList.ForEach(async item => { item.Translation = await DBOperations.DBTranslation(item.SystemName); });
                propertyOrServiceUnitList.ForEach(async item => { item.Translation = await DBOperations.DBTranslation(item.SystemName); });

                propertyOrServiceTypeList.ForEach(async item => {
                    item.Translation = await DBOperations.DBTranslation(item.SystemName);
                    item.PropertyGroupTranslation = propertyGroupList.Where(a => a.Id == item.PropertyGroupId).Select(a => a.Translation).FirstOrDefault();
                    item.PropertyOrServiceUnitType = propertyOrServiceUnitList.Where(a => a.Id == item.PropertyOrServiceUnitTypeId).Select(a => a.Translation).FirstOrDefault();
                });
                DgListView.ItemsSource = propertyOrServiceTypeList;
                DgListView.Items.Refresh();

                
                cb_propertyGroup.ItemsSource = propertyGroupList;
                cb_unit.ItemsSource = propertyOrServiceUnitList;
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        private void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "PropertyGroupTranslation") { e.Header = Resources["propertyGroup"].ToString(); e.DisplayIndex = 1; }
                    else if (headername == "SystemName") { e.Header = Resources["systemName"].ToString(); e.DisplayIndex = 2; }
                    else if (headername == "Sequence") { e.Header = Resources["sequence"].ToString(); e.DisplayIndex = 3; }
                    else if (headername == "Translation") { e.Header = Resources["translation"].ToString(); e.DisplayIndex = 4; } 
                    else if (headername == "PropertyOrServiceUnitType") { e.Header = Resources["unit"].ToString(); e.DisplayIndex = 5; } 
                    else if (headername == "IsSearchRequired") { e.Header = Resources["searchRequired"].ToString(); e.DisplayIndex = 6; } 
                    else if (headername == "IsService") { e.Header = Resources["service"].ToString(); e.DisplayIndex = 7; } 
                    else if (headername == "SearchDefaultBit") { e.Header = Resources["defaultBit"].ToString(); e.DisplayIndex = 8; } 
                    else if (headername == "IsFeeInfoRequired") { e.Header = Resources["feeInfoRequired"].ToString(); e.DisplayIndex = 9; }
                    else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; } 
                    
                    else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "PropertyGroupId") e.Visibility = Visibility.Hidden;
                    else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                    else if (headername == "IsBit") e.Visibility = Visibility.Hidden;
                    else if (headername == "IsValue") e.Visibility = Visibility.Hidden;
                    else if (headername == "IsRangeTime") e.Visibility = Visibility.Hidden;
                    else if (headername == "IsRangeValue") e.Visibility = Visibility.Hidden;
                    else if (headername == "IsValueRangeAllowed") e.Visibility = Visibility.Hidden;
                    else if (headername == "SearchDefaultBit") e.Visibility = Visibility.Hidden;
                    else if (headername == "SearchDefaultValue") e.Visibility = Visibility.Hidden;
                    else if (headername == "SearchDefaultMin") e.Visibility = Visibility.Hidden;
                    else if (headername == "SearchDefaultMax") e.Visibility = Visibility.Hidden;
                    else if (headername == "IsFeeRangeAllowed") e.Visibility = Visibility.Hidden;
                    else if (headername == "PropertyOrServiceUnitTypeId") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    PropertyOrServiceTypeList user = e as PropertyOrServiceTypeList;
                    return user.SystemName.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.PropertyGroupTranslation) && user.PropertyGroupTranslation.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.Translation) && user.Translation.ToLower().Contains(filter.ToLower());
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void NewRecord() {
            selectedRecord = new PropertyOrServiceTypeList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (PropertyOrServiceTypeList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (PropertyOrServiceTypeList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await CommApi.DeleteApiRequest(ApiUrls.PropertyOrServiceTypeList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (PropertyOrServiceTypeList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (PropertyOrServiceTypeList)DgListView.SelectedItem; } else { selectedRecord = new PropertyOrServiceTypeList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.SystemName = txt_systemName.Text;
                selectedRecord.Sequence = int.Parse(txt_sequence.Value.ToString());
                selectedRecord.Timestamp = DateTimeOffset.Now.DateTime;

                selectedRecord.PropertyGroupId = (cb_propertyGroup.SelectedItem != null) ? (int?)((PropertyGroupList)cb_propertyGroup.SelectedItem).Id : null;
                selectedRecord.PropertyOrServiceUnitTypeId = (cb_unit.SelectedItem != null) ? ((PropertyOrServiceUnitList)cb_unit.SelectedItem).Id : 0;
                selectedRecord.IsService = (bool)chb_service.IsChecked;
                selectedRecord.IsSearchRequired = (bool)chb_searchRequired.IsChecked;

                //type Part
                selectedRecord.IsBit = (bool)chb_isBit.IsChecked;
                selectedRecord.IsValue = (bool)chb_isValue.IsChecked;

                //Range part
                selectedRecord.IsValueRangeAllowed = (bool)chb_isValueRangeAllowed.IsChecked;
                if (selectedRecord.IsValueRangeAllowed) {
                    selectedRecord.IsRangeValue = (bool)chb_isRangeValue.IsChecked;
                    selectedRecord.IsRangeTime = (bool)chb_isRangeTime.IsChecked;
                } else { selectedRecord.IsRangeValue = selectedRecord.IsRangeTime = false; }

                //Search Part
                if (selectedRecord.IsBit) { selectedRecord.SearchDefaultBit = (bool)chb_defaultBit.IsChecked; selectedRecord.SearchDefaultValue = null; selectedRecord.SearchDefaultMin = selectedRecord.SearchDefaultMax = null; } else {
                    selectedRecord.SearchDefaultBit = false;
                    selectedRecord.SearchDefaultValue = (string.IsNullOrWhiteSpace(txt_defaultValue.Text)) ? txt_defaultValue.Text : null;
                    selectedRecord.SearchDefaultMin = (int?)(txt_defaultMin.Value != null ? (int)txt_defaultMin.Value : txt_defaultMin.Value);
                    selectedRecord.SearchDefaultMax = (int?)(txt_defaultMax.Value != null ? (int)txt_defaultMax.Value : txt_defaultMax.Value);
                }

                //Fee part
                selectedRecord.IsFeeInfoRequired = (bool)chb_feeInfoRequired.IsChecked;
                selectedRecord.IsFeeRangeAllowed = (bool)chb_feeRangeAllowed.IsChecked;

                selectedRecord.PropertyOrServiceUnitType = null;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await CommApi.PutApiRequest(ApiUrls.PropertyOrServiceTypeList, httpContent, null, App.UserData.Authentification.Token);
                } else { dBResult = await CommApi.PostApiRequest(ApiUrls.PropertyOrServiceTypeList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.RecordCount > 0) {
                    selectedRecord = new PropertyOrServiceTypeList();
                    await LoadDataList();
                    SetRecord(false);
                } else { await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (PropertyOrServiceTypeList)DgListView.SelectedItem : new PropertyOrServiceTypeList();
            SetRecord(false);
        }

        private void SetRecord(bool showForm, bool copy = false) {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;
            txt_systemName.Text = selectedRecord.SystemName;
            txt_sequence.Value = (txt_id.Value == 0) ? propertyGroupList.Any() ? propertyGroupList.Max(a => a.Sequence) + 10 : 10 : selectedRecord.Sequence;
            lbl_translation.Content = selectedRecord.Translation;

            cb_propertyGroup.SelectedItem = selectedRecord.PropertyGroupId == null ? null : propertyGroupList.Where(a=> a.Id == selectedRecord.PropertyGroupId).FirstOrDefault();
            cb_unit.SelectedItem = propertyOrServiceUnitList.Where(a => a.Id == selectedRecord.PropertyOrServiceUnitTypeId).FirstOrDefault();
            chb_service.IsChecked = selectedRecord.IsService;
            chb_searchRequired.IsChecked = selectedRecord.IsSearchRequired;

            //type Part
            chb_isBit.IsChecked = selectedRecord.IsBit;
            chb_isValue.IsChecked = selectedRecord.IsValue;

            //Rande part
            chb_isValueRangeAllowed.IsChecked = selectedRecord.IsValueRangeAllowed;
            chb_isRangeValue.IsChecked = selectedRecord.IsRangeValue;
            chb_isRangeTime.IsChecked = selectedRecord.IsRangeTime;

            //Search Part
            chb_defaultBit.IsChecked = selectedRecord.SearchDefaultBit;
            txt_defaultValue.Text = selectedRecord.SearchDefaultValue;
            txt_defaultMin.Value = selectedRecord.SearchDefaultMin;
            txt_defaultMax.Value = selectedRecord.SearchDefaultMax;

            //Fee part
            chb_feeInfoRequired.IsChecked = selectedRecord.IsFeeInfoRequired;
            chb_feeRangeAllowed.IsChecked = selectedRecord.IsFeeRangeAllowed;

            if (showForm) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            } else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
            }
        }

        private void SearchCheckedClick(object sender, RoutedEventArgs e) {
            gb_searchEngine.IsEnabled = (bool)chb_searchRequired.IsChecked;
            if (gb_searchEngine.IsEnabled) { chb_isBit.IsChecked = chb_defaultBit.IsChecked = true; }
        }

        private void ValueRangeAllowedClick(object sender, RoutedEventArgs e) {
            chb_isRangeValue.IsEnabled = chb_isRangeTime.IsEnabled = (bool)(chb_isRangeValue.IsChecked = (bool)chb_isValueRangeAllowed.IsChecked);
            if (!(bool)chb_isValueRangeAllowed.IsChecked) { chb_isRangeValue.IsChecked = chb_isRangeTime.IsChecked = false; }
        }

        private void IsBitClick(object sender, RoutedEventArgs e) {
            if (dataViewSupport.FormShown) {
                txt_defaultValue.IsEnabled = chb_isValueRangeAllowed.IsEnabled = txt_defaultMin.IsEnabled = txt_defaultMax.IsEnabled = !(bool)chb_isBit.IsChecked;
                chb_defaultBit.IsEnabled = !txt_defaultValue.IsEnabled;
                chb_isRangeValue.IsEnabled = chb_isRangeTime.IsEnabled = (chb_isValueRangeAllowed.IsEnabled && (bool)chb_isValueRangeAllowed.IsChecked);
            }
        }
    }
}