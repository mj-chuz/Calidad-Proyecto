using Planetario.Handlers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Planetario.Controllers {
  public class EstadisticasController : Controller {
    CategoriaHandler AccesoACategorias;
    EstadisticasHandler AccesoAEstadisticas;
    public EstadisticasController() {
      AccesoACategorias = new CategoriaHandler();
      AccesoAEstadisticas = new EstadisticasHandler();
    }

    [Authorize]
    public ActionResult Estadisticas() {
      String filtrosObtenidos = Request.Form["filtrosEscogidos"];
      ViewBag.DatosObtenidos = AccesoAEstadisticas.ObtenerPublicoDeActividadesParaPublicoMetaYNivel(filtrosObtenidos);
      return View();
    }

    [Authorize]
    [HttpPost]
    public ActionResult Estadisticas(String publicoSeleccionado) {
      String filtrosObtenidos = Request.Form["filtrosEscogidos"];
      ViewBag.DatosObtenidos = AccesoAEstadisticas.ObtenerPublicoDeActividadesParaPublicoMetaYNivel(filtrosObtenidos);
      return View();
    }

    [Authorize]
    public ActionResult EstadisticasCategoriaTopico() {
      ViewData["Categorias"] = AccesoACategorias.ObtenerCategorias();
      ViewBag.PublicoCategoria = AccesoAEstadisticas.ObtenerTopCategorias(3);
      ViewBag.PublicoTopico = AccesoAEstadisticas.ObtenerTopTopicos(5);
      return View();
    }

    [Authorize]
    [HttpPost]
    public ActionResult EstadisticasCategoriaTopico(String topicosSeleccionados) {
      ViewData["Categorias"] = AccesoACategorias.ObtenerCategorias();
      String filtrosCategoria = Request.Form["categoriasSeleccionado"];
      String filtrosTopicos = Request.Form["topicosSeleccionados"];
      ViewBag.PublicoCategoria = AccesoAEstadisticas.ObtenerPublicoPorCategoria(filtrosCategoria);
      ViewBag.PublicoTopico = AccesoAEstadisticas.ObtenerPublicoPorTopicos(filtrosTopicos);
      return View();
    }

    public JsonResult ObtenerListaTopicos(String categoria) {
      List<String> topicos = AccesoACategorias.ObtenerTopicosCategoria(categoria);
      List<SelectListItem> topicosProcesados = new List<SelectListItem>();
      foreach (string topico in topicos) {
        topicosProcesados.Add(new SelectListItem { Text = topico, Value = topico });
      }
      return Json(new SelectList(topicosProcesados, "Value", "Text"));
    }
    [Authorize]
    public ActionResult EstadisticasIdiomasFuncionarios() {
      List<SelectListItem> listaIdiomasHablados = AccesoAEstadisticas.ObtenerIdiomasHablados();
      ViewData["Idiomas"] = listaIdiomasHablados;
      ViewBag.idiomasSeleccionado = new String[0];
      ViewBag.FuncionariosPorIdioma = AccesoAEstadisticas.ObtenerTodosFuncionariosConIdioma();
      return View();
    }
    [HttpPost]
    [Authorize]
    public ActionResult EstadisticasIdiomasFuncionarios(String idiomasSeleccionados) {
      ViewData["Idiomas"] = AccesoAEstadisticas.ObtenerIdiomasHablados();
      String filtrosIdiomas = Request.Form["idiomasSeleccionado"];
      ViewBag.idiomasSeleccionado = filtrosIdiomas.Split(';');
      ViewBag.FuncionariosPorIdioma = AccesoAEstadisticas.ObtenerFuncionariosPorIdioma(filtrosIdiomas);
      return View();
    }
  }
}