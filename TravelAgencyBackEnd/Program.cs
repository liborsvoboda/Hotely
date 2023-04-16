using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using TravelAgencyBackEnd.CoreClasses;
using TravelAgencyBackEnd.DBModel;

namespace TravelAgencyBackEnd
{
    public class Program {

        public static readonly string BackendServiceName = "TravelAgencySystem";
        public static readonly bool DebugMode = "Development" == Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public static readonly bool UseDBLocalAutoupdatedDials = true;


        /// <summary>
        /// Local DB dials: LanguageList
        /// </summary>
        /// <param name="args"></param>
        public static List<LanguageList> ServerDBLanguageList = new();


        /// <summary>
        /// Server Startup Process
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args) {

            //Load StartupDBdata
            if (UseDBLocalAutoupdatedDials) ServerStartupDbDataLoading();

            CreateHostBuilder(args).Build().Run();
        }


        /// <summary>
        /// Final Preparing Server HostBuilder Definition
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(new string[] { "http://*:5000", "https://*:5001" });
                });


        /// <summary>
        /// Server Startup DB Data loaging for minimise DB Connect TO Frequency Dials Without Changes
        /// Example: LanguageList 
        /// </summary>
        private static void ServerStartupDbDataLoading() {
            DBOperations.LoadStaticDbDials();
        }
    }
}
