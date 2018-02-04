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
        SQLiteConnection con = new SQLiteConnection(@"Data Source="+ConstVar.DbName+"; Version=3;");

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
    }
}
