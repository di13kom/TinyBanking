using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankingTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Server server = new Server();
            server.Operate();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Console.WriteLine($"Not handled Exception: {ex.Message}\nSource: {ex.Source}\nStackTrace: {ex.StackTrace}");

            Environment.Exit(1);
        }
    }
}
