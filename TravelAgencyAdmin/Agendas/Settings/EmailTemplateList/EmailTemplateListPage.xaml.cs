using GlobalClasses;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.Classes;
using TravelAgencyAdmin.GlobalOperations;
using TravelAgencyAdmin.GlobalStyles;

namespace TravelAgencyAdmin.Pages {

    public partial class EmailTemplateListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static EmailTemplateList selectedRecord = new EmailTemplateList();

        private List<EmailTemplateList> emailTemplateList = new List<EmailTemplateList>();

        public EmailTemplateListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);


            ObservableCollection<UpdateVariant> Templates = new ObservableCollection<UpdateVariant>() {
                                                                new UpdateVariant() { Name = Resources["verification"].ToString(), Value = "verification" },
                                                                new UpdateVariant() { Name = Resources["registration"].ToString(), Value = "registration"},
                                                                new UpdateVariant() { Name = Resources["resetPassword"].ToString(), Value = "resetPassword"}
                                                             };

            try {
                lbl_id.Content = Resources["id"].ToString();
                lbl_templateName.Content = Resources["templateName"].ToString();
                lbl_variables.Content = Resources["variables"].ToString();
                lbl_subjectCz.Content = Resources["subjectCz"].ToString();
                lbl_subjectEn.Content = Resources["subjectEn"].ToString();

                lbl_emailCz.Content = Resources["emailCz"].ToString();
                lbl_emailEn.Content = Resources["emailEn"].ToString();

                btn_save.Content = Resources["btn_save"].ToString();
                btn_cancel.Content = Resources["btn_cancel"].ToString();

                cb_templateName.ItemsSource = Templates;

            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            _ = LoadDataList();
            SetRecord(false);
        }

        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {

                emailTemplateList = await ApiCommunication.GetApiRequest<List<EmailTemplateList>>(ApiUrls.EmailTemplateList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);

                emailTemplateList.ForEach(async template => {
                    template.TemplateNameTranslation = await DBOperations.DBTranslation(template.TemplateName);
                });

                DgListView.ItemsSource = emailTemplateList;
                DgListView.Items.Refresh();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }


            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        private void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "TemplateNameTranslation") { e.Header = Resources["templateName"].ToString(); e.DisplayIndex = 1; }
                    else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }

                    else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "TemplateName") e.Visibility = Visibility.Hidden;
                    else if (headername == "SubjectCz") e.Visibility = Visibility.Hidden;
                    else if (headername == "EmailCz") e.Visibility = Visibility.Hidden;
                    else if (headername == "SubjectEn") e.Visibility = Visibility.Hidden;
                    else if (headername == "EmailEn") e.Visibility = Visibility.Hidden;
                    else if (headername == "Variables") e.Visibility = Visibility.Hidden;
                    else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    EmailTemplateList user = e as EmailTemplateList;
                    return user.TemplateName.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.SubjectCz) && user.SubjectCz.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.EmailCz) && user.EmailCz.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.SubjectEn) && user.SubjectEn.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.EmailEn) && user.EmailEn.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void NewRecord() {
            selectedRecord = new EmailTemplateList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (EmailTemplateList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (EmailTemplateList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessage(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await ApiCommunication.DeleteApiRequest(ApiUrls.EmailTemplateList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (EmailTemplateList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (EmailTemplateList)DgListView.SelectedItem; } else { selectedRecord = new EmailTemplateList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.TemplateName = (string)cb_templateName.SelectedValue;
                selectedRecord.Variables = txt_variables.Text;

                selectedRecord.SubjectCz = txt_subjectCz.Text;
                selectedRecord.SubjectEn = txt_subjectEn.Text;
                
                selectedRecord.EmailCz = html_emailCz.Text;
                selectedRecord.EmailEn = html_emailEn.Text;

                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.Timestamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await ApiCommunication.PutApiRequest(ApiUrls.EmailTemplateList, httpContent, null, App.UserData.Authentification.Token);
                } else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.EmailTemplateList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.RecordCount > 0) {
                    selectedRecord = new EmailTemplateList();
                    await LoadDataList();
                    SetRecord(false);
                } else { await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (EmailTemplateList)DgListView.SelectedItem : new EmailTemplateList();
            SetRecord(false);
        }

        private void SetRecord(bool showForm, bool copy = false) {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;
            cb_templateName.SelectedValue = txt_id.Value == 0 ? "" : selectedRecord.TemplateName;
            txt_variables.Text = selectedRecord.Variables;

            txt_subjectCz.Text = selectedRecord.SubjectCz;
            txt_subjectEn.Text = selectedRecord.SubjectEn;

            html_emailCz.Text = selectedRecord.EmailCz;
            html_emailEn.Text = selectedRecord.EmailEn;

            if (showForm) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            } else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
            }
        }

        private void TemplateNameChanged(object sender, SelectionChangedEventArgs e) {

            switch (cb_templateName.SelectedValue) {
                case "verification":
                    txt_variables.Text = "[verifyCode]";
                    break;
                case "registration":
                    txt_variables.Text = "[firstname],[lastname],[email],[password]";
                    break;
                case "resetPassword":
                    txt_variables.Text = "[firstname],[lastname],[email],[password]";
                    break;
                default:
                    txt_variables.Text = "";
                    break;
            }
        }
    }
}