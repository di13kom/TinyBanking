using System;
using System.Collections.Generic;
using System.Linq;
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
            ordObject.ExpireDate = DateTime.Today;
            ordObject.OrderId = 2342424;
            //
            this.DataContext = ordObject;
        }
    }
}
