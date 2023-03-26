using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TravelAgencyBackEnd.DBModel;

namespace BACKENDCORE.Services
{
    public static class LoginHistoryService
    {

        public static async void WriteVisit(string ipAddress, int userId, string userName)
        {
            // Save new visit
            if (!string.IsNullOrWhiteSpace(userName))
            {
                LoginHistoryList record = new() { IpAddress = ipAddress,UserId = userId, UserName = userName, Timestamp = DateTimeOffset.Now.DateTime };
                var result = new hotelsContext().LoginHistoryLists.Add(record);
                await result.Context.SaveChangesAsync();
            }
        }
    }
}
