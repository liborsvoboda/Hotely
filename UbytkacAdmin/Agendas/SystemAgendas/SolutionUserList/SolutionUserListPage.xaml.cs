using EasyITSystemCenter.Api;
using EasyITSystemCenter.Classes;
using EasyITSystemCenter.GlobalOperations;
using EasyITSystemCenter.GlobalStyles;
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
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace EasyITSystemCenter.Pages {

    public partial class SolutionUserListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static SolutionUserList selectedRecord = new SolutionUserList();

        private List<SolutionUserList> userList = new List<SolutionUserList>();
        private List<SolutionUserRoleList> userRoleList = new List<SolutionUserRoleList>();

        public SolutionUserListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);

            try {
                _ = DataOperations.TranslateFormFields(ListForm);
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            _ = LoadDataList();
            SetRecord(false);
        }

        //change datasource
        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;

            try {

                userRoleList = await CommApi.GetApiRequest<List<SolutionUserRoleList>>(ApiUrls.SolutionUserRoleList, null, App.UserData.Authentification.Token);
                userList = await CommApi.GetApiRequest<List<SolutionUserList>>(ApiUrls.SolutionUserList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                

                userList.ForEach(async user => { user.Translation = await DBOperations.DBTranslation(userRoleList.First(a => a.Id == user.RoleId).SystemName); });
                userRoleList.ForEach(async role => { role.Translation = await DBOperations.DBTranslation(role.SystemName); });

                DgListView.ItemsSource = userList;
                DgListView.Items.Refresh();
                cb_roleId.ItemsSource = userRoleList;
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            DgListView.Items.Refresh();
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        // set translate columns in listView
        private void DgListView_Translate(object sender, EventArgs ex) {
            ((DataGrid)sender).Columns.ToList().ForEach(e => {
                string headername = e.Header.ToString();
                if (headername == "Username") { e.Header = Resources["userName"].ToString(); e.DisplayIndex = 1; }
                else if (headername == "Translation") { e.Header = Resources["role"].ToString(); e.DisplayIndex = 2; }
                else if (headername == "Password") e.Header = Resources["password"].ToString();
                else if (headername == "Name") e.Header = Resources["name"].ToString();
                else if (headername == "Surname") e.Header = Resources["surname"].ToString();
                else if (headername == "Description") e.Header = Resources["description"].ToString();
                else if (headername == "Expiration") e.Header = Resources["expiration"].ToString();
                else if (headername == "Active") { e.Header = Resources["active"].ToString(); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 2; }
                else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }
                else if (headername == "Id") e.DisplayIndex = 0;
                else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                else if (headername == "RoleId") e.Visibility = Visibility.Hidden;
                else if (headername == "Photo") e.Visibility = Visibility.Hidden;
                else if (headername == "Token") e.Visibility = Visibility.Hidden;
                else if (headername == "PhotoPath") e.Visibility = Visibility.Hidden;
                else if (headername == "MimeType") e.Visibility = Visibility.Hidden;
            });
        }

        //change filter fields
        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    SolutionUserList user = e as SolutionUserList;
                    return user.Username.ToLower().Contains(filter.ToLower())
                    || user.Name.ToLower().Contains(filter.ToLower())
                    || user.Surname.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.Description) && user.Description.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void NewRecord() {
            selectedRecord = new SolutionUserList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (SolutionUserList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (SolutionUserList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await CommApi.DeleteApiRequest(ApiUrls.SolutionUserList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + dBResult.ErrorMessage);
                _ = LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (SolutionUserList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) {
                selectedRecord = (SolutionUserList)DgListView.SelectedItem;
            }
            else { selectedRecord = new SolutionUserList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        //change dataset save method
        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.RoleId = ((SolutionUserRoleList)cb_roleId.SelectedItem).Id;
                selectedRecord.Username = txt_userName.Text;
                selectedRecord.Password = pb_password.Password;
                selectedRecord.Name = txt_name.Text;
                selectedRecord.Surname = txt_surname.Text;
                selectedRecord.Description = txt_description.Text;
                selectedRecord.Active = (bool)chb_active.IsChecked;
                selectedRecord.Timestamp = DateTimeOffset.Now.DateTime;
                selectedRecord.Token = txt_token.Text;
                selectedRecord.Expiration = dp_expiration.Value;

                if (!string.IsNullOrWhiteSpace(txt_photoPath.Text)) {
                    selectedRecord.MimeType = MimeMapping.GetMimeMapping(txt_photoPath.Text);
                    selectedRecord.Photo = File.ReadAllBytes(txt_photoPath.Text);
                    selectedRecord.PhotoPath = txt_photoPath.Text;
                }

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await CommApi.PutApiRequest(ApiUrls.SolutionUserList, httpContent, null, App.UserData.Authentification.Token);
                }
                else { dBResult = await CommApi.PostApiRequest(ApiUrls.SolutionUserList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.RecordCount > 0) {
                    selectedRecord = new SolutionUserList();
                    await LoadDataList();
                    SetRecord(null);
                }
                else { await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (SolutionUserList)DgListView.SelectedItem : new SolutionUserList();
            SetRecord(false);
        }

        //change dataset prepare for working
        private void SetRecord(bool? showForm = null, bool copy = false) {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;

            cb_roleId.SelectedItem = (selectedRecord.Id == 0) ? userRoleList.FirstOrDefault() : userRoleList.First(a => a.Id == selectedRecord.RoleId);
            txt_userName.Text = selectedRecord.Username;
            pb_password.Password = selectedRecord.Password;
            txt_name.Text = selectedRecord.Name;
            txt_surname.Text = selectedRecord.Surname;
            txt_description.Text = selectedRecord.Description;
            chb_active.IsChecked = (selectedRecord.Id == 0) ? bool.Parse(App.appRuntimeData.AppClientSettings.First(a => a.Key == "beh_activeNewInputDefault").Value) : selectedRecord.Active;
            dp_timestamp.Value = selectedRecord.Timestamp;
            txt_token.Text = selectedRecord.Token;
            dp_expiration.Value = selectedRecord.Expiration;
            img_photoPath.Source = (!string.IsNullOrWhiteSpace(selectedRecord.PhotoPath)) ? MediaOperations.ByteToImage(selectedRecord.Photo) : new BitmapImage(new Uri(Path.Combine(App.appRuntimeData.settingFolder, "no_photo.png")));
            txt_photoPath.Text = null;

            if (showForm != null && showForm == true) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            }
            else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = showForm == null && !bool.Parse(App.appRuntimeData.AppClientSettings.First(a => a.Key == "beh_CloseFormAfterSave").Value);
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e) {
            try {
                OpenFileDialog dlg = new OpenFileDialog() { DefaultExt = ".png", Filter = "Image files |*.png;*.jpg;*.jpeg", Title = Resources["fileOpenDescription"].ToString() };
                if (dlg.ShowDialog() == true) {
                    img_photoPath.Source = new BitmapImage(new Uri(dlg.FileName));
                    txt_photoPath.Text = dlg.FileName;
                    selectedRecord.Photo = File.ReadAllBytes(dlg.FileName);
                }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }
    }
}