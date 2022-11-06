using Planetario.Handlers;
using Planetario.Models;
using System;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace Planetario.Controllers
{

  public class VisitanteController : Controller
  {
    public VisitanteHandler AccesoMetodosVisitante;
    public ActividadHandler AccesoDatosActividad;
    private const double IVA = 1.13;
    public VisitanteController()
    {
      AccesoMetodosVisitante = new VisitanteHandler();
      AccesoDatosActividad = new ActividadHandler();
    }

    public ActionResult InscribirVisitanteNuevoTienda()
    {
      return RedirectToAction("InscribirVisitante", new
      {
        cantidadDeCupos = 0,
        tituloActividad = "",
        fecha = new DateTime(),
        tipoInscripcion = "tienda"
      });
    }

    public ActionResult InscribirVisitante(int cantidadDeCupos = 0, String tituloActividad = "",
                                           DateTime fecha = new DateTime(), String tipoInscripcion = "actividad")
    {
      if (tipoInscripcion == "actividad")
      {
        ActividadModel actividad = AccesoDatosActividad.ObtenerActividad(tituloActividad, fecha);
        ViewBag.Actividad = actividad;
      }
      ViewBag.Cupos = cantidadDeCupos;
      ViewBag.TipoInscripcion = tipoInscripcion;
      return View();
    }


    [HttpPost]
    public ActionResult InscribirVisitante(VisitanteModel visitante, int cuposComprados = 0)
    {
      visitante.Pais = Request.Form["paisSeleccionado"];
      String tipoInscripcion = Request.Form["tipoInscripcion"];
      ActividadModel actividad = new ActividadModel();
      if (tipoInscripcion == "actividad")
      {
        actividad = AccesoDatosActividad.ObtenerActividad(visitante.TituloActividadInscrita, visitante.FechaActividadInscrita);
        ViewBag.Actividad = actividad;
      }
      ViewBag.Exito = false;
      if (visitante.Pais == "")
      {
        ModelState.AddModelError("ErrorPaises", "Es necesario que seleccione un pais");
      }
      try
      {
        if (ModelState.IsValid)
        {
          AccesoMetodosVisitante.AgregarVisitante(visitante);
          ViewBag.Message = "La inscripción fue realizada con éxito";
          ModelState.Clear();
          ViewBag.Exito = true;
          if (tipoInscripcion == "actividad")
            return RedirectToAction("PagoActividad", "Pago", new
            {
              identificacionVisitante = visitante.NumeroIdentificacion,
              nombreActividad = actividad.Titulo,
              fechaActividad = actividad.Fecha.ToString(),
              cantidadCupos = cuposComprados
            });
          else if (tipoInscripcion == "tienda")
            return RedirectToAction("PagoProductos", "Pago", new { identificacionVisitante = visitante.NumeroIdentificacion });
        }
        return View();
      }
      catch
      {
        ViewBag.Message = "Algo salió mal y no fue posible realizar la inscripción";
        ViewBag.Cupos = cuposComprados;
        ViewBag.TipoInscripcion = tipoInscripcion;
        return View();
      }
    }
    public ActionResult VerificarVisitanteInscritoTienda()
    {
      return RedirectToAction("VerificarInscripcionEnPlanetario", new
      {
        cantidadDeCupos = 0,
        tituloActividad = "",
        fecha = new DateTime(),
        tipoInscripcion = "tienda"
      });
    }

    public ActionResult VerificarInscripcionEnPlanetario(int cantidadDeCupos = 0, String tituloActividad = "", DateTime fecha = new DateTime(), String tipoInscripcion = "actividad")
    {
      if (tipoInscripcion == "actividad")
      {
        ActividadModel actividad = AccesoDatosActividad.ObtenerActividad(tituloActividad, fecha);
        ViewBag.Actividad = actividad;
      }
      ViewBag.Cupos = cantidadDeCupos;
      ViewBag.TipoInscripcion = tipoInscripcion;
      return View();
    }

    [HttpPost]
    public ActionResult VerificarInscripcionEnPlanetario(VisitanteModel visitante, int cuposComprados)
    {
      String tipoInscripcion = Request.Form["tipoInscripcion"];
      ActividadModel actividad = new ActividadModel();
      if (tipoInscripcion == "actividad")
      {
        actividad = AccesoDatosActividad.ObtenerActividad(visitante.TituloActividadInscrita, visitante.FechaActividadInscrita);
        ViewBag.Actividad = actividad;
      }
      ViewBag.Exito = false;
      try
      {
        if (AccesoMetodosVisitante.VerificarInscripcion(visitante.NumeroIdentificacion))
        {
          return RedireccionarAPago(visitante, tipoInscripcion, actividad, cuposComprados);
        }
        else
        {
          ViewBag.Cupos = cuposComprados;
          ViewBag.TipoInscripcion = tipoInscripcion;
          ViewBag.Message = "Algo salió mal y no fue posible realizar la inscripción";
        }
        return View();
      }
      catch
      {
        ViewBag.Cupos = cuposComprados;
        ViewBag.TipoInscripcion = tipoInscripcion;
        ViewBag.Message = "Algo salió mal y no fue posible realizar la inscripción";
        return View();
      }
    }
    private ActionResult RedireccionarAPago(VisitanteModel visitante, String tipoInscripcion, ActividadModel actividad, int cuposComprados)
    {
      VisitanteModel visitanteRecuperado = AccesoMetodosVisitante.RecuperarVisitante(visitante.NumeroIdentificacion);
      if (tipoInscripcion == "actividad")
      {
        visitanteRecuperado.FechaActividadInscrita = visitante.FechaActividadInscrita;
        visitanteRecuperado.TituloActividadInscrita = visitante.TituloActividadInscrita;
      }
      ViewBag.Message = "La inscripción fue realizada con éxito";
      ViewBag.Exito = true;
      ModelState.Clear();
      if (tipoInscripcion == "actividad")
      {
        return RedirectToAction("PagoActividad", "Pago", new { identificacionVisitante = visitante.NumeroIdentificacion, nombreActividad = actividad.Titulo, fechaActividad = actividad.Fecha.ToString(), cantidadCupos = cuposComprados });
      }
      else if (tipoInscripcion == "tienda")
      {
        return RedirectToAction("PagoProductos", "Pago", new { identificacionVisitante = visitante.NumeroIdentificacion });
      }
      else
      {
        throw new ArgumentException("Tipo de inscripción inválido");
      }
    }
  }
}