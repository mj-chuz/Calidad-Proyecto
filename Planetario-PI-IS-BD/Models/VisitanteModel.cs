using System;
using System.ComponentModel.DataAnnotations;

namespace Planetario.Models {
  public class VisitanteModel {

    [Required(ErrorMessage = "Es necesario que ingrese su nombre")]
    [Display(Name = "Nombre*")]
    public String Nombre { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese su primer apellido")]
    [Display(Name = "Primer apellido*")]
    public String PrimerApellido { get; set; }

    [Display(Name = "Segundo apellido:")]
    public String SegundoApellido { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese su nacionalidad")]
    [Display(Name = "Nacionalidad*")]
    public String Pais { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese su correo")]
    [Display(Name = "Correo*")]
    public String Correo { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese su número de identificación ")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar numeros")]
    [Display(Name = "Número de identificación*")]
    public String NumeroIdentificacion { get; set; }

    [Required(ErrorMessage = "Es necesario que seleccione su género")]
    [Display(Name = "Género*")]
    public String Genero { get; set; }

    [Required(ErrorMessage = "Es necesario que seleccione su fecha de nacimiento")]
    [Display(Name = "Fecha de nacimiento*")]
    public DateTime FechaDeNacimiento { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese su nivel educativo")]
    [Display(Name = "Nivel educativo*")]
    public String NivelEducativo { get; set; }

    public String TituloActividadInscrita { get; set; }

    public DateTime FechaActividadInscrita { get; set; }
  }
}