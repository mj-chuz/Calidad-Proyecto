using System;
using System.ComponentModel.DataAnnotations;

namespace Planetario.Models {
  public class TelescopiadaPeliculaModel {
    public class FechaFutura : ValidationAttribute {
      public override bool IsValid(object valor) {
        return valor != null && (DateTime)valor > DateTime.Now;
      }
    }

    [Required(ErrorMessage = "Es necesario que ingrese un título ")]
    [Display(Name = "Título*")]
    public String Titulo { get; set; }

    [FechaFutura(ErrorMessage = "La fecha debe ser posterior a la fecha actual")]
    [Required(ErrorMessage = "Es necesario que ingrese una fecha")]
    [Display(Name = "Fecha y la hora de inicio*")]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese una descripción")]
    [Display(Name = "Descripción*")]
    public String Descripcion { get; set; }

    [Required(ErrorMessage = "Es necesario que seleccione el tipo de evento")]
    [Display(Name = "Tipo de evento*")]
    public String Tipo { get; set; }

    public String NumeroIdentificacionFuncionario { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese una duración en minutos")]
    [Display(Name = "Duración en minutos*")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar un número válido")]
    public int Duracion { get; set; }    
  }
}