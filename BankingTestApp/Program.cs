using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BankingTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string chars = "localhost:7777";
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add("http://" + chars + "/");
                listener.Start();
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest req = context.Request;
                if (req.ContentType == "\"application/json\"" && req.HttpMethod == "POST")
                {
                    HttpListenerResponse resp = context.Response;
                    resp.ContentType = "application/json";
                    using (Stream str = resp.OutputStream)
                    {
                        string stringOut = "{\"GU\":25, \"GB\":35}";
                        byte[] bytes = Encoding.UTF8.GetBytes(stringOut);
                        str.Write(bytes, 0, bytes.Count());
                        //Console.Read();
                    }
                    //str.Close();
                }
                listener.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
        }
    }
}
