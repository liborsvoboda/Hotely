using EASYTools.CrashReporter;
using TravelAgencyAdmin.Classes;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.GlobalClasses;

namespace TravelAgencyAdmin.Helper
{
     /// <summary>
    /// Libreria condivisa
    /// </summary>
    public class CrashReporterGlobalField
    {
        /// <summary>
        /// FirstRun
        /// </summary>
        /// 
        //public static ReportCrash _ReportCrash = new ReportCrash()
        //{
        //    FromEmail = UserPrincipal.Current.EmailAddress,

        //    ToEmail = App.ServerSetting.Find(a => a.Key == ServerSettingKeys.ServiceEmail.ToString()).Value,
        //    SMTPHost = App.ServerSetting.Find(a => a.Key == ServerSettingKeys.SMTPServer.ToString()).Value,
        //    Port = int.Parse(App.ServerSetting.Find(a => a.Key == ServerSettingKeys.SMTPPort.ToString()).Value),
        //    UserName = App.ServerSetting.Find(a => a.Key == ServerSettingKeys.SMTPUserName.ToString()).Value,
        //    Password = App.ServerSetting.Find(a => a.Key == ServerSettingKeys.SMTPPassword.ToString()).Value,
        //    EnableSSL = true,
        //};


    }
}
