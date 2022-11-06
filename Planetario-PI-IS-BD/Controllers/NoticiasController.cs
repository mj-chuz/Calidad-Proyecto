using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetario.Models;
using Planetario.Handlers;
using System.Diagnostics;

namespace Planetario.Controllers {
  public class NoticiasController : Controller {
    public ActionResult Noticia(string nombre) {
      NoticiasHandler noticias = new NoticiasHandler();
      ViewBag.Noticia = noticias.ObtenerNoticia(nombre);
      return View();
    }

    public ActionResult PaginaNoticias(int paginaMostrar = 1) {
      NoticiasHandler noticias = new NoticiasHandler();
      int cantidadTotalDePaginas = noticias.CantidadTotalDePaginas;
      paginaMostrar = noticias.ValidarNumeroDePagina(paginaMostrar);
      Tuple<int, int> limitesPaginacion = noticias.CalcularLimitesValidacion(paginaMostrar);
      ViewBag.Noticias = noticias.GetPaginaNoticias(paginaMostrar);
      ViewBag.NumeroInicioPaginacion = limitesPaginacion.Item1;
      ViewBag.NumeroFinalPaginacion = limitesPaginacion.Item2;
      ViewBag.PaginaActual = paginaMostrar;
      ViewBag.CantidadTotalDePaginas = cantidadTotalDePaginas;
      return View();
    }

    [Authorize]
    public ActionResult CrearNoticia() {
      return View();
    }

    [HttpPost]
    public ActionResult PaginaNoticias(FuncionarioModel funcionario) {
      if (Convert.ToString(funcionario.CodigoFuncionario) == "c00rd_Not") {
        return RedirectToAction("CrearNoticia");
      } else {
        ViewBag.MensajeErrorVerificacion = "Código de verificación de funcionario incorrecto";
        return PaginaNoticias();
      }
    }
    [Authorize]
    [HttpPost]
    public ActionResult CrearNoticia(NoticiaModel noticia) {
      ViewBag.ExitoAlCrear = false;
      try {
        if (ModelState.IsValid) {
          NoticiasHandler accesoDatos = new NoticiasHandler();
          noticia.Fecha = DateTime.Now;
          ViewBag.ExitoAlCrear = accesoDatos.GuardarNoticia(noticia);
          if (ViewBag.ExitoAlCrear) {
            ViewBag.Message = "La noticia fue creada con éxito";
            ModelState.Clear();
          }
        }
        return View();
      }
      catch {
        ViewBag.Message = "Algo salió mal y no fue posible crear la noticia";
        return View();
      }
    }
  }
}