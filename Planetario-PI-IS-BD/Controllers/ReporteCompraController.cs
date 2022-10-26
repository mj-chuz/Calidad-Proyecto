using PagedList;
using Planetario.Handlers;
using Planetario.Models;
using System;
using System.Web.Mvc;


namespace Planetario.Controllers
{
  public class ReporteCompraController : Controller
  {

    private ReporteCompraHandler _accesoAReporteCompra;
    private ValidadorFechasReporte _validadorFechas;
    private ProductoHandler _accesoAProductos;

    public ReporteCompraController()
    {
      _accesoAReporteCompra = new ReporteCompraHandler();
      _validadorFechas = new ValidadorFechasReporte();
      _accesoAProductos = new ProductoHandler();
    }


    [Authorize]
    public ActionResult ReporteCompraSimple(int? paginaActual, DateTime? fechaInicio = null, DateTime? fechaFinal = null, String ordenamiento = null)
    {
      int pagina = (paginaActual ?? 1);
      IPagedList<DatoReporteModel> informacionReporteSimple = _accesoAReporteCompra.ObtenerInformacionReporteCompraSimple(fechaInicio.ToString(), fechaFinal.ToString(), ordenamiento).ToPagedList(pagina, 10);
      if (!_validadorFechas.ValidarFechasFiltros(fechaInicio, fechaFinal))
      {
        ViewBag.MensajeError = "Por favor seleccione un formato de fechas válido.";
      }
      ViewBag.FechaInicio = fechaInicio;
      ViewBag.FechaFinal = fechaFinal;
      ViewBag.Ordenamiento = ordenamiento;
      ViewBag.InformacionReporteSimple = informacionReporteSimple;
      return View(informacionReporteSimple);
    }

    [Authorize]
    public ActionResult ReporteCompraAvanzado(int? paginaActual, String tipoReporte = null, String idProducto = null,
                                              String nivelEducativo = null, String genero = null, String diaSemana = null,
                                              DateTime? fechaInicio = null, DateTime? fechaFinal = null, String ordenamiento = null)
    {
      int pagina = (paginaActual ?? 1);
      if (tipoReporte == null || tipoReporte == "")
      {
        tipoReporte = "vacio";
      }
      if (!_validadorFechas.ValidarFechasFiltros(fechaInicio, fechaFinal))
      {
        ViewBag.MensajeError = "Por favor seleccione un formato de fechas válido.";
        ViewBag.TipoReporte = "vacio";
      }
      else
      {
        ViewBag.TipoReporte = tipoReporte;
      }
      IPagedList<DatoReporteModel> informacionReporteAvanzado = _accesoAReporteCompra.ObtenerInformacionReporteCompraAvanzado(tipoReporte, idProducto, nivelEducativo, genero, diaSemana, ordenamiento, fechaInicio, fechaFinal).ToPagedList(pagina, 10);
      ViewBag.InformacionReporteAvanzado = informacionReporteAvanzado;
      if (idProducto != null && idProducto != "")
        ViewBag.NombreProducto = _accesoAProductos.ObtenerProductoModel(idProducto).Nombre;
      else
        ViewBag.NombreProducto = "";
      ViewBag.IdProducto = idProducto;
      ViewBag.NivelEducativo = nivelEducativo;
      ViewBag.Genero = genero;
      ViewBag.DiaSemana = diaSemana;
      ViewBag.FechaInicio = fechaInicio;
      ViewBag.FechaFinal = fechaFinal;
      ViewBag.Ordenamiento = ordenamiento;
      ViewData["Productos"] = _accesoAProductos.ObtenerNombreProductos();
      return View(informacionReporteAvanzado);
    }

  }
}