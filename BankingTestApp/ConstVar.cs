﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingTestApp
{
    public static class ConstVar
    {
        public static string BaseUri = "http://localhost:7777";
        public static string[] Prefixes = new string[]
        {
                    "/PayIn/",
                    "/Refund/",
                    "/GetStatus/"
        };
        public static string JsonMIME = "application/json";

        //DB
        public static string DbName = @".\BankDB.db";
        public static string DbOperationTable = "Cards_Operations";
        public static string DbDepositCardsTable = "Cards_Deposit";
        public static string DbCustomersTable = "Customers";
    }
}