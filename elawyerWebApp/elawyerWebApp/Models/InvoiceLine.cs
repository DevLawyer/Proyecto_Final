//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace elawyerWebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvoiceLine
    {
        public int No { get; set; }
        public string Concept { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public int NoInvoiceHeader { get; set; }
    
        public virtual InvoiceHeader InvoiceHeader { get; set; }
    }
}
