using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingTestApp
{
    interface IDbClass
    {
        int GetPaymentStatus(double id);
        int RefundPayment(double id);
        int PayIn(double subjectId, double cardNumber, decimal expireDate, short cvv, string cardHolder, long amount);
    }
}
