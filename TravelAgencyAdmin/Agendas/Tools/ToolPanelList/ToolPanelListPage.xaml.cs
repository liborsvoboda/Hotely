using UbytkacAdmin.Api;
using UbytkacAdmin.Classes;
using UbytkacAdmin.GlobalGenerators;
using UbytkacAdmin.GlobalOperations;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace UbytkacAdmin.Pages {

    public partial class ToolPanelListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static ToolPanelDefinitionList selectedRecord = new ToolPanelDefinitionList();

        private List<ToolPanelDefinitionList> toolPanelDefinitionList = new List<ToolPanelDefinitionList>();
        private List<SvgIconList> svgIconList = new List<SvgIconList>();
        private List<ToolTypeList> toolTypeList = new List<ToolTypeList>();


        public ToolPanelListPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            //try {
            //    ti_onlineManuals.Header = DBOperations.DBTranslation("onlineManuals");
            //    ti_docTools.Header = DBOperations.DBTranslation("docTools");
            //    ti_serverDeveloperHelp.Header = DBOperations.DBTranslation("ServerDeveloperHelp");
            //    ti_systemDeveloperHelp.Header = DBOperations.DBTranslation("SystemDeveloperHelp");
            //    ti_marketing.Header = DBOperations.DBTranslation("Marketing");
            //} catch { }

            _ = LoadDataList();
            SetRecord(false);
        }

        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try {

                //TabMenuList.Items.Clear();
                //wp_onlineManuals.Children.Clear();
                //wp_docTools.Children.Clear();
                //wp_webTools.Children.Clear();
                //wp_serverTools.Children.Clear();
                //wp_solutionTools.Children.Clear();
                //wp_moreProducts.Children.Clear();
                //wp_devTools.Children.Clear();



                TabMenuList.Items.Clear();
                toolTypeList = await ApiCommunication.GetApiRequest<List<ToolTypeList>>(ApiUrls.ToolTypeList, null, App.UserData.Authentification.Token);
                toolPanelDefinitionList = await ApiCommunication.GetApiRequest<List<ToolPanelDefinitionList>>(ApiUrls.ToolPanelDefinitionList, (dataViewSupport.AdvancedFilter == null) ? null : "Filter/" + WebUtility.UrlEncode(dataViewSupport.AdvancedFilter.Replace("[!]", "").Replace("{!}", "")), App.UserData.Authentification.Token);
                svgIconList = await ApiCommunication.GetApiRequest<List<SvgIconList>>(ApiUrls.SvgIconList, null, App.UserData.Authentification.Token);

                toolTypeList.ForEach(async toolType => {
                    WrapPanel tabMenuPanel = new WrapPanel() { Name="wp_" + toolType.Name , HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center};
                    TabItem tabMenu = new TabItem() { Name = toolType.Name, Header = await DBOperations.DBTranslation(toolType.Name), Content = tabMenuPanel };
                    TabMenuList.Items.Add(tabMenu);
                });


                foreach (ToolPanelDefinitionList panel in toolPanelDefinitionList) {
                    panel.ToolTypeName = toolTypeList.First(a => a.Id == panel.ToolTypeId).Name;
                    panel.BitmapImage = IconMaker.Icon((Color)ColorConverter.ConvertFromString(panel.IconColor), svgIconList.FirstOrDefault(a => a.Name == panel.IconName).SvgIconPath);

                    var toolPanel = new Tile() {
                        Title = await DBOperations.DBTranslation(panel.SystemName),
                        Background = (Brush)new BrushConverter().ConvertFromString(panel.BackgroundColor),
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
                        ToolTip = (!string.IsNullOrWhiteSpace(panel.Description)) ? panel.Description : null,
                        Tag = panel.Id
                    };

                    Image icon = new Image() { Width = 120, Height = 120, Source = panel.BitmapImage };

                    toolPanel.Content = icon;
                    toolPanel.Click += ToolPanelListPage_Click;
                    ((WrapPanel)TabMenuList.FindChild<TabItem>(toolPanelDefinitionList.Where(a => a.Id == int.Parse(toolPanel.Tag.ToString())).First().ToolTypeName).Content).Children.Add(toolPanel);
/*
                     switch (toolPanelDefinitionList.Where(a => a.Id == int.Parse(toolPanel.Tag.ToString())).First().ToolTypeName) {
                        case "OnlineManuals":
                            wp_onlineManuals.Children.Add(toolPanel);
                            break;
                        case "DocTools":
                            wp_docTools.Children.Add(toolPanel);
                            break;
                        case "WebTools":
                            wp_webTools.Children.Add(toolPanel);
                            break;
                        case "ServerTools":
                            wp_serverTools.Children.Add(toolPanel);
                            break;
                        case "SolutionTools":
                            wp_solutionTools.Children.Add(toolPanel);
                            break;
                        case "MoreProducts":
                            wp_moreProducts.Children.Add(toolPanel);
                            break;
                        case "DevTools":
                            wp_devTools.Children.Add(toolPanel);
                            break;
                            
                     }
*/
                }

            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        private void ToolPanelListPage_Click(object sender, RoutedEventArgs e) {
            var selectedPanel = toolPanelDefinitionList.Where(a => a.Id == int.Parse(((Tile)sender).Tag.ToString())).FirstOrDefault();
            SystemOperations.StartExternalProccess(selectedPanel.Type, (selectedPanel.Type == "EDCservice" ? App.Setting.ApiAddress : "" ) + selectedPanel.Command);
        }

        private void SetRecord(bool showForm) {
            MainWindow.DataGridSelected = MainWindow.DataGridSelectedIdListIndicator = false; MainWindow.dataGridSelectedId = 0; MainWindow.DgRefresh = false;
            dataViewSupport.FormShown = true;
        }


    }
}