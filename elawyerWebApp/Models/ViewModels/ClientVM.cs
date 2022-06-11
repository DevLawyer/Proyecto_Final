using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace elawyerWebApp.Models.ViewModels
{
    public class ClientVM
    {
        public int No { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Surname { get; set; }
        [Required]
        [StringLength(50)]
        public string NIF { get; set; }
        [StringLength(50)]
        public string Address1 { get; set; }
        [StringLength(50)]
        public string Address2 { get; set; }
        [StringLength(12)]
        public string Telephone { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        public Nullable<int> NoUser { get; set; }
    }
}