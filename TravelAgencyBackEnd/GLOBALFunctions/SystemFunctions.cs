using System.Text;
using System.IO;
using System;
using TravelAgencyBackEnd.DBModel;
using System.Linq;

namespace TravelAgencyBackEnd
{
    class SystemFunctions
    {
        public static string GetUserApiErrMessage(Exception exception, int msgCount = 1)
        {
            return exception != null ? string.Format("{0}: {1}\n{2}", msgCount, exception.Message, GetUserApiErrMessage(exception.InnerException, ++msgCount)) : string.Empty;
        }

        public static string GetSystemErrMessage(Exception exception, int msgCount = 1)
        {
            return exception != null ? string.Format("{0}: {1}\n{2}", msgCount, (exception.Message + Environment.NewLine + exception.StackTrace + Environment.NewLine), GetSystemErrMessage(exception.InnerException, ++msgCount)) : string.Empty;
        }

    }
}