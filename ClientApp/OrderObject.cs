using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class UserObject_Class : INotifyPropertyChanged
    {
        long _orderId;
        decimal _amount;
        double _cardNumber;
        decimal _expireDate;
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

        public decimal ExpireDate
        {
            get
            {
                return _expireDate;
            }

            set
            {
                if (_expireDate == 0)
                    _expireDate = value;
                else
                {
                    if (value > 1)
                        _expireDate = (_expireDate - Math.Truncate(_expireDate)) + value;
                    else
                        _expireDate = Math.Truncate(_expireDate) + value;
                }

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

    public class StatusReflection : INotifyPropertyChanged
    {
        string _message;
        bool _isWarningMessage;

        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
                NotifyPropertyChanged("Message");
            }
        }

        public bool IsWarningMessage
        {
            get
            {
                return _isWarningMessage;
            }

            set
            {
                _isWarningMessage = value;
                NotifyPropertyChanged("IsWarningMessage");
            }
        }

        public void SetValues(string inStr, bool inFlag)
        {
            Message = inStr;
            IsWarningMessage = inFlag;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
