using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;

namespace TravelAgencyBackEnd
{

    public class ServerModulesConfiguration
    {

        public static readonly string SwaggerModuleDescription = "Full Backend Server DB & API & WebSocket model";


        internal static void ConfigureSwagger(ref IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
                { Name = "Authorization", Type = SecuritySchemeType.Http, Scheme = "basic", In = ParameterLocation.Header, Description = "Basic Authorization header for getting Bearer Token." });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" } }, new List<string>() } });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                { Description = "JWT Authorization header using the Bearer scheme for All safe APIs.", Name = "Authorization", In = ParameterLocation.Header, Scheme = "bearer", Type = SecuritySchemeType.Http, BearerFormat = "JWT" });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new List<string>() } });


                c.SchemaGeneratorOptions = new SchemaGeneratorOptions { SchemaIdSelector = type => type.FullName };
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TravelAgencyBackEnd Server API",
                    Version = "v1",
                    Description = SwaggerModuleDescription,
                    Contact = new OpenApiContact { Name = "Libor Svoboda", Email = "Libor.Svoboda@GroupWare-Solution.Eu", Url = new Uri("https://groupware-solution.eu/contactus") },
                    License = new OpenApiLicense { Name = Program.BackendServiceName + " Server License", Url = new Uri("https://www.groupware-solution.eu/") }
                });

                var xmlFile = Path.Combine(AppContext.BaseDirectory, "TravelAgencyBackEnd.xml");
                if (File.Exists(xmlFile)) c.IncludeXmlComments(xmlFile, true);

                //c.InferSecuritySchemes();
                c.UseOneOfForPolymorphism();
                c.DescribeAllParametersInCamelCase();
                    
                c.EnableAnnotations(true, true);
                c.UseAllOfForInheritance();
                c.UseInlineDefinitionsForEnums();
                c.SupportNonNullableReferenceTypes();
                //c.UseAllOfToExtendReferenceSchemas();
                c.DocInclusionPredicate((docName, description) => true);
                c.CustomSchemaIds(type => type.FullName);
                c.ResolveConflictingActions(x => x.First());
            });
            //services.AddSwaggerGenNewtonsoftSupport();

        }
    }




    public class ServerModulesEnabling
    {


        /// <summary>
        /// Server Module: Enable Swagger Api Doc Generator And Online Tester
        /// </summary>
        /// <param name="services"></param>
        internal static void EnableSwagger(ref IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "AdminApiDocs";
                c.DocumentTitle = ServerModulesConfiguration.SwaggerModuleDescription;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server API V1");
                c.DocExpansion(DocExpansion.None);
                c.EnableTryItOutByDefault();
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
                c.EnableFilter();
                c.DisplayOperationId();
                c.DefaultModelExpandDepth(3);
                c.DefaultModelRendering(ModelRendering.Model);
                c.DefaultModelsExpandDepth(0);
                //c.EnablePersistAuthorization();
                c.EnableValidator();
                c.ShowCommonExtensions();
                c.ShowExtensions();
                c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head);
                c.UseRequestInterceptor("(request) => { return request; }");
                c.UseResponseInterceptor("(response) => { return response; }");
            });

        }
    }
}