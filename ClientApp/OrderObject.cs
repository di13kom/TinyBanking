using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class UserObject_Class : BankObject, INotifyPropertyChanged
    {
        override public Int64 Amount
        {
            get { return base.Amount; }
            set
            {
                base.Amount = value;
                NotifyPropertyChanged("Amount");
            }
        }

        override public double CardNumber
        {
            get { return base.CardNumber; }
            set
            {
                base.CardNumber = value;
                NotifyPropertyChanged("CardNumber");
            }
        }

        override public decimal ExpireDate
        {
            get { return base.ExpireDate; }
            set
            {
                base.ExpireDate = value;
                NotifyPropertyChanged("ExpireDate");
            }
        }

        override public short Cvv
        {
            get { return base.Cvv; }
            set
            {
                base.Cvv = value;
                NotifyPropertyChanged("Cvv");
            }
        }

        override public string CardHolder
        {
            get { return base.CardHolder; }
            set
            {
                base.CardHolder = value;
                NotifyPropertyChanged("CardHolder");
            }
        }

        override public long OrderId
        {
            get { return base.OrderId; }
            set
            {
                base.OrderId = value;
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
