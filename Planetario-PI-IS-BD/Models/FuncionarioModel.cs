using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Planetario.Models {
  public class FuncionarioModel {
    public class FechaFutura : ValidationAttribute {
      public override bool IsValid(object valor) {
        return valor != null && (DateTime)valor < DateTime.Now;
      }
    }

    [Required(ErrorMessage = "Es necesario que ingrese su número de identificación ")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar numeros")]
    [Display(Name = "Número de identificación*")]
    public String NumeroIdentificacion { get; set; }

    [Required(ErrorMessage = "Es necesario que indique su nombre")]
    [Display(Name = "Nombre*")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "Es necesario que indique su primer apellido")]
    [Display(Name = "Primer apellido*")]
    public string PrimerApellido { get; set; }

    [Display(Name = "Segundo apellido")]
    public string SegundoApellido { get; set; }

    [Required(ErrorMessage = "Es necesario que indique su correo")]
    [EmailAddress(ErrorMessage = "Correo inválido")]
    [Display(Name = "Correo*")]
    public string Correo { get; set; }
    [Required(ErrorMessage = "Es necesario que seleccione el país de nacimiento")]
    [Display(Name = "País de nacimiento*")]
    public string Pais { get; set; }

    [Required(ErrorMessage = "Es necesario que seleccione su rol")]
    [Display(Name = "Rol dentro del planetario*")]
    public string Rol { get; set; }

    [Required(ErrorMessage = "Es necesario que indique su número de teléfono")]
    [Display(Name = "Número de teléfono*")]
    public string Telefono { get; set; }

    [Required(ErrorMessage = "Es necesario que indique una descripción suya")]
    [Display(Name = "Descripción suya*")]
    public string Descripcion { get; set; }

    [Required(ErrorMessage = "Es necesario que indique un título académico")]
    [Display(Name = "Título académico*")]
    public string TituloAcademico { get; set; }

    [Required(ErrorMessage = "Es necesario que indique su ocupación")]
    [Display(Name = "Ocupación*")]
    public string Ocupacion { get; set; }

    [Display(Name = "Género")]
    public string Genero { get; set; }
    [FechaFutura(ErrorMessage = "La fecha de nacimiento debe ser anterior a la fecha actual")]
    [Required(ErrorMessage = "Es necesario que indique su fecha de nacimiento")]
    [Display(Name = "Fecha de nacimiento*")]
    public DateTime FechaDeNacimiento { get; set; }

    public string IdentificadorFoto {get; set;}

    [Required(ErrorMessage = "Es necesario que agregue una foto")]
    [Display(Name = "Foto*")]
    public HttpPostedFileBase archivoFoto { get; set; }

    public String Contrasena { get; set; }

    public string GetNombreCompleto() {
      return Nombre + ' ' + PrimerApellido + ' ' + SegundoApellido;
    }

    [Required(ErrorMessage = "Es necesario que seleccione al menos un idioma")]
    [Display(Name = "Idiomas*")]
    public List<string> Idiomas { get; set; }
    [Display(Name = "Código de funcionario:")]
    public string CodigoFuncionario { get; set; }

    public FuncionarioModel() {

    }
  }
}
