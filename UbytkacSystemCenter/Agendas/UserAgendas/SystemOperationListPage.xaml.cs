using EasyITSystemCenter.Api;
using EasyITSystemCenter.GlobalClasses;
using EasyITSystemCenter.GlobalClasses;
using EasyITSystemCenter.GlobalGenerators;
using EasyITSystemCenter.GlobalOperations;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EasyITSystemCenter.Pages {

    public partial class SystemOperationListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static ServerToolPanelDefinitionList selectedRecord = new ServerToolPanelDefinitionList();

        private List<SolutionMixedEnumList> mixedEnumTypesList = new List<SolutionMixedEnumList>();
        private List<SolutionOperationList> solutionOperationList = new List<SolutionOperationList>();
        //private List<SystemSvgIconList> systemSvgIconList = new List<SystemSvgIconList>();

        public SystemOperationListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);

            _ = LoadDataList();
            SetRecord(false);
        }

        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {
                mixedEnumTypesList = await CommunicationManager.GetApiRequest<List<SolutionMixedEnumList>>(ApiUrls.SolutionMixedEnumList, "ByGroup/OperationTypes", App.UserData.Authentification.Token);
                //systemSvgIconList = await CommApi.GetApiRequest<List<SystemSvgIconList>>(ApiUrls.SystemSvgIconList, null, App.UserData.Authentification.Token);
                solutionOperationList = await CommunicationManager.GetApiRequest<List<SolutionOperationList>>(ApiUrls.SolutionOperationList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);

                //Generate Menu Panel
                TabMenuList.Items.Clear();
                mixedEnumTypesList.ForEach(async operationType => {
                    try {
                        WrapPanel tabMenuPanel = new WrapPanel() { Name = "wp_" + operationType.Id.ToString(), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                        TabItem tabMenu = new TabItem() { Name = Regex.Replace(operationType.Name, @"[^a-zA-Z]", "_"), Header = await DBOperations.DBTranslation(operationType.Name), Content = tabMenuPanel };
                        TabMenuList.Items.Add(tabMenu);
                    } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
                });

                foreach (SolutionOperationList panel in solutionOperationList) {
                    var toolPanel = new Tile() {
                        Tag = panel.Id.ToString(),
                        Name = Regex.Replace(panel.Name, @"[^a-zA-Z]", "_"),
                        Title = await DBOperations.DBTranslation(panel.Name),
                        Background = (Brush)new BrushConverter().ConvertFrom("#2B91C4"),
                        Width = 200,
                        Height = 200,
                        Margin = new Thickness(3),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Cursor = Cursors.Hand,
                        FontWeight = FontWeights.DemiBold,
                        HorizontalTitleAlignment = HorizontalAlignment.Left,
                        TitleFontSize = 20,
                        VerticalTitleAlignment = VerticalAlignment.Bottom,
                        ClickMode = ClickMode.Press,
                        ToolTip = (!string.IsNullOrWhiteSpace(panel.Description)) ? panel.Description : null
                    };

                    BitmapImage spinner = new BitmapImage();
                    switch (panel.InheritedTypeName) {
                        case "DB_SP_GET_Operace":
                            spinner = IconMaker.Icon((Color)ColorConverter.ConvertFromString("#C48C2B"), App.SystemSvgIconList.FirstOrDefault(a => a.Name.ToLower() == "spinner").SvgIconPath);
                            break;

                        case "DB_SP_POST_Operace":
                            spinner = IconMaker.Icon((Color)ColorConverter.ConvertFromString("#C48C2B"), App.SystemSvgIconList.FirstOrDefault(a => a.Name.ToLower() == "spinner2").SvgIconPath);
                            break;

                        case "API_GET_Request":
                            spinner = IconMaker.Icon((Color)ColorConverter.ConvertFromString("#C48C2B"), App.SystemSvgIconList.FirstOrDefault(a => a.Name.ToLower() == "spinner4").SvgIconPath);
                            break;

                        case "API_POST_Request":
                            spinner = IconMaker.Icon((Color)ColorConverter.ConvertFromString("#C48C2B"), App.SystemSvgIconList.FirstOrDefault(a => a.Name.ToLower() == "spinner5").SvgIconPath);
                            break;

                        default:
                            spinner = IconMaker.Icon((Color)ColorConverter.ConvertFromString("#C48C2B"), App.SystemSvgIconList.FirstOrDefault(a => a.Name.ToLower() == "spinner3").SvgIconPath);
                            break;
                    }

                    Image icon = new Image() { Width = 120, Height = 120, Source = spinner };
                    toolPanel.Content = icon;
                    toolPanel.Click += ToolPanelListPage_Click;
                    ((WrapPanel)TabMenuList.FindChild<TabItem>(Regex.Replace(solutionOperationList.Where(a => a.Id == int.Parse(toolPanel.Tag.ToString())).First().InheritedTypeName, @"[^a-zA-Z]", "_")).Content).Children.Add(toolPanel);
                }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        private async void ToolPanelListPage_Click(object sender, RoutedEventArgs e) {
            MainWindow.progressRing = Visibility.Visible;
            SolutionOperationList selectedPanel = solutionOperationList.Where(a => a.Id == int.Parse(((Tile)sender).Tag.ToString())).FirstOrDefault();
            List<CustomOneRowList> messageResponse = null;
            List<GenericValue> jsonResponse = null; string json = null;

            try { //Request
                switch (selectedPanel.InheritedTypeName) {
                    case "DB_SP_GET_Operace":
                        if (selectedPanel.InheritedResultTypeName == "message") {
                            messageResponse = await CommunicationManager.GetApiRequest<List<CustomOneRowList>>(ApiUrls.StoredProceduresList, "SpProcedure/Message/" + selectedPanel.InputData, App.UserData.Authentification.Token);
                            json = messageResponse[0].MessageList;
                        }
                        else {
                            jsonResponse = await CommunicationManager.GetApiRequest<List<GenericValue>>(ApiUrls.StoredProceduresList, "SpProcedure/Json/" + selectedPanel.InputData, App.UserData.Authentification.Token);
                            JavaScriptSerializer serializer = new JavaScriptSerializer(); json = "{";
                            jsonResponse.ForEach(key => {
                                DeserializedJson jsonObject = serializer.Deserialize<DeserializedJson>(key.Value);
                                bool isNumber = int.TryParse(jsonObject.Value, out int res);
                                json += (json != "{" ? "," : "") + "\"" + jsonObject.Key + "\":" + (!isNumber ? "\"" + jsonObject.Value + "\"" : jsonObject.Value);
                            }); json += "}";
                        }
                        break;

                    case "DB_SP_POST_Operace":
                        //response = await CommApi.PostApiRequest<List<GlobalClasses.Message>>(ApiUrls.SystemOperations, selectedPanel.InputData, App.UserData.Authentification.Token);
                        //json = response[0].MessageList;
                        break;

                    case "API_GET_Request":
                        using (HttpClient httpClient = new HttpClient()) {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.UserData.Authentification.Token);
                            json = await httpClient.GetStringAsync(App.appRuntimeData.AppClientSettings.First(b => b.Key == "conn_apiAddress").Value + "/" + (selectedPanel.InputData.StartsWith("/") ? selectedPanel.InputData.Substring(1) : selectedPanel.InputData));
                        }
                        break;

                    case "API_POST_Request":
                        break;
                }
                MainWindow.progressRing = Visibility.Hidden;
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }

            try {
               
                //Response
                switch (selectedPanel.InheritedResultTypeName) {
                    case "message":
                        if (json == null) { await MainWindow.ShowMessageOnMainWindow(true, await DBOperations.DBTranslation("EmptyResponse")); }
                        else { await MainWindow.ShowMessageOnMainWindow(false, json); }
                        break;

                    case "File":
                        try {
                            if (json == null) { await MainWindow.ShowMessageOnMainWindow(true, await DBOperations.DBTranslation("EmptyResponse")); }
                            else {
                                SaveFileDialog dlg = new SaveFileDialog { DefaultExt = ".*", Filter = "All files |*.*", Title = Resources["fileOpenDescription"].ToString() };
                                if (dlg.ShowDialog() == true) { FileOperations.WriteToFile(dlg.FileName, json); }
                            }
                        } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
                        break;
                }
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
        }

        private void SetRecord(bool? showForm = null) {
            MainWindow.DataGridSelected = MainWindow.DataGridSelectedIdListIndicator = false; MainWindow.dataGridSelectedId = 0; MainWindow.DgRefresh = false;
            dataViewSupport.FormShown = true;
        }
    }
}