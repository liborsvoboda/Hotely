using ICSharpCode.AvalonEdit.Highlighting;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UbytkacAdmin.Api;
using UbytkacAdmin.Classes;
using UbytkacAdmin.GlobalOperations;
using UbytkacAdmin.GlobalStyles;


namespace UbytkacAdmin.Pages {

    public partial class WebMottoListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static WebMottoList selectedRecord = new WebMottoList();

        private List<WebMottoList> WebMottoList = new List<WebMottoList>();
        private List<CodeLibraryList> codeLibraryList = new List<CodeLibraryList>();


        public WebMottoListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);

            try {
                lbl_id.Content = Resources["id"].ToString();
                lbl_name.Content = Resources["fname"].ToString();
                lbl_active.Content = Resources["active"].ToString();

                btn_loadFromFile.Content = Resources["loadFromFile"].ToString();

                btn_save.Content = Resources["btn_save"].ToString();
                btn_saveClose.Content = Resources["saveClose"].ToString();
                btn_cancel.Content = Resources["btn_cancel"].ToString();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            _ = LoadDataList();
            SetRecord(false);
        }

        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                codeLibraryList = await CommApi.GetApiRequest<List<CodeLibraryList>>(ApiUrls.CodeLibraryList, null, App.UserData.Authentification.Token);
                DgListView.ItemsSource = WebMottoList = await CommApi.GetApiRequest<List<WebMottoList>>(ApiUrls.WebMottoList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);

                lb_dataList.ItemsSource = codeLibraryList;

            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }


            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        private void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "Name") { e.Header = Resources["fname"].ToString(); e.DisplayIndex = 1; }
                    else if (headername == "Active") { e.Header = Resources["active"].ToString(); e.DisplayIndex = 2; }

                    else if (headername == "TimeStamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }

                    else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "HtmlContent") e.Visibility = Visibility.Hidden;
                    else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    WebMottoList user = e as WebMottoList;
                    return user.Name.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.HtmlContent) && user.HtmlContent.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void NewRecord() {
            selectedRecord = new WebMottoList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (WebMottoList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (WebMottoList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await CommApi.DeleteApiRequest(ApiUrls.WebMottoList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (WebMottoList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (WebMottoList)DgListView.SelectedItem; } else { selectedRecord = new WebMottoList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        private void SetRecord(bool showForm, bool copy = false) {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;
            txt_name.Text = selectedRecord.Name;

            //html_htmlContent.HtmlContent = selectedRecord.DescriptionCz;
            html_htmlContent.Browser.OpenDocument(selectedRecord.HtmlContent);
            if ((bool)EditorSelector.IsChecked) { html_htmlContent.Browser.OpenDocument(selectedRecord.HtmlContent); }
            else { txt_codeContent.Text = selectedRecord.HtmlContent; }

            chb_active.IsChecked = selectedRecord.Active;


            if (showForm) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            } else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e) => await SaveRecord(false, false);
        private async void BtnSaveClose_Click(object sender, RoutedEventArgs e) => await SaveRecord(true, false);
        private async void BtnSaveAsNew_Click(object sender, RoutedEventArgs e) => await SaveRecord(false, true);

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (WebMottoList)DgListView.SelectedItem : new WebMottoList();
            SetRecord(false);
        }

        private void BtnLoadFromFile_Click(object sender, RoutedEventArgs e) {
            try {
                OpenFileDialog dlg = new OpenFileDialog() { DefaultExt = ".html", Filter = "Html files |*.html|All files (*.*)|*.*", Title = Resources["fileOpenDescription"].ToString() };
                if (dlg.ShowDialog() == true) {
                    if ((bool)EditorSelector.IsChecked) { html_htmlContent.Browser.OpenDocument(File.ReadAllText(dlg.FileName, FileOperations.FileDetectEncoding(dlg.FileName))); }
                    else { txt_codeContent.Text = File.ReadAllText(dlg.FileName, FileOperations.FileDetectEncoding(dlg.FileName)); }
                }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private async Task<bool> SaveRecord(bool closeForm, bool asNew) {
            try {
                MainWindow.ProgressRing = Visibility.Visible;

                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.Name = txt_name.Text;

                if ((bool)EditorSelector.IsChecked) { selectedRecord.HtmlContent = html_htmlContent.Browser.GetCurrentHtml(); }
                else { selectedRecord.HtmlContent = txt_codeContent.Text; }

                selectedRecord.Active = (bool)chb_active.IsChecked;
                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.TimeStamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await CommApi.PutApiRequest(ApiUrls.WebMottoList, httpContent, null, App.UserData.Authentification.Token);
                }
                else { dBResult = await CommApi.PostApiRequest(ApiUrls.WebMottoList, httpContent, null, App.UserData.Authentification.Token); }

                if (closeForm) { await LoadDataList(); selectedRecord = new WebMottoList(); SetRecord(false); }
                if (dBResult.RecordCount == 0) { await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden;
            return true;
        }


        //Include Template on end in Editor
        private void DataListDoubleClick(object sender, MouseButtonEventArgs e) {
            if (lb_dataList.SelectedItems.Count > 0) {
                if ((bool)EditorSelector.IsChecked) { html_htmlContent.HtmlContent += ((CodeLibraryList)lb_dataList.SelectedItem).HtmlContent; }
                else { txt_codeContent.Text += ((CodeLibraryList)lb_dataList.SelectedItem).HtmlContent; }

            }
        }


        private void HighlightCodeChanged(object sender, SelectionChangedEventArgs e) {
            txt_codeContent.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(((ListBoxItem)code_selector.SelectedValue).Content.ToString());
        }

        private void EditorSelectorStatus(object sender, RoutedEventArgs e) {
            if ((bool)EditorSelector.IsChecked) {
                html_htmlContent.Browser.OpenDocument(selectedRecord.HtmlContent);
                html_htmlContent.Visibility = Visibility.Visible;
                txt_codeContent.Visibility = Visibility.Hidden;
                code_selector.Visibility = Visibility.Hidden;
            }
            else {
                code_selector.Visibility = Visibility.Visible;
                txt_codeContent.Text = selectedRecord.HtmlContent;
                txt_codeContent.Visibility = Visibility.Visible;
                html_htmlContent.Visibility = Visibility.Hidden;
            }
        }
    }
}