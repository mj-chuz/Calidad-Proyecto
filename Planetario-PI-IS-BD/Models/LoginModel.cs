using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Planetario.Models {
  public class LoginModel {
    [Required(ErrorMessage = "Es necesario que ingrese su número de identificación ")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar numeros")]
    [Display(Name = "Número de identificación")]
    public String NumeroIdentificacion { get; set; }
    [Required(ErrorMessage = "Es necesario que ingrese su contraseña ")]
    [Display(Name = "Contraseña")]
    public String Contrasena { get; set; }

  }
}