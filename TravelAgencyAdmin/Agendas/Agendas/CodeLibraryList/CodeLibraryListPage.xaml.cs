using UbytkacAdmin.Api;
using UbytkacAdmin.Classes;
using UbytkacAdmin.GlobalOperations;
using UbytkacAdmin.GlobalStyles;
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
using Microsoft.Win32;
using System.IO;
using EASYTools.HTMLFullEditor.Code;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit;

namespace UbytkacAdmin.Pages {

    public partial class CodeLibraryListPage : UserControl {

        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static CodeLibraryList selectedRecord = new CodeLibraryList();

        private List<CodeLibraryList> CodeLibraryList = new List<CodeLibraryList>();

        public CodeLibraryListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            try {
                lbl_id.Content = Resources["id"].ToString();
                lbl_name.Content = Resources["fname"].ToString();

                //btn_openInBrowser.Content = Resources["openInBrowser"].ToString();
                btn_loadFromFile.Content = Resources["loadFromFile"].ToString();

                lb_idColumn.Header = Resources["id"].ToString();
                lb_nameColumn.Header = Resources["fname"].ToString();

                btn_saveAsNew.Content = Resources["saveAsNew"].ToString();
                btn_save.Content = Resources["btn_save"].ToString();
                btn_saveClose.Content = Resources["saveClose"].ToString();
                btn_cancel.Content = Resources["btn_cancel"].ToString();



                html_htmlContent.HtmlContentDisableInitialChange = true;
                html_htmlContent.Toolbar.SetSourceMode(true); 
                html_htmlContent.Browser.ToggleSourceEditor(html_htmlContent.Toolbar, true);
  
                LoadParameters();

               


            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }



            _ = LoadDataList();
            SetRecord(false);
        }

        private async void LoadParameters() {
            DgListView.RowHeight = int.Parse(await DataOperations.ParameterCheck("WebAgendasFormsRowHeight"));
           
        }

        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {

                CodeLibraryList = await ApiCommunication.GetApiRequest<List<CodeLibraryList>>(ApiUrls.CodeLibraryList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);

                DgListView.ItemsSource = CodeLibraryList;
                lb_dataList.ItemsSource = CodeLibraryList;
                DgListView.Items.Refresh();

            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }


        private void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "Name") { e.Header = Resources["fname"].ToString(); e.DisplayIndex = 1; }
                    else if (headername == "Description") e.Header = Resources["description"].ToString();
                    else if (headername == "TimeStamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }

                    else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                    else if (headername == "HtmlContent") e.Visibility = Visibility.Hidden;

                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }


        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    CodeLibraryList user = e as CodeLibraryList;
                    return user.Name.ToLower().Contains(filter.ToLower())
                    || user.HtmlContent.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }


        public void NewRecord() {
            selectedRecord = new CodeLibraryList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }


        public void EditRecord(bool copy) {
            selectedRecord = (CodeLibraryList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }


        public async void DeleteRecord() {
            selectedRecord = (CodeLibraryList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await ApiCommunication.DeleteApiRequest(ApiUrls.CodeLibraryList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }


        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (CodeLibraryList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }


        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (CodeLibraryList)DgListView.SelectedItem; }
            else { selectedRecord = new CodeLibraryList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;

        }


        private async void BtnSave_Click(object sender, RoutedEventArgs e) => await SaveRecord(false,false);
        private async void BtnSaveClose_Click(object sender, RoutedEventArgs e) => await SaveRecord(true,false);
        private async void BtnSaveAsNew_Click(object sender, RoutedEventArgs e) => await SaveRecord(false,true);

        private async void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (CodeLibraryList)DgListView.SelectedItem : new CodeLibraryList();
            await LoadDataList();
            SetRecord(false);
        }


        private async void SetRecord(bool showForm, bool copy = false) {

            EditorSelector.IsChecked = bool.Parse(await DataOperations.ParameterCheck("WebBuilderHtmlEditorIsDefault"));
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;

            lb_dataList.SelectedItem = selectedRecord.Id == 0 ? null : selectedRecord;
            txt_name.Text = selectedRecord.Name;
            txt_description.Text = selectedRecord.Description;

            if ((bool)EditorSelector.IsChecked) { html_htmlContent.Browser.OpenDocument(selectedRecord.HtmlContent); }
            else { txt_codeContent.Text = selectedRecord.HtmlContent; }


            if (showForm) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            }
            else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
            }
        }

        private void BtnLoadFromFile_Click(object sender, RoutedEventArgs e) {
            try {
                OpenFileDialog dlg = new OpenFileDialog() { DefaultExt = ".html", Filter = "Html files |*.html; *.cshtml; *.js; *.css|All files (*.*)|*.*", Title = Resources["fileOpenDescription"].ToString() };
                if (dlg.ShowDialog() == true) {
                    if ((bool)EditorSelector.IsChecked) { html_htmlContent.Browser.OpenDocument(File.ReadAllText(dlg.FileName, FileOperations.FileDetectEncoding(dlg.FileName))); }
                    else { txt_codeContent.Text = File.ReadAllText(dlg.FileName, FileOperations.FileDetectEncoding(dlg.FileName)); }
                }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        //private async void BtnOpenInBrowser_Click(object sender, RoutedEventArgs e) {
        //    await SaveRecord(false, false);
        //    SystemOperations.StartExternalProccess(SystemOperations.ProcessTypes.First(a => a.Value.ToLower() == "url").Value, App.Setting.ApiAddress + (await DataOperations.ParameterCheck("WebBuilderCodePreview")) + "/" + txt_id.Value.ToString());
        //}

        private async Task<bool> SaveRecord(bool closeForm, bool asNew) {
            try {
                MainWindow.ProgressRing = Visibility.Visible;
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) && !asNew ? txt_id.Value : 0);
                selectedRecord.Name = txt_name.Text;
                selectedRecord.Description = txt_description.Text;


                if ((bool)EditorSelector.IsChecked) { selectedRecord.HtmlContent = html_htmlContent.Browser.GetCurrentHtml(); }
                else { selectedRecord.HtmlContent = txt_codeContent.Text; }
                

                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.TimeStamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await ApiCommunication.PutApiRequest(ApiUrls.CodeLibraryList, httpContent, null, App.UserData.Authentification.Token);
                }
                else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.CodeLibraryList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.RecordCount > 0) { await LoadDataList();}
                if (closeForm) { selectedRecord = new CodeLibraryList();SetRecord(false); }
                if (dBResult.RecordCount == 0) { await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden;
            return true;
        }


        private void DataListDoubleClick(object sender, MouseButtonEventArgs e) {
            if (lb_dataList.SelectedItems.Count > 0) { selectedRecord = (CodeLibraryList)lb_dataList.SelectedItem; }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
            
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
                txt_codeContent.Text = selectedRecord.HtmlContent;
                code_selector.Visibility = Visibility.Visible;
                txt_codeContent.Visibility = Visibility.Visible;
                html_htmlContent.Visibility = Visibility.Hidden;
            }
        }
    }
}