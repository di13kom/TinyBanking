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
        StatusReflection MessWithStatus = new StatusReflection();
        HttpClient NetworkClient;
        string SettingsFile = "Settings.json";
        public MainWindow()
        {
            InitializeComponent();
            MonthCombo.ItemsSource = Enumerable.Range(1, 12);
            YearCombo.ItemsSource = Enumerable.Range(2015, 20);

            BankingObject = LoadSettings();

            this.DataContext = BankingObject;
            MainTextBlock.DataContext = MessWithStatus;

            NetworkClient = new HttpClient();
        }

        private async void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            string respString;
            string btnName = ((Button)sender).Name;
            string baseUrl = "http://localhost:7777";
            string uri = string.Empty;
            string url;
            if (btnName == Pay_Button.Name)
            {
                uri = "/PayIn/";
            }
            else if (btnName == Refund_Button.Name)
            {
                uri = "/Refund/";
            }
            else if (btnName == GetStatus_Button.Name)
            {
                uri = "/GetStatus/";
            }
            try
            {
                url = baseUrl + uri;
                using (HttpResponseMessage response = await SendAsync(url))
                {
                    respString = await response.Content.ReadAsStringAsync();
                    //response.Dispose();
                    MessWithStatus.SetValues(respString, respString.Contains("\"Status\":\"Ok\"") == false);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                MessWithStatus.SetValues(ex.Message, true);
            }
        }

        async Task<HttpResponseMessage> SendAsync(string inUrl)
        {
            StringContent strCont;
            string jsonObj = JsonConvert.SerializeObject(BankingObject);

            using (strCont = new StringContent(jsonObj, Encoding.UTF8, "application/json"))
            {
                //strCont.Headers.Add("Allow", "Application/json");
                //strCont.Headers.Add("Content-type", "Aplication/json");
                strCont.Headers.Add("Content-Length", jsonObj.Length.ToString());
                return await NetworkClient.PostAsync(inUrl, strCont);
            }
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

                    MessWithStatus.SetValues("Settings file's been loaded", false);
                }
                else
                {
                    retVal = new UserObject_Class();

                    MessWithStatus.SetValues("Settings file is emtpy", true);
                }
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

                MessWithStatus.SetValues("Settings file's been saved", false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessWithStatus.SetValues("Settings file saving error", true);
            }
        }

        private void SaveSettings_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }
    }

}
