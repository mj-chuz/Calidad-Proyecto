using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Planetario.Models {
  public class ActividadModel {
    public const string APROBADA = "Aprobada";
    public const string PENDIENTE = "Pendiente";
    public const string RECHAZADA = "Rechazada";
    public const string CHARLA = "Charla";
    public const string TALLER = "Taller";

    public class FechaFutura : ValidationAttribute {
      public override bool IsValid(object valor) {
        return valor != null && (DateTime)valor > DateTime.Now;
      }
    }

    public struct IdentificadorActividad {
      public String Nombre { get; set; }
      public DateTime Fecha { get; set; }

      public override string ToString() {
        return Fecha.ToString("yyyyMMddHHmmss") + " " + Nombre;
      }

      public IdentificadorActividad(String datos) {
        Fecha = DateTime.ParseExact(datos.Substring(0, 10), "dd-MM-yyyy", null);
        Nombre = datos.Substring(11);
      }

      public static implicit operator IdentificadorActividad(String codificada) {
        return new IdentificadorActividad(codificada);
      }
    }
    [FechaFutura(ErrorMessage = "La fecha debe ser posterior a la fecha actual")]
    [Required(ErrorMessage = "Es necesario que ingrese una fecha")]
    [Display(Name = "Fecha y la hora de inicio*")]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese un título")]
    [Display(Name = "Título*")]
    public String Titulo { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese una descripción")]
    [Display(Name = "Descripción*")]
    public String Descripcion { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese un precio sugerido en colones")]
    [Display(Name = "Precio sugerido en colones*")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar un número")]
    public int PrecioSugerido { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese una duración en minutos")]
    [Display(Name = "Duración en minutos*")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar un número")]
    public int Duracion { get; set; }

    public String Estado { get; set; }

    [Required(ErrorMessage = "Es necesario que seleccione un tipo de modalidad")]
    [Display(Name = "Modalidad*")]
    public String Modalidad { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese un correo")]
    [Display(Name = "Correo*")]
    [EmailAddress(ErrorMessage = "Correo inválido")]
    public String Correo { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese el público meta")]
    [Display(Name = "Público meta*")]
    public List<String> PublicoMeta { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese el nivel de complejidad")]
    [Display(Name = "Nivel de complejidad*")]
    public String NivelDeComplejidad { get; set; }

    [Required(ErrorMessage = "Es necesario que seleccione el tipo de actividad")]
    [Display(Name = "Tipo de actividad*")]
    public String TipoDeActividad { get; set; }

    public List<MaterialModel> Materiales { get; set; }
    
    [Required(ErrorMessage = "Es necesario que seleccione por lo menos un tópico")]
    [Display(Name = "Tópicos de la actividad*")]
    public List<String> Topicos { get; set; }

    [Required(ErrorMessage = "Es necesario que seleccione una categoría")]
    [Display(Name = "Categoría de la actividad*")]
    public String Categoria { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese una cantidad de cupos máxima")]
    [Display(Name = "Cupos disponibles*")]
    public int CuposDisponibles { get; set; }

    public String NumeroIdentificacionFuncionario { get; set; }

    [Display(Name = "Enlace a la transmisión de la actividad")]
    [Url(ErrorMessage = "Enlace inválido")]
    public String EnlaceStream { get; set; }
  }
}