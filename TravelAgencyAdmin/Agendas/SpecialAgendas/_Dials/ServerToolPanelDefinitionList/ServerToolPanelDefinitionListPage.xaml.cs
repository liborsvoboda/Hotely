using UbytkacAdmin.Api;
using UbytkacAdmin.Classes;
using UbytkacAdmin.GlobalClasses;
using UbytkacAdmin.GlobalGenerators;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UbytkacAdmin.Pages {

    public partial class ServerToolPanelDefinitionListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static ServerToolPanelDefinitionList selectedRecord = new ServerToolPanelDefinitionList();

        public static ServerToolPanelDefinitionList generatedIcon = new ServerToolPanelDefinitionList() {
            SystemName = "",
            IconName = "star",
            IconColor = "RoyalBlue",
            BackgroundColor = "DarkSeaGreen",
            Description = ""
            ,
            BitmapImage = new BitmapImage(ResourceAccessor.Get())
        };

        private List<ServerToolPanelDefinitionList> toolPanelDefinitionList = new List<ServerToolPanelDefinitionList>();
        private List<SystemSvgIconList> svgIconList = new List<SystemSvgIconList>();
        private List<ServerToolTypeList> toolTypeList = new List<ServerToolTypeList>();

        private bool pageLoaded = false;

        public ServerToolPanelDefinitionListPage() {
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
                toolPanelDefinitionList = await CommApi.GetApiRequest<List<ServerToolPanelDefinitionList>>(ApiUrls.ServerToolPanelDefinitionList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                svgIconList = await CommApi.GetApiRequest<List<SystemSvgIconList>>(ApiUrls.SystemSvgIconList, null, App.UserData.Authentification.Token);
                toolTypeList = await CommApi.GetApiRequest<List<ServerToolTypeList>>(ApiUrls.ServerToolTypeList, null, App.UserData.Authentification.Token);

                if (svgIconList.Any()) { lv_iconViewer.Items.Clear(); }
                svgIconList.ForEach(icon => {
                    try {
                        icon.BitmapImage = IconMaker.Icon(Colors.RoyalBlue, icon.SvgIconPath);
                    } catch { }
                    lv_iconViewer.Items.Add(icon);
                });

                toolPanelDefinitionList.ForEach(async item => {
                    item.Translation = await DBOperations.DBTranslation(item.SystemName);
                    item.ToolTypeName = toolTypeList.First(a => a.Id == item.ToolTypeId).Name;
                });
                cb_type.ItemsSource = SystemLocalEnumSets.ProcessTypes;
                cb_toolType.ItemsSource = toolTypeList;
                DgListView.ItemsSource = toolPanelDefinitionList;
                DgListView.Items.Refresh();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        private void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "SystemName") { e.Header = Resources["systemName"].ToString(); e.DisplayIndex = 1; }
                    else if (headername == "Translation") { e.Header = Resources["translation"].ToString(); e.DisplayIndex = 2; }
                    else if (headername == "ToolTypeName") { e.Header = Resources["toolType"].ToString(); e.DisplayIndex = 3; }
                    else if (headername == "Type") e.Header = Resources["processType"].ToString();
                    else if (headername == "Command") e.Header = Resources["command"].ToString();
                    else if (headername == "Description") e.Header = Resources["description"].ToString();
                    else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }
                    else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "ToolTypeId") e.Visibility = Visibility.Hidden;
                    else if (headername == "BitmapImage") e.Visibility = Visibility.Hidden;
                    else if (headername == "IconName") e.Visibility = Visibility.Hidden;
                    else if (headername == "IconColor") e.Visibility = Visibility.Hidden;
                    else if (headername == "BackgroundColor") e.Visibility = Visibility.Hidden;
                    else if (headername == "SvgIconPath") e.Visibility = Visibility.Hidden;
                    else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    ServerToolPanelDefinitionList user = e as ServerToolPanelDefinitionList;
                    return user.SystemName.ToLower().Contains(filter.ToLower())
                    || user.Translation.ToLower().Contains(filter.ToLower())
                    || user.Type.ToLower().Contains(filter.ToLower())
                    || user.Command.ToLower().Contains(filter.ToLower())
                    || !string.IsNullOrEmpty(user.Description) && user.Description.ToLower().Contains(filter.ToLower());
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        public void NewRecord() {
            selectedRecord = new ServerToolPanelDefinitionList();
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        public void EditRecord(bool copy) {
            selectedRecord = (ServerToolPanelDefinitionList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true, copy);
        }

        public async void DeleteRecord() {
            selectedRecord = (ServerToolPanelDefinitionList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            MessageDialogResult result = await MainWindow.ShowMessageOnMainWindow(false, Resources["deleteRecordQuestion"].ToString() + " " + selectedRecord.Id.ToString(), true);
            if (result == MessageDialogResult.Affirmative) {
                DBResultMessage dBResult = await CommApi.DeleteApiRequest(ApiUrls.ServerToolPanelDefinitionList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token);
                if (dBResult.RecordCount == 0) await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + dBResult.ErrorMessage);
                await LoadDataList(); SetRecord(false);
            }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (ServerToolPanelDefinitionList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(true);
        }

        private void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (DgListView.SelectedItems.Count > 0) { selectedRecord = (ServerToolPanelDefinitionList)DgListView.SelectedItem; }
            else { selectedRecord = new ServerToolPanelDefinitionList(); }
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord(false);
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            try {
                DBResultMessage dBResult;
                selectedRecord.Id = (int)((txt_id.Value != null) ? txt_id.Value : 0);
                selectedRecord.SystemName = txt_systemName.Text;

                selectedRecord.ToolTypeId = ((ServerToolTypeList)cb_toolType.SelectedItem).Id;
                selectedRecord.Type = ((Language)cb_type.SelectedItem).Value;
                selectedRecord.Command = txt_command.Text;
                selectedRecord.IconName = generatedIcon.IconName;
                selectedRecord.IconColor = xct_iconColor.SelectedColorText;
                selectedRecord.BackgroundColor = xct_backgroundColor.SelectedColorText;
                selectedRecord.Description = txt_description.Text;

                selectedRecord.UserId = App.UserData.Authentification.Id;
                selectedRecord.Timestamp = DateTimeOffset.Now.DateTime;

                string json = JsonConvert.SerializeObject(selectedRecord);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                if (selectedRecord.Id == 0) {
                    dBResult = await CommApi.PutApiRequest(ApiUrls.ServerToolPanelDefinitionList, httpContent, null, App.UserData.Authentification.Token);
                }
                else { dBResult = await CommApi.PostApiRequest(ApiUrls.ServerToolPanelDefinitionList, httpContent, null, App.UserData.Authentification.Token); }

                if (dBResult.RecordCount > 0) {
                    selectedRecord = new ServerToolPanelDefinitionList();
                    await LoadDataList();
                    SetRecord(null);
                }
                else { await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + dBResult.ErrorMessage); }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            selectedRecord = (DgListView.SelectedItems.Count > 0) ? (ServerToolPanelDefinitionList)DgListView.SelectedItem : new ServerToolPanelDefinitionList();
            SetRecord(false);
        }

        private void SetRecord(bool? showForm = null, bool copy = false) {
            txt_id.Value = (copy) ? 0 : selectedRecord.Id;

            txt_systemName.Text = generatedIcon.SystemName = selectedRecord.SystemName;
            generatedIcon.IconName = selectedRecord.IconName;

            cb_toolType.SelectedItem = (selectedRecord.Id == 0 || toolTypeList.Count == 0) ? toolTypeList.FirstOrDefault() : toolTypeList.First(a => a.Id == selectedRecord.ToolTypeId);
            cb_type.SelectedItem = (selectedRecord.Id == 0) ? SystemLocalEnumSets.ProcessTypes.FirstOrDefault() : SystemLocalEnumSets.ProcessTypes.First(a => a.Value == selectedRecord.Type);
            txt_command.Text = selectedRecord.Command;

            if (selectedRecord.Id != 0) { generatedIcon.IconColor = selectedRecord.IconColor; xct_iconColor.SelectedColor = (Color)ColorConverter.ConvertFromString(selectedRecord.IconColor); }
            if (selectedRecord.Id != 0) { generatedIcon.BackgroundColor = selectedRecord.BackgroundColor; xct_backgroundColor.SelectedColor = (Color)ColorConverter.ConvertFromString(selectedRecord.BackgroundColor); }

            txt_description.Text = generatedIcon.Description = selectedRecord.Description;

            if (showForm != null && showForm == true) {
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = false;
                ListView.Visibility = Visibility.Hidden; ListForm.Visibility = Visibility.Visible; dataViewSupport.FormShown = true;
                pageLoaded = true; GeneratedIconChanged();
            }
            else {
                pageLoaded = false;
                MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = selectedRecord.Id != 0; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
                ListForm.Visibility = Visibility.Hidden; ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = showForm == null && !bool.Parse(App.appRuntimeData.AppClientSettings.First(a => a.Key == "beh_CloseFormAfterSave").Value);
            }
        }

        private void IconSelect_Click(object sender, SelectionChangedEventArgs e) {
            try {
                if (((ListView)sender).SelectedItems.Count > 0) {
                    generatedIcon.IconName = ((SystemSvgIconList)((ListView)sender).SelectedItem).Name;
                    GeneratedIconChanged();
                }
            } catch { }
        }

        private void ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            if (((Xceed.Wpf.Toolkit.ColorPicker)sender).Name == "xct_iconColor") { generatedIcon.IconColor = ((Xceed.Wpf.Toolkit.ColorPicker)sender).SelectedColorText; }
            else if (((Xceed.Wpf.Toolkit.ColorPicker)sender).Name == "xct_backgroundColor") { generatedIcon.BackgroundColor = ((Xceed.Wpf.Toolkit.ColorPicker)sender).SelectedColorText; }
            GeneratedIconChanged();
        }

        private void TextChanged(object sender, TextChangedEventArgs e) {
            if (((TextBox)sender).Name == "txt_systemName") {
                generatedIcon.SystemName = ((TextBox)sender).Text;
            }
            else if (((TextBox)sender).Name == "txt_command") {
                generatedIcon.Command = ((TextBox)sender).Text;
            }
            else if (((TextBox)sender).Name == "txt_description") { generatedIcon.Description = ((TextBox)sender).Text; }

            GeneratedIconChanged();
        }

        private async void GeneratedIconChanged() {
            if (pageLoaded && svgIconList.Any()) {
                t_generatedIcon.Title = await DBOperations.DBTranslation(generatedIcon.SystemName, true);
                t_generatedIcon.Foreground = (Brush)new BrushConverter().ConvertFromString(generatedIcon.IconColor);
                t_generatedIcon.Background = (Brush)new BrushConverter().ConvertFromString(generatedIcon.BackgroundColor);
                t_generatedIcon.ToolTip = generatedIcon.Description;
                i_generatedIcon.Source = string.IsNullOrWhiteSpace(generatedIcon.IconName) ? new BitmapImage(ResourceAccessor.Get())
                    : IconMaker.Icon((Color)ColorConverter.ConvertFromString(generatedIcon.IconColor), svgIconList.Where(a => a.Name == generatedIcon.IconName).Select(a => a.SvgIconPath).First());
            }
        }

        private void BtnCommandTest_Click(object sender, RoutedEventArgs e) {
            SystemOperations.StartExternalProccess(((Language)cb_type.SelectedItem).Value, (((Language)cb_type.SelectedItem).Value == "EDCservice" ? App.appRuntimeData.AppClientSettings.First(b => b.Key == "conn_apiAddress").Value : "") + txt_command.Text);
        }
    }
}