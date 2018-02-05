using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class BankObject
    {
        long _orderId;
        Int64 _amount;
        double _cardNumber;
        decimal _expireDate;
        short _cvv;
        string _cardholder;

        virtual public Int64 Amount
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
            }
        }

        virtual public double CardNumber
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
            }
        }

        virtual public decimal ExpireDate
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
            }
        }

        virtual public short Cvv
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
            }
        }

        virtual public string CardHolder
        {
            get
            {
                return _cardholder;
            }

            set
            {
                _cardholder = value;
            }
        }

        virtual public long OrderId
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
            }
        }
    }
}
