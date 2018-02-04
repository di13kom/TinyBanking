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
        Int64 _amount;
        double _cardNumber;
        decimal _expireDate;
        short _cvv;
        string _cardholder;

        public Int64 Amount
        {
            get
            {
                return _amount;
            }

            set
            {
                if (value > 0)
                    _amount = value;
                else
                    _amount = 0;
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
                if (value > 0)
                    _cardNumber = value;
                else
                    _cardNumber = 0;
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
                if (value > 0)
                    _cvv = value;
                else
                    _cvv = 0;
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
                if (value > 0)
                    _orderId = value;
                else
                    _orderId = 0;
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
