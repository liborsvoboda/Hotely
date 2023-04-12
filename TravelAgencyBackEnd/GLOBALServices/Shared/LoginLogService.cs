using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TravelAgencyBackEnd.DBModel;

namespace TravelAgencyBackEnd.Services
{
    public static class LoginLogService
    {

        public static async void WriteAppLogin(string ipAddress, int userId, string userName)
        {
            // Save new visit
            if (!string.IsNullOrWhiteSpace(userName))
            {
                AdminLoginHistoryList record = new() { IpAddress = ipAddress,UserId = userId, UserName = userName, Timestamp = DateTimeOffset.Now.DateTime };
                var result = new hotelsContext().AdminLoginHistoryLists.Add(record);
                await result.Context.SaveChangesAsync();
            }
        }

        public static async void WriteWebLogin(string ipAddress, int guestId, string email)
        {
            // Save new visit
            if (!string.IsNullOrWhiteSpace(email))
            {
                GuestLoginHistoryList record = new() { IpAddress = ipAddress, GuestId = guestId, Email = email, Timestamp = DateTimeOffset.Now.DateTime };
                var result = new hotelsContext().GuestLoginHistoryLists.Add(record);
                await result.Context.SaveChangesAsync();
            }
        }
    }
}
