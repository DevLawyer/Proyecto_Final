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
        [Required(ErrorMessage = "El nombre es obligatorio y no puede tener más de 50 caracteres.")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio y no puede tener más de 50 caracteres.")]
        [StringLength(50)]
        public string Surname { get; set; }
        [Required(ErrorMessage = "El NIF es obligatorio y no puede tener más de 10 caracteres.")]
        [StringLength(10)]
        public string NIF { get; set; }
        [Required(ErrorMessage = "El teléfono es obligatorio y no puede tener más de 12 caracteres.")]
        [StringLength(12)]
        public string Telephone { get; set; }
        [Required(ErrorMessage = "El email es obligatorio y no puede tener más de 50 caracteres.")]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "El nombre de usuario es obligatorio y no puede tener más de 30 caracteres.")]
        [StringLength(30)]
        public string Username { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria y no puede tener más de 30 caracteres.")]
        [StringLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "El rol es obligatorio y no puede tener más de 1 caracter.")]
        [StringLength(1)]
        public int NoRole { get; set; }
    }
}