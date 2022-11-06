using System;
using System.Web.Mvc;
using Planetario.Handlers;
using Planetario.Models;



namespace Planetario.Controllers {
  public class PreguntasController: Controller {
    PreguntaHandler AccesoAPreguntas = new PreguntaHandler();
    CategoriaHandler AccesoACategorias = new CategoriaHandler();

    public ActionResult PreguntasFrecuentes() {
      ViewBag.Preguntas = AccesoAPreguntas.ObtenerListaPreguntas();
      ViewBag.CategoriasYTopicos = AccesoAPreguntas.ObtenerCategoriasYTopicos();
      ViewBag.ListaCategorias = AccesoACategorias.ObtenerListaCategoriasView();
      return View();
    }

    [HttpPost]
    public ActionResult PreguntasFrecuentes(BuscadorPreguntasModel buscadorPreguntas) {
      ViewBag.Preguntas = AccesoAPreguntas.ObtenerListaPreguntas(buscadorPreguntas.topicosFiltrados);
      ViewBag.CategoriasYTopicos = AccesoAPreguntas.ObtenerCategoriasYTopicos(buscadorPreguntas.topicosFiltrados);
      ViewBag.ListaCategorias = AccesoACategorias.ObtenerListaCategoriasView();
      return View();
    }


    public ActionResult VerificarFuncionario() {

      return RedirectToAction("PreguntasFrecuentes");
    }

    [HttpPost]
    public ActionResult VerificarFuncionario(BuscadorPreguntasModel buscadorPreguntas) {

      if (Convert.ToString(buscadorPreguntas.FuncionarioCrearPregunta.CodigoFuncionario).Equals("c0lab_pR")){
        return RedirectToAction("CrearPregunta");
      }
      else
      {
        ViewBag.MensajeErrorVerificacion = "Código de verificación de funcionario incorrecto";
        return RedirectToAction("PreguntasFrecuentes");
      }
    }

    public ActionResult CrearPregunta() {
      ViewBag.ListaCategorias = AccesoACategorias.ObtenerListaCategoriasView();
      return View();
    }

    [HttpPost]
    public ActionResult CrearPregunta(PreguntaModel pregunta) {
      ViewBag.ListaCategorias = AccesoACategorias.ObtenerListaCategoriasView();
      ViewBag.ExitoAlCrear = false;
      try {
        if (ModelState.IsValid) {
          PreguntaHandler accesoDatos = new PreguntaHandler();
          ViewBag.ExitoAlCrear = accesoDatos.AgregarPreguntaABase(pregunta);
          if (ViewBag.ExitoAlCrear) {
            ViewBag.Message = "La pregunta fue creada con éxito";
            ModelState.Clear();
          }
        }
        return View();
      }
      catch {
        ViewBag.Message = "Algo salió mal y no fue posible crear la pregunta";
        return View(); 
      }
    }
  }
}