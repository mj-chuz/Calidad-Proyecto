using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planetario.Models {
  public class ImagenModel {
    public string Nombre { get; set; }
    public string Extension { get; set; }
    public HttpPostedFileBase ArchivoImagen { get; set; }
  }
}