using UbytkacAdmin.Api;
using UbytkacAdmin.Classes;
using UbytkacAdmin.GlobalClasses;
using UbytkacAdmin.GlobalOperations;
using UbytkacAdmin.GlobalStyles;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace UbytkacAdmin.Pages {

    public partial class SystemReportListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static SystemReportList selectedRecord = new SystemReportList();

        private List<SpTableList> systemTableList = new List<SpTableList>();
        private List<SystemTranslatedTableList> systemTranslatedTableList = new List<SystemTranslatedTableList>();
        private List<SystemReportList> reportList = new List<SystemReportList>();
        private bool reportSupportForListOnly = true;

        public SystemReportListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);

            try {
                _ = DataOperations.TranslateFormFields(ListForm);

                LoadParameters();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            _ = LoadDataList();
            SetRecord(false);
        }

        private async void LoadParameters() {
            reportSupportForListOnly = bool.Parse(await DataOperations.ParameterCheck("ReportSupportForListOnly"));
        }

        //change datasource
        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                systemTableList = await CommApi.GetApiRequest<List<SpTableList>>(ApiUrls.StoredProceduresList, "SystemSpGetTableList", App.UserData.Authentification.Token);
                reportList = await CommApi.GetApiRequest<List<SystemReportList>>(ApiUrls.SystemReportList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);

                systemTranslatedTableList.Clear();
                systemTableList.ForEach(async table => {
                    systemTranslatedTableList.Add(new SystemTranslatedTableList() { TableName = table.TableList, Translate = await DBOperations.DBTranslation(table.TableList) });
                });

                cb_pageName.ItemsSource = systemTranslatedTableList.OrderBy(a => a.Translate);

                reportList.ForEach(async report => {
                    report.Translation = await DBOperations.DBTranslation(report.SystemName);
                    report.PageTranslation = systemTranslatedTableList.FirstOrDefault(a => a.TableName == report.PageName).Translate;
                });

                DgListView.ItemsSource = reportList;
                DgListView.Items.Refresh();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        // set translate columns in listView
        private void DgListView_Translate(object sender, EventArgs ex) {
            ((DataGrid)sender).Columns.ToList().ForEach(e => {
                string headername = e.Header.ToString();
                if (headername == "SystemName") { e.Header = Resources["systemName"].ToString(); e.DisplayIndex = 1; }
                else if (headername == "Translation") { e.Header = Resources["translation"].ToString(); e.DisplayIndex = 2; }
                else if (headername == "PageTranslation") { e.Header = Resources["tableName"].ToString(); e.DisplayIndex = 3; }
                else if (headername == "JoinedId") e.Header = Resources["joinedId"].ToString();
                else if (headername == "Description") e.Header = Resources["description"].ToString();
                else if (headername == "Default") { e.Header = Resources["default"].ToString(); e.DisplayIndex = DgListView.Columns.Count - 3; }
                else if (headername == "Active") { e.Header = Resources["active"].ToString(); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 2; }
                else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }
                else if (headername == "Id") e.DisplayIndex = 0;
                else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                else if (headername == "ReportPath") e.Visibility = Visibility.Hidden;
                else if (headername == "File") e.Visibility = Visibility.Hidden;
                else if (headername == "MimeType") e.Visibility = Visibility.Hidden;
                else if (headername == "PageName") e.Visibility = Visibility.Hidden;
            });
        }

        //change filter fields
        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    SystemReportList report = e as SystemReportList;
                    return report.PageName.ToLower().Contains(filter.ToLower())
                    || report.SystemName.ToLower().Contains(filter.ToLower())
                    || report.PageTranslation.ToLower().Contains(filter.ToLower())
                    || report.Translation.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(report.Description) && report.Description.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void NewRecord() {
            selectedRecord = new SystemReportList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (SystemReportList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (SystemReportList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await CommApi.DeleteApiRequest(ApiUrls.SystemReportList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + dBResult.ErrorMessage);
                _ = LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (SystemReportList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) {
                selectedRecord = (SystemReportList)DgListView.SelectedItem;
            }
            else { selectedRecord = new SystemReportList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        //change dataset save method
        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.PageName = ((SystemTranslatedTableList)cb_pageName.SelectedItem).TableName;
                selectedRecord.SystemName = txt_systemName.Text;
                selectedRecord.JoinedId = (bool)chb_joinedId.IsChecked;
                selectedRecord.Description = txt_description.Text;
                selectedRecord.Default = (bool)chb_default.IsChecked;

                if (!string.IsNullOrWhiteSpace(txt_reportPath.Text)) {
                    selectedRecord.ReportPath = txt_reportPath.Text;
                    selectedRecord.MimeType = MimeMapping.GetMimeMapping(txt_reportPath.Text);
                    selectedRecord.File = System.IO.File.ReadAllBytes(txt_reportPath.Text);
                }

                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.Timestamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await CommApi.PutApiRequest(ApiUrls.SystemReportList, httpContent, null, App.UserData.Authentification.Token);
                }
                else { dBResult = await CommApi.PostApiRequest(ApiUrls.SystemReportList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.RecordCount > 0) {
                    selectedRecord = new SystemReportList();
                    await LoadDataList();
                    SetRecord(null);
                }
                else { await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (SystemReportList)DgListView.SelectedItem : new SystemReportList();
            SetRecord(false);
        }

        //change dataset prepare for working
        private void SetRecord(bool? showForm = null, bool copy = false) {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;

            cb_pageName.Text = (selectedRecord.Id == 0) ? null : selectedRecord.PageTranslation;
            txt_systemName.Text = selectedRecord.SystemName;

            chb_joinedId.IsChecked = selectedRecord.JoinedId;
            txt_reportPath.Text = null;
            txt_description.Text = selectedRecord.Description;
            chb_default.IsChecked = selectedRecord.Default;
            dp_timestamp.Value = selectedRecord.Timestamp;

            if (showForm != null && showForm == true) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            }
            else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = showForm == null && !bool.Parse(App.appRuntimeData.AppClientSettings.First(a => a.Key == "beh_CloseFormAfterSave").Value);
            }
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e) {
            try {
                OpenFileDialog dlg = new OpenFileDialog {
                    DefaultExt = ".rdl",
                    Filter = "Report files |*.rdl",
                    Title = Resources["fileOpenDescription"].ToString()
                };
                if (dlg.ShowDialog() == true) {
                    txt_reportPath.Text = dlg.FileName;
                    selectedRecord.MimeType = MimeMapping.GetMimeMapping(dlg.FileName);
                    selectedRecord.File = System.IO.File.ReadAllBytes(dlg.FileName);
                }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e) {
            try {
                SaveFileDialog dlg = new SaveFileDialog { DefaultExt = ".rdl", Filter = "Report files |*.rdl", Title = Resources["fileOpenDescription"].ToString() };
                if (dlg.ShowDialog() == true) { FileOperations.ByteArrayToFile(dlg.FileName, selectedRecord.File); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }
    }
}