using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace elawyerWebApp.Models.ViewModels
{
    public class IssueVM
    {
        public int No { get; set; }
        [Required]
        [StringLength(15)]
        public string NoRef { get; set; }
        [Required]
        [StringLength(300)]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public System.DateTime CreationDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public System.DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> EndDate { get; set; }        
        public Nullable<decimal> Hours { get; set; }
        [Required]
        public decimal Fee { get; set; }
        [Required]
        public int NoClient { get; set; }
        [Required]
        public int NoStatus { get; set; }
    }
}