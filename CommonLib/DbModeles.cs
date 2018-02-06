using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public interface ICustomers
    {
        string FirstName { get; set; }
        string SecondName { get; set; }
    }

    public interface IDeposit
    {
        double CardNumber { get; set; }
        short CVV { get; set; }
        decimal ExpireDate { get; set; }
    }
   
}
