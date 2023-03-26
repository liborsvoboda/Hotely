using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stripe;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Text;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TravelAgencyBackEnd.DBModel;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.WebSockets;

namespace TravelAgencyBackEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddEndpointsApiExplorer();
            services.AddDbContext<hotelsContext>(opt => opt.UseSqlServer("Server=95.183.52.33;Database=hotels;User ID=sa;Password=Hotel2023+;"));
            services.AddControllersWithViews(options => {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            }).AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }).AddJsonOptions(x => {
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                x.JsonSerializerOptions.WriteIndented = true;
            });

            services.AddRouting();

            StripeConfiguration.ApiKey = "sk_test_51Ix5jxB3kuAuBTpumfFOUXqYLSAiDcCYrIejgIX48knYENVJdkaB42V1VdqBPIuvM9KbuWwSrkfMNxkjYSrQucx500FLoljwSC";

            if (Program.EnableSwagger || Program.EnableReDoc)
            {
                services.AddSwaggerGen(c =>
                {
                    //c.InferSecuritySchemes();
                    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                    { Name = "Authorization", Type = SecuritySchemeType.Http, Scheme = "basic", In = ParameterLocation.Header, Description = "Basic Authorization header using the Bearer scheme." });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                        { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basic" } }, new string[] {} } });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    { Description = "JWT Authorization header using the Bearer scheme.", Name = "Authorization", In = ParameterLocation.Header, Scheme = "bearer", Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http, BearerFormat = "JWT" });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                        { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new List<string>() } });

                    c.SchemaGeneratorOptions = new SchemaGeneratorOptions { SchemaIdSelector = type => type.FullName };
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Travel Agency BackEnd Server API",
                        Version = "v1",
                        Description = Program.BackendServiceName,
                        Contact = new OpenApiContact { Name = "Libor Svoboda", Email = "Libor.Svoboda@GroupWare-Solution.Eu", Url = new Uri("https://groupware-solution.eu/contactus") },
                        License = new OpenApiLicense { Name = Program.BackendServiceName + " Server License", Url = new Uri("https://www.groupware-solution.eu/") }
                    });

                    var xmlName = "TravelAgencyBackEnd.xml";
                    var xmlFile = Path.Combine(AppContext.BaseDirectory, xmlName);
                    if (System.IO.File.Exists(xmlFile)) c.IncludeXmlComments(xmlFile, true);

                    c.UseOneOfForPolymorphism();
                    c.DescribeAllParametersInCamelCase();
                    c.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);
                    c.UseInlineDefinitionsForEnums();
                    c.SupportNonNullableReferenceTypes();
                    ////c.UseAllOfToExtendReferenceSchemas();
                    c.DocInclusionPredicate((docName, description) => true);
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                });
                services.AddSwaggerGen();
            }

            services.AddCors();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.FromMinutes(0),
                };
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }

            if (Program.EnableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "AdminApiDocs";
                    c.DocumentTitle = Program.BackendServiceName;
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server API V1");
                    c.DocExpansion(DocExpansion.None);
                    c.EnableTryItOutByDefault();
                    c.DisplayRequestDuration();
                    c.EnableDeepLinking();
                    c.EnableFilter();
                    c.DisplayOperationId();
                    c.DefaultModelExpandDepth(2);
                    c.DefaultModelRendering(ModelRendering.Model);
                    c.DefaultModelsExpandDepth(1);
                    //c.EnablePersistAuthorization();
                    c.EnableValidator();
                    c.ShowCommonExtensions();
                    c.ShowExtensions();
                    c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head);
                    c.UseRequestInterceptor("(request) => { return request; }");
                    c.UseResponseInterceptor("(response) => { return response; }");
                });
            }

            if (Program.EnableReDoc)
            {
                app.UseSwagger();
                app.UseReDoc(c =>
                {
                    c.RoutePrefix = "ViewerApiDocs";
                    c.DocumentTitle = Program.BackendServiceName;
                    c.SpecUrl("/swagger/v1/swagger.json");
                    c.EnableUntrustedSpec();
                    c.ScrollYOffset(10);
                    //c.HideHostname();
                    //c.HideDownloadButton();
                    c.ExpandResponses("200,201");
                    //c.ExpandResponses("all");
                    c.RequiredPropsFirst();
                    //c.NoAutoAuth();
                    c.PathInMiddlePanel();
                    //c.HideLoading();
                    c.NativeScrollbars();
                    //c.DisableSearch();
                    //c.OnlyRequiredInSamples();
                    c.SortPropsAlphabetically();
                });
            }



            /*
            return app.UseCors(
                    builder =>
                    {
                        var allowedDomains = new[] { "http://localhost", "http://localhost:4200", "http://localhost:8080" };

                        builder
                        .WithOrigins(allowedDomains)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    }
                );
            });
            */

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
            //app.UseHttpsRedirection();
            app.UseWebSockets();
            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
