using Planetario.Handlers;
using Planetario.Models;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using PagedList;

namespace Planetario.Controllers {
  public class PreguntaSatisfaccionController : Controller {

    private PreguntaSatisfaccionHandler _accesoAPreguntaSatisfaccion;
    private ReporteCompraHandler _accesoAReporteCompra;
    private ValidadorFechasReporte _validadorFechas;

    public PreguntaSatisfaccionController() {
      _accesoAPreguntaSatisfaccion = new PreguntaSatisfaccionHandler();
      _accesoAReporteCompra = new ReporteCompraHandler();
      _validadorFechas = new ValidadorFechasReporte();

    }

    [Authorize]
    public ActionResult ReporteDeSatisfaccion(int? paginaActual, String nivelEducativo = null, String genero = null, String categoria = null,
                                         DateTime? fechaInicio = null, DateTime? fechaFinal = null, String ordenamiento = null) {
      int pagina = (paginaActual ?? 1);
      if (!_validadorFechas.ValidarFechasFiltros(fechaInicio, fechaFinal)) {
        ViewBag.MensajeError = "Por favor seleccione un formato de fechas válido.";
      }
      String fechaInicial = Convert.ToString(fechaInicio);
      String fechaFin = Convert.ToString(fechaFinal);
      IPagedList<ReporteSatisfaccionModel> informacionReporteAvanzado = _accesoAPreguntaSatisfaccion.ObtenerReporteSatisfaccionPreguntaCategoria(categoria, genero, nivelEducativo, ordenamiento,fechaInicial, fechaFin).ToPagedList(pagina, 10);
      ViewBag.InformacionReporteAvanzado = informacionReporteAvanzado;
      ViewBag.NivelEducativo = nivelEducativo;
      ViewBag.Genero = genero;
      ViewBag.Categoria = categoria;
      ViewBag.FechaInicio = fechaInicio;
      ViewBag.FechaFinal = fechaFinal;
      ViewBag.Ordenamiento = ordenamiento;
      ViewBag.Categorias = _accesoAPreguntaSatisfaccion.ObtenerCategorias();
      return View(informacionReporteAvanzado);
    }

    public ActionResult CuestionarioSatisfaccionCompra(String identificacionVisitante, String tipoActividad) {
      if (tipoActividad != null && identificacionVisitante != null) {
        List<PreguntaSatisfaccionModel> preguntas = _accesoAPreguntaSatisfaccion.ObtenerPreguntaCategoria(tipoActividad);
        ViewBag.Preguntas = preguntas;
        ViewBag.Identificacion = identificacionVisitante;
        ViewBag.TipoActividad = tipoActividad;
        if (preguntas.Count == 0) {
          return RedirectToAction("Index", "Home");
        } else {
          return View();
        }
      }
      return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public ActionResult CuestionarioSatisfaccionCompra(FormCollection formularios) {
      String tipoActividad = formularios["tipoActividad"];
      String identificacion = formularios["identificacionVisitante"];
      List<PreguntaSatisfaccionModel> preguntas = _accesoAPreguntaSatisfaccion.ObtenerPreguntaCategoria(tipoActividad);
      for (int numeroPregunta = 0; numeroPregunta < preguntas.Count; numeroPregunta++) {
        preguntas[numeroPregunta].Respuesta = Convert.ToInt32(formularios["pregunta-" + Convert.ToString(numeroPregunta)]);
      }
      _accesoAPreguntaSatisfaccion.AgregarRespuestas(preguntas, identificacion);
      return RedirectToAction("Index", "Home");
    }

  }
}