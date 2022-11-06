using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace Planetario.Models {
  public class MaterialModel {

    [Required(ErrorMessage = "Debe ingresar el nombre del autor")]
    [Display(Name = "Autor:")]
    public String Autor { get; set; }
    [Required(ErrorMessage = "Debe ingresar el título")]
    [Display(Name = "Título:")]
    public String Titulo { get; set; }
    [Required(ErrorMessage = "Debe seleccionar al menos un tópico")]
    public List<String>Topicos { get; set; }
    public ActividadModel.IdentificadorActividad Actividad { get; set; }
    [Required(ErrorMessage = "Debe seleccionar una actividad")]
    [Display(Name = "Actividad:")]
    public String ActividadCodificada { get; set; }
    public DateTime FechaDePublicacion { get; set; }
    [Required(ErrorMessage = "Debe ingresar un archivo")]
    public HttpPostedFileBase Archivo { get; set; }
    public String NombreArchivo { get; set; }

    public String IdCreador { get; set; }

    public String getNombreCarpeta() {
      return this.Titulo + "_" + this.FechaDePublicacion.ToString("yyyyMMdd");
    }
  }
}