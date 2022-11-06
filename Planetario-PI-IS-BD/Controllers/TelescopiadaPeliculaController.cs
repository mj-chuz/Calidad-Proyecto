using System.Web.Mvc;
using Planetario.Models;
using Planetario.Handlers;
using System;
using System.Web;

namespace Planetario.Controllers{
    public class TelescopiadaPeliculaController : Controller{

    private TelescopiadaPeliculaHandler AccesoADatosTelescopiadaPelicula;

    public TelescopiadaPeliculaController() {
      AccesoADatosTelescopiadaPelicula = new TelescopiadaPeliculaHandler();
    }
    [Authorize]
    public ActionResult CrearTelescopiadaPelicula() {
      return View();
    }
    [Authorize]
    [HttpPost]
    public ActionResult CrearTelescopiadaPelicula(TelescopiadaPeliculaModel telescopiadaPelicula) {
      ViewBag.ExitoAlCrear = false;
      try {
        if (ModelState.IsValid) {
          String baseEnlace = Request.Url.GetLeftPart(UriPartial.Authority);
          String enlace = baseEnlace + Url.Action("Evento", "TelescopiadaPelicula", new { nombre = telescopiadaPelicula.Titulo, fecha = telescopiadaPelicula.Fecha }, null);
          ViewBag.ExitoAlCrear = AccesoADatosTelescopiadaPelicula.CrearTelescopiadaPelicula(telescopiadaPelicula, enlace);
          if (ViewBag.ExitoAlCrear) {
            ViewBag.Message = "El evento fue creado con éxito";
            ModelState.Clear();
          }
        }
        return View();
      }
      catch {
        ViewBag.Message = "Algo salió mal y no fue posible crear el evento";
        return View();
      }
    }

    public ActionResult Evento(String nombre, DateTime fecha) {
      ViewBag.TelescopiadaPelicula = AccesoADatosTelescopiadaPelicula.ObtenerTelescopiadaPelicula(nombre, fecha);
      return View();
    }

  }
}