using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace elawyerWebApp.Models.ViewModels
{
    public class UserVM
    {
        public int No { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Surname { get; set; }
        [Required]
        [StringLength(10)]
        public string NIF { get; set; }
        [Required]
        [StringLength(12)]
        public string Telephone { get; set; }
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(30)]
        public string Username { get; set; }
        [Required]
        [StringLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int NoRole { get; set; }
    }
}