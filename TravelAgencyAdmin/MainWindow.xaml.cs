using Dragablz;
using GlobalClasses;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UbytkacAdmin.Api;
using UbytkacAdmin.Classes;
using UbytkacAdmin.GlobalOperations;
using UbytkacAdmin.Helper;
using UbytkacAdmin.Pages;
using UbytkacAdmin.Properties;
using UbytkacAdmin.SystemConfiguration;
using UbytkacAdmin.SystemStructure;

namespace UbytkacAdmin {

    public partial class MainWindow : MetroWindow {

        #region Definitions

        private static bool _hackyIsFirstWindow = true;
        public static DateTimeOffset lastUserAction = DateTimeOffset.UtcNow.AddSeconds(App.Setting.TimeToEnable);
        public readonly Timer AppSystemTimer = new Timer() { Enabled = false, Interval = 1 };

        /// <summary>
        /// Variables for indicators
        /// </summary>
        public static bool dataGridSelected, dgIdSetted, dgRefresh, serviceRunning, userLogged, updateChecked = false;

        public static int dataGridSelectedId, downloadShow, downloadStatus = 0, vncProcessId = 0;
        public static string serviceStatus;
        public static Visibility progressRing = Visibility.Hidden;

        public SolidColorBrush vncRunning = Brushes.Red;
        public Process vncProccess;

        public static event EventHandler DataGridSelectedChanged, DataGridSelectedIdListIndicatorChanged, DgRefreshChanged, ServiceStatusChanged, ServiceRunningChanged, DownloadStatusChanged, DownloadShowChanged, ProgressRingChanged, UserLoggedChanged, VncRunningChanged = delegate { };

