using CommonLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    class NetworkAgent
    {
        HttpClient NetworkClient;

        public NetworkAgent()
        {
            NetworkClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> SendAsync(string inUrl, BankObject BankingObject)
        {
            StringContent strCont;
            string jsonObj = JsonConvert.SerializeObject(BankingObject);

            using (strCont = new StringContent(jsonObj, Encoding.UTF8, ConstVar.JsonMIME))
            {
                //strCont.Headers.Add("Allow", "Application/json");
                //strCont.Headers.Add("Content-type", "Aplication/json");
                strCont.Headers.Add("Content-Length", jsonObj.Length.ToString());
                return await NetworkClient.PostAsync(inUrl, strCont);
            }
        }
    }
}
