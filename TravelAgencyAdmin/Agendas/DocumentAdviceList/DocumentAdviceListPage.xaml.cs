using Newtonsoft.Json;
using TravelAgencyAdmin.Classes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TravelAgencyAdmin.GlobalFunctions;
using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.GlobalStyles;
using MahApps.Metro.Controls.Dialogs;
using System.Net;
using TravelAgencyAdmin.GlobalClasses;

namespace TravelAgencyAdmin.Pages
{
    public partial class DocumentAdviceListPage : UserControl
    {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static ExtendedDocumentAdviceList selectedRecord = new ExtendedDocumentAdviceList();

        private List<UserList> adminUserList = new List<UserList>();
        private List<BranchList> branchList = new List<BranchList>();
        private List<DocumentTypeList> documentTypeList = new List<DocumentTypeList>();
        public DocumentAdviceListPage()
        {
            InitializeComponent();
            _ = MediaFunctions.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            //translate fields in detail form
            lbl_id.Content = Resources["id"].ToString();
            lbl_branch.Content = Resources["branch"].ToString();
            lbl_documentType.Content = Resources["documentType"].ToString();
            lbl_prefix.Content = Resources["prefix"].ToString();
            lbl_number.Content = Resources["number"].ToString();
            lb_startDate.Content = Resources["startDate"].ToString();
            dp_startDate.Value = DateTime.Now;
            lb_endDate.Content = Resources["endDate"].ToString();
            dp_endDate.Value = DateTime.Now;
            lbl_owner.Content = Resources["owner"].ToString();

            btn_save.Content = Resources["btn_save"].ToString();
            btn_cancel.Content = Resources["btn_cancel"].ToString();

            _ = LoadDataList();
            SetRecord(false);
        }

        //change datasource
        public async Task<bool> LoadDataList()
        {
            MainWindow.ProgressRing = Visibility.Visible;
            List<DocumentAdviceList> documentAdviceList = new List<DocumentAdviceList>();
            List<ExtendedDocumentAdviceList> extendedDocumentAdviceList = new List<ExtendedDocumentAdviceList>();
            try
            {
                cb_branch.ItemsSource = branchList = await ApiCommunication.GetApiRequest<List<BranchList>>(ApiUrls.BranchList, null, App.UserData.Authentification.Token);
                documentAdviceList = await ApiCommunication.GetApiRequest<List<DocumentAdviceList>>(ApiUrls.DocumentAdviceList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);

                //set document types translation
                cb_documentType.ItemsSource = documentTypeList = await ApiCommunication.GetApiRequest<List<DocumentTypeList>>(ApiUrls.DocumentTypeList, null, App.UserData.Authentification.Token);
                cb_documentType.DisplayMemberPath = (App.appLanguage == "cs-CZ") ? "DescriptionCz" : "DescriptionEn";


                documentAdviceList.ForEach(record =>
                {
                    ExtendedDocumentAdviceList item = new ExtendedDocumentAdviceList()
                    {
                        Id = record.Id,
                        BranchId = record.BranchId,
                        DocumentType = SystemFunctions.DBTranslation(documentTypeList.Where(a => a.Id == record.DocumentTypeId).Select(a=>a.SystemName).FirstOrDefault()),
                        Prefix = record.Prefix,
                        Number = record.Number,
                        StartDate = record.StartDate,
                        EndDate = record.EndDate,
                        UserId = record.UserId,
                        TimeStamp = record.TimeStamp,
                        Branch = branchList.First(a => a.Id == record.BranchId).CompanyName
                    };
                    extendedDocumentAdviceList.Add(item);
                });
                DgListView.ItemsSource = extendedDocumentAdviceList;
                DgListView.Items.Refresh();

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin")
                {
                    cb_owner.ItemsSource = adminUserList = await ApiCommunication.GetApiRequest<List<UserList>>(ApiUrls.UserList, null, App.UserData.Authentification.Token);
                    lbl_owner.Visibility = cb_owner.Visibility = Visibility.Visible;
                }

            } catch { }

            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        // set translate columns in listView
        private void DgListView_Translate(object sender, EventArgs ex)
        {
            ((DataGrid)sender).Columns.ToList().ForEach(e =>
            {
                string headername = e.Header.ToString();
                if (headername == "DocumentType") e.Header = Resources["documentType"].ToString();
                else if (headername == "Branch") { e.Header = Resources["branch"].ToString(); e.DisplayIndex = 1; }
                else if (headername == "Prefix") e.Header = Resources["prefix"].ToString();
                else if (headername == "Number") e.Header = Resources["number"].ToString();
                else if (headername == "StartDate") { e.Header = Resources["startDate"].ToString(); (e as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy"; e.CellStyle = DatagridStyles.gridTextRightAligment; }
                else if (headername == "EndDate") { e.Header = Resources["endDate"].ToString(); (e as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy"; e.CellStyle = DatagridStyles.gridTextRightAligment; }
                else if (headername == "TimeStamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }

                else if (headername == "Id") e.DisplayIndex = 0;
                else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                else if (headername == "DocumentTypeId") e.Visibility = Visibility.Hidden;
                else if (headername == "BranchId") e.Visibility = Visibility.Hidden;
            });
        }

        //change filter fields
        public void Filter(string filter)
        {
            try
            {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    ExtendedDocumentAdviceList report = e as ExtendedDocumentAdviceList;
                    return report.DocumentType.ToLower().Contains(filter.ToLower())
                    || report.Branch.ToLower().Contains(filter.ToLower())
                    || report.Prefix.ToLower().Contains(filter.ToLower())
                    || report.Number.ToLower().Contains(filter.ToLower());
                };
            }
            catch { }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void NewRecord()
        {
            selectedRecord = new ExtendedDocumentAdviceList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy)
        {
            selectedRecord = (ExtendedDocumentAdviceList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord()
        {
            selectedRecord = (ExtendedDocumentAdviceList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessage(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative)
            {
                DBResultMessage dBResult = await ApiCommunication.DeleteApiRequest(ApiUrls.DocumentAdviceList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.recordCount == 0) await MainWindow.ShowMessage(true, "Exception Error : " + dBResult.ErrorMessage);
                _ = LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (ExtendedDocumentAdviceList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgListView.SelectedItems.Count > 0)
            { selectedRecord = (ExtendedDocumentAdviceList)DgListView.SelectedItem;
            } else { selectedRecord = new ExtendedDocumentAdviceList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        //change dataset save method
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.BranchId = ((BranchList)cb_branch.SelectedItem).Id;
                selectedRecord.DocumentTypeId = ((DocumentTypeList)cb_documentType.SelectedItem).Id;
                selectedRecord.Prefix = txt_prefix.Text;
                selectedRecord.Number = txt_number.Text;
                selectedRecord.StartDate = (DateTime)dp_startDate.Value;
                selectedRecord.EndDate = (DateTime)dp_endDate.Value;
                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.TimeStamp = DateTimeOffset.Now.DateTime;


                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin")
                    selectedRecord.UserId = ((UserList)cb_owner.SelectedItem).Id;


                selectedRecord.Branch = selectedRecord.DocumentType = null;
                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0)
                { dBResult = await ApiCommunication.PutApiRequest(ApiUrls.DocumentAdviceList, httpContent, null, App.UserData.Authentification.Token);
                } else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.DocumentAdviceList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.recordCount > 0)
                {
                    selectedRecord = new ExtendedDocumentAdviceList();
                    await LoadDataList();
                    SetRecord(false);
                } else { await MainWindow.ShowMessage(true, "Exception Error : " + dBResult.ErrorMessage); }
            }
            catch { }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (ExtendedDocumentAdviceList)DgListView.SelectedItem : new ExtendedDocumentAdviceList();
            SetRecord(false);
        }

        //change dataset prepare for working
        private void SetRecord(bool showForm, bool copy = false)
        {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;

            cb_branch.Text = selectedRecord.Branch;
            cb_documentType.Text = (string.IsNullOrWhiteSpace(selectedRecord.DocumentType)) ? null : selectedRecord.DocumentType.ToString();
            txt_prefix.Text = selectedRecord.Prefix;
            txt_number.Text = selectedRecord.Number;
            dp_startDate.Value = (selectedRecord.Id == 0) ? (DateTime)dp_startDate.Value : selectedRecord.StartDate;
            dp_endDate.Value = (selectedRecord.Id == 0) ? (DateTime)dp_endDate.Value : selectedRecord.EndDate;

            //Only for Admin: Owner/UserId Selection
            if (App.UserData.Authentification.Role == "Admin")
                cb_owner.Text = adminUserList.Where(a => a.Id == selectedRecord.UserId).Select(a => a.UserName).FirstOrDefault();

            if (showForm)
            {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true; 
            } else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
            }
        }

    }
}