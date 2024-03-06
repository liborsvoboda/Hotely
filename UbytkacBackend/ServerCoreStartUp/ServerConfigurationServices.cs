using FubarDev.FtpServer;
using FubarDev.FtpServer.AccountManagement;
using FubarDev.FtpServer.FileSystem.DotNet;
using LettuceEncrypt;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quartz;
using SimpleMvcSitemap;
using Snickler.RSSCore.Extensions;
using UbytkacBackend.Controllers;

namespace UbytkacBackend.ServerCoreConfiguration {

    /// <summary>
    /// Server Core Configuration Settings of Security, Communications, Technologies, Modules Rules,
    /// Rights, Conditions, Formats, Services, Logging, etc..
    /// </summary>
    public class ServerConfigurationServices {

        /// <summary>
        /// Custom Secure FTP Server
        /// </summary>
        /// <param name="services">The services.</param>
        internal static void ConfigureFTPServer(ref IServiceCollection services) {
            if (ServerConfigSettings.ServerFtpEngineEnabled) {
                services.AddFtpServer(
                                    builder => {
                                        if (!ServerConfigSettings.ServerFtpSecurityEnabled) { builder.EnableAnonymousAuthentication(); }
                                        builder.UseDotNetFileSystem().DisableChecks().UseSingleRoot();
                                    });

                services.Configure<FtpServerOptions>(opt => opt.ServerAddress = "*");
                services.Configure<DotNetFileSystemOptions>(opt => {
                    opt.RootPath = Path.Combine(ServerRuntimeData.WebRoot_path, ServerConfigSettings.ServerFtpStorageRootPath);
                    opt.AllowNonEmptyDirectoryDelete = true;
                });
                services.AddSingleton<IMembershipProvider, HostedFtpServerMembershipProvider>();
                services.AddHostedService<HostedFtpServer>();

                using (var serviceProvider = services.BuildServiceProvider()) {
                    ServerRuntimeData.ServerFTPProvider = serviceProvider.GetRequiredService<IFtpServerHost>();
                }
            }
        }

        /// <summary>
        /// Server Core: Configure Cookie Politics
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureCookie(ref IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(options => {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
                options.Secure = CookieSecurePolicy.Always;
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
            });
        }

        /// <summary>
        /// Server Core: Configure Server Controllers
        /// options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = [ValidateNever]
        /// in Class options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        /// = [JsonIgnore] in Class
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureControllers(ref IServiceCollection services) {
            //services.AddServerSideBlazor(options => { options.RootComponents.MaxJSRootComponents = 1000; });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddControllersWithViews(options => {
                //if (ServerConfigSettings.ConfigServerStartupOnHttps) { options.Filters.Add(new RequireHttpsAttribute()); }
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            }).AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.Formatting = Formatting.Indented;
            }).AddJsonOptions(x => {
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                x.JsonSerializerOptions.WriteIndented = true;
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        }

        /// <summary>
        /// Server Core: Configure Server Logging
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureLogging(ref IServiceCollection services) {
            if (ServerRuntimeData.DebugMode) {
                services.AddLogging(builder => {
                    builder.AddConsole().AddDebug().SetMinimumLevel(LogLevel.Debug)
                    .AddFilter<ConsoleLoggerProvider>(category: null, level: LogLevel.Debug)
                    .AddFilter<DebugLoggerProvider>(category: null, level: LogLevel.Debug);
                });
            }
            services.AddHttpLogging(logging => {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua"); logging.ResponseHeaders.Add("RequestJsonFormatNotCorrectly");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = logging.ResponseBodyLogLimit = 4096;
            });
        }

        /// <summary>
        /// Server Core: Configure Server Authentication Support
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureAuthentication(ref IServiceCollection services) {
            //FUTURE  Certificate Auth
            //if (ServerConfigSettings.ConfigServerStartupHttpAndHttps || ServerConfigSettings.ConfigServerStartupOnHttps) {
            //    services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate(options =>
            //    {
            //        options.Events = new CertificateAuthenticationEvents {
            //            OnCertificateValidated = context => { var claims = new[]{
            //        new Claim(context.ClientCertificate.Subject,
            //            ClaimValueTypes.String, context.Options.ClaimsIssuer),
            //        new Claim(ClaimTypes.Name,context.ClientCertificate.Subject,
            //            ClaimValueTypes.String, context.Options.ClaimsIssuer)};
            //                context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
            //                context.Success();return Task.CompletedTask;
            //            }
            //        };
            //    });
            //}

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
                x.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = CoreOperations.ValidAndGetTokenParameters();
                x.ForwardSignIn = "/Login";

                if (ServerConfigSettings.ConfigTimeTokenValidationEnabled) { x.TokenValidationParameters.LifetimeValidator = AuthenticationService.LifetimeValidator; }

                x.Events = new JwtBearerEvents {
                    OnAuthenticationFailed = context => {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException)) {
                            context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        /// <summary>
        /// Configures the MVC server pages on Server format "cshtml"
        /// </summary>
        /// <param name="services">The services.</param>
        internal static void ConfigureServerWebPages(ref IServiceCollection services) {
            if (ServerConfigSettings.WebRazorPagesEngineEnabled) {
                if (ServerConfigSettings.WebRazorPagesCompileOnRuntime) {
                    services.AddMvc(options => {
                        options.CacheProfiles.Add("Default30", new CacheProfile() { Duration = 300 });
                    }).AddRazorPagesOptions(opt => {
                        opt.RootDirectory = "/ServerCorePages";
                    }).AddRazorRuntimeCompilation();
                }
                else {
                    services.AddMvc(options => {
                        options.CacheProfiles.Add("Default30", new CacheProfile() { Duration = 300 });
                    }).AddRazorPagesOptions(opt => {
                        opt.RootDirectory = "/ServerCorePages";
                    }).AddRazorRuntimeCompilation();
                }//services.AddRazorPages();
            }

            if (ServerConfigSettings.WebMvcPagesEngineEnabled) {
                if (ServerConfigSettings.WebRazorPagesCompileOnRuntime) {
                    services.AddMvc(options => {
                        options.EnableEndpointRouting = false;
                        options.AllowEmptyInputInBodyModelBinding = true;
                    }).AddRazorRuntimeCompilation();
                }
                else {
                    services.AddMvc(options => {
                        options.EnableEndpointRouting = false;
                        options.AllowEmptyInputInBodyModelBinding = true;
                    });
                }
            }
        }

        /// <summary>
        /// Server core: Configures LetsEncrypt using. Certificate will be saved in DataPath
        /// </summary>
        /// <param name="services">The services.</param>
        internal static void ConfigureLetsEncrypt(ref IServiceCollection services) {
            if (ServerConfigSettings.ConfigServerGetLetsEncrypt) {
                services.AddLettuceEncrypt(option => {
                    List<string> domainList = ServerConfigSettings.ConfigCertificateDomain.Contains(',')
                    ? ServerConfigSettings.ConfigCertificateDomain.Split(',').ToList()
                    : ServerConfigSettings.ConfigCertificateDomain.Split(';').ToList();

                    domainList.ForEach(domain => { if (string.IsNullOrWhiteSpace(domain)) domainList.Remove(domain); });
                    option.DomainNames = domainList.ToArray();
                    option.EmailAddress = ServerConfigSettings.EmailerServiceEmailAddress;
                    option.AcceptTermsOfService = true;
                }).PersistDataToDirectory(new DirectoryInfo(System.IO.Path.Combine(ServerRuntimeData.Startup_path, ServerRuntimeData.DataPath)), ServerConfigSettings.ConfigCertificatePassword);
            }
        }

        internal static void ConfigureRSSfeed(ref IServiceCollection services) {
            if (ServerConfigSettings.WebRSSFeedsEnabled) { services.AddRSSFeed<SomeRSSProvider>(); }
        }

        /// <summary>
        /// Server core: Configures the WebSocket logger monitor. For multi monitoring and for
        /// Example Posibilities
        /// </summary>
        /// <param name="services">The services.</param>
        internal static void ConfigureWebSocketLoggerMonitor(ref IServiceCollection services) {
            if (ServerConfigSettings.WebSocketServerMonitorEnabled) { services.AddSingleton<ILoggerProvider, WebSocketLogProvider>(); }
        }

        /// <summary>
        /// Server Core: Configure HTTP Client for work with third party API
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureThirdPartyApi(ref IServiceCollection services) {
            //services.AddHttpClient();
        }

        /// <summary>
        /// Server Core: Configures the singletons. Its Register Custom Listeners For Actions
        /// </summary>
        /// <param name="services">The services.</param>
        internal static void ConfigureSingletons(ref IServiceCollection services) {
            services.AddSingleton<IHttpContextAccessor, CommunicationController>();
            services.AddSingleton<ISitemapProvider, SitemapProvider>();
            services.AddSingleton<IHealthCheckPublisher, DelegateHealthCheckPublisher>();
        }

        /// <summary>
        /// Server Core: Configure Custom Core Services
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureScoped(ref IServiceCollection services) {
            services.AddScoped(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>));
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        }

        /// <summary>
        /// Server Core: Configure Hosted Service to Runtime Core For Access to Root Functionalities
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureCentralServicesProviders(ref IServiceCollection services) {
            ServerRuntimeData.ServerServiceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Server Core: Configure Custom Services
        /// </summary>
        /// <param name="services"></param>
        internal static void ConfigureDatabaseContext(ref IServiceCollection services) {
            if (ServerRuntimeData.DebugMode) { services.AddDatabaseDeveloperPageExceptionFilter(); }
            try {
                services.AddDbContext<hotelsContext>(opt => opt.UseSqlServer(ServerConfigSettings.DatabaseConnectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            } catch (Exception ex) { }
        }
    }
}