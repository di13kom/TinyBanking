using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingTestApp
{
    public class DbAdapter_Class
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
            int retVal = -1;
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
                    retVal = 6;//payment not exist
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
            int retVal = -1;
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
                    double amount = double.Parse(reader["Amount"].ToString());
                    double cardNum = double.Parse(reader["CardId"].ToString());

                    sc.CommandText = $"UPDATE {ConstVar.DbOperationTable} SET IsRefunded=1 WHERE SubjectId={id}";
                    sc.ExecuteNonQuery();
                    //refund amount to deposit
                    sc.CommandText = $"UPDATE {ConstVar.DbDepositCardsTable} SET Amount=Amount+{amount} WHERE Id={cardNum}";
                    sc.ExecuteNonQuery();
                }
                else
                    retVal = 6;//payment not exist
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
            int retVal = -1;

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
                                retVal = 0;
                        }
                        else
                            retVal = 30;//Exipre Card
                    }
                    else
                        retVal = 20;//Insufficient balance
                }
                else
                    retVal = 10;//Wrong UserData
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
}
