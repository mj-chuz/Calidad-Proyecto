using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Planetario.Models {
  public class PreguntaModel {

    [Required(ErrorMessage = "Es necesario que ingrese una pregunta")]
    [Display(Name = "Ingrese la pregunta")]
    public string PreguntaHecha { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese la respuesta")]
    [Display(Name = "Ingrese la respuesta")]
    public string Respuesta { get; set; }

    public string Categoria { get; set; }

    [Required(ErrorMessage = "Es necesario que ingrese al menos un topico")]
    [Display(Name = "Seleccione un topico")]
    public List<string> Topicos { get; set; }
  }
}