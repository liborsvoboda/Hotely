using EasyITSystemCenter.Api;
using EasyITSystemCenter.GlobalClasses;
using EasyITSystemCenter.GlobalOperations;
using EasyITSystemCenter.GlobalStyles;
using HelixToolkit.Wpf;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;


// This is Template ListView
namespace EasyITSystemCenter.Pages {

    public partial class BasicAttachmentListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static BasicViewAttachmentList selectedRecord = new BasicViewAttachmentList();

        public BasicAttachmentListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);
            webBrowser.EnsureCoreWebView2Async(App.appRuntimeData.WebViewEnvironment);
            

            _ = LoadDataList();
            SetRecord();
        }

        //change datasource
        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                DgListView.ItemsSource = await CommunicationManager.GetApiRequest<List<BasicViewAttachmentList>>(ApiUrls.BasicAttachmentList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        // set translate columns in listView
        private void DgListView_Translate(object sender, EventArgs ex) {
            try {
                ((DataGrid)sender).Columns.ToList().ForEach(e => {
                    string headername = e.Header.ToString();
                    if (headername == "Active") { e.Header = Resources["active"].ToString(); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 2; }
                    else if (headername == "PartNumber") { e.Header = Resources["partNumber"].ToString(); }
                    else if (headername == "FileName") { e.Header = Resources["fileName"].ToString(); }
                    else if (headername == "Timestamp") { e.Header = Resources["timestamp"].ToString(); e.CellStyle = ProgramaticStyles.gridTextRightAligment; e.DisplayIndex = DgListView.Columns.Count - 1; }
                    else if (headername == "Id") e.DisplayIndex = 0;
                    else if (headername == "UserId") e.Visibility = Visibility.Hidden;
                });
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        //change filter fields
        public void Filter(string filter) {
            try {
                if (filter.Length == 0) { dataViewSupport.FilteredValue = null; DgListView.Items.Filter = null; return; }
                dataViewSupport.FilteredValue = filter;
                DgListView.Items.Filter = (e) => {
                    BasicViewAttachmentList user = e as BasicViewAttachmentList;
                    return user.TimeStamp.ToShortDateString().ToLower().Contains(filter.ToLower())
                    || user.FileName.ToLower().Contains(filter.ToLower());
                };
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void DgListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (DgListView.SelectedItems.Count == 0) return;
            selectedRecord = (BasicViewAttachmentList)DgListView.SelectedItem;
            dataViewSupport.SelectedRecordId = selectedRecord.Id;
            SetRecord();
        }

        private async void DgListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                if (DgListView.SelectedItems.Count > 0) {
                    selectedRecord = (BasicViewAttachmentList)DgListView.SelectedItem;
                    dataViewSupport.SelectedRecordId = selectedRecord.Id;

                    viewPort3d.IsEnabled = webBrowser.IsEnabled = false;
                    viewPort3d.Visibility = webBrowser.Visibility = Visibility.Hidden;

                    string filePath = Path.Combine(App.appRuntimeData.tempFolder, selectedRecord.FileName);
                    FileOperations.ByteArrayToFile(filePath, (await CommunicationManager.GetApiRequest<BasicAttachmentList>(ApiUrls.BasicAttachmentList, selectedRecord.Id.ToString(), App.UserData.Authentification.Token)).Attachment);
                    switch (selectedRecord.FileName.ToLower().Split('.').Last()) {
                        case "stl":
                            viewPort3d.IsEnabled = true; viewPort3d.Visibility = Visibility.Visible; //viewPort3d.Viewport.Print("");
                            ModelVisual3D device3D = new ModelVisual3D { Content = await Display3d(filePath) };
                            viewPort3d.Children.Add(device3D); viewPort3d.ZoomExtents();
                            break;

                        default:
                            
                            webBrowser.IsEnabled = true; webBrowser.Visibility = Visibility.Visible;
                            webBrowser.Source = new Uri(filePath);
                            break;
                    }
                }
                else { selectedRecord = new BasicViewAttachmentList(); }
                dataViewSupport.SelectedRecordId = selectedRecord.Id;
                SetRecord();
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        //change dataset prepare for working
        private void SetRecord() {
            MainWindow.DataGridSelected = true; MainWindow.DataGridSelectedIdListIndicator = false; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
            ListView.Visibility = Visibility.Visible; dataViewSupport.FormShown = false;
        }

        private async Task<Model3D> Display3d(string modelPath) {
            Model3D device = null;
            try {
                viewPort3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);
                ModelImporter import = new ModelImporter();
                import.DefaultMaterial = new DiffuseMaterial(new SolidColorBrush(Color.FromRgb(255, 255, 255)));
                device = import.Load(modelPath);
            } catch (Exception ex) { await MainWindow.ShowMessageOnMainWindow(true, "Exception Error : " + ex.Message + Environment.NewLine + ex.StackTrace); }
            return device;
        }
    }
}