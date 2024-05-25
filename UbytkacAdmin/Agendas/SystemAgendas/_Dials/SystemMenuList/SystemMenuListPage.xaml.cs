using EasyITSystemCenter.Api;
using EasyITSystemCenter.Classes;
using EasyITSystemCenter.GlobalClasses;
using EasyITSystemCenter.GlobalOperations;
using EasyITSystemCenter.GlobalStyles;
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

    public partial class SystemMenuListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static SystemMenuList selectedRecord = new SystemMenuList();

        private List<SystemMenuList> SystemMenuList = new List<SystemMenuList>();
        private List<GenericDataList> systemTableList = new List<GenericDataList>();
        private List<SystemTranslatedTableList> systemTranslatedTableList = new List<SystemTranslatedTableList>();
        private List<SystemGroupMenuList> systemGroupMenuList = new List<SystemGroupMenuList>();
        private List<SolutionUserRoleList> userRoleList = new List<SolutionUserRoleList>();

        public SystemMenuListPage() {
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
            DgListView.RowHeight = int.Parse(await DataOperations.ParameterCheck("DialsFormsRowHeight"));
        }

        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                systemTableList = await CommApi.GetApiRequest<List<GenericDataList>>(ApiUrls.StoredProceduresList, "SpGetSystemPageList", App.UserData.Authentification.Token);

                userRoleList = await CommApi.GetApiRequest<List<SolutionUserRoleList>>(ApiUrls.SolutionUserRoleList, null, App.UserData.Authentification.Token);
                systemGroupMenuList = await CommApi.GetApiRequest<List<SystemGroupMenuList>>(ApiUrls.SystemGroupMenuList, null, App.UserData.Authentification.Token);
                SystemMenuList = await CommApi.GetApiRequest<List<SystemMenuList>>(ApiUrls.SystemMenuList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);

                systemTranslatedTableList.Clear();
                systemTableList.ForEach(async table => {
                    if (SystemMenuList.Where(a => a.FormPageName == table.Data).Count() == 0) {
                        systemTranslatedTableList.Add(new SystemTranslatedTableList() { TableName = table.Data, Translate = await DBOperations.DBTranslation(table.Data) });
                    }
                });

                userRoleList.ForEach(async role => { role.Translation = await DBOperations.DBTranslation(role.SystemName); });
                systemGroupMenuList.ForEach(async group => { group.Translation = await DBOperations.DBTranslation(group.SystemName); });
                SystemMenuList.ForEach(async menu => {
                    menu.GroupName = systemGroupMenuList.First(a => a.Id == menu.GroupId).Translation;
                    menu.FormTranslate = await DBOperations.DBTranslation(menu.FormPageName);
                });

                SystemLocalEnumSets.MenuTypes.AsEnumerable().ToList().ForEach(async menutype => { menutype.Value = await DBOperations.DBTranslation(menutype.Name); });

                DgListView.ItemsSource = SystemMenuList;
                DgListView.Items.Refresh();

                cb_groupName.ItemsSource = systemGroupMenuList.OrderBy(a => a.Translation).ToList();
                cb_menuType.ItemsSource = SystemLocalEnumSets.MenuTypes.OrderBy(a => a.Value).ToList();

                cb_formPageName.ItemsSource = systemTranslatedTableList.OrderBy(a => a.Translate).ToList();
                cb_formPageName.Items.Refresh();

                cb_accessRole.ItemsSource = userRoleList.OrderBy(a => a.Translation).ToList();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        private async void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(async e => {
                    string headername = e.Header.ToString().ToLower();

                    if (headername == "translation") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 3; }
                    else if (headername == "groupname") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 1; }
                    else if (headername == "menutype") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 2; }
                    else if (headername == "formpagename") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 4; }
                    else if (headername == "formtranslate") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 5; }
                    else if (headername == "accessrole") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 6; }
                    else if (headername == "notshowinmenu") { e.Header = await DBOperations.DBTranslation(headername); e.DisplayIndex = 7; }
                    else if (headername == "description") e.Header = await DBOperations.DBTranslation(headername);
                    else if (headername == "active") { e.Header = await DBOperations.DBTranslation(headername); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 2; }
                    else if (headername == "timestamp") { e.Header = await DBOperations.DBTranslation(headername); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }
                    else if (headername == "id") e.DisplayIndex = 0;
                    else if (headername == "userud") e.Visibility = Visibility.Hidden;
                    else if (headername == "groupid") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    SystemMenuList user = e as SystemMenuList;
                    return user.FormPageName.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.GroupName) && user.GroupName.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.AccessRole) && user.AccessRole.ToLower().Contains(filter.ToLower())
                     || !string.IsNullOrEmpty(user.FormTranslate) && user.FormTranslate.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.MenuType) && user.MenuType.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.Description) && user.Description.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void NewRecord() {
            selectedRecord = new SystemMenuList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (SystemMenuList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (SystemMenuList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await CommApi.DeleteApiRequest(ApiUrls.SystemMenuList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (SystemMenuList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (SystemMenuList)DgListView.SelectedItem; }
            else { selectedRecord = new SystemMenuList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);

                selectedRecord.GroupId = ((SystemGroupMenuList)cb_groupName.SelectedItem).Id;
                selectedRecord.MenuType = ((Language)cb_menuType.SelectedItem).Name;
                selectedRecord.FormPageName = ((SystemTranslatedTableList)cb_formPageName.SelectedItem).TableName;

                selectedRecord.AccessRole = "";
                for (int i = 0; i < cb_accessRole.SelectedItems.Count; i++) { selectedRecord.AccessRole += ((SolutionUserRoleList)cb_accessRole.SelectedItems[i]).SystemName + ","; }

                selectedRecord.Description = txt_description.Text;
                selectedRecord.NotShowInMenu = (bool)chb_notShowInMenu.IsChecked;
                selectedRecord.UserId = App.UserData.Authentification.Id;

                selectedRecord.Active = (bool)chb_active.IsChecked;
                selectedRecord.TimeStamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await CommApi.PutApiRequest(ApiUrls.SystemMenuList, httpContent, null, App.UserData.Authentification.Token);
                }
                else { dBResult = await CommApi.PostApiRequest(ApiUrls.SystemMenuList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.RecordCount > 0) {
                    selectedRecord = new SystemMenuList();
                    await LoadDataList();
                    SetRecord(null);
                }
                else { await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (SystemMenuList)DgListView.SelectedItem : new SystemMenuList();
            SetRecord(false);
        }

        private async void SetRecord(bool? showForm = null, bool copy = false) {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;

            cb_groupName.SelectedItem = (selectedRecord.Id == 0) ? systemGroupMenuList.FirstOrDefault() : systemGroupMenuList.First(a => a.Id == selectedRecord.GroupId);

            int index = 0;
            cb_menuType.Items.SourceCollection.Cast<Language>().ToList().ForEach(language => { if (language.Name == selectedRecord.MenuType) { cb_menuType.SelectedIndex = index; } index++; });

            // Insert missing
            if (selectedRecord.FormPageName != null && systemTranslatedTableList.FirstOrDefault(a => a.TableName == selectedRecord.FormPageName) == null) {
                SystemTranslatedTableList page = new SystemTranslatedTableList() { TableName = selectedRecord.FormPageName, Translate = await DBOperations.DBTranslation(selectedRecord.FormPageName) };
                systemTranslatedTableList.Add(page);
                cb_formPageName.ItemsSource = systemTranslatedTableList.OrderBy(a => a.Translate).ToList();
                cb_formPageName.Items.Refresh();
            }
            cb_formPageName.SelectedItem = selectedRecord.FormPageName == null ? null : systemTranslatedTableList.FirstOrDefault(a => a.TableName == selectedRecord.FormPageName);

            cb_accessRole.SelectedItems.Clear();
            if (!string.IsNullOrWhiteSpace(selectedRecord.AccessRole))
                selectedRecord.AccessRole.Split(',').ToList().ForEach(role => { if (!string.IsNullOrEmpty(role)) cb_accessRole.SelectedItems.Add(userRoleList.First(a => a.SystemName == role)); });

            txt_description.Text = selectedRecord.Description;
            chb_notShowInMenu.IsChecked = selectedRecord.NotShowInMenu;
            chb_active.IsChecked = (selectedRecord.Id == 0) ? bool.Parse(App.appRuntimeData.AppClientSettings.First(a => a.Key == "beh_activeNewInputDefault").Value) : selectedRecord.Active;

            if (showForm != null && showForm == true) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            }
            else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = showForm == null && !bool.Parse(App.appRuntimeData.AppClientSettings.First(a => a.Key == "beh_CloseFormAfterSave").Value);
            }
        }
    }
}