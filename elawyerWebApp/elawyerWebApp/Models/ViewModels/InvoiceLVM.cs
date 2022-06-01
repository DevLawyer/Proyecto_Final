using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace elawyerWebApp.Models.ViewModels
{
    public class InvoiceLVM
    {
        public string Concept { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public int NoInvoiceHeader { get; set; }
    }
}