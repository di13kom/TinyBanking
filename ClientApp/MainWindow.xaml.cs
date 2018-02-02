using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OrderObject ordObject;
        HttpClient client;
        public MainWindow()
        {
            InitializeComponent();
            MonthCombo.ItemsSource = Enumerable.Range(1, 12);
            YearCombo.ItemsSource = Enumerable.Range(2015, 20);

            ordObject = new OrderObject();
            //
            ordObject.Amount = 100;
            ordObject.CardHolder = "noName";
            ordObject.CardNumber = 12341234;
            ordObject.Cvv = 5344;
            ordObject.ExpireDate = 01.2018m;
            ordObject.OrderId = 2342424;
            //
            this.DataContext = ordObject;

            client = new HttpClient();
        }

        private async void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            string respString;
            try
            {
                using (HttpResponseMessage response = await SendAsync())
                    //Console.WriteLine($"response: {response.Content}");
                    respString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task<HttpResponseMessage> SendAsync()
        {
            StringContent strCont;
            string jsonObj = JsonConvert.SerializeObject(ordObject);
            strCont = new StringContent(jsonObj, Encoding.UTF8, "application/json");
            //strCont.Headers.Add("Allow", "Application/json");
            //strCont.Headers.Add("Content-type", "Aplication/json");
            strCont.Headers.Add("Content-Length", jsonObj.Length.ToString());
            return await client.PostAsync("http://localhost:7777/", strCont);

        }
    }
}
