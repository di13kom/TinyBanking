using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        UserObject_Class BankingObject;
        HttpClient NetworkClient;
        string SettingsFile = "Settings.json";
        public MainWindow()
        {
            InitializeComponent();
            MonthCombo.ItemsSource = Enumerable.Range(1, 12);
            YearCombo.ItemsSource = Enumerable.Range(2015, 20);

            BankingObject = LoadSettings();
            //BankingObject = new BankingUserObject();
            ////
            //BankingObject.Amount = 100;
            //BankingObject.CardHolder = "noName";
            //BankingObject.CardNumber = 12341234;
            //BankingObject.Cvv = 5344;
            //BankingObject.ExpireDate = 01.2018m;
            //BankingObject.OrderId = 2342424;
            //
            this.DataContext = BankingObject;

            NetworkClient = new HttpClient();
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
            string jsonObj = JsonConvert.SerializeObject(BankingObject);
            strCont = new StringContent(jsonObj, Encoding.UTF8, "application/json");
            //strCont.Headers.Add("Allow", "Application/json");
            //strCont.Headers.Add("Content-type", "Aplication/json");
            strCont.Headers.Add("Content-Length", jsonObj.Length.ToString());
            return await NetworkClient.PostAsync("http://localhost:7777/", strCont);
        }

        private UserObject_Class LoadSettings()
        {
            UserObject_Class retVal = null;
            try
            {
                if (File.Exists(SettingsFile))
                {
                    string inFile = File.ReadAllText(SettingsFile);
                    retVal = JsonConvert.DeserializeObject<UserObject_Class>(inFile);
                }
                else
                    retVal = new UserObject_Class();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return retVal;
        }

        private void SaveSettings()
        {
            try
            {
                string jsObj = JsonConvert.SerializeObject(BankingObject);
                File.WriteAllText(SettingsFile, jsObj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveSettings_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }
    }

}
