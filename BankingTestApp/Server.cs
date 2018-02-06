using CommonLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankingTestApp
{
    public class Server
    {
        private SemaphoreSlim Sem;
        private IDbClass DBClass;
        HttpListener listener;

        public Server()
        {
            Sem = new SemaphoreSlim(0);
            DBClass = new DataBaseContext();
            listener = new HttpListener();
        }

        public void Operate()
        {
            try
            {
                foreach (var str in ConstVar.Prefixes)
                {

                    listener.Prefixes.Add(ConstVar.BaseUri + str);
                }
                listener.Start();
                while (listener.IsListening)
                {
                    listener.BeginGetContext(new AsyncCallback(AcceptCallback), listener);
                    Console.WriteLine("....");

                    Sem.Wait();
                }
                listener.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
        }

        private void AcceptCallback(IAsyncResult result)
        {
            int val = (int)ErrorCodes.UnknownError;
            string stringOut = string.Empty;
            JObject jObj;

            HttpListener listener = (HttpListener)result.AsyncState;
            Sem.Release();
            HttpListenerContext context = listener.EndGetContext(result);

            HttpListenerRequest req = context.Request;
            Console.WriteLine($"incoming connection from: {req.RemoteEndPoint.Address.ToString()}");
            if (req.ContentType.Contains(ConstVar.JsonMIME) && req.HttpMethod == "POST")
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
                Console.WriteLine($"requested Url: {req.RawUrl}");
                jObj = JObject.Parse(inDataB.ToString());

                string reqUrl = req.RawUrl;

                double idVal = double.Parse(jObj["OrderId"].ToString());
                if (reqUrl == ConstVar.Prefixes[0])// "/PayIn/":
                {
                    double cardNum = double.Parse(jObj["CardNumber"].ToString());
                    decimal expDate = decimal.Parse(jObj["ExpireDate"].ToString());
                    short cvv = short.Parse(jObj["CVV"].ToString());
                    string firstName = jObj["FirstName"].ToString();
                    string SecondName = jObj["SecondName"].ToString();
                    long amount = long.Parse(jObj["Amount"].ToString());

                    val = DBClass.PayIn(idVal, cardNum, expDate, cvv, string.Join(" ", SecondName, firstName), amount);
                }
                else if (reqUrl == ConstVar.Prefixes[1])// "/Refund/":
                {
                    val = DBClass.RefundPayment(idVal);
                }
                else if (reqUrl == ConstVar.Prefixes[2])// "/GetStatus/":
                {
                    val = DBClass.GetPaymentStatus(idVal);
                }
                if (val == (int)ErrorCodes.OperationSuccess)
                    stringOut = $"{{\"Status\":\"Ok\", \"Value\":{val}}}";
                else
                    stringOut = $"{{\"Status\":\"Error\", \"Value\":{val}}}";

                HttpListenerResponse resp = context.Response;
                resp.ContentType = ConstVar.JsonMIME;
                using (Stream str = resp.OutputStream)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(stringOut);
                    str.Write(bytes, 0, bytes.Count());
                }
                //str.Close();
            }
        }
    }
}
