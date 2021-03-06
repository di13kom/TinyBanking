﻿using CommonLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingTestApp
{
    public class DbAdapter_Class : IDbClass
    {
        SQLiteConnection con = new SQLiteConnection(@"Data Source=" + ConstVar.DbName + "; Version=3;");

        public void test()
        {
            con.Open();

            SQLiteCommand sc = con.CreateCommand();
            sc.CommandText = "SELECT * FROM Customers";
            SQLiteDataReader r = sc.ExecuteReader();

            string line = string.Empty;
            while (r.Read())
            {
                line = r["Id"].ToString() + "-"
                    + r["FirstName"].ToString() + "-"
                    + r["SecondName"].ToString();
                Console.WriteLine(line);
            }
            r.Close();
            sc.Dispose();
            con.Dispose();
        }

        public int GetPaymentStatus(double id)
        {
            int retVal = (int)ErrorCodes.UnknownError;
            SQLiteCommand sc = null;
            SQLiteDataReader reader = null;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();

                sc = con.CreateCommand();
                sc.CommandText = $"SELECT * FROM { ConstVar.DbOperationTable} where SubjectId={id}";
                reader = sc.ExecuteReader();

                if (reader.Read() == true)
                {
                    retVal = int.Parse(reader["IsRefunded"].ToString());
                }
                else
                    retVal = (int)ErrorCodes.WrongOrderId;//6 payment not exist
            }
            catch (SQLiteException sqlEx)
            {
                Console.WriteLine($"Sqlite error in GetPaymentStatus: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Common error in GetPaymentStatus: {ex.Message}");
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (sc != null)
                    sc.Dispose();
            }

            return retVal;
        }

        public int RefundPayment(double id)
        {
            int retVal = (int)ErrorCodes.UnknownError;
            SQLiteCommand sc = null;
            SQLiteDataReader reader = null;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();

                sc = con.CreateCommand();
                sc.CommandText = $"SELECT * FROM { ConstVar.DbOperationTable} where SubjectId={id}";
                reader = sc.ExecuteReader();

                if (reader.Read() == true)
                {
                    //

                    double amount = double.Parse(reader["Amount"].ToString());
                    double cardNum = double.Parse(reader["CardId"].ToString());
                    int isAlreadyRefunded = int.Parse(reader["IsRefunded"].ToString());
                    //
                    reader.Close();
                    reader = null;
                    //
                    if (isAlreadyRefunded == 0)
                    {
                        sc.CommandText = $"UPDATE {ConstVar.DbOperationTable} SET IsRefunded=1 WHERE SubjectId={id}";
                        if (sc.ExecuteNonQuery() == 1)
                            retVal = (int)ErrorCodes.OperationSuccess;
                        //Added Trigger
                        ////refund amount to deposit 
                        //sc.CommandText = $"UPDATE {ConstVar.DbDepositCardsTable} SET Amount=Amount+{amount} WHERE Id={cardNum}";
                        //sc.ExecuteNonQuery();
                    }
                    else
                        retVal = (int)ErrorCodes.PaymentRefunded;
                }
                else
                    retVal = (int)ErrorCodes.WrongOrderId;//6 payment not exist
            }
            catch (SQLiteException sqlEx)
            {
                Console.WriteLine($"Sqlite error in GetPaymentStatus: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Common error in GetPaymentStatus: {ex.Message}");
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (sc != null)
                    sc.Dispose();
            }

            return retVal;
        }

        public int PayIn(double subjectId, double cardNumber, decimal expireDate, short cvv, string cardHolder, long amount)
        {
            int retVal = (int)ErrorCodes.UnknownError;

            SQLiteCommand sc = null;
            SQLiteDataReader reader = null;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();

                sc = con.CreateCommand();
                sc.CommandText = $"SELECT dpst.Amount FROM { ConstVar.DbCustomersTable} cst JOIN {ConstVar.DbDepositCardsTable} dpst "
                    + $"ON dpst.CardHolder=cst.Id WHERE dpst.CardNumber={cardNumber} AND dpst.ExpireDate={expireDate} "
                    + $"AND dpst.CVV={cvv} AND cst.SecondName||' '||cst.FirstName='{cardHolder}';";
                //reader = sc.ExecuteReader();
                object obj = sc.ExecuteScalar();
                //if (reader.Read() == true)
                if (obj != null)
                {
                    long dbAmount = long.Parse(obj.ToString());
                    if (dbAmount - amount > 0)
                    {
                        sc.CommandText = $"SELECT CASE WHEN CAST(STRFTIME('%m.%Y','now')AS DECIMAL)<{expireDate} THEN 1 ELSE 0 END " +
                            $"FROM {ConstVar.DbDepositCardsTable} WHERE CardNumber={cardNumber}";
                        if (int.Parse(sc.ExecuteScalar().ToString()) == 1)
                        {
                            sc.CommandText = $"INSERT INTO {ConstVar.DbOperationTable} (CardId, Amount, SubjectId) "
                                + $"SELECT Id, {amount}, {subjectId} FROM {ConstVar.DbDepositCardsTable} "
                                + $"WHERE CardNumber={cardNumber};";
                            if (sc.ExecuteNonQuery() == 1)
                                retVal = (int)ErrorCodes.OperationSuccess;
                        }
                        else
                            retVal = (int)ErrorCodes.Cardisexpired;//30 Exipre Card
                    }
                    else
                        retVal = (int)ErrorCodes.Insufficientbalance;//20 Insufficient balance
                }
                else
                    retVal = (int)ErrorCodes.WrongUserData;//10 Wrong UserData
            }
            catch (SQLiteException sqlEx)
            {
                Console.WriteLine($"Sqlite error in GetPaymentStatus: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Common error in GetPaymentStatus: {ex.Message}");
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (sc != null)
                    sc.Dispose();
            }
            return retVal;
        }
    }

    public class DataBaseContext : DbContext, IDbClass
    {
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Cards_Deposit> Deposit { get; set; }
        public DbSet<Cards_Operations> Operation { get; set; }
        public DataBaseContext() : base("DefaultConnection")
        {
            try
            {
                Customers.Load();
                Deposit.Load();
                Operation.Load();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public int GetPaymentStatus(double id)
        {
            int retVal = (int)ErrorCodes.UnknownError;
            try
            {
                var Val = Operation.FirstOrDefault(x => x.SubjectId == id);
                if (Val == null)
                    throw new DbException(ErrorCodes.WrongOrderId);
                else
                    retVal = Val.IsRefunded;

            }
            catch (DbException dbEx)
            {
                retVal = (int)dbEx.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }

        public int RefundPayment(double id)
        {
            int retVal = (int)ErrorCodes.UnknownError;
            try
            {
                var Val = Operation.FirstOrDefault(x => x.SubjectId == id);

                if (Val == null)
                    throw new DbException(ErrorCodes.WrongOrderId);
                if (Val.IsRefunded > 0)
                    throw new DbException(ErrorCodes.PaymentRefunded);

                Val.IsRefunded = 1;
                this.SaveChanges();
                retVal = (int)ErrorCodes.OperationSuccess;

            }
            catch (DbException dbEx)
            {
                retVal = (int)dbEx.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }

        public int PayIn(double subjectId, double cardNumber, decimal expireDate, short cvv, string cardHolder, long in_amount)
        {
            int retVal = (int)ErrorCodes.UnknownError;
            try
            {
                if (Operation.Any(x => x.SubjectId == subjectId))
                    throw new DbException(ErrorCodes.SubjectIdNotUnique);
                if (in_amount <= 0)
                    throw new DbException(ErrorCodes.WrongAmount);

                var Value = Deposit
                    .Where(x => x.ExpireDate == expireDate && x.CardNumber == cardNumber && x.CVV == cvv && (x.Customers.SecondName + " " + x.Customers.FirstName) == cardHolder)
                    .Select(z => new
                    {
                        z.Amount,
                        z.ExpireDate,
                        CardId = z.Id
                    })
                    .FirstOrDefault();

                if (Value == null)
                    throw new DbException(ErrorCodes.WrongOrderId);

                if (Value.Amount < in_amount)
                    throw new DbException(ErrorCodes.Insufficientbalance);

                if (Value.ExpireDate < decimal.Parse(DateTime.Now.ToString("yyyy.MM")))
                    throw new DbException(ErrorCodes.Cardisexpired);

                Operation.Add(new Cards_Operations
                {
                    CardId = Value.CardId,
                    SubjectId = (int)subjectId,
                    Amount = in_amount,
                    IsRefunded = 0,//default
                });
                this.SaveChanges();
                retVal = (int)ErrorCodes.OperationSuccess;
            }
            catch (DbException dbEx)
            {
                retVal = (int)dbEx.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }

        private class DbException : Exception
        {
            private ErrorCodes _id;

            public DbException(ErrorCodes id)
            {
                _id = id;
            }

            public ErrorCodes Id
            {
                get { return _id; }
                set { _id = value; }
            }
        }
    }
}
