﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Entities
{
    public class Payment
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