        public SolidColorBrush VncRunning {
            get => vncRunning;
            set {
                vncRunning = value;
                ip_rdpServer.Foreground = (vncRunning == Brushes.Green) ? Brushes.Green : Brushes.Red;
                VncRunningChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static bool UserLogged {
            get => userLogged;
            set {
                userLogged = value;
                UserLoggedChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Service Status description
        /// </summary>
        public static bool ServiceRunning {
            get => serviceRunning;
            set {
                serviceRunning = value;
                ServiceRunningChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Indicator for enable Refresh Button Indicator
        /// </summary>
        public static bool DgRefresh {
            get => dgRefresh;
            set {
                dgRefresh = value;
                DgRefreshChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Indicator for Enable New datagrid Button
        /// </summary>
        public static bool DataGridSelected {
            get => dataGridSelected;
            set {
                dataGridSelected = value;
                DataGridSelectedChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Datagrid have selected record indicator
        /// </summary>
        public static bool DataGridSelectedIdListIndicator {
            get => dgIdSetted;
            set {
                dgIdSetted = value;
                DataGridSelectedIdListIndicatorChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Service Status public Variable
        /// </summary>
        public static string ServiceStatus {
            get => serviceStatus;
            set {
                serviceStatus = value;
                ServiceStatusChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Downloading of update status variable
        /// </summary>
        public static int DownloadStatus {
            get => downloadStatus;
            set {
                downloadStatus = value;
                DownloadStatusChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Indicator for show Downloading area
        /// </summary>
        public static int DownloadShow {
            get => downloadShow;
            set {
                downloadShow = value;
                DownloadShowChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// ProgressRing Visibility indicator
        /// </summary>
        public static Visibility ProgressRing {
            get => progressRing;
            set {
                progressRing = value;
                ProgressRingChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        #endregion Definitions

        /// <summary>
        /// Initialize Application Main Window
        /// </summary>
        public MainWindow() {
            try {
                InitializeComponent();
                SystemOperations.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);
                Title = Resources["appName"].ToString();

                if (_hackyIsFirstWindow) {
                    ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));

                    XamlMainWindow.Height = Settings.Default.Height; XamlMainWindow.Width = Settings.Default.Width;
                    XamlMainWindow.Left = Settings.Default.Left; XamlMainWindow.Top = Settings.Default.Top;

                    if (App.Setting.TopMost) Topmost = true;
                }
                _hackyIsFirstWindow = false;

                mi_appearance.Header = Resources["appearance"].ToString(); mi_color.Header = Resources["color"].ToString(); btn_about.ToolTip = Resources["about"].ToString();

                //MENUS THIS IS FOR MANUAL UPDATE
                //Vertical main menu
                tv_dials.Header = Resources["dials"].ToString(); tv_agendas.Header = Resources["agendas"].ToString();
                tv_settings.Header = Resources["settings"].ToString(); tv_system.Header = Resources["system"].ToString();
                tv_accommodations.Header = Resources["accommodations"].ToString(); tv_accommodationConfiguration.Header = Resources["accommodationConfiguration"].ToString();
                tv_hosts.Header = Resources["hosts"].ToString();tv_webAdmin.Header = Resources["webAdmin"].ToString(); 
                tv_guests.Header = Resources["guests"].ToString();tv_approvalProcesses.Header = Resources["approvalProcesses"].ToString();

                //Standard Application menu
                tm_reportList.Header = Resources["reportList"].ToString(); tm_calendar.Header = Resources["calendar"].ToString();
                tm_userList.Header = Resources["userList"].ToString(); tm_userRoleList.Header = Resources["userRoleList"].ToString();
                tm_clientSettings.Header = Resources["clientSettings"].ToString(); tm_adminLoginHistoryList.Header = Resources["adminLoginHistoryList"].ToString();
                tm_support.Header = Resources["support"].ToString(); tm_documentAdviceList.Header = Resources["documentAdviceList"].ToString();
                tm_currencyList.Header = Resources["currencyList"].ToString(); tm_exchangeRateList.Header = Resources["exchangeRateList"].ToString();
                tm_cityList.Header = Resources["cityList"].ToString(); tm_countryList.Header = Resources["countryList"].ToString();
                tm_interestAreaList.Header = Resources["interestAreaList"].ToString();

                tm_addressList.Header = Resources["addressList"].ToString(); tm_parameterList.Header = Resources["parameterList"].ToString();
                tm_branchList.Header = Resources["branchList"].ToString(); tm_reportQueueList.Header = Resources["reportQueueList"].ToString();
                tm_languageList.Header = Resources["languageList"].ToString(); tm_documentTypeList.Header = Resources["documentTypeList"].ToString();
                tm_systemFailList.Header = Resources["systemFailList"].ToString(); tm_accessRoleList.Header = Resources["accessRoleList"].ToString();
                tm_ignoredExceptionList.Header = Resources["ignoredExceptionList"].ToString();

                tm_hotelRoomTypeList.Header = Resources["hotelRoomTypeList"].ToString(); tm_propertyOrServiceUnitList.Header = Resources["propertyOrServiceUnitList"].ToString();
                tm_propertyOrServiceTypeList.Header = Resources["propertyOrServiceTypeList"].ToString(); tm_hotelActionTypeList.Header = Resources["hotelActionTypeList"].ToString();
                tm_accommodation.Header = Resources["accommodation"].ToString(); tm_roomList.Header = Resources["roomList"].ToString();
                tm_propertyOrServiceList.Header = Resources["propertyOrServiceList"].ToString(); tm_approvingProcess.Header = Resources["approvingProcess"].ToString();
                tm_guestLoginHistoryList.Header = Resources["guestLoginHistoryList"].ToString(); tm_mottoList.Header = Resources["mottoList"].ToString();
                tm_guestList.Header = Resources["guestList"].ToString(); tm_hotelImagesList.Header = Resources["hotelImagesList"].ToString();
                tm_propertyGroupList.Header = Resources["propertyGroupList"].ToString();

                tm_holidayTipsList.Header = Resources["holidayTipsList"].ToString(); tm_oftenQuestionList.Header = Resources["oftenQuestionList"].ToString();
                tm_registrationInfoList.Header = Resources["registrationInfoList"].ToString(); tm_ubytkacInfoList.Header = Resources["ubytkacInfoList"].ToString();
                tm_countryAreaList.Header = Resources["countryAreaList"].ToString(); tm_emailTemplateList.Header = Resources["emailTemplateList"].ToString();
                tm_privacyPolicyList.Header = Resources["privacyPolicyList"].ToString(); tm_termsList.Header = Resources["termsList"].ToString();
                tm_webSettingList.Header = Resources["webSettingList"].ToString(); tm_hotelReservationStatusList.Header = Resources["hotelReservationStatusList"].ToString();

                tm_hotelReservationReviewList.Header = Resources["hotelReservationReviewList"].ToString(); tm_hotelReservationList.Header = Resources["hotelReservationList"].ToString();
                tm_hotelReservationReviewApprovalList.Header = Resources["hotelReservationReviewApprovalList"].ToString(); tm_creditPackageList.Header = Resources["creditPackageList"].ToString();
                tm_systemLanguageList.Header = Resources["systemLanguageList"].ToString(); tm_guestFavoriteList.Header = Resources["guestFavoriteList"].ToString();
                tm_hotelReservedRoomList.Header = Resources["hotelReservedRoomList"].ToString();

                //right panel
                tb_search.SetValue(TextBoxHelper.WatermarkProperty, Resources["search"].ToString()); mi_logout.Header = Resources["logon"].ToString();

                //grid panel
                mi_reload.Header = Resources["reload"].ToString(); mi_new.Header = Resources["new"].ToString();
                mi_edit.Header = Resources["edit"].ToString(); mi_copy.Header = Resources["copy"].ToString(); mi_delete.Header = Resources["delete"].ToString();

                cb_filter.SelectedIndex = 0;

                //Core Startup / Closing handlers
                Loaded += MainWindow_Loaded;
                KeyDown += MainWindow_KeyDown;
                Closing += MainWindow_Closing;

                //User Activity Handlers
                PreviewMouseDown += MainWindow_MouseLeave;
                PreviewKeyDown += MainWindow_PreviewKeyDown;
                PreviewMouseMove += MainWindow_PreviewMouseMove;

                ShowLoginDialog();
            } catch (Exception ex) { App.ApplicationLogging(ex); }
        }

        /// <summary>
        /// Writing Last User action for monitoring Free Time Used by: SceenSaver
        /// </summary>
        internal void MainWindow_MouseLeave(object sender, MouseEventArgs e) => SetLastUserAction();

        internal void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e) => SetLastUserAction();

        private void MainWindow_PreviewMouseMove(object sender, MouseEventArgs e) => SetLastUserAction();

        internal DateTimeOffset SetLastUserAction() {
            ///ScreenSaver
            if (App.Setting.DisableOnActivity && this.FindChild<ScreenSaverPage>("") != null) {
                try {
                    SystemTabs existingTab = ((SystemWindowDataModel)DataContext).TabContents.ToList().FirstOrDefault(a => (string)a.Tag == nameof(App.Setting.ActiveSystemSaver));
                    var result = (SystemWindowDataModel)this.DataContext;
                    result.TabContents.Remove(existingTab);
                } catch (Exception ex) { App.ApplicationLogging(ex); }
            }
            return lastUserAction = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Show Info and Error message dialog
        /// </summary>
        /// <param name="error">  </param>
        /// <param name="message"></param>
        /// <param name="confirm"></param>
        /// <returns></returns>
        public static async Task<MessageDialogResult> ShowMessage(bool error, string message, bool confirm = false) {
            if (error) App.ApplicationLogging(new Exception(), message);

            ProgressRing = Visibility.Hidden; MessageDialogResult result;
            MetroWindow metroWindow = Application.Current.MainWindow as MetroWindow;
            if (confirm) {
                MetroDialogSettings settings = new MetroDialogSettings() { AffirmativeButtonText = metroWindow.Resources["yes"].ToString(), NegativeButtonText = metroWindow.Resources["no"].ToString() };
                result = await metroWindow.ShowMessageAsync(metroWindow.Resources["warning"].ToString(), message, MessageDialogStyle.AffirmativeAndNegative, settings);
            } else { result = await metroWindow.ShowMessageAsync(error ? metroWindow.Resources["error"].ToString() : metroWindow.Resources["info"].ToString(), message); }
            return result;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            try {
                MenuSortOnStart();
                this.Invoke(() => {
                    AppSystemTimer.Elapsed += SystemTimerController; AppSystemTimer.Enabled = true;
                    _ = SystemOperations.IncreaseFileVersionBuild();
                    AddOrRemoveTab(Resources["support"].ToString(), new SupportPage());
                });
                //Load Theme
                AppTheme theme = ThemeManager.AppThemes.FirstOrDefault(t => t.Name.Equals(App.Setting.ThemeName));
                Accent accent = ThemeManager.Accents.FirstOrDefault(a => a.Name.Equals(App.Setting.AccentName));
                if ((theme != null) && (accent != null)) { ThemeManager.ChangeAppStyle(Application.Current, accent, theme); }
            } catch (Exception ex) { App.ApplicationLogging(ex); }
        }

        /// <summary>
        /// Auto Sorting Main level of Menu
        /// </summary>
        private void MenuSortOnStart() { foreach (TreeViewItem menuItem in tb_verticalMenu.Items) { menuItem.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending)); } }

        /// <summary>
        /// Backend Time for check server connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private async void SystemTimerController(object sender, ElapsedEventArgs e) {
            await this.Invoke(async () => {
                AppSystemTimer.Interval = App.Setting.ServerCheckIntervalSec;
                try {
                    //System Saver
                    if (App.Setting.ActiveSystemSaver && (DateTimeOffset.UtcNow - lastUserAction).TotalSeconds > App.Setting.TimeToEnable && this.FindChild<ScreenSaverPage>("") == null) {
                        SetLastUserAction();
                        string translatedName = await DBOperations.DBTranslation("activeSystemSaver");
                        this.AddOrRemoveTab(translatedName, new ScreenSaverPage(), nameof(App.Setting.ActiveSystemSaver));
                    }

                    //Check server connection
                    if (await ApiCommunication.CheckApiConnection()) {
                        ServiceRunning = true; ServiceStatus = Resources["running"].ToString();
                        UserLogged = ServiceRunning && !string.IsNullOrWhiteSpace((string)si_loggedIn.Content);
                        mi_logout.Header = UserLogged ? Resources["logout"].ToString() : Resources["logon"].ToString();

                        //Load All startup DB Settings
                        if (App.ParameterList == null || !App.ParameterList.Any()) DBOperations.LoadStartupDBData();

                        //ONETime Update
                        if (ServiceRunning && !updateChecked && UserLogged) { this.Invoke(() => { if (App.Setting.AutomaticUpdate != "never") { SystemUpdater.CheckUpdate(false); } updateChecked = true; }); }
                    } else { SetServiceStop(); }
                } catch (Exception ex) { App.ApplicationLogging(ex); }
            });
        }

        internal void SetServiceStop() {
            ServiceStatus = Resources["stopped"].ToString();
            DataGridSelected = DataGridSelectedIdListIndicator = DgRefresh = ServiceRunning = false;
            UserLogged = ServiceRunning && !string.IsNullOrWhiteSpace(si_loggedIn.Content.ToString());
            mi_logout.Header = UserLogged ? Resources["logout"].ToString() : Resources["logon"].ToString();
        }

        /// <summary>
        /// Full dynamic about information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private async void Btn_about_click(object sender, RoutedEventArgs e) {
            await this.ShowMessageAsync(Resources["appName"].ToString(),
            Resources["appDescription"].ToString() + "\n" + Resources["version"].ToString() + App.AppVersion + "\n" + Resources["author"].ToString() + "\n\n" +
            Resources["myCompany"].ToString() + "\n" + Resources["myName"].ToString() + "\n" + Resources["myStreet"].ToString() + "\n" +
            Resources["myState"].ToString() + "\n" + Resources["myInvoiceInfo"].ToString() + "\n" + Resources["myPhone"].ToString() + "\n" +
            Resources["myEmail"].ToString() + "\n" + Resources["myAccount"].ToString(), MessageDialogStyle.Affirmative);
        }

        /// <summary>
        /// MainWindow Keyboard pointer to Keyboard Central Application controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void MainWindow_KeyDown(object sender, KeyEventArgs e) => HardwareOperations.ApplicationKeyboardMaping(e);

        /// <summary>
        /// Full dynamic Show Help File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void Btn_LaunchHelp_Click(object sender, RoutedEventArgs e) => System.Windows.Forms.Help.ShowHelp(null, "Manual\\index.chm");

        /// <summary>
        /// Full dynamic Application Login dialog
        /// </summary>
        private async void ShowLoginDialog() {
            try {
                ProgressRing = Visibility.Visible;
                LoginDialogData result = await this.ShowLoginAsync(Resources["login"].ToString(), Resources["loginToApp"].ToString(),
                    new LoginDialogSettings {
                        ColorScheme = MetroDialogOptions.ColorScheme,
                        InitialUsername = "tester",
                        InitialPassword = "tester",
                        AffirmativeButtonText = Resources["logon"].ToString(),
                        NegativeButtonText = Resources["end"].ToString(),
                        AnimateShow = true,
                        EnablePasswordPreview = true,
                        NegativeButtonVisibility = Visibility.Visible
                    });
                if (result == null) { App.AppQuitRequest(false); } else {
                    App.UserData.UserName = result.Username;
                    Authentification dBResult = await ApiCommunication.Authentification(ApiUrls.Authentication, result.Username, result.Password);
                    if (dBResult == null || dBResult.Token == null) {
                        if (!UserLogged) { MessageDialogResult messageResult = await this.ShowMessageAsync(Resources["login"].ToString(), Resources["loginServiceError"].ToString() + "\n" + App.Setting.ApiAddress + "/" + ApiUrls.Authentication + " " + Resources["loginServiceError1"].ToString()); ShowLoginDialog(); } else { MessageDialogResult messageResult = await this.ShowMessageAsync(Resources["login"].ToString(), Resources["loginError"].ToString()); ShowLoginDialog(); }
                    } else {
                        App.UserData.Authentification = dBResult;
                        si_loggedIn.Content = Resources["loggedIn"].ToString() + " " + ((App.UserData.Authentification.Name.Length > 0 || App.UserData.Authentification.SurName.Length > 0) ? App.UserData.Authentification.Name + " " + App.UserData.Authentification.SurName : result.Username);
                        DBOperations.LoadOrRefreshUserData();
                        ProgressRing = Visibility.Hidden;
                    }
                }

                ProgressRing = Visibility.Hidden;
            } catch (Exception ex) { App.ApplicationLogging(ex); }
        }

        /// <summary>
        /// Applications Close Request Controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        public static void MainWindow_Closing(object sender, CancelEventArgs e) {
            if (e.Cancel) return; e.Cancel = !e.Cancel; App.AppQuitRequest(false);
        }

        /// <summary>
        /// Application Logout button Controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void Mi_logout_Click(object sender, RoutedEventArgs e) { App.UserData = new UserData(); si_loggedIn.Content = null; ShowLoginDialog(); }

        /// <summary>
        /// Full dynamic Tool Reaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void BtnKeyboardClick(object sender, RoutedEventArgs e) => TouchKeyboard.IsOpen = !TouchKeyboard.IsOpen;

        private void BtnCalculatorClick(object sender, RoutedEventArgs e) => Calc.Visibility = (Calc.IsVisible != true) ? Visibility.Visible : Visibility.Hidden;

        private void BtnStartRdpServerClick(object sender, RoutedEventArgs e) {
            if (vncProcessId > 0) { vncProccess.Kill(); vncProcessId = 0; VncRunning = Brushes.Red; } else {
                if (FileOperations.CheckFile(Path.Combine(App.startupPath, "Data", "AddOn", "winvnc.exe"))) {
                    if (FileOperations.VncServerIniFile(Path.Combine(App.startupPath, "Data", "AddOn"))) {
                        vncProccess = new Process();
                        ProcessStartInfo info = new ProcessStartInfo() {
                            FileName = Path.Combine(App.startupPath, "Data", "AddOn", "winvnc.exe"),
                            WorkingDirectory = Path.Combine(App.startupPath, "Data", "AddOn"),
                            Arguments = $@" -inifile {Path.Combine(App.startupPath, "Data", "AddOn", "server.ini")} -run",
                            LoadUserProfile = true,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            Verb = (Environment.OSVersion.Version.Major >= 6) ? "runas" : "",
                        };
                        vncProccess.StartInfo = info;
                        vncProccess.Start();
                        vncProcessId = vncProccess.Id;
                        if (vncProcessId > 0) VncRunning = Brushes.Green; else VncRunning = Brushes.Red;
                    }
                }
            }
        }

        private void BtnCaptureAppClick(object sender, RoutedEventArgs e) => MediaOperations.SaveAppScreenShot(this);

        //----------------------------------------------------------------- BEGIN OF MENU REACTIONS -------------------------------------------------------------------------

        /// <summary>
        /// Full dynamic apply setted filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void Cb_FilterDropDownClosed(object sender, EventArgs e) {
            SystemTabs SelectedTab = (SystemTabs)TabablzControl.GetLoadedInstances().Last().SelectedItem;
            string advancedFilter = SystemOperations.FilterToString(cb_filter);

            ((DataViewSupport)Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + SelectedTab.Content.GetType().Name)).GetType().GetField("dataViewSupport").GetValue(null)).AdvancedFilter = advancedFilter;
            cb_filter.SelectedIndex = 0;
            _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + SelectedTab.Content.GetType().Name)).GetType()
                .GetMethod("LoadDataList").Invoke(((SystemTabs)InitialTabablzControl.SelectedItem).Content, null);
        }

        /// <summary>
        /// Full dynamic Show/Hidden datagrid advanced Filter Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void Mi_filter_Click(object sender, RoutedEventArgs e) {
            string mark = DateTime.UtcNow.Ticks.ToString();

            if (((Button)sender).Name == "mi_plus") {
                SystemTabs SelectedTab = (SystemTabs)TabablzControl.GetLoadedInstances().Last().SelectedItem;
                var viewFields = Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + SelectedTab.Content.GetType().Name)).GetType().GetField("selectedRecord").GetValue(null);

                ComboBox cbFieldsBox = new ComboBox() { Name = "field_" + mark, Width = 200, Height = 30 };
                cbFieldsBox.SelectionChanged += FilterField_SelectionChanged;
                foreach (PropertyInfo propertyInfo in viewFields.GetType().GetProperties()) {
                    if (propertyInfo.ToString().Split(' ')[0].ToLower().Replace("system.", "") != "byte[]") {
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
            } else if (((Button)sender).Name == "mi_open" || ((Button)sender).Name == "mi_and" || ((Button)sender).Name == "mi_or") {
                if (((WrapPanel)cb_filter.Items[cb_filter.Items.Count - 1]).Name.Split('_')[0] == "condition") {
                    ((WrapPanel)cb_filter.Items[cb_filter.Items.Count - 1]).Children.Add(new Label() { Content = " " + ((Button)sender).Content + " ", Height = 30, FontSize = 20, Padding = new Thickness(0) });
                } else {
                    WrapPanel dockPanel = new WrapPanel() { Name = "condition_" + mark, Width = 230, Height = 30 };
                    Button removeBtn = new Button() { Name = "remove_" + mark, Width = 30, Content = "X", Height = 30, FontSize = 20, Padding = new Thickness(0) };
                    removeBtn.Click += RemoveFilterItem_Click;
                    dockPanel.Children.Add(removeBtn);
                    dockPanel.Children.Add(new Label() { Content = " " + ((Button)sender).Content + " ", Height = 30, FontSize = 20, Padding = new Thickness(0) });
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
        /// <param name="filterBox">     </param>
        /// <param name="advancedFilter"></param>
        /// <returns></returns>
        public ComboBox StringToFilter(ComboBox filterBox, string advancedFilter) {
            advancedFilter = (!string.IsNullOrWhiteSpace(advancedFilter)) ? advancedFilter : "";
            //clear Filter
            int filterItemCount = filterBox.Items.Count;
            if (filterItemCount > 1) {
                for (int index = 0; index < filterItemCount - 1; index++) {
                    filterBox.Items.RemoveAt(1);
                }
            }

            //Add existing items
            string[] filterLines = advancedFilter.Split(new[] { "[!]" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string filterLine in filterLines) {
                string mark = DateTime.UtcNow.Ticks.ToString();
                if (filterLine.FirstOrDefault().ToString() == " ") { //condition line
                    WrapPanel dockPanel = new WrapPanel() { Name = "condition_" + mark, Width = 230, Height = 30 };
                    Button removeBtn = new Button() { Name = "remove_" + mark, Width = 30, Content = "X", Height = 30, FontSize = 20, Padding = new Thickness(0) };
                    removeBtn.Click += RemoveFilterItem_Click;
                    dockPanel.Children.Add(removeBtn);

                    string[] filterItems = filterLine.Split(new[] { "{!}" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string filterItem in filterItems) {
                        dockPanel.Children.Add(new Label() { Content = filterItem, Height = 30, FontSize = 20, Padding = new Thickness(0) });
                    }
                    filterBox.Items.Add(dockPanel);
                } else // Item Field
                {
                    SystemTabs SelectedTab = (SystemTabs)TabablzControl.GetLoadedInstances().Last().SelectedItem;
                    var viewFields = Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + SelectedTab.Content.GetType().Name)).GetType().GetField("selectedRecord").GetValue(null);

                    ComboBox cbFieldsBox = new ComboBox() { Name = "field_" + mark, Width = 200, Height = 30 };
                    cbFieldsBox.SelectionChanged += FilterField_SelectionChanged;
                    foreach (PropertyInfo propertyInfo in viewFields.GetType().GetProperties()) {
                        if (propertyInfo.ToString().Split(' ')[0].ToLower().Replace("system.", "") != "byte[]") {
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
                    ((TextBox)((WrapPanel)cb_filter.Items[cb_filter.Items.Count - 1]).Children[3]).Text = filterItems[2].Replace("'", "");
                }
            }
            return filterBox;
        }

        /// <summary>
        /// Full dynamic Remove Item from datagrid advanced Filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        public void RemoveFilterItem_Click(object sender, RoutedEventArgs e) {
            ComboBox cb_newFilterItems = cb_filter;
            try { foreach (WrapPanel filterItem in cb_filter.Items) { if (filterItem.Name.Split('_')[1] == ((Button)sender).Name.Split('_')[1]) { cb_newFilterItems.Items.Remove(filterItem); cb_newFilterItems.Items.Refresh(); } } } catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            cb_filter = cb_newFilterItems;
        }

        /// <summary>
        /// Full dynamic set sign datagrid advanced filter type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void FilterField_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            int index = 0;
            try {
                foreach (WrapPanel filterItem in cb_filter.Items) {
                    if (index > 0 && filterItem.Name.Split('_')[1] == ((ComboBox)sender).Name.Split('_')[1]) {
                        ((ComboBox)filterItem.Children[2]).Items.Clear();
                        if ("string?".Contains(((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag.ToString())) ((ComboBox)filterItem.Children[2]).Items.Add(new ComboBoxItem() { Content = " LIKE ", Width = 80 });
                        if ("string?,float?,int?,int32?,int64?,decimal?,double?,bit?,boolean?,datetime?,date?".Contains(((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag.ToString())) {
                            ((ComboBox)filterItem.Children[2]).Items.Add(new ComboBoxItem() { Content = " = ", Width = 80, FontSize = 20 });
                            ((ComboBox)filterItem.Children[2]).Items.Add(new ComboBoxItem() { Content = " <> ", Width = 80, FontSize = 20 });
                        }
                        if (",int?,float?,int32?,int64?,decimal?,double?,datetime?,date?".Contains(((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag.ToString())) {
                            ((ComboBox)filterItem.Children[2]).Items.Add(new ComboBoxItem() { Content = " > ", Width = 80, FontSize = 20 });
                            ((ComboBox)filterItem.Children[2]).Items.Add(new ComboBoxItem() { Content = " < ", Width = 80, FontSize = 20 });
                        }
                    }
                    index++;
                }
            } catch (Exception ex) { App.ApplicationLogging(ex); }
        }

        /// <summary>
        /// Full dynamic on TabPanel drag actually not used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void MainGrid_IsDraggingChanged(object sender, RoutedPropertyChangedEventArgs<bool> e) { }

        /// <summary>
        /// Full dynamic Print Report selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private async void Cb_PrintReportsSelected(object sender, SelectionChangedEventArgs e) {
            try {
                if (cb_printReports.SelectedIndex > -1) {
                    ProgressRing = Visibility.Visible;
                    SqlConnection cnn = new SqlConnection(App.Setting.ReportConnectionString);
                    cnn.Open();
                    if (cnn.State == System.Data.ConnectionState.Open) {
                        cnn.Close();
                        SystemTabs SelectedTab = (SystemTabs)TabablzControl.GetLoadedInstances().Last().SelectedItem;
                        string advancedFilter = ((DataViewSupport)Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + SelectedTab.Content.GetType().Name)).GetType().GetField("dataViewSupport").GetValue(null)).AdvancedFilter;
                        advancedFilter = (string.IsNullOrWhiteSpace(advancedFilter)) ? "1=1" : advancedFilter.Replace("[!]", "").Replace("{!}", "");

                        //Update Filter data for generate Report
                        SetReportFilter setReportFilter = new SetReportFilter() { TableName = SelectedTab.Content.GetType().Name.Replace("Page", ""), Filter = advancedFilter, Search = tb_search.Text, RecId = dataGridSelectedId };
                        string json = JsonConvert.SerializeObject(setReportFilter);
                        StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        DBResultMessage dBResult = await ApiCommunication.PostApiRequest(ApiUrls.ReportQueueList, httpContent, "WriteFilter", App.UserData.Authentification.Token);

                        string reportFile = Path.Combine(App.reportFolder, DateTimeOffset.Now.Ticks.ToString() + ".rdl");
                        if (FileOperations.ByteArrayToFile(reportFile, ((ReportList)cb_printReports.SelectedItem).File)) {
                            Process exeProcess = new Process();
                            ProcessStartInfo info = new ProcessStartInfo() {
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
                    } else {
                        cnn.Close();
                        await ShowMessage(true, Resources["connectionStringIsNotValid"].ToString());
                    }
                    ProgressRing = Visibility.Hidden;
                }
            } catch (Exception ex) {
                App.ApplicationLogging(ex);
                await ShowMessage(true, Resources["connectionStringIsNotValid"].ToString());
                ProgressRing = Visibility.Hidden;
            }
            cb_printReports.SelectedIndex = -1;
        }

        /// <summary>
        /// THIS IS FOR MANUAL UPDATE only for Horizontal menu Only ADD New Page - MENU REACTION and
        /// REWRITE PageName and change And Report CALL as /XXXX open or select existed TabPanel
        /// VERTICAL MENU - Copy and CHANGE ONLY Page Name AND Report CALL as /XXXX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private async void Menu_Selected(object sender, MouseButtonEventArgs e) {
            try {
                string name = ((FrameworkElement)sender).Name;
                if (name.StartsWith("tv_") && !((TreeViewItem)sender).IsExpanded) {
                    foreach (TreeViewItem menuItem in tb_verticalMenu.Items) { menuItem.IsExpanded = false; }
                    ((TreeViewItem)sender).IsExpanded = !((TreeViewItem)sender).IsExpanded;
                } else if (!name.StartsWith("tv_")) {
                    switch (name) {
                        case "tm_adminLoginHistoryList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new AdminLoginHistoryListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/AdminLoginHistoryList", App.UserData.Authentification.Token);
                            break;

                        case "tm_accommodation":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new HotelListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/HotelList", App.UserData.Authentification.Token);
                            break;

                        case "tm_approvingProcess":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new HotelApprovalListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/HotelApprovalList", App.UserData.Authentification.Token);
                            break;

                        case "tm_accessRoleList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new AccessRoleListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/AccessRoleList", App.UserData.Authentification.Token);
                            break;

                        case "tm_addressList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new AddressListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/AddressList", App.UserData.Authentification.Token);
                            break;

                        case "tm_branchList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new BranchListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/BranchList", App.UserData.Authentification.Token);
                            break;

                        case "tm_calendar":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new CalendarPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = null;
                            break;

                        case "tm_cityList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new CityListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/CityList", App.UserData.Authentification.Token);
                            break;

                        case "tm_clientSettings":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new ClientSettingsPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = null;
                            break;

                        case "tm_countryList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new CountryListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/CountryList", App.UserData.Authentification.Token);
                            break;
                        case "tm_countryAreaList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new CountryAreaListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/CountryAreaList", App.UserData.Authentification.Token);
                            break;
                        case "tm_creditPackageList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new CreditPackageListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/CreditPackageList", App.UserData.Authentification.Token);
                            break;

                        case "tm_currencyList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new CurrencyListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/CurrencyList", App.UserData.Authentification.Token);
                            break;

                        case "tm_documentAdviceList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new DocumentAdviceListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/DocumentAdviceList", App.UserData.Authentification.Token);
                            break;

                        case "tm_documentTypeList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new DocumentTypeListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/DocumentTypeList", App.UserData.Authentification.Token);
                            break;

                        case "tm_emailTemplateList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new EmailTemplateListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/EmailTemplateList", App.UserData.Authentification.Token);
                            break;

                        case "tm_hotelImagesList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new HotelImagesListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/HotelImagesList", App.UserData.Authentification.Token);
                            break;

                        case "tm_guestFavoriteList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new GuestFavoriteListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/GuestFavoriteList", App.UserData.Authentification.Token);
                            break;

                        case "tm_guestList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new GuestListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/GuestList", App.UserData.Authentification.Token);
                            break;

                        case "tm_guestLoginHistoryList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new GuestLoginHistoryListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/GuestLoginHistoryList", App.UserData.Authentification.Token);
                            break;

                        case "tm_holidayTipsList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new HolidayTipsListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/HolidayTipsList", App.UserData.Authentification.Token);
                            break; 

