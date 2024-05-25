using EasyITSystemCenter.Api;
using EasyITSystemCenter.GlobalClasses;
using EasyITSystemCenter.Classes;
using EasyITSystemCenter.GlobalOperations;
using MahApps.Metro.Controls.Dialogs;
using Markdig;
using Markdig.Wpf;
using Microsoft.Web.WebView2.Core;
using Pek.Markdig.HighlightJs;
using SimpleBrowser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WebView2.DevTools.Dom;
using EasyITSystemCenter.GlobalGenerators;
using System.Collections;
using Catel.Collections;




namespace EasyITSystemCenter.Pages {

    public partial class MltTblGenEsbSrvFormPage : UserControl {

        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static DataRowView selectedRecord = null;

        private DataRow newRow;
        private DataTable definedDataList = new DataTable();
        private SystemCustomPageList systemCustomPageList = new SystemCustomPageList();
        private List<SolutionMixedEnumList> solutionMixedEnumList = new List<SolutionMixedEnumList>();
        List<Dictionary<string, string>> RecordForSave = new List<Dictionary<string, string>>();
        private Dictionary<string, string> webElement = new Dictionary<string, string>();
        private WebView2DevToolsContext devToolsContext;
        string WebBrowserData = "";
        string WebBrowserCommandResult = "";
            
        public MltTblGenEsbSrvFormPage() {
            try {
                InitializeComponent();
                _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);
                if (!App.appRuntimeData.webServerRunning) {
                    _ = MainWindow.ShowMessageOnMainWindow(true, DBOperations.DBTranslation("WebServerNotRunningCheckClientConfiguration").GetAwaiter().GetResult());
                } else { IsEnabled = false; IsEnabledChanged += (s, e) => { if (IsEnabled) { _ = LoadDataList(); } }; }

                SetRecord(false);

                Loaded += MltTblGenEsbSrvFormPage_Loaded;
          

            } catch (Exception ex) { }
            MainWindow.ProgressRing = Visibility.Hidden;
        }

        private void MltTblGenEsbSrvFormPage_Loaded(object sender, RoutedEventArgs e) {

            //Initiate WebBrowsers
            webBrowser.CoreWebView2InitializationCompleted += WebBrowser_CoreWebView2InitializationCompleted;
            webBrowser.EnsureCoreWebView2Async(null);
            if (systemCustomPageList.ShowHelpTab && systemCustomPageList.InheritedHelpTabSourceType.ToLower().Contains("helpurl")) { helpWebBrowser.EnsureCoreWebView2Async(null); }
            else if (systemCustomPageList.ShowHelpTab) {
                new MarkdownPipelineBuilder().UseEmphasisExtras().UseAbbreviations().UseAdvancedExtensions().UseBootstrap()
                .UseDiagrams().UseEmphasisExtras().UseEmojiAndSmiley(true).UseDefinitionLists().UseTableOfContent().UseTaskLists()
                .UseSupportedExtensions().UseSmartyPants().UsePipeTables().UseMediaLinks().UseMathematics().UseListExtras().UseHighlightJs()
                .UseGridTables().UseGlobalization().UseGenericAttributes().UseFootnotes().UseFooters().UseSyntaxHighlighting().UseFigures().Build();
            }
        }

        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {

                
                await GetGenericTableData();
                //DOM Initiate
                //webElement = new Dictionary<string, string>();
                //webElement.Add(systemCustomPageList.ColumnName, "text");
                //devToolsContext = await webBrowser.CoreWebView2.CreateDevToolsContextAsync();
                //var htmlDivElement = await devToolsContext.QuerySelectorAsync<HtmlDivElement>("#myDivElementId");
                //await htmlDivElement.SetInnerTextAsync("Welcome!");
                //await htmlDivElement.GetInnerTextAsync();



                ////Initiate WebBrowsers
                //webBrowser.CoreWebView2InitializationCompleted += WebBrowser_CoreWebView2InitializationCompleted;
                //await webBrowser.EnsureCoreWebView2Async(null);
                //if (systemCustomPageList.ShowHelpTab && systemCustomPageList.InheritedHelpTabSourceType.ToLower().Contains("helpurl")) { await helpWebBrowser.EnsureCoreWebView2Async(null); }

                _ = FormOperations.TranslateFormFields(ListView);
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden;
            return true;
        }


