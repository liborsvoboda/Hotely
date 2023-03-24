using TravelAgencyBackEnd.DBModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Transactions;

namespace BACKENDCORE.Services
{
    public static class LoginHistoryService
    {
        public static async void WriteVisit(string ipAddress, string userName, string terminalName = "1")
        {
            // Save new visit
            LoginHistoryList record = new() { IpAddress = ipAddress, UserName = userName, TerminalName = terminalName, Timestamp = DateTimeOffset.Now.DateTime };
            var result = new hotelsContext().LoginHistoryLists.Add(record);
            await result.Context.SaveChangesAsync();
        }
    }
}
