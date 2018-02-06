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

        override public short CVV
        {
            get { return base.CVV; }
            set
            {
                base.CVV = value;
                NotifyPropertyChanged("CVV");
            }
        }

        override public string SecondName
        {
            get { return base.SecondName; }
            set
            {
                base.SecondName = value.Trim();
                NotifyPropertyChanged("SecondName");
            }
        }

        override public string FirstName
        {
            get { return base.FirstName; }
            set
            {
                base.FirstName = value.Trim();
                NotifyPropertyChanged("FirstName");
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
                _message = string.Join(" - ",DateTime.Now.ToString(), value);
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