        private async void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(async e => {
                    string headername = e.Header.ToString().ToLower();
                    if (headername == "userid".ToLower()) { e.SetCurrentValue(DataGridColumn.VisibilityProperty, Visibility.Hidden); }
                    else if (headername.ToLower().Contains("content")) { e.SetCurrentValue(DataGridColumn.VisibilityProperty, Visibility.Hidden); }
                    else if (headername.ToLower() == "description") { e.SetCurrentValue(DataGridColumn.VisibilityProperty, Visibility.Hidden); }
                    e.SetCurrentValue(DataGridColumn.HeaderProperty, await DBOperations.DBTranslation(headername));
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }


        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => { DataRowView search = e as DataRowView;
                    return search.ToJson().ToLower().Contains(filter.ToLower()); };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }



        public void NewRecord() {
            DgListView.Items.Add(newRow); DgListView.Items.MoveCurrentToLast();
            selectedRecord = ((DataRow)DgListView.Items.CurrentItem).Table.AsDataView()[0];
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (DataRowView)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = int.Parse(((DataRowView)DgListView.SelectedItem).Row.ItemArray[0].ToString());
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (DataRowView)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = int.Parse(((DataRowView)DgListView.SelectedItem).Row.ItemArray[0].ToString());

            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + int.Parse(((DataRowView)DgListView.SelectedItem).Row.ItemArray[0].ToString()), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await CommunicationManager.DeleteApiRequest(ApiUrls.SolutionWebsiteList, ((DataRowView)DgListView.SelectedItem).Row.ItemArray[0].ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (DataRowView)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = int.Parse(((DataRowView)DgListView.SelectedItem).Row.ItemArray[0].ToString());
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (DataRowView)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = int.Parse(((DataRowView)DgListView.SelectedItem).Row.ItemArray[0].ToString());
            SetRecord(false);
        }


        private async Task<bool> SaveForm(bool closeForm) {
            try {
                MainWindow.ProgressRing = Visibility.Visible;DBResultMessage dBResult;

                if (webBrowser.CoreWebView2 == null) { await webBrowser.EnsureCoreWebView2Async(null); 
                    devToolsContext = await webBrowser.CoreWebView2.CreateDevToolsContextAsync();}

                string dataForBrowser = XamlFormGenerators.StandardXamlDataFormController(ref userForm, ref selectedRecord, ref RecordForSave, false, systemCustomPageList);
                if (selectedRecord != null ) {
                    if (!string.IsNullOrWhiteSpace(WebBrowserData) && systemCustomPageList.UseIooverDom) { await DOMGetDataFromBrowser(); }
                    else if (!string.IsNullOrWhiteSpace(WebBrowserData)) { await JSGetDataFromBrowser(); }
                } else if(selectedRecord == null) {
                    await MainWindow.ShowMessageOnMainWindow(true, "Any Data For Save");
                }
                else {
                    //INSERT DATA TO SAVE COOLECTION 
                    RecordForSave.Add(new Dictionary<string, string>() { { systemCustomPageList.ColumnName, WebBrowserData } });

                    //TODO CHECK COLLECTION BEFORE SAVE
                }




                string json = Newtonsoft.Json.JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (int.Parse(((DataGridRow)DgListView.SelectedItem).ToString()) == 0) {
                    // dBResult = await CommunicationManager.PutApiRequest(ApiUrls.SolutionWebsiteList, httpContent, null, App.UserData.Authentification.Token);
                } else {
                    // dBResult = await CommunicationManager.PostApiRequest(ApiUrls.SolutionWebsiteList, httpContent, null, App.UserData.Authentification.Token);
                }
                //if (dBResult.RecordCount > 0) {
                //    await LoadDataList(); if (closeForm) { selectedRecord = null; SetRecord(null); }
                //}
                //else { await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + dBResult.ErrorMessage); }

            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden;
            return true;
        }



        private async void SetRecord(bool? showForm = null, bool copy = false) {
            try {
                //SET FOMRS 
                if (int.Parse(selectedRecord.Row.ItemArray[0].ToString()) == 0) { XamlFormGenerators.DataFormNewRecordReset(ref newRow, ref userForm, ref selectedRecord); }
                else { XamlFormGenerators.StandardXamlDataFormController(ref userForm,ref selectedRecord, ref RecordForSave, true, systemCustomPageList); }

                //REINIT WEBVIEW
                if (webBrowser.CoreWebView2 == null) { await webBrowser.EnsureCoreWebView2Async(null); }
                devToolsContext = await webBrowser.CoreWebView2.CreateDevToolsContextAsync();

                //SET DATA TO FORM AND BROWSER
                if (!string.IsNullOrWhiteSpace(WebBrowserData) && systemCustomPageList.UseIooverDom) {await DOMSetDataFromBrowser();} 
                else if(!string.IsNullOrWhiteSpace(WebBrowserData)) { await JSSetDataFromBrowser();}
          
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            if (showForm != null && showForm == true) {
                int recIt = DgListView.SelectedItem == null ? 0 : int.Parse(selectedRecord.Row.ItemArray[0].ToString());
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = recIt != 0;
                MainWindow.dataGridSelectedId = recIt; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            }
            else {
                int recIt = DgListView.SelectedItem == null ? 0 : int.Parse(selectedRecord.Row.ItemArray[0].ToString());
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = recIt != 0;
                MainWindow.dataGridSelectedId = recIt; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = showForm == null && !bool.Parse(App.appRuntimeData.AppClientSettings.First(a => a.Key.ToLower() == "beh_closeformaftersave".ToLower()).Value);
            }
            MainWindow.ProgressRing = Visibility.Hidden;
        }


        private async void BtnSave_Click(object sender, RoutedEventArgs e) => await SaveForm(false);
        private async void BtnSaveClose_Click(object sender, RoutedEventArgs e) => await SaveForm(true);
        private void BtnCancel_Click(object sender, RoutedEventArgs e) { SetRecord(false); }

        /// <summary>
        /// Generic API Request
        /// </summary>
        /// <returns></returns>
        private async Task<bool> GetGenericTableData() {
            MainWindow.ProgressRing = Visibility.Visible;
            if (systemCustomPageList.Id == 0) {
                systemCustomPageList = await CommunicationManager.GetApiRequest<SystemCustomPageList>(ApiUrls.EasyITCenterSystemCustomPageList, this.Uid.ToString(), App.UserData.Authentification.Token);
            }
            List<Dictionary<string, string>> parameters = new List<Dictionary<string, string>>();
            parameters.Add(new Dictionary<string, string>() { { "SpProcedure", "SpGetTableDataList" } });
            parameters.Add(new Dictionary<string, string>() { { "tableName", systemCustomPageList.DbtableName } });
            parameters.Add(new Dictionary<string, string>() { { "userRole", App.UserData.Authentification.Role } });
            parameters.Add(new Dictionary<string, string>() { { "userId", App.UserData.Authentification.Id.ToString() } });

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(parameters);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            definedDataList = await CommunicationManager.ApiManagerPostRequest<DataTable>(UrlSourceTypes.EicWebServerGenericGetTableApi, null, httpContent);
            DgListView.SetCurrentValue(ItemsControl.ItemsSourceProperty, definedDataList.AsDataView());
            newRow = definedDataList.Rows[0];
            await LoadInheritedDataList();

            //Generate User Form 
            userForm = XamlFormGenerators.StandardXamlFormViewGenerator(ref userForm, definedDataList, systemCustomPageList, solutionMixedEnumList);

            bool res = XamlFormGenerators.DataFormNewRecordReset(ref newRow, ref userForm, ref selectedRecord);
            TabMenuList.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedIndexProperty, 0);
            userForm.UpdateLayout();
            MainWindow.ProgressRing = Visibility.Hidden;
            await FormOperations.TranslateFormFields(ListForm);
            await FormOperations.TranslateFormFields(userForm);
            return true;

        }

        //Load INHERITED TABLE
        private async Task<bool> LoadInheritedDataList() {
            if (systemCustomPageList?.InheritedSetName?.Length > 0) {
                solutionMixedEnumList = await CommunicationManager.GetApiRequest<List<SolutionMixedEnumList>>(ApiUrls.EasyITCenterSolutionMixedEnumList, "ByGroup/" + systemCustomPageList.InheritedSetName, App.UserData.Authentification.Token);
            } return true;
        }


        private async void WebBrowser_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e) {
            try {

                devToolsContext = await webBrowser.CoreWebView2.CreateDevToolsContextAsync();
                webBrowser.SetCurrentValue(Microsoft.Web.WebView2.Wpf.WebView2.SourceProperty, new Uri(systemCustomPageList.IsServerUrl ? App.appRuntimeData.AppClientSettings.First(a => a.Key == "conn_apiAddress").Value +
                    (systemCustomPageList.StartupUrl != null && systemCustomPageList.StartupUrl.StartsWith("/") ? systemCustomPageList.StartupUrl : "/" + systemCustomPageList.StartupUrl)
                    : systemCustomPageList.IsSystemUrl ? App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_localWebServerUrl").Value + systemCustomPageList.StartupUrl
                    : systemCustomPageList.StartupUrl
                    ));
                if (systemCustomPageList.DevModeEnabled) { webBrowser.CoreWebView2.Settings.AreDevToolsEnabled = true; } else { webBrowser.CoreWebView2.Settings.AreDevToolsEnabled = false; }
                if (systemCustomPageList.ShowHelpTab && systemCustomPageList.HelpTabUrl != null) {

                    switch (systemCustomPageList.InheritedHelpTabSourceType.ToLower()) {
                        case "eicserverhelpurl":
                            ti_helpUrl.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                            helpWebBrowser.SetCurrentValue(Microsoft.Web.WebView2.Wpf.WebView2.SourceProperty, new Uri(App.appRuntimeData.AppClientSettings.First(a => a.Key == "conn_apiAddress").Value + $"{(systemCustomPageList.HelpTabUrl.StartsWith("/") ? systemCustomPageList.HelpTabUrl : "/" + systemCustomPageList.HelpTabUrl)}"));
                            break;
                        case "esbsystemhelpurl":
                            ti_helpUrl.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                            helpWebBrowser.SetCurrentValue(Microsoft.Web.WebView2.Wpf.WebView2.SourceProperty, new Uri(App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_localWebServerUrl").Value + systemCustomPageList.HelpTabUrl));
                            break;
                        case "publicwebhelpurl":
                            ti_helpUrl.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                            helpWebBrowser.SetCurrentValue(Microsoft.Web.WebView2.Wpf.WebView2.SourceProperty, new Uri(systemCustomPageList.HelpTabUrl));
                            break;
                        case "eicservermdfile":
                            ti_helpDoc.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                            helpDocument.SetCurrentValue(MarkdownViewer.MarkdownProperty, await CommunicationManager.ApiManagerGetRequest(UrlSourceTypes.EicWebServer, systemCustomPageList.HelpTabUrl));
                            break;
                        case "esbsystemmdfile":
                            ti_helpDoc.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                            helpDocument.SetCurrentValue(MarkdownViewer.MarkdownProperty, await CommunicationManager.ApiManagerGetRequest(UrlSourceTypes.EsbWebServer, systemCustomPageList.HelpTabUrl));
                            //helpDocument.Markdown = System.IO.File.ReadAllText(System.IO.Path.Combine(App.appRuntimeData.webDataUrlPath) + FileOperations.ConvertSystemFilePathFromUrl(systemCustomPageList.HelpTabUrl));
                            break;
                        case "publicwebmdfile":
                            ti_helpDoc.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                            helpDocument.SetCurrentValue(MarkdownViewer.MarkdownProperty, await CommunicationManager.ApiManagerGetRequest(UrlSourceTypes.WebUrl, systemCustomPageList.HelpTabUrl));
                            break;
                    }
                }

            } catch { } try {
                webBrowser.CoreWebView2.ContextMenuRequested += async delegate (object sender1, CoreWebView2ContextMenuRequestedEventArgs args) {
                    IList<CoreWebView2ContextMenuItem> menuList = args.MenuItems;
                    CoreWebView2ContextMenuItem openConsole = webBrowser.CoreWebView2.Environment.CreateContextMenuItem(await DBOperations.DBTranslation("openConsole"), null, CoreWebView2ContextMenuItemKind.Command); openConsole.CustomItemSelected += openConsoleSelected;
                    CoreWebView2ContextMenuItem openTaskManager = webBrowser.CoreWebView2.Environment.CreateContextMenuItem(await DBOperations.DBTranslation("printPage"), null, CoreWebView2ContextMenuItemKind.Command); openTaskManager.CustomItemSelected += openTaskManagerSelected;
                    CoreWebView2ContextMenuItem testJsSetCommand = webBrowser.CoreWebView2.Environment.CreateContextMenuItem(await DBOperations.DBTranslation("Run JS Set Browser Command Test Data"), null, CoreWebView2ContextMenuItemKind.Command); testJsSetCommand.CustomItemSelected += TestJsSetCommandSelected;
                    CoreWebView2ContextMenuItem testJsGetCommand = webBrowser.CoreWebView2.Environment.CreateContextMenuItem(await DBOperations.DBTranslation("Run JS Get Browser Command"), null, CoreWebView2ContextMenuItemKind.Command); testJsGetCommand.CustomItemSelected += TestJsGetCommandSelected;
                    CoreWebView2ContextMenuItem testDOMSetCommand = webBrowser.CoreWebView2.Environment.CreateContextMenuItem(await DBOperations.DBTranslation("Run DOM Set Browser Command Test Data"), null, CoreWebView2ContextMenuItemKind.Command); testDOMSetCommand.CustomItemSelected += TestDOMSetCommandSelected;
                    CoreWebView2ContextMenuItem testDOMGetCommand = webBrowser.CoreWebView2.Environment.CreateContextMenuItem(await DBOperations.DBTranslation("Run DOM Get Browser Command"), null, CoreWebView2ContextMenuItemKind.Command); testDOMGetCommand.CustomItemSelected += TestDOMGetCommandSelected;
                    CoreWebView2ContextMenuItem testJSSetFormCommand = webBrowser.CoreWebView2.Environment.CreateContextMenuItem(await DBOperations.DBTranslation("Run JS Set Browser Command Form Data"), null, CoreWebView2ContextMenuItemKind.Command); testJSSetFormCommand.CustomItemSelected += TestJsSetFORMCommandSelected;
                    CoreWebView2ContextMenuItem testDOMSetFormCommand = webBrowser.CoreWebView2.Environment.CreateContextMenuItem(await DBOperations.DBTranslation("Run DOM Set Browser Command Form Data"), null, CoreWebView2ContextMenuItemKind.Command); testDOMSetFormCommand.CustomItemSelected += TestDOMSetFORMCommandSelected;
                    menuList.Add(openConsole); menuList.Add(openTaskManager); menuList.Add(testJsSetCommand); menuList.Add(testJsGetCommand);
                    menuList.Add(testDOMSetCommand); menuList.Add(testDOMGetCommand); menuList.Add(testJSSetFormCommand); menuList.Add(testDOMSetFormCommand);
                };
            
            } catch { }
            MainWindow.ProgressRing = Visibility.Hidden;
        }
        private void openConsoleSelected(object sender, object e) { webBrowser.CoreWebView2.OpenDevToolsWindow(); }
        private void openTaskManagerSelected(object sender, object e) { webBrowser.CoreWebView2.ShowPrintUI(); }
        private async void TestDOMGetCommandSelected(object sender, object e) => await DOMGetDataFromBrowser();
        private async void TestDOMSetCommandSelected(object sender, object e) => await DOMSetDataFromBrowser(true);
        private async void TestJsSetCommandSelected(object sender, object e) => await JSSetDataFromBrowser(true);
        private async void TestJsGetCommandSelected(object sender, object e) => await JSGetDataFromBrowser();
        private async void TestDOMSetFORMCommandSelected(object sender, object e) => await DOMSetDataFromBrowser(false);
        private async void TestJsSetFORMCommandSelected(object sender, object e) => await JSSetDataFromBrowser(false);

    //ONTABCHANGE UPDATING DATA
    private async void TabSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (((TabControl)sender).SelectedIndex == 0) {
                if (systemCustomPageList.UseIooverDom) { await DOMGetDataFromBrowser(); }
                else { await JSGetDataFromBrowser(); }
            } else {
                if (systemCustomPageList.UseIooverDom) { await DOMSetDataFromBrowser(); }
                else { await JSSetDataFromBrowser(); }
            }
            if (systemCustomPageList.DevModeEnabled) {
                App.ApplicationLogging(new Exception($"Data Updating on Tab Change: actual Command Reusult: " +
                    $"{WebBrowserCommandResult}{Environment.NewLine} Actual Saved Data: {Environment.NewLine}{WebBrowserData}"));
            }
        }




        private async Task<bool> DOMSetDataFromBrowser(bool isTest = false) {
            if (webBrowser.CoreWebView2 == null) { await webBrowser.EnsureCoreWebView2Async(null); }
            devToolsContext = await webBrowser.CoreWebView2.CreateDevToolsContextAsync();
            try {
                var htmlDivElement = await devToolsContext.QuerySelectorAsync<HtmlDivElement>(!systemCustomPageList.DomhtmlElementName.StartsWith("#") ? $"#{systemCustomPageList.DomhtmlElementName}" : systemCustomPageList.DomhtmlElementName);
                await htmlDivElement.SetInnerHtmlAsync(WebBrowserData.Replace("data", (isTest ? "This Is Test Data" : WebBrowserData)));
                if (systemCustomPageList.DevModeEnabled) { App.ApplicationLogging(new Exception($"DOM Set InnerHtml Object {systemCustomPageList.DomhtmlElementName}")); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            return true;
        }

        private async Task<bool> JSSetDataFromBrowser(bool isTest = false) {
            if (webBrowser.CoreWebView2 == null) { await webBrowser.EnsureCoreWebView2Async(null); }
            devToolsContext = await webBrowser.CoreWebView2.CreateDevToolsContextAsync();
            try {
                string result = await webBrowser.CoreWebView2.ExecuteScriptAsync(systemCustomPageList.SetWebDataJscriptCmd.Replace("data",(isTest ? "This Is Test Data" : WebBrowserData)));
                if (systemCustomPageList.DevModeEnabled) { App.ApplicationLogging(new Exception($"JS Set ExecuteScriptAsync {systemCustomPageList.SetWebDataJscriptCmd.Replace("data", WebBrowserData)} Function Response Message:{Environment.NewLine}{result}")); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            return true;
        }

        private async Task<bool> DOMGetDataFromBrowser() {
            if (webBrowser.CoreWebView2 == null) { await webBrowser.EnsureCoreWebView2Async(null); }
            devToolsContext = await webBrowser.CoreWebView2.CreateDevToolsContextAsync();
            try {
                var htmlDivElement = await devToolsContext.QuerySelectorAsync<HtmlDivElement>(!systemCustomPageList.DomhtmlElementName.StartsWith("#") ? $"#{systemCustomPageList.DomhtmlElementName}" : systemCustomPageList.DomhtmlElementName);
                WebBrowserData = await htmlDivElement.GetInnerHtmlAsync();
                if (systemCustomPageList.DevModeEnabled) { App.ApplicationLogging(new Exception($"DOM Get InnerHtml From Object '{systemCustomPageList.DomhtmlElementName}' Function Response Message:{Environment.NewLine}{WebBrowserData}")); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            return true;
        }

        private async Task<bool> JSGetDataFromBrowser() {
            if (webBrowser.CoreWebView2 == null) { await webBrowser.EnsureCoreWebView2Async(null); }
            devToolsContext = await webBrowser.CoreWebView2.CreateDevToolsContextAsync();
            try {
                WebBrowserData = await webBrowser.CoreWebView2.ExecuteScriptAsync(systemCustomPageList.GetWebDataJscriptCmd);
                if (systemCustomPageList.DevModeEnabled) { App.ApplicationLogging(new Exception($"JS Get ExecuteScriptAsync {systemCustomPageList.GetWebDataJscriptCmd} Function Response Message:{Environment.NewLine}{WebBrowserData}")); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            return true;
        }        
    }
}