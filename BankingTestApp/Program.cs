using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                while (listener.IsListening)
                {
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest req = context.Request;
                    Console.WriteLine($"incoming connection from: {req.RemoteEndPoint.Address.ToString()}");
                    if (req.ContentType.Contains("application/json") && req.HttpMethod == "POST")
                    {
                        //string inData;
                        StringBuilder inDataB = new StringBuilder();
                        byte[] inputByte = new byte[10];

                        using (Stream inStr = req.InputStream)
                        //using (StreamReader reader = new StreamReader(inStr))
                        {
                            int readEl = 0;
                            do
                            {
                                readEl = inStr.Read(inputByte, 0, 10);
                                inDataB.Append(Encoding.UTF8.GetChars(inputByte), 0, readEl);
                            }
                            while (readEl > 0);
                            //inData = reader.ReadToEnd();
                        }
                        JObject jObj = JObject.Parse(inDataB.ToString());
                        HttpListenerResponse resp = context.Response;
                        resp.ContentType = "application/json";
                        using (Stream str = resp.OutputStream)
                        {
                            string stringOut = $"{{\"Status\":\"Ok\", \"ErrorCode\":0, \"Sum\":{jObj["Amount"]}}}";
                            byte[] bytes = Encoding.UTF8.GetBytes(stringOut);
                            str.Write(bytes, 0, bytes.Count());
                            //Console.Read();
                        }
                        //str.Close();
                    }
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