                        case "tm_hotelActionTypeList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new HotelActionTypeListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/HotelActionTypeList", App.UserData.Authentification.Token);
                            break;

                        case "tm_hotelReservationList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new HotelReservationListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/HotelReservationList", App.UserData.Authentification.Token);
                            break;

                        case "tm_hotelReservationReviewApprovalList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new HotelReservationReviewApprovalListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/HotelReservationReviewApprovalList", App.UserData.Authentification.Token);
                            break;
                            
                        case "tm_hotelReservationReviewList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new HotelReservationReviewListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/HotelReservationReviewList", App.UserData.Authentification.Token);
                            break;

                        case "tm_hotelReservationStatusList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new HotelReservationStatusListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/HotelReservationStatusList", App.UserData.Authentification.Token);
                            break;

                        case "tm_hotelReservedRoomList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new HotelReservedRoomListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/HotelReservedRoomList", App.UserData.Authentification.Token);
                            break;

                        case "tm_hotelRoomTypeList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new HotelRoomTypeListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/HotelRoomTypeList", App.UserData.Authentification.Token);
                            break;

                        case "tm_exchangeRateList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new ExchangeRateListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/ExchangeRateList", App.UserData.Authentification.Token);
                            break;

                        case "tm_ignoredExceptionList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new IgnoredExceptionListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/IgnoredExceptionList", App.UserData.Authentification.Token);
                            break;

                        case "tm_interestAreaList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) {
                                AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new InterestAreaListPage());
                            }
                            else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/InterestAreaList", App.UserData.Authentification.Token);
                            break;

                        case "tm_languageList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new LanguageListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/LanguageList", App.UserData.Authentification.Token);
                            break;

                        case "tm_mottoList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new MottoListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/MottoList", App.UserData.Authentification.Token);
                            break;

                        case "tm_oftenQuestionList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new OftenQuestionListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/OftenQuestionList", App.UserData.Authentification.Token);
                            break;

                        case "tm_parameterList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new ParameterListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/ParameterList", App.UserData.Authentification.Token);
                            break;
                        case "tm_privacyPolicyList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new PrivacyPolicyListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/PrivacyPolicyList", App.UserData.Authentification.Token);
                            break;
                        case "tm_propertyGroupList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new PropertyGroupListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/PropertyGroupList", App.UserData.Authentification.Token);
                            break;

                        case "tm_propertyOrServiceList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new HotelPropertyAndServiceListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/HotelPropertyOrServiceList", App.UserData.Authentification.Token);
                            break;

                        case "tm_propertyOrServiceTypeList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) 
                                { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new PropertyOrServiceTypeListPage()); 
                            } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/PropertyOrServiceTypeList", App.UserData.Authentification.Token);
                            break;

                        case "tm_propertyOrServiceUnitList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new PropertyOrServiceUnitListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/PropertyOrServiceUnitList", App.UserData.Authentification.Token);
                            break;

                        case "tm_registrationInfoList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new RegistrationInfoListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/RegistrationInfoList", App.UserData.Authentification.Token);
                            break;

                        case "tm_reportList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new ReportListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/ReportList", App.UserData.Authentification.Token);
                            break;

                        case "tm_reportQueueList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new ReportQueueListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/ReportQueueList", App.UserData.Authentification.Token);
                            break;

                        case "tm_roomList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new HotelRoomListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/HotelRoomList", App.UserData.Authentification.Token);
                            break;

                        case "tm_systemLanguageList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new SystemLanguageListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/SystemLanguageList", App.UserData.Authentification.Token);
                            break;
                            
                        case "tm_support":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources["support"].ToString(), new SupportPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = null;
                            break;
                        case "tm_termsList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new TermsListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/TermsList", App.UserData.Authentification.Token);
                            break;
                            
                        case "tm_ubytkacInfoList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new UbytkacInfoListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/UbytkacInfoList", App.UserData.Authentification.Token);
                            break;
                            
                        case "tm_userList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new UserListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/UserList", App.UserData.Authentification.Token);
                            break;

                        case "tm_userRoleList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new UserRoleListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/UserRoleList", App.UserData.Authentification.Token);
                            break;

                        case "tm_systemFailList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new SystemFailListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/SystemFailList", App.UserData.Authentification.Token);
                            break;
                        case "tm_webSettingList":
                            if (TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().Count(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()) == 0) { AddOrRemoveTab(Resources[name.Split('_')[1]].ToString(), new WebSettingListPage()); } else { InitialTabablzControl.SelectedIndex = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders().First(a => a.Content.ToString() == Resources[name.Split('_')[1]].ToString()).LogicalIndex; }
                            StringToFilter(cb_filter, "");
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/WebSettingList", App.UserData.Authentification.Token);
                            break;
                            
                        default:
                            cb_printReports.ItemsSource = null;
                            break;
                    }
                    cb_printReports.IsEnabled = cb_printReports.Items.Count > 0;
                    tb_verticalMenu.IsOverflowOpen = false;
                }
            } catch (Exception ex) { App.ApplicationLogging(ex); }
        }

        /// <summary>
        /// THIS IS FOR MANUAL UPDATE only for Horizontal menu Only ADD New Page - MENU REACTION and
        /// REWRITE PageName and change And Report CALL as /XXXX open or select existed TabPanel
        /// VERTICAL MENU - Copy and CHANGE ONLY Page Name AND Report CALL as /XXXX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private void Menu_action_Click(object sender, RoutedEventArgs e) {
            try {
                SystemTabs SelectedTab = (SystemTabs)TabablzControl.GetLoadedInstances().Last().SelectedItem;
                string senderName = SelectedTab?.Content.GetType().Name;
                DBResultMessage dBResult = new DBResultMessage();
                string buttonSenderName = ((FrameworkElement)sender).Name;
                switch (buttonSenderName) {
                    //AUTOMATIC LIST VIEW PART NOT MODIFY
                    case "tb_search":
                        _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + senderName)).GetType()
                            .GetMethod("Filter").Invoke(((SystemTabs)InitialTabablzControl.SelectedItem).Content, new object[] { ((TextBox)e.Source).Text });
                        break;

                    case "mi_reload":
                        _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + senderName)).GetType()
                             .GetMethod("LoadDataList").Invoke(((SystemTabs)InitialTabablzControl.SelectedItem).Content, null);
                        break;

                    case "mi_new":
                        _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + senderName)).GetType()
                            .GetMethod("NewRecord").Invoke(((SystemTabs)InitialTabablzControl.SelectedItem).Content, null);
                        break;

                    case "mi_edit":
                        _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + senderName)).GetType()
                            .GetMethod("EditRecord").Invoke(((SystemTabs)InitialTabablzControl.SelectedItem).Content, new object[] { false });
                        break;

                    case "mi_copy":
                        _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + senderName)).GetType()
                            .GetMethod("EditRecord").Invoke(((SystemTabs)InitialTabablzControl.SelectedItem).Content, new object[] { true });
                        break;

                    case "mi_delete":
                        _ = Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + senderName)).GetType()
                            .GetMethod("DeleteRecord").Invoke(((SystemTabs)InitialTabablzControl.SelectedItem).Content, null);
                        break;

                    default: break;
                }
                cb_printReports.IsEnabled = cb_printReports.Items.Count > 0;
            } catch (Exception ex) { App.ApplicationLogging(ex); }
        }

        /// <summary>
        /// Full dynamic Open New Tab
        /// </summary>
        /// <param name="headerName"></param>
        /// <param name="tabPage">   </param>
        /// <param name="tagText">   </param>
        internal void AddOrRemoveTab(string headerName, object tabPage = null, string tagText = null) {
            try {
                IEnumerable<DragablzItem> existedTabs = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders();
                existedTabs = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders();

                if (tabPage == null) //Removing TabPage by name
                {
                } else if (existedTabs.Count() > 0) // Select Existing TabPage
                {
                    SystemTabs tc1 = new SystemTabs(headerName, tabPage, tagText);
                    TabablzControl.AddItem(tc1, existedTabs.Last().DataContext, AddLocationHint.After);
                    TabablzControl.SelectItem(tc1);
                    existedTabs = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders();
                }

                if (existedTabs.Count() == 0) // Insert New TabPage
                {
                    SystemWindowDataModel result = new SystemWindowDataModel();
                    result.TabContents.Add(new SystemTabs(headerName, tabPage, tagText));
                    DataContext = result;
                    existedTabs = TabablzControl.GetLoadedInstances().Last().GetOrderedHeaders();
                }
            } catch (Exception ex) { App.ApplicationLogging(ex); }
        }

        /// <summary>
        /// THIS IS FOR MANUAL CHECK - MENU Change REACTION Select AND write your Page name to
        /// correct Page Type: Setting - Special form, View - List Only, Form - List with Detail
        /// Form FORMAT OF PAGE NAME IS API CALLPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">     </param>
        private async void TabPanelOnSelectionChange(object sender, SelectionChangedEventArgs e) {
            try {
                // Closing TabPanel
                if (e.RemovedItems.Count > 0 && ((SystemTabs)e.RemovedItems[0]).Tag != null && ((SystemTabs)e.RemovedItems[0]).Tag.ToString().ToUpperInvariant() == "ActiveSystemSaver".ToUpperInvariant())
                    SetLastUserAction();

                ///Insert TabPanel
                SystemTabs SelectedTab = (SystemTabs)TabablzControl.GetLoadedInstances().Last().SelectedItem;
                DataViewSupport dataViewSupport = new DataViewSupport();

                if (SelectedTab == null || (((FrameworkElement)SelectedTab.Content).Tag != null && ((FrameworkElement)SelectedTab.Content).Tag.ToString() == "Setting")) {
                    //SETTING
                    tb_search.Text = null; dataGridSelectedId = 0; DataGridSelected = false; DataGridSelectedIdListIndicator = false;
                    DgRefresh = false; cb_printReports.ItemsSource = null;
                } else if (SelectedTab == null || new string[] { "View", "Form" }.Contains(((FrameworkElement)SelectedTab.Content).Tag.ToString())) {
                    DataGridSelected = true; DgRefresh = true; string senderName = SelectedTab.Content.GetType().Name;
                    var AutoPageGeneration = Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + senderName)).GetType();
                    switch (((FrameworkElement)SelectedTab.Content).Tag.ToString()) {
                        //FORMS - LIST + DETAIL FORM
                        case "Form":
                            dataViewSupport = ((DataViewSupport)Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + senderName)).GetType().GetField("dataViewSupport").GetValue(null));
                            tb_search.Text = dataViewSupport.FilteredValue;
                            if (dataViewSupport.FormShown) { dataGridSelectedId = 0; DataGridSelectedIdListIndicator = false; DataGridSelected = false; DgRefresh = false; } else {
                                if (dataViewSupport.SelectedRecordId == 0) { dataGridSelectedId = 0; DataGridSelectedIdListIndicator = false; } else { dataGridSelectedId = dataViewSupport.SelectedRecordId; DataGridSelectedIdListIndicator = true; }
                            }
                            StringToFilter(cb_filter, dataViewSupport.AdvancedFilter);
                            cb_printReports.ItemsSource = await ApiCommunication.GetApiRequest<List<ReportList>>(ApiUrls.ReportList, dataGridSelectedId.ToString() + "/" + senderName.Replace("Page", ""), App.UserData.Authentification.Token);
                            break;

                        //VIEWS - LIST ONLY
                        case "View":
                            dataViewSupport = ((DataViewSupport)Convert.ChangeType(SelectedTab.Content, Type.GetType("UbytkacAdmin.Pages." + senderName)).GetType().GetField("dataViewSupport").GetValue(null));
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
            } catch (Exception ex) { App.ApplicationLogging(ex); }
        }
    }
}