using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using TravelAgencyAdmin.Classes;
using Dragablz;
using System.ComponentModel;
using System.Reflection;
using System.Timers;
using System.Diagnostics;
using Newtonsoft.Json;
using MahApps.Metro;
using System.IO;
using TravelAgencyAdmin.Properties;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TravelAgencyAdmin.Pages;
using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.Extension;
using TravelAgencyAdmin.GlobalFunctions;
using TravelAgencyAdmin.Helper;
using System.Net.Http;
using TravelAgencyAdmin.GlobalClasses;

namespace TravelAgencyAdmin
{
    public partial class MainWindow : MetroWindow
    {
        #region Definitions
        private bool metroWindowClosing;
        private static bool _hackyIsFirstWindow = true;
        private readonly Timer timer10sec = new Timer() { Enabled = false, Interval = 1 };

        /// <summary>
        /// Variables for indicators
        /// </summary>
        public static bool dataGridSelected, dgIdSetted, dgRefresh, serviceRunning, userLogged, updateChecked = false;
        public static int dataGridSelectedId, downloadShow, downloadStatus = 0;
        public static string serviceStatus;
        public static Visibility progressRing = Visibility.Hidden;

        /// <summary>
        /// Handlers for indicators
        /// </summary>
        public static event EventHandler DataGridSelectedChanged, DataGridSelectedIdListIndicatorChanged, DgRefreshChanged, ServiceStatusChanged, ServiceRunningChanged, DownloadStatusChanged, DownloadShowChanged, ProgressRingChanged, UserLoggedChanged = delegate { };

        /// <summary>
        /// User logged indicator
        /// </summary>
        public static bool UserLogged
        {
            get => userLogged;
            set
            {
                userLogged = value;
                UserLoggedChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Service Status description
        /// </summary>
        public static bool ServiceRunning
        {
            get => serviceRunning;
            set
            {
                serviceRunning = value;
                ServiceRunningChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Indicator for enable Refresh Button Indicator
        /// </summary>
        public static bool DgRefresh
        {
            get => dgRefresh;
            set
            {
                dgRefresh = value;
                DgRefreshChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Indicator for Enable New datagrid Button
        /// </summary>
        public static bool DataGridSelected
        {
            get => dataGridSelected;
            set
            {
                dataGridSelected = value;
                DataGridSelectedChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Datagrid have selected record indicator
        /// </summary>
        public static bool DataGridSelectedIdListIndicator
        {
            get => dgIdSetted;
            set
            {
                dgIdSetted = value;
                DataGridSelectedIdListIndicatorChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Service Status public Variable
        /// </summary>
        public static string ServiceStatus
        {
            get => serviceStatus;
            set
            {
                serviceStatus = value;
                ServiceStatusChanged?.Invoke(null, EventArgs.Empty);
            }
        }


        /// <summary>
        /// Downloading of update status variable
        /// </summary>
        public static int DownloadStatus
        {

            get => downloadStatus;
            set
            {
                downloadStatus = value;
                DownloadStatusChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Indicator for show Downloading area
        /// </summary>
        public static int DownloadShow
        {

            get => downloadShow;
            set
            {
                downloadShow = value;
                DownloadShowChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// ProgressRing Visibility indicator
        /// </summary>
        public static Visibility ProgressRing
        {

            get => progressRing;
            set
            {
                progressRing = value;
                ProgressRingChanged?.Invoke(null, EventArgs.Empty);
            }
        }
        #endregion


        /// <summary>
        /// Initialize Application Main Window
        /// </summary>
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                MediaFunctions.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

                if (_hackyIsFirstWindow)
                {
                    ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));

                    mainWindow.Height = Settings.Default.Height; mainWindow.Width = Settings.Default.Width;
                    mainWindow.Left = Settings.Default.Left; mainWindow.Top = Settings.Default.Top;

                    if (App.Setting.TopMost) Topmost = true;
                }
                _hackyIsFirstWindow = false;

                mi_appearance.Header = Resources["appearance"].ToString(); mi_color.Header = Resources["color"].ToString(); btn_about.ToolTip = Resources["about"].ToString();


                //MENUS THIS IS FOR MANUAL UPDATE
                //Vertical main menu
                tv_dials.Header = Resources["dials"].ToString(); tv_crm.Header = Resources["crm"].ToString(); tv_agendas.Header = Resources["agendas"].ToString(); 
                tv_settings.Header = Resources["settings"].ToString(); tv_system.Header = Resources["system"].ToString();
                tv_accommodations.Header = Resources["accommodations"].ToString();

                //Standard Application menu
                tm_reportList.Header = Resources["reportList"].ToString(); tm_calendar.Header = Resources["calendar"].ToString();
                tm_userList.Header = Resources["userList"].ToString(); tm_userRoleList.Header = Resources["userRoleList"].ToString();
                tm_clientSettings.Header = Resources["clientSettings"].ToString(); tm_loginHistoryList.Header = Resources["loginHistoryList"].ToString();
                tm_support.Header = Resources["support"].ToString(); tm_documentAdviceList.Header = Resources["documentAdviceList"].ToString();
                tm_currencyList.Header = Resources["currencyList"].ToString();tm_exchangeRateList.Header = Resources["exchangeRateList"].ToString();

                tm_addressList.Header = Resources["addressList"].ToString(); tm_parameterList.Header = Resources["parameterList"].ToString();
                tm_branchList.Header = Resources["branchList"].ToString(); tm_reportQueueList.Header = Resources["reportQueueList"].ToString();
                tm_languageList.Header = Resources["languageList"].ToString(); tm_documentTypeList.Header = Resources["documentTypeList"].ToString();
                tm_systemFailList.Header = Resources["systemFailList"].ToString(); tm_accessRoleList.Header = Resources["accessRoleList"].ToString();

                //right panel
                tb_search.SetValue(TextBoxHelper.WatermarkProperty, Resources["search"].ToString()); mi_logout.Header = Resources["logon"].ToString();

                //grid panel
                mi_reload.Header = Resources["reload"].ToString(); mi_new.Header = Resources["new"].ToString();
                mi_edit.Header = Resources["edit"].ToString(); mi_copy.Header = Resources["copy"].ToString(); mi_delete.Header = Resources["delete"].ToString();

                Loaded += MainWindow_Loaded;
                cb_filter.SelectedIndex = 0;
                ShowLoginDialog();
            } catch (Exception ex) { App.log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(ex));
            }
        }

        /// <summary>
        /// App Quit
        /// </summary>
        /// <param name="silent"></param>
        public static async void AppQuit(bool silent)
        {
            if (!silent)
            {
                MetroWindow metroWindow = Application.Current.MainWindow as MetroWindow;
                MetroDialogSettings settings = new MetroDialogSettings() { AffirmativeButtonText = metroWindow.Resources["yes"].ToString(), NegativeButtonText = metroWindow.Resources["no"].ToString() };
                MessageDialogResult result = await metroWindow.ShowMessageAsync(metroWindow.Resources["closeAppTitle"].ToString(), metroWindow.Resources["closeAppQuestion"].ToString(), MessageDialogStyle.AffirmativeAndNegative, settings);
                if (result == MessageDialogResult.Affirmative) { MainWindowViewModel.SaveTheme(); FileFunctions.ClearFolder(App.reportFolder); Application.Current.Shutdown(); } //Window.GetWindow(this).Close();
            }
            else { MainWindowViewModel.SaveTheme(); FileFunctions.ClearFolder(App.reportFolder); Application.Current.Shutdown(); }
        }

        /// <summary>
        /// Show Info and Error message dialog
        /// </summary>
        /// <param name="error"></param>
        /// <param name="message"></param>
        /// <param name="confirm"></param>
        /// <returns></returns>
        public static async Task<MessageDialogResult> ShowMessage(bool error, string message, bool confirm = false)
        {
            if (error) SystemFunctions.SaveSystemFailMessage(message);
            
            ProgressRing = Visibility.Hidden; MessageDialogResult result;
            MetroWindow metroWindow = Application.Current.MainWindow as MetroWindow;
            if (confirm)
            {
                MetroDialogSettings settings = new MetroDialogSettings() { AffirmativeButtonText = metroWindow.Resources["yes"].ToString(), NegativeButtonText = metroWindow.Resources["no"].ToString() };
                result = await metroWindow.ShowMessageAsync(metroWindow.Resources["warning"].ToString(), message, MessageDialogStyle.AffirmativeAndNegative, settings);
            }
            else { result = await metroWindow.ShowMessageAsync(error ? metroWindow.Resources["error"].ToString() : metroWindow.Resources["info"].ToString(), message); }
            return result;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuSortOnStart();
                this.Invoke(() =>
                {
                    timer10sec.Elapsed += Timer10sec_Elapsed; timer10sec.Enabled = true;
                    _ = MediaFunctions.IncreaseFileVersionBuild();
                    AddNewTab(Resources["support"].ToString(), new SupportPage());
                });
                //Load Theme
                AppTheme theme = ThemeManager.AppThemes.FirstOrDefault(t => t.Name.Equals(App.Setting.ThemeName));
                Accent accent = ThemeManager.Accents.FirstOrDefault(a => a.Name.Equals(App.Setting.AccentName));
                if ((theme != null) && (accent != null)) { ThemeManager.ChangeAppStyle(Application.Current, accent, theme); }
            } catch (Exception ex) { App.log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(ex));
            }
        }

        /// <summary>
        /// Auto Sorting Main level of Menu
        /// </summary>
        private void MenuSortOnStart() { foreach (TreeViewItem menuItem in tb_verticalMenu.Items) { menuItem.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending)); } }

        /// <summary>
        /// Backend Time for check server connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Timer10sec_Elapsed(object sender, ElapsedEventArgs e)
        {
            await this.Invoke(async () => {
                timer10sec.Interval = App.Setting.ServerCheckIntervalSec;
                try
                {   //Check server connection
                    if (await ApiCommunication.CheckApiConnection())
                    {
                        ServiceRunning = true; ServiceStatus = Resources["running"].ToString();
                        UserLogged = ServiceRunning && !string.IsNullOrWhiteSpace((string)si_loggedIn.Content);
                        mi_logout.Header = UserLogged ? Resources["logout"].ToString() : Resources["logon"].ToString();

                        //Load DB Settings and Language
                        if (App.Parameters == null) { 
                            App.Parameters = await ApiCommunication.GetApiRequest<List<ParameterList>>(ApiUrls.ParameterList, null, null); 
                            App.LanguageList = await ApiCommunication.GetApiRequest<List<LanguageList>>(ApiUrls.LanguageList, null, null);
                        }
                        
                        //ONETime Update
                        if (ServiceRunning && !updateChecked && UserLogged) { this.Invoke(() => { if (App.Setting.AutomaticUpdate != "never") { Updater.CheckUpdate(false); } updateChecked = true; }); }
                    } else { SetServiceStop(); }
                } catch (Exception ex) { App.log.Error(ex.Message + Environment.NewLine + ex.StackTrace); SetServiceStop();
                    SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(ex));
                }
            });
        }

        private void SetServiceStop()
        {
            ServiceStatus = Resources["stopped"].ToString();
            DataGridSelected = DataGridSelectedIdListIndicator = DgRefresh = ServiceRunning = false;
            UserLogged = ServiceRunning && !string.IsNullOrWhiteSpace(si_loggedIn.Content.ToString());
            mi_logout.Header = UserLogged ? Resources["logout"].ToString() : Resources["logon"].ToString();
        }


        /// <summary>
        /// Full dynamic about information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Btn_about_click(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync(Resources["appName"].ToString(),
            Resources["appDescription"].ToString() + "\n" + Resources["version"].ToString() + App.AppVersion + "\n" + Resources["author"].ToString() + "\n\n" +
            Resources["myCompany"].ToString() + "\n" + Resources["myName"].ToString() + "\n" + Resources["myStreet"].ToString() + "\n" +
            Resources["myState"].ToString() + "\n" + Resources["myInvoiceInfo"].ToString() + "\n" + Resources["myPhone"].ToString() + "\n" +
            Resources["myEmail"].ToString() + "\n" + Resources["myAccount"].ToString(), MessageDialogStyle.Affirmative);
        }

        /// <summary>
        /// On Keypress actual Help and Quit App
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1) { System.Windows.Forms.Help.ShowHelp(null, "Manual\\index.chm"); e.Handled = true; }
            else if (Keyboard.IsKeyDown(Key.Q) && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) { ExitMenuItem_Click(null, null); Mouse.OverrideCursor = null; e.Handled = true; }
            else if (Keyboard.IsKeyDown(Key.R) && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) { Mouse.OverrideCursor = null; e.Handled = true; }
            else if (Keyboard.IsKeyDown(Key.C) && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) { Mouse.OverrideCursor = null; e.Handled = true; }
        }

        /// <summary>
        /// Full dynamic Show Help File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_LaunchHelp_Click(object sender, RoutedEventArgs e) => System.Windows.Forms.Help.ShowHelp(null, "Manual\\index.chm");


        /// <summary>
        /// Full dynamic Application Login dialog
        /// </summary>
        private async void ShowLoginDialog()
        {
            try
            {
                ProgressRing = Visibility.Visible;
                LoginDialogData result = await this.ShowLoginAsync(Resources["login"].ToString(), Resources["loginToApp"].ToString(),
                    new LoginDialogSettings
                    {
                        ColorScheme = MetroDialogOptions.ColorScheme,
                        InitialUsername = "tester",
                        InitialPassword = "tester",
                        AffirmativeButtonText = Resources["logon"].ToString(),
                        NegativeButtonText = Resources["end"].ToString(),
                        AnimateShow = true,
                        EnablePasswordPreview = true,
                        NegativeButtonVisibility = Visibility.Visible
                    });
                if (result == null) { AppQuit(false); }
                else
                {
                    App.UserData.UserName = result.Username;
                    Authentification dBResult = await ApiCommunication.Authentification(ApiUrls.Authentication, result.Username, result.Password);
                    if (dBResult == null || dBResult.Token == null)
                    {
                        if (!UserLogged) { MessageDialogResult messageResult = await this.ShowMessageAsync(Resources["login"].ToString(), Resources["loginServiceError"].ToString() + "\n" + App.Setting.ApiAddress + "/" + ApiUrls.Authentication + " " + Resources["loginServiceError1"].ToString()); ShowLoginDialog(); }
                        else { MessageDialogResult messageResult = await this.ShowMessageAsync(Resources["login"].ToString(), Resources["loginError"].ToString()); ShowLoginDialog(); }

                    } else {
                        App.UserData.Authentification = dBResult;
                        App.Parameters.AddRange(await ApiCommunication.GetApiRequest<List<ParameterList>>(ApiUrls.ParameterList, App.UserData.Authentification.Id.ToString(), App.UserData.Authentification.Token));

                        //MessageDialogResult messageResult = await this.ShowMessageAsync(Resources["login"].ToString(), Resources["successLogin"].ToString() + Environment.NewLine + Resources["successHelp"].ToString() + Environment.NewLine + Resources["successThanks"].ToString());
                        si_loggedIn.Content = Resources["loggedIn"].ToString() + " " + ((App.UserData.Authentification.Name.Length > 0 || App.UserData.Authentification.SurName.Length > 0) ? App.UserData.Authentification.Name + " " + App.UserData.Authentification.SurName : result.Username);
                        ProgressRing = Visibility.Hidden;
                    }
                }
                ProgressRing = Visibility.Hidden;
            } catch (Exception ex) { App.log.Error(ex.Message); await ShowMessage(true, ex.Message);
                SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(ex));
            }
        }

        private void ApplicationClosing(object sender, CancelEventArgs e)
        {
            try
            {
                if (DataContext == null) return;
                if (e.Cancel) return;
                e.Cancel = !e.Cancel;
                if (metroWindowClosing) { metroWindowClosing = false; return; }
                AppQuit(false);

            } catch (Exception ex) { App.log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(ex));
            }

        }

        internal static void AppRestart()
        {
            MainWindowViewModel.SaveTheme(); Process.Start(Assembly.GetEntryAssembly().EscapedCodeBase);
            Process.GetCurrentProcess().Kill();
        }

        private void Mi_logout_Click(object sender, RoutedEventArgs e) { App.UserData = new UserData(); si_loggedIn.Content = null; ShowLoginDialog(); }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) => AppQuit(false);

        /// <summary>
        /// Full dynamic Tool Reaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IpKeyboardClick(object sender, MouseButtonEventArgs e) => TouchKeyboard.IsOpen = !TouchKeyboard.IsOpen; 
        private void IpCalculatorClick(object sender, MouseButtonEventArgs e) => Calc.Visibility = (Calc.IsVisible != true) ? Visibility.Visible : Visibility.Hidden;
        private void IpcaptureAppClick(object sender, MouseEventArgs e) => MediaFunctions.SaveAppScreenShot(this);




        //----------------------------------------------------------------- BEGIN OF MENU REACTIONS -------------------------------------------------------------------------

        /// <summary>
        /// Full dynamic apply setted filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cb_FilterDropDownClosed(object sender, EventArgs e)
        {
            TabContent SelectedTab = (TabContent)TabablzControl.GetLoadedInstances().Last().SelectedItem;
            string advancedFilter = SystemFunctions.FilterToString(cb_filter);

            ((DataViewSupport)Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + SelectedTab.Content.GetType().Name)).GetType().GetField("dataViewSupport").GetValue(null)).AdvancedFilter = advancedFilter;
            cb_filter.SelectedIndex = 0;
            _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + SelectedTab.Content.GetType().Name)).GetType()
                .GetMethod("LoadDataList").Invoke(((TabContent)InitialTabablzControl.SelectedItem).Content, null);
        }


        /// <summary>
        /// Full dynamic Show/Hidden datagrid advanced Filter Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mi_filter_Click(object sender, RoutedEventArgs e)
        {
            string mark = DateTime.UtcNow.Ticks.ToString();

            if (((Button)sender).Name == "mi_plus") {
                TabContent SelectedTab = (TabContent)TabablzControl.GetLoadedInstances().Last().SelectedItem;
                var viewFields = Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + SelectedTab.Content.GetType().Name)).GetType().GetField("selectedRecord").GetValue(null);

                ComboBox cbFieldsBox = new ComboBox() { Name = "field_" + mark, Width = 200, Height = 30 };
                cbFieldsBox.SelectionChanged += FilterField_SelectionChanged;
                foreach (PropertyInfo propertyInfo in viewFields.GetType().GetProperties())
                {
                    if (propertyInfo.ToString().Split(' ')[0].ToLower().Replace("system.", "") != "byte[]")
                    {
                        ComboBoxItem cbItem = new ComboBoxItem() { Content = propertyInfo.Name, Tag = propertyInfo.ToString().Split(' ')[0].ToLower().Replace("system.", ""), Width = 200, Height = 30 };
                        cbFieldsBox.Items.Add(cbItem);
                    }
                }
                WrapPanel dockPanel = new WrapPanel() { Name = "filter_" + mark, Width = 230, Height = 60 };
                dockPanel.Children.Add(cbFieldsBox);
                Button removeBtn = new Button() { Name = "remove_" + mark, Width = 30, Content = "X", Height = 30, FontSize = 20, Padding = new Thickness(0) };
                removeBtn.Click += RemoveFilterItem_Click;
                dockPanel.Children.Add(removeBtn);

                ComboBox cbSignBox = new ComboBox() { Name = "sign_" + mark, Width = 80, Height = 30 };
                dockPanel.Children.Add(cbSignBox);
                dockPanel.Children.Add(new TextBox() { Name = "text_" + mark, Width = 150, FontSize = 16 });

                cb_filter.Items.Add(dockPanel);
            } else if (((Button)sender).Name == "mi_open" || ((Button)sender).Name == "mi_and" || ((Button)sender).Name == "mi_or")
            {
                if (((WrapPanel)cb_filter.Items[cb_filter.Items.Count - 1]).Name.Split('_')[0] == "condition") {
                    ((WrapPanel)cb_filter.Items[cb_filter.Items.Count - 1]).Children.Add(new Label() { Content = " " + ((Button)sender).Content + " ", Height = 30, FontSize = 20, Padding = new Thickness(0) });
                } else {
                    WrapPanel dockPanel = new WrapPanel() { Name = "condition_" + mark, Width = 230, Height = 30 };
                    Button removeBtn = new Button() { Name = "remove_" + mark, Width = 30, Content = "X", Height = 30, FontSize = 20, Padding = new Thickness(0) };
                    removeBtn.Click += RemoveFilterItem_Click;
                    dockPanel.Children.Add(removeBtn);
                    dockPanel.Children.Add(new Label() { Content = " " + ((Button)sender).Content + " ", Height = 30, FontSize=20, Padding = new Thickness(0) });
                    cb_filter.Items.Add(dockPanel);
                }

            } else if (((Button)sender).Name == "mi_close") {
                WrapPanel dockPanel = new WrapPanel() { Name = "condition_" + mark, Width = 230, Height = 30 };
                Button removeBtn = new Button() { Name = "remove_" + mark, Width = 30, Content = "X", Height = 30, FontSize = 20, Padding = new Thickness(0) };
                removeBtn.Click += RemoveFilterItem_Click;
                dockPanel.Children.Add(removeBtn);
                dockPanel.Children.Add(new Label() { Content = " " + ((Button)sender).Content + " ", Height = 30, FontSize = 20, Padding = new Thickness(0) });
                cb_filter.Items.Add(dockPanel);
            }

        }


        /// <summary>
        /// Full dynamic build filter on selected page from saved advanced filter
        /// </summary>
        /// <param name="filterBox"></param>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        public ComboBox StringToFilter(ComboBox filterBox, string advancedFilter)
        {
            advancedFilter = (!string.IsNullOrWhiteSpace(advancedFilter)) ? advancedFilter : "";
            //clear Filter
            int filterItemCount = filterBox.Items.Count;
            if (filterItemCount > 1)
            {
                for (int index = 0; index < filterItemCount - 1 ; index++)
                {
                    filterBox.Items.RemoveAt(1);
                }
            }

            //Add existing items
            string[] filterLines = advancedFilter.Split(new[] { "[!]" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string filterLine in filterLines)
            {
                string mark = DateTime.UtcNow.Ticks.ToString();
                if (filterLine.FirstOrDefault().ToString() == " ") { //condition line
                    WrapPanel dockPanel = new WrapPanel() { Name = "condition_" + mark, Width = 230, Height = 30 };
                    Button removeBtn = new Button() { Name = "remove_" + mark, Width = 30, Content = "X", Height = 30, FontSize = 20, Padding = new Thickness(0) };
                    removeBtn.Click += RemoveFilterItem_Click;
                    dockPanel.Children.Add(removeBtn);

                    string[] filterItems = filterLine.Split(new[] { "{!}" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string filterItem in filterItems)
                    {
                        dockPanel.Children.Add(new Label() { Content = filterItem, Height = 30, FontSize = 20, Padding = new Thickness(0) });
                    }
                    filterBox.Items.Add(dockPanel);
                } else // Item Field
                {
                    TabContent SelectedTab = (TabContent)TabablzControl.GetLoadedInstances().Last().SelectedItem;
                    var viewFields = Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + SelectedTab.Content.GetType().Name)).GetType().GetField("selectedRecord").GetValue(null);

                    ComboBox cbFieldsBox = new ComboBox() { Name = "field_" + mark, Width = 200, Height = 30 };
                    cbFieldsBox.SelectionChanged += FilterField_SelectionChanged;
                    foreach (PropertyInfo propertyInfo in viewFields.GetType().GetProperties())
                    {
                        if (propertyInfo.ToString().Split(' ')[0].ToLower().Replace("system.", "") != "byte[]")
                        {
                            ComboBoxItem cbItem = new ComboBoxItem() { Content = propertyInfo.Name, Tag = propertyInfo.ToString().Split(' ')[0].ToLower().Replace("system.", ""), Width = 200, Height = 30 };
                            cbFieldsBox.Items.Add(cbItem);
                        }
                    }
                    WrapPanel dockPanel = new WrapPanel() { Name = "filter_" + mark, Width = 230, Height = 60 };
                    dockPanel.Children.Add(cbFieldsBox);
                    Button removeBtn = new Button() { Name = "remove_" + mark, Width = 30, Content = "X", Height = 30, FontSize = 20, Padding = new Thickness(0) };
                    removeBtn.Click += RemoveFilterItem_Click;
                    dockPanel.Children.Add(removeBtn);

                    ComboBox cbSignBox = new ComboBox() { Name = "sign_" + mark, Width = 80, Height = 30 };
                    dockPanel.Children.Add(cbSignBox);
                    dockPanel.Children.Add(new TextBox() { Name = "text_" + mark, Width = 150, FontSize = 16 });

                    cb_filter.Items.Add(dockPanel);
                    string[] filterItems = filterLine.Split(new[] { "{!}" }, StringSplitOptions.None);
                    ((ComboBox)((WrapPanel)cb_filter.Items[cb_filter.Items.Count - 1]).Children[0]).Text = filterItems[0];
                    ((ComboBox)((WrapPanel)cb_filter.Items[cb_filter.Items.Count - 1]).Children[2]).Text = filterItems[1];
                    ((TextBox)((WrapPanel)cb_filter.Items[cb_filter.Items.Count - 1]).Children[3]).Text = filterItems[2].Replace("'","");
                }
            }
            return filterBox;
        }

        /// <summary>
        /// Full dynamic Remove Item from datagrid advanced Filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RemoveFilterItem_Click(object sender, RoutedEventArgs e)
        {
            try { foreach (WrapPanel filterItem in cb_filter.Items) { if (filterItem.Name.Split('_')[1] == ((Button)sender).Name.Split('_')[1]) { cb_filter.Items.Remove(filterItem); } } } 
            catch { }
        }

        /// <summary>
        ///  Full dynamic set sign datagrid advanced filter type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = 0;
            try { foreach (WrapPanel filterItem in cb_filter.Items) {
                    if (index > 0 && filterItem.Name.Split('_')[1] == ((ComboBox)sender).Name.Split('_')[1])
                    {
                        ((ComboBox)filterItem.Children[2]).Items.Clear();
                        if ("string?".Contains(((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag.ToString())) ((ComboBox)filterItem.Children[2]).Items.Add(new ComboBoxItem() { Content = " LIKE ", Width = 80 });
                        if ("string?,float?,int?,int32?,int64?,decimal?,double?,bit?,boolean?,datetime?,date?".Contains(((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag.ToString()))
                        {
                            ((ComboBox)filterItem.Children[2]).Items.Add(new ComboBoxItem() { Content = " = ", Width = 80, FontSize = 20 });
                            ((ComboBox)filterItem.Children[2]).Items.Add(new ComboBoxItem() { Content = " <> ", Width = 80, FontSize = 20 });
                        }
                        if (",int?,float?,int32?,int64?,decimal?,double?,datetime?,date?".Contains(((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag.ToString()))
                        {
                            ((ComboBox)filterItem.Children[2]).Items.Add(new ComboBoxItem() { Content = " > ", Width = 80, FontSize = 20 });
                            ((ComboBox)filterItem.Children[2]).Items.Add(new ComboBoxItem() { Content = " < ", Width = 80, FontSize = 20 });
                        }
                    }
                    index++;
                }
            } catch (Exception ex) { SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(ex)); }
        }


        /// <summary>
        /// Full dynamic on TabPanel drag actually not used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainGrid_IsDraggingChanged(object sender, RoutedPropertyChangedEventArgs<bool> e) { }


        /// <summary>
        /// Full dynamic Print Report selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Cb_PrintReportsSelected(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cb_printReports.SelectedIndex > -1)
                {
                    ProgressRing = Visibility.Visible;
                    SqlConnection cnn = new SqlConnection(App.Setting.ReportConnectionString);
                    cnn.Open();
                    if (cnn.State == System.Data.ConnectionState.Open)
                    {
                        cnn.Close();
                        TabContent SelectedTab = (TabContent)TabablzControl.GetLoadedInstances().Last().SelectedItem;
                        string advancedFilter = ((DataViewSupport)Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + SelectedTab.Content.GetType().Name)).GetType().GetField("dataViewSupport").GetValue(null)).AdvancedFilter;
                        advancedFilter = (string.IsNullOrWhiteSpace(advancedFilter)) ? "1=1" : advancedFilter.Replace("[!]", "").Replace("{!}", "");

                        //Update Filter data for generate Report
                        SetReportFilter setReportFilter = new SetReportFilter() { TableName = SelectedTab.Content.GetType().Name.Replace("Page", ""), Filter = advancedFilter, Search = tb_search.Text, RecId = dataGridSelectedId };
                        string json = JsonConvert.SerializeObject(setReportFilter);
                        StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        DBResultMessage dBResult = await ApiCommunication.PostApiRequest(ApiUrls.ReportQueueList, httpContent, "WriteFilter", App.UserData.Authentification.Token);

                        string reportFile = Path.Combine(App.reportFolder, DateTimeOffset.Now.Ticks.ToString() + ".rdl");
                        if (FileFunctions.ByteArrayToFile(reportFile, ((ReportList)cb_printReports.SelectedItem).File))
                        {
                            Process exeProcess = new Process();
                            ProcessStartInfo info = new ProcessStartInfo()
                            {
                                FileName = App.Setting.ReportingPath,
                                WorkingDirectory = Path.GetDirectoryName(App.Setting.ReportingPath) + "\\",
                                Arguments = reportFile + " -p \"Connect=" + App.Setting.ReportConnectionString + "&TableName=" + SelectedTab.Content.GetType().Name.Replace("Page", "") + "&Search=%" + tb_search.Text + "%&Id=" + dataGridSelectedId.ToString() + "&Filter=" + advancedFilter + "\"",
                                LoadUserProfile = true,
                                CreateNoWindow = false,
                                UseShellExecute = false,
                                WindowStyle = ProcessWindowStyle.Normal,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                Verb = (Environment.OSVersion.Version.Major >= 6) ? "runas" : "",
                            };
                            exeProcess.StartInfo = info;
                            exeProcess.Start();
                        }
                    }
                    else
                    {
                        cnn.Close();
                        await ShowMessage(true, Resources["connectionStringIsNotValid"].ToString());
                    }
                    ProgressRing = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(ex));
                App.log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                await ShowMessage(true, Resources["connectionStringIsNotValid"].ToString());
                ProgressRing = Visibility.Hidden;
            }
            cb_printReports.SelectedIndex = -1;
        }


        /// <summary>
        /// THIS IS FOR MANUAL UPDATE  only for Horizontal menu Only ADD New Page - MENU REACTION and REWRITE PageName and change And Report CALL  as /XXXX
        /// open or select existed TabPanel VERTICAL MENU -  Copy and CHANGE ONLY Page Name AND Report CALL  as /XXXX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Cb_verticalMenuSelected(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string name = ((FrameworkElement)sender).Name;
                if (name.StartsWith("tv_") && !((TreeViewItem)sender).IsExpanded)
                {
                    ((TreeViewItem)sender).IsExpanded = !((TreeViewItem)sender).IsExpanded;
                } else if (!name.StartsWith("tv_")) {
                    switch (name)
                    {

                        case "tm_accessRoleList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new AccessRoleListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/AccessRoleList", App.UserData.Authentification.Token);
                            break;
                        case "tm_addressList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new AddressListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, ""); 
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/AddressList", App.UserData.Authentification.Token);
                            break;
                        case "tm_branchList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new BranchListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, ""); 
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/BranchList", App.UserData.Authentification.Token);
                            break;
                        case "tm_calendar":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new CalendarPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, ""); 
                            cb_printReports.ItemsSource = null;
                            break;
                        case "tm_clientSettings":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new ClientSettingsPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, ""); 
                            cb_printReports.ItemsSource = null;
                            break;
                        case "tm_currencyList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new CurrencyListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/CurrencyList", App.UserData.Authentification.Token);
                            break;
                        case "tm_documentAdviceList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new DocumentAdviceListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/DocumentAdviceList", App.UserData.Authentification.Token);
                            break;
                        case "tm_documentTypeList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new DocumentTypeListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/DocumentTypeList", App.UserData.Authentification.Token);
                            break;
                        case "tm_exchangeRateList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new ExchangeRateListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/ExchangeRateList", App.UserData.Authentification.Token);
                            break;
                        case "tm_languageList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new LanguageListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/LanguageList", App.UserData.Authentification.Token);
                            break;
                        case "tm_loginHistoryList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new LoginHistoryListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, ""); 
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/LoginHistoryList", App.UserData.Authentification.Token);
                            break;
                        case "tm_parameterList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new ParameterListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/ParameterList", App.UserData.Authentification.Token);
                            break;
                        case "tm_reportList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new ReportListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/ReportList", App.UserData.Authentification.Token);
                            break;
                        case "tm_reportQueueList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new ReportQueueListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/ReportQueueList", App.UserData.Authentification.Token);
                            break;
                        case "tm_support":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources["support"].ToString(), new SupportPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = null;
                            break;
                        case "tm_userList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new UserListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/UserList", App.UserData.Authentification.Token);
                            break;
                        case "tm_userRoleList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new UserRoleListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/UserRoleList", App.UserData.Authentification.Token);
                            break;
                        case "tm_systemFailList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0)
                            { AddNewTab(Resources[name.Split('_')[1]].ToString(), new SystemFailListPage()); }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/SystemFailList", App.UserData.Authentification.Token);
                            break;
                            
                        default:
                            cb_printReports.ItemsSource = null;
                            break;
                    }
                    cb_printReports.IsEnabled = cb_printReports.Items.Count > 0;
                    tb_verticalMenu.IsOverflowOpen = false; 
                }
            }
            catch (Exception ex) { App.log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(ex));
            }
        }

        /// <summary>
        /// THIS IS FOR MANUAL UPDATE  only for Horizontal menu Only ADD New Page - MENU REACTION and REWRITE PageName and change And Report CALL  as /XXXX
        /// open or select existed TabPanel VERTICAL MENU -  Copy and CHANGE ONLY Page Name AND Report CALL  as /XXXX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_action_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TabContent SelectedTab = (TabContent)TabablzControl.GetLoadedInstances().Last().SelectedItem;
                string senderName = SelectedTab?.Content.GetType().Name;
                DBResultMessage dBResult = new DBResultMessage();
                string buttonSenderName = ((FrameworkElement)sender).Name;
                switch (buttonSenderName)
                {
                    //AUTOMATIC LIST VIEW PART NOT MODIFY
                    case "tb_search":
                        _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + senderName)).GetType()
                            .GetMethod("Filter").Invoke(((TabContent)InitialTabablzControl.SelectedItem).Content, new object[] { ((TextBox)e.Source).Text });
                        break;
                    case "mi_reload":
                        _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + senderName)).GetType()
                             .GetMethod("LoadDataList").Invoke(((TabContent)InitialTabablzControl.SelectedItem).Content, null);
                        break;
                    case "mi_new":
                        _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + senderName)).GetType()
                            .GetMethod("NewRecord").Invoke(((TabContent)InitialTabablzControl.SelectedItem).Content, null);
                        break;
                    case "mi_edit":
                        _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + senderName)).GetType()
                            .GetMethod("EditRecord").Invoke(((TabContent)InitialTabablzControl.SelectedItem).Content, new object[] { false });
                        break;
                    case "mi_copy":
                        _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + senderName)).GetType()
                            .GetMethod("EditRecord").Invoke(((TabContent)InitialTabablzControl.SelectedItem).Content, new object[] { true });
                        break;
                    case "mi_delete":
                        _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + senderName)).GetType()
                            .GetMethod("DeleteRecord").Invoke(((TabContent)InitialTabablzControl.SelectedItem).Content, null);
                        break;
                    default: break;
                }
                cb_printReports.IsEnabled = cb_printReports.Items.Count > 0;
            } catch (Exception ex) {
                SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(ex));
                App.log.Error(ex.Message + Environment.NewLine + ex.StackTrace); 
            }
        }


        /// <summary>
        /// Full dynamic Open New Tab
        /// </summary>
        /// <param name="headerName"></param>
        /// <param name="tabPage"></param>
        private void AddNewTab(string headerName, object tabPage)
        {
            try
            {
                IEnumerable<DragablzItem> existedTabs = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders();
                existedTabs = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders();
                if (existedTabs.Count() > 0)
                {
                    TabContent tc1 = new TabContent(headerName, tabPage);
                    TabablzControl.AddItem(tc1, existedTabs.Last().DataContext, AddLocationHint.After);
                    TabablzControl.SelectItem(tc1);
                    existedTabs = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders();
                }

                if (existedTabs.Count() == 0)
                {
                    MainWindowViewModel result = new MainWindowViewModel();
                    result.TabContents.Add(new TabContent(headerName, tabPage));
                    DataContext = result;
                    existedTabs = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders();
                }

            } catch (Exception ex) { App.log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(ex));
            }
        }



        /// <summary>
        /// THIS IS FOR MANUAL CHECK  - MENU Change REACTION 
        /// Select AND write your Page name to correct Page Type:  Setting - Special form, View - List Only, Form - List with Detail Form
        /// FORMAT OF PAGE NAME IS API CALLPage 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TabPanelOnSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TabContent SelectedTab = (TabContent)TabablzControl.GetLoadedInstances().Last().SelectedItem;
                DataViewSupport dataViewSupport = new DataViewSupport();

                if (SelectedTab == null || ((FrameworkElement)SelectedTab.Content).Tag.ToString() == "Setting") {
                    //SETTING 
                    tb_search.Text = null; dataGridSelectedId = 0; DataGridSelected = false; DataGridSelectedIdListIndicator = false;
                    DgRefresh = false; cb_printReports.ItemsSource = null;
                } else if (SelectedTab == null || new string[] { "View", "Form" }.Contains(((FrameworkElement)SelectedTab.Content).Tag.ToString()))
                {
                    DataGridSelected = true; DgRefresh = true; string senderName = SelectedTab.Content.GetType().Name;
                    var AutoPageGeneration = Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + senderName)).GetType();
                    switch (((FrameworkElement)SelectedTab.Content).Tag.ToString())
                    {
                        //FORMS - LIST + DETAIL FORM
                        case "Form":
                            dataViewSupport = ((DataViewSupport)Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + senderName)).GetType().GetField("dataViewSupport").GetValue(null));
                            tb_search.Text = dataViewSupport.FilteredValue;
                            if (dataViewSupport.FormShown) { dataGridSelectedId = 0; DataGridSelectedIdListIndicator = false; DataGridSelected = false; DgRefresh = false; }
                            else { if (dataViewSupport.SelectedRecordId == 0) { dataGridSelectedId = 0; DataGridSelectedIdListIndicator = false; }
                                else { dataGridSelectedId = dataViewSupport.SelectedRecordId; DataGridSelectedIdListIndicator = true; } }
                            StringToFilter(cb_filter, dataViewSupport.AdvancedFilter);
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/" + senderName.Replace("Page",""), App.UserData.Authentification.Token);
                            break;


                        //VIEWS - LIST ONLY
                        case "View":
                            dataViewSupport = ((DataViewSupport)Convert.ChangeType(SelectedTab.Content, Type.GetType("TravelAgencyAdmin.Pages." + senderName)).GetType().GetField("dataViewSupport").GetValue(null));
                            tb_search.Text = dataViewSupport.FilteredValue; dataGridSelectedId = dataViewSupport.SelectedRecordId;
                            DataGridSelected = DataGridSelectedIdListIndicator = false; DgRefresh = true;
                            StringToFilter(cb_filter, dataViewSupport.AdvancedFilter);
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/" + senderName.Replace("Page", ""), App.UserData.Authentification.Token);
                            break;

                      
                        default:
                            break;
                    }
                }

                cb_printReports.IsEnabled = cb_printReports.Items.Count > 0;
            } catch (Exception ex) { App.log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(ex));
            }
        }



        /// <summary>
        /// Full dynamic Tab panel close click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabPanelCloseClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (e.Source.ToString().Contains("Dragablz.TabablzControl") && e.OriginalSource.ToString() == "System.Windows.Controls.Button") { metroWindowClosing = true; }
            } 
            catch (Exception ex) { App.log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                SystemFunctions.SaveSystemFailMessage(SystemFunctions.GetExceptionMessages(ex));
            }
        }

    }
}
