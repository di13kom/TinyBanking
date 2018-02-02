using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class OrderObject : INotifyPropertyChanged
    {
        long _orderId;
        decimal _amount;
        double _cardNumber;
        DateTime _expireDate;
        short _cvv;
        string _cardholder;

        public decimal Amount
        {
            get
            {
                return _amount;
            }

            set
            {
                _amount = value;
                NotifyPropertyChanged("Amount");
            }
        }

        public double CardNumber
        {
            get
            {
                return _cardNumber;
            }

            set
            {
                _cardNumber = value;
                NotifyPropertyChanged("CardNumber");
            }
        }

        public DateTime ExpireDate
        {
            get
            {
                return _expireDate;
            }

            set
            {
                _expireDate = value;
                NotifyPropertyChanged("ExpireDate");
            }
        }

        public short Cvv
        {
            get
            {
                return _cvv;
            }

            set
            {
                _cvv = value;
                NotifyPropertyChanged("Cvv");
            }
        }

        public string CardHolder
        {
            get
            {
                return _cardholder;
            }

            set
            {
                _cardholder = value;
                NotifyPropertyChanged("CardHolder");
            }
        }

        public long OrderId
        {
            get
            {
                return _orderId;
            }

            set
            {
                _orderId = value;
                NotifyPropertyChanged("OrderId");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
