using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    static class ConstVar
    {
        public static string SettingsFile = "Settings.json";

        public static string BaseUri = "http://localhost:7777";
        public static string[] Prefixes = new string[]
        {
                    "/PayIn/",
                    "/Refund/",
                    "/GetStatus/"
        };
        public static string JsonMIME = "application/json";

        public static Dictionary<int, string> ErrorDesc = new Dictionary<int, string>
        {
            {-1,    "Unknown Error" },
            { 0,    "Operation Success" },
            { 1,    "Payment Refunded" },
            { 6,    "Wrong OrderId" },
            { 10,   "Wrong User Data" },
            { 20,   "Insufficient balance" },
            { 30,   "Card is expired" },

        };
    }
}
