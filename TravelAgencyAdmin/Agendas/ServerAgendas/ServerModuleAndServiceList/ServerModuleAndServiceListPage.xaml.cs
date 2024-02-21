using EasyITSystemCenter.Api;
using EasyITSystemCenter.Classes;
using EasyITSystemCenter.GlobalOperations;
using EasyITSystemCenter.GlobalStyles;
using ICSharpCode.AvalonEdit.Highlighting;
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

namespace EasyITSystemCenter.Pages {

    public partial class ServerModuleAndServiceListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static ServerModuleAndServiceList selectedRecord = new ServerModuleAndServiceList();

        private List<SolutionMixedEnumList> solutionMixedEnumList = new List<SolutionMixedEnumList>();
        private List<ServerModuleAndServiceList> ServerModuleAndServiceList = new List<ServerModuleAndServiceList>();
        private List<SolutionUserRoleList> userRoleList = new List<SolutionUserRoleList>();

        public ServerModuleAndServiceListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);

            try {
                _ = DataOperations.TranslateFormFields(ListForm);
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            _ = LoadDataList();
            SetRecord(false);
        }

        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                solutionMixedEnumList = await CommApi.GetApiRequest<List<SolutionMixedEnumList>>(ApiUrls.EasyITCenterSolutionMixedEnumList, "ByGroup/ModulePageTypes", App.UserData.Authentification.Token);
                userRoleList = await CommApi.GetApiRequest<List<SolutionUserRoleList>>(ApiUrls.EasyITCenterSolutionUserRoleList, null, App.UserData.Authentification.Token);
                ServerModuleAndServiceList = await CommApi.GetApiRequest<List<ServerModuleAndServiceList>>(ApiUrls.ServerModuleAndServiceList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);

                solutionMixedEnumList.ForEach(async tasktype => { tasktype.Translation = await DBOperations.DBTranslation(tasktype.Name); });
                userRoleList.ForEach(async role => { role.Translation = await DBOperations.DBTranslation(role.SystemName); });
                ServerModuleAndServiceList.ForEach(module => { module.PageTypeTranslation = solutionMixedEnumList.FirstOrDefault(a => a.Name == module.InheritedPageType).Translation; });

                DgListView.ItemsSource = ServerModuleAndServiceList;
                DgListView.Items.Refresh();
                cb_inheritedPageType.ItemsSource = solutionMixedEnumList;
                cb_allowedRoles.ItemsSource = userRoleList.OrderBy(a => a.Translation).ToList();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        private async void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(async e => {
                    string headername = e.Header.ToString().ToLower();
                    if (headername == "name") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 1; }
                    else if (headername == "pagetypetranslation") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 2; }
                    else if (headername == "urlsubpath") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 3; }
                    else if (headername == "restrictedaccess") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 4; }
                    else if (headername == "redirecttologinservice") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 5; }
                    else if (headername == "redirectpathonerror") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 6; }
                    else if (headername == "isloginmodule") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 7; }
                    else if (headername == "allowedroles") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 8; }
                    else if (headername == "active") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 7; }
                    else if (headername == "timestamp") { e.Header = await DBOperations.DBTranslation(headername); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }
                    else if (headername == "id") e.DisplayIndex = 0;
                    else if (headername == "userid") e.Visibility = Visibility.Hidden;
                    else if (headername == "description") e.Visibility = Visibility.Hidden;
                    else if (headername == "optionalconfiguration") e.Visibility = Visibility.Hidden;
                    else if (headername == "razorpagesetting") e.Visibility = Visibility.Hidden;
                    else if (headername == "customhtmlcontent") e.Visibility = Visibility.Hidden;
                    else if (headername == "razorpageallowed") e.Visibility = Visibility.Hidden;
                    else if (headername == "pathsetallowed") e.Visibility = Visibility.Hidden;
                    else if (headername == "restrictionsetallowed") e.Visibility = Visibility.Hidden;
                    else if (headername == "htmlsetallowed") e.Visibility = Visibility.Hidden;
                    else if (headername == "redirectsetallowed") e.Visibility = Visibility.Hidden;
                    else if (headername == "inheritedpagetype") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    ServerModuleAndServiceList user = e as ServerModuleAndServiceList;
                    return user.Name.ToLower().Contains(filter.ToLower())
                    || user.PageTypeTranslation.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.UrlSubPath) && user.UrlSubPath.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.OptionalConfiguration) && user.OptionalConfiguration.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.RedirectPathOnError) && user.RedirectPathOnError.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.Description) && user.Description.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void NewRecord() {
            selectedRecord = new ServerModuleAndServiceList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (ServerModuleAndServiceList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (ServerModuleAndServiceList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await CommApi.DeleteApiRequest(ApiUrls.ServerModuleAndServiceList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (ServerModuleAndServiceList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (ServerModuleAndServiceList)DgListView.SelectedItem; } else { selectedRecord = new ServerModuleAndServiceList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);

                selectedRecord.InheritedPageType = ((SolutionMixedEnumList)cb_inheritedPageType.SelectedItem).Name;
                selectedRecord.Name = txt_name.Text;
                selectedRecord.Description = txt_description.Text;

                selectedRecord.UrlSubPath = txt_urlSubPath.Text;
                selectedRecord.OptionalConfiguration = txt_optionalConfiguration.Text;

                selectedRecord.AllowedRoles = "";
                for (int i = 0; i < cb_allowedRoles.SelectedItems.Count; i++) { selectedRecord.AllowedRoles += ((SolutionUserRoleList)cb_allowedRoles.SelectedItems[i]).SystemName + ","; }

                selectedRecord.IsLoginModule = (bool)chb_isLoginModule.IsChecked;
                selectedRecord.RestrictedAccess = (bool)chb_restrictedAccess.IsChecked;
                selectedRecord.RedirectPathOnError = txt_redirectPathOnError.Text;
                selectedRecord.CustomHtmlContent = CustomHtmlContent.Text;

                selectedRecord.Active = (bool)chb_active.IsChecked;
                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.TimeStamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await CommApi.PutApiRequest(ApiUrls.ServerModuleAndServiceList, httpContent, null, App.UserData.Authentification.Token);
                }
                else { dBResult = await CommApi.PostApiRequest(ApiUrls.ServerModuleAndServiceList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.RecordCount > 0) {
                    selectedRecord = new ServerModuleAndServiceList();
                    await LoadDataList();
                    SetRecord(null);
                }
                else { await MainWindow.ShowMessageOnMainWindow(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (ServerModuleAndServiceList)DgListView.SelectedItem : new ServerModuleAndServiceList();
            SetRecord(false);
        }

        private void SetRecord(bool? showForm = null, bool copy = false) {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;

            cb_inheritedPageType.SelectedItem = (selectedRecord.Id == 0) ? solutionMixedEnumList.FirstOrDefault() : solutionMixedEnumList.First(a => a.Name == selectedRecord.InheritedPageType);
            txt_name.Text = selectedRecord.Name;
            txt_description.Text = selectedRecord.Description;

            txt_urlSubPath.Text = selectedRecord.UrlSubPath;
            txt_optionalConfiguration.Text = selectedRecord.OptionalConfiguration;

            cb_allowedRoles.SelectedItems.Clear();
            if (!string.IsNullOrWhiteSpace(selectedRecord.AllowedRoles))
                selectedRecord.AllowedRoles.Split(',').ToList().ForEach(role => { if (!string.IsNullOrEmpty(role)) cb_allowedRoles.SelectedItems.Add(userRoleList.First(a => a.SystemName == role)); });

            chb_isLoginModule.IsChecked = selectedRecord.IsLoginModule;
            chb_restrictedAccess.IsChecked = selectedRecord.RestrictedAccess;
            txt_redirectPathOnError.Text = selectedRecord.RedirectPathOnError;
            CustomHtmlContent.Text = selectedRecord.CustomHtmlContent;

            chb_active.IsChecked = (selectedRecord.Id == 0) ? bool.Parse(App.appRuntimeData.AppClientSettings.First(a => a.Key == "beh_activeNewInputDefault").Value) : selectedRecord.Active;

            if (showForm != null && showForm == true) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
                //setAllowedFields();
            }
            else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = showForm == null && !bool.Parse(App.appRuntimeData.AppClientSettings.First(a => a.Key == "beh_CloseFormAfterSave").Value);
            }
        }

        private void HighlightCodeChanged(object sender, SelectionChangedEventArgs e) {
            CustomHtmlContent.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(((ListBoxItem)code_selector.SelectedValue).Content.ToString());
        }

        private void setAllowedFields() {
            txt_urlSubPath.IsEnabled = selectedRecord.PathSetAllowed;
            chb_restrictedAccess.IsEnabled = selectedRecord.RedirectSetAllowed;
            CustomHtmlContent.IsEnabled = selectedRecord.HtmlSetAllowed;
            txt_redirectPathOnError.IsEnabled = selectedRecord.RedirectSetAllowed;
        }

        private void RestrictedAccessChanged(object sender, RoutedEventArgs e) {
            if (dataViewSupport.FormShown) {
                txt_redirectPathOnError.Text = null;
                txt_redirectPathOnError.IsEnabled = !(bool)chb_restrictedAccess.IsChecked;
            }
        }
    }
}