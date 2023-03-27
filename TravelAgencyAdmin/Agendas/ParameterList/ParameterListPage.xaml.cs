using Newtonsoft.Json;
using TravelAgencyAdmin.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Web;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.IO;
using TravelAgencyAdmin;
using Org.BouncyCastle.Asn1.X509;
using System.Threading.Tasks;
using TravelAgencyAdmin.GlobalFunctions;
using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.GlobalStyles;
using MahApps.Metro.Controls.Dialogs;
using TravelAgencyAdmin.GlobalClasses;
using System.Globalization;

namespace TravelAgencyAdmin.Pages
{
    public partial class ParameterListPage : UserControl
    {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static ParameterList selectedRecord = new ParameterList();

        //Param types list
        private readonly ObservableCollection<ReportSelection> ParamTypes = new ObservableCollection<ReportSelection>() {
                                                               new ReportSelection() { Name = "bit" },new ReportSelection() { Name = "string" },new ReportSelection() { Name = "int" },new ReportSelection() { Name = "numeric" },new ReportSelection() { Name = "date" },new ReportSelection() { Name = "time" },new ReportSelection() { Name = "datetime" },
                                                             };
        private List<ParameterList> parametersList = new List<ParameterList>();
        private List<UserList> adminUserList = new List<UserList>();
        public ParameterListPage()
        {
            InitializeComponent();
            _ = MediaFunctions.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            //translate fields in detail form
            lbl_id.Content = Resources["id"].ToString();
            lbl_parameter.Content = Resources["parameter"].ToString();
            lbl_value.Content = Resources["value"].ToString();
            lbl_type.Content = Resources["type"].ToString();
            lbl_description.Content = Resources["description"].ToString();
            lbl_timestamp.Content = Resources["timestamp"].ToString();
            lbl_owner.Content = Resources["owner"].ToString();

            btn_check.Content = Resources["check"].ToString();
            btn_save.Content = Resources["btn_save"].ToString();
            btn_cancel.Content = Resources["btn_cancel"].ToString();


            cb_type.ItemsSource = ParamTypes;

            _ = LoadDataList();
            SetRecord(false);
        }

        //change datasource
        public async Task<bool> LoadDataList()
        {
            MainWindow.ProgressRing = Visibility.Visible;
            try { 
                parametersList = await ApiCommunication.GetApiRequest<List<ParameterList>>(ApiUrls.ParameterList, App.UserData.Authentification.Id.ToString(), App.UserData.Authentification.Token);
                parametersList.ForEach(parameter => { parameter.Translation = SystemFunctions.DBTranslation(parameter.SystemName); });

                DgListView.ItemsSource = parametersList;
                DgListView.Items.Refresh();

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin")
                {
                    cb_owner.ItemsSource = adminUserList = await ApiCommunication.GetApiRequest<List<UserList>>(ApiUrls.UserList, null, App.UserData.Authentification.Token);
                    lbl_owner.Visibility = cb_owner.Visibility = Visibility.Visible;
                }

            }
            catch { }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }


        // set translate columns in listView
        private void DgListView_Translate(object sender, EventArgs ex)
        {
            ((DataGrid)sender).Columns.ToList().ForEach(e =>
            {
                string headername = e.Header.ToString();
                if (headername == "Translation") { e.Header = Resources["parameter"].ToString(); e.DisplayIndex = 1; }
                else if (headername == "Value") { e.Header = Resources["value"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; }
                else if (headername == "Type") { e.Header = Resources["type"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; }
                else if (headername == "Description") { e.Header = Resources["description"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; }
                else if (headername == "Active") { e.Header = Resources["active"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 2; }
                else if (headername == "TimeStamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = DatagridStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }

                else if (headername == "Id") e.DisplayIndex = 0;
                else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                else if (headername == "SystemName") e.Visibility = Visibility.Hidden;
            });
        }


        //change filter fields
        public void Filter(string filter)
        {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    ParameterList param = e as ParameterList;
                    return param.SystemName.ToLower().Contains(filter.ToLower())
                    || param.Value.ToLower().Contains(filter.ToLower())
                    || param.Type.ToLower().Contains(filter.ToLower())
                    || param.Type.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(param.Description) && param.Description.ToLower().Contains(filter.ToLower())
                    ;
                };
            } catch { }
        }

        public void NewRecord()
        {
            selectedRecord = new ParameterList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy)
        {
            selectedRecord = (ParameterList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord()
        {
            selectedRecord = (ParameterList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessage(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative)
            {
                DBResultMessage dBResult = await ApiCommunication.DeleteApiRequest(ApiUrls.ParameterList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.recordCount == 0) await MainWindow.ShowMessage(true, "Exception Error : " + dBResult.ErrorMessage);
                _ = LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (ParameterList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgListView.SelectedItems.Count > 0)
            { selectedRecord = (ParameterList)DgListView.SelectedItem;
            } else { selectedRecord = new ParameterList(); }
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
                selectedRecord.SystemName = txt_parameter.Text;
                selectedRecord.Value = txt_value.Text;
                selectedRecord.Type = ((ReportSelection)cb_type.SelectedItem).Name;
                selectedRecord.Description = txt_description.Text;
                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.TimeStamp = DateTimeOffset.Now.DateTime;

                //Only for Admin: Owner/UserId Selection
                if (App.UserData.Authentification.Role == "Admin")
                    selectedRecord.UserId = ((UserList)cb_owner.SelectedItem).Id;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0)
                { dBResult = await ApiCommunication.PutApiRequest(ApiUrls.ParameterList, httpContent, null, App.UserData.Authentification.Token);
                } else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.ParameterList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.recordCount > 0)
                {
                    // Refresh Param in memory
                    if (App.Parameters.Where(a => a.SystemName == selectedRecord.SystemName).Count() == 0)
                    { App.Parameters.Add(selectedRecord); }
                    else { App.Parameters.Where(a => a.SystemName == selectedRecord.SystemName).First().Value = selectedRecord.Value; }

                    selectedRecord = new ParameterList();
                    await LoadDataList();
                    SetRecord(false);
                } else { await MainWindow.ShowMessage(true, "Exception Error : " + dBResult.ErrorMessage); }
            }
            catch { }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (ParameterList)DgListView.SelectedItem : new ParameterList();
            SetRecord(false);
        }

        //change dataset prepare for working
        private void SetRecord(bool showForm, bool copy = false)
        {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;

            txt_parameter.Text = selectedRecord.SystemName;
            txt_value.Text = selectedRecord.Value;
            cb_type.Text = selectedRecord.Type;
            txt_description.Text = selectedRecord.Description;
            lbl_translation.Content = selectedRecord.Translation;

            //Only for Admin: Owner/UserId Selection
            if (App.UserData.Authentification.Role == "Admin")
                cb_owner.Text = adminUserList.Where(a => a.Id == selectedRecord.UserId).Select(a => a.UserName).FirstOrDefault();

            if (showForm) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
            } else {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
            }
        }

        private void Value_TextChanged(object sender, TextChangedEventArgs e) => btn_save.IsEnabled = false;
        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e) => btn_save.IsEnabled = false;

        private void CopyToClipClick(object sender, MouseButtonEventArgs e) { Clipboard.SetDataObject(lbl_translation.Content.ToString()); } 

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_save.IsEnabled = false;
                switch ((string)cb_type.SelectedValue)
                {
                    case "bit":
                        lbl_paramCheckResult.Content = bool.Parse(txt_value.Text);
                        break;
                    case "int":
                        lbl_paramCheckResult.Content = int.Parse(txt_value.Text);
                        break;
                    case "numeric":
                        lbl_paramCheckResult.Content = double.Parse(txt_value.Text);
                        break;
                    case "date":
                        //DateTime.ParseExact(DateTime.Now.ToShortDateString(), txt_value.Text, CultureInfo.InvariantCulture);
                        lbl_paramCheckResult.Content = DateTime.Now.ToString(txt_value.Text);
                        break;
                    case "time":
                        lbl_paramCheckResult.Content = DateTime.Now.ToString(txt_value.Text);
                        break;
                    case "datetime":
                        lbl_paramCheckResult.Content = DateTime.Now.ToString(txt_value.Text);
                        break;
                }
                btn_save.IsEnabled = true;
            }
            catch { }
        }

    }
}