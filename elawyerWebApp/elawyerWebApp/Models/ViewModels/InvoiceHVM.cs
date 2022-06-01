using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace elawyerWebApp.Models.ViewModels
{
    public class InvoiceHVM
    {
        public int No { get; set; }
        [Required]
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public int NoUser { get; set; }
        [Required]
        public int NoIssue { get; set; }
        [Required]
        public int NoStatus { get; set; }
    }
}