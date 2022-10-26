using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Planetario.Models {
  public class NoticiaModel {
    [Required(ErrorMessage = "Es necesario que la noticia tenga título")]
    [Display(Name = "Ingrese el título de la noticia")]
    public string Titulo { get; set; }
    [AllowHtml]
    public string Contenido { get; set; }
    public DateTime Fecha { get; set; }
    public string NombreImagen { get; set; }
    [Required(ErrorMessage = "Debe agregar una imagen")]
    [Display(Name = "Inserte una imagen")]
    public HttpPostedFileBase ArchivoImagen { get; set; }
    public string NombreArchivo { get; set; }

  }
}