using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Planetario.Handlers;
using Planetario.Models;

namespace Planetario.Controllers {
    public class ImagenController : Controller {

    [HttpGet]
    public FileResult AccederImagen(string nombreImagen) {
      ImagenHandler accesoDatosImagen = new ImagenHandler();
      ImagenModel imagenRecuperada = accesoDatosImagen.ObtenerImagen(nombreImagen);
      return File(ObtenerBytes(imagenRecuperada.ArchivoImagen), imagenRecuperada.Extension);
    }

    public byte[] ObtenerBytes(HttpPostedFileBase archivo) {
      byte[] bytesDeArchivo;
      BinaryReader lectorBytesArchivo = new BinaryReader(archivo.InputStream);
      bytesDeArchivo = lectorBytesArchivo.ReadBytes(archivo.ContentLength);
      return bytesDeArchivo;
    }

  }
}