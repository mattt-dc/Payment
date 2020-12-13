using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Data
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) :
            base(options)
        {
        }

        public DbSet<Authorization> Authorizations { get; set; }

        //The connection string should be in a config file
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(@"Server=db;Database=master;User=sa;Password=Pa55w0rd;");
    }
}
