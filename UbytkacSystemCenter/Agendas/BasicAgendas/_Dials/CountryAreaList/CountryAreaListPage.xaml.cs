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
using EasyITSystemCenter.GlobalClasses;

namespace EasyITSystemCenter.Pages {

    public partial class CountryAreaListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static CountryAreaList selectedRecord = new CountryAreaList();

        private List<CountryAreaList> CountryAreaList = new List<CountryAreaList>();
        List<CountryAreaCityList> countryAreaCityList = new List<CountryAreaCityList>();
        private List<CountryList> countryList = new List<CountryList>();
        private List<CityList> cityList = new List<CityList>();

        public CountryAreaListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);

            //translate fields in detail form
            try {
                _ = FormOperations.TranslateFormFields(ListForm);
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            _ = LoadDataList();
            SetRecord(false);
        }

        //change datasource
        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                CountryAreaList = await CommunicationManager.GetApiRequest<List<CountryAreaList>>(ApiUrls.CountryAreaList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                CountryAreaList.ForEach(async area => { area.AreaTranslation = await DBOperations.DBTranslation(area.SystemName); });

                countryList = await CommunicationManager.GetApiRequest<List<CountryList>>(ApiUrls.CountryList, null, App.UserData.Authentification.Token);
                countryList.ForEach(async country => { country.CountryTranslation = await DBOperations.DBTranslation(country.SystemName); });

                CountryAreaList.ForEach(area => {
                    area.CountryTranslation = countryList.FirstOrDefault(a => a.Id == area.CountryId).CountryTranslation;
                });
                

                DgListView.ItemsSource = CountryAreaList;
                DgListView.Items.Refresh();

                cb_country.ItemsSource = countryList;

            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            //MainWindow.ProgressRing = Visibility.Hidden; 
            return true;
        }

        // set translate columns in listView
        private async void DgListView_Translate(object sender, EventArgs ex) {
            ((DataGrid)sender).Columns.ToList().ForEach(async e => {
                string headername = e.Header.ToString().ToLower();
                if (headername == "SystemName".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 1; } 
                else if (headername == "AreaTranslation".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 2; }
                else if (headername == "CountryTranslation".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 3; }
                else if (headername == "Description".ToLower()) e.Header = await DBOperations.DBTranslation(headername);
                else if (headername == "TimeStamp".ToLower()) { e.Header = await DBOperations.DBTranslation(headername); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; } 
                else if (headername == "Id".ToLower()) e.DisplayIndex = 0;
                else if (headername == "UserId".ToLower()) e.Visibility = Visibility.Hidden;
                else if (headername == "CountryId".ToLower()) e.Visibility = Visibility.Hidden;
            });
        }

        //change filter fields
        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    DataRowView search = e as DataRowView;
                    return search.ObjectToJson().ToLower().Contains(filter.ToLower());
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }


        public void NewRecord() {
            selectedRecord = new CountryAreaList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (CountryAreaList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (CountryAreaList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await CommunicationManager.DeleteApiRequest(ApiUrls.CountryAreaList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage);
                _ = LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (CountryAreaList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) {
                selectedRecord = (CountryAreaList)DgListView.SelectedItem;
            } else { selectedRecord = new CountryAreaList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        //change dataset save method
        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.SystemName = txt_systemName.Text;
                selectedRecord.Description = tb_description.Text;

                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.TimeStamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await CommunicationManager.PutApiRequest(ApiUrls.CountryAreaList, httpContent, null, App.UserData.Authentification.Token);
                } else { dBResult = await CommunicationManager.PostApiRequest(ApiUrls.CountryAreaList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.RecordCount > 0) {

                    //SaveItem
                    countryAreaCityList.ForEach(item => { item.Id = 0; item.Icacid = dBResult.InsertedId; item.UserId = App.UserData.Authentification.Id; });
                    dBResult = await CommunicationManager.DeleteApiRequest(ApiUrls.CountryAreaCityList, dBResult.InsertedId.ToString(), App.UserData.Authentification.Token);
                    json = JsonConvert.SerializeObject(countryAreaCityList); httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    dBResult = await CommunicationManager.PutApiRequest(ApiUrls.CountryAreaCityList, httpContent, null, App.UserData.Authentification.Token);
                    if (dBResult.RecordCount != countryAreaCityList.Count()) {
                        await MainWindow.ShowMessageOnMainWindow(true, Resources["itemsDBError"].ToString() + Environment.NewLine + dBResult.ErrorMessage);
                    }
                    else {
                        selectedRecord = new CountryAreaList();
                        await LoadDataList();
                        SetRecord(false);
                    }

                } else { await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (CountryAreaList)DgListView.SelectedItem : new CountryAreaList();
            SetRecord(false);
        }

        //change dataset prepare for working
        private async void SetRecord(bool showForm, bool copy = false) {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;
            txt_systemName.Text = selectedRecord.SystemName;
            cb_country.SelectedItem = txt_id.Value == 0 ? null : countryList.FirstOrDefault(x => x.Id == selectedRecord.CountryId);
            tb_description.Text = selectedRecord.Description;


            if (showForm) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
                SubListView.Visibility = Visibility.Visible; await LoadSubDataList();
            } else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
            }
        }

        //change subdatasource
        public async Task<bool> LoadSubDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                countryAreaCityList = await CommunicationManager.GetApiRequest<List<CountryAreaCityList>>(ApiUrls.CountryAreaCityList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                foreach (CountryAreaCityList item in countryAreaCityList) {
                    item.CityTranslation = await DBOperations.DBTranslation((await CommunicationManager.GetApiRequest<CityList>(ApiUrls.CityList, item.CityId.ToString(), App.UserData.Authentification.Token)).City, true);
                }
                
                DgSubListView.ItemsSource = countryAreaCityList;
                DgSubListView.Items.Refresh();

                if (countryAreaCityList.Count > 0) {
                    cb_country.SelectedItem = countryList.FirstOrDefault(a => a.Id == selectedRecord.CountryId);
                    cb_country.IsEnabled = false;
                }
                else { cb_country.IsEnabled = true; }
            } 
            catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden;
            return true;
        }

        private void DgSubListView_Translate(object sender, EventArgs ex) {
            ((DataGrid)sender).Columns.ToList().ForEach(e => {
                string headername = e.Header.ToString();
                if (headername == "CityTranslation") e.Header = Resources["city"].ToString();
                
                else if (headername == "Id") e.Visibility = Visibility.Hidden;
                else if (headername == "Icacid") e.Visibility = Visibility.Hidden;
                else if (headername == "CityId") e.Visibility = Visibility.Hidden;
                else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                else if (headername == "Timestamp") e.Visibility = Visibility.Hidden;
            });
        }

        private async void CountrySelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cb_country.SelectedItem != null) {
                MainWindow.ProgressRing = Visibility.Visible;
                cb_city.SelectedItem = null;
                cityList = await CommunicationManager.GetApiRequest<List<CityList>>(ApiUrls.CityList, "ByCountry/" + ((CountryList)cb_country.SelectedItem).Id.ToString(), App.UserData.Authentification.Token); ;

                cityList.ForEach(async city => {
                    city.CityTranslation = await DBOperations.DBTranslation(city.City, true);
                    city.CountryTranslation = await DBOperations.DBTranslation(countryList.FirstOrDefault(a => a.Id == city.CountryId).SystemName);
                });
                cb_city.ItemsSource = cityList;
                selectedRecord.CountryId = ((CountryList)cb_country.SelectedItem).Id;
                MainWindow.ProgressRing = Visibility.Hidden;
            }
        }

        private async void BtnSaveCity_Click(object sender, RoutedEventArgs e) {
            CountryAreaCityList countryAreaCity = new CountryAreaCityList() {
                CityId = ((CityList)cb_city.SelectedItem).Id,
                CityTranslation = ((CityList)cb_city.SelectedItem).CityTranslation,
                UserId = App.UserData.Authentification.Id,
                Timestamp = DateTimeOffset.Now.DateTime
            };
            if (countryAreaCityList.Where(a => a.CityId == ((CityList)cb_city.SelectedItem).Id).Any()) {
                await MainWindow.ShowMessageOnMainWindow(false, Resources["itemAlreadyExists"].ToString());
            } else { countryAreaCityList.Add(countryAreaCity); }
            DgSubListView.Items.Refresh();
            if (countryAreaCityList.Count > 0 ) { cb_country.IsEnabled = false; } else { cb_country.IsEnabled = true; }
        }

        private void BtnDeleteCity_Click(object sender, RoutedEventArgs e) {
            countryAreaCityList.RemoveAt(DgSubListView.SelectedIndex);
            DgSubListView.Items.Refresh();
            btn_deleteCity.IsEnabled = false;
            if (countryAreaCityList.Count > 0) { cb_country.IsEnabled = false; } else { cb_country.IsEnabled = true; }
        }

        private void CitySelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cb_city.SelectedItem != null) {
                btn_saveCity.IsEnabled = true;
            } else { btn_saveCity.IsEnabled = false;}
        }

        private void DgSubListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgSubListView.SelectedItems.Count > 0) {
                btn_deleteCity.IsEnabled = true;
            } else { btn_deleteCity.IsEnabled = false; }
        }
    }
}