using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace elawyerWebApp.Models.ViewModels
{
    public class InvoiceGral
    {
        public InvoiceHVM InvHeader {get; set;}
        public InvoiceLVM InvLine { get; set; }
    }
}