﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public class Customers
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int Age { get; set; }
        public int Sex { get; set; }

        public ICollection<Cards_Deposit> Card_Deposit { get; set; }
    }

    public class Cards_Deposit
    {
        public int Id { get; set; }
        public int CardNumber { get; set; }
        public short CVV { get; set; }
        public decimal ExpireDate { get; set; }
        [ForeignKey("Customers")]
        public int CardHolder { get; set; }
        public virtual Customers Customers { get; set; }
        public double Amount { get; set; }
        public double Limit { get; set; }

        public ICollection<Cards_Operations> Operations { get; set; }
    }

    public class Cards_Operations
    {
        public int Id { get; set; }
        [ForeignKey("Cards_Deposit")]
        public int CardId { get; set; }
        public virtual Cards_Deposit Cards_Deposit { get; set; }
        public double Amount { get; set; }
        public int SubjectId { get; set; }
        public int IsRefunded { get; set; }
    }
}
