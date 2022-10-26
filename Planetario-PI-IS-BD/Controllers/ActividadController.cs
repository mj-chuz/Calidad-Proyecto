using Planetario.Handlers;
using Planetario.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Text.RegularExpressions;


namespace Planetario.Controllers {

  public class ActividadController : Controller {
    public CategoriaHandler AccesoADatosCategoria;
    public ActividadHandler AccesoADatosActividad;
    public CorreoHandler AccesoAManejadorCorreos;

    public ActividadController() {
      AccesoADatosCategoria = new CategoriaHandler();
      AccesoADatosActividad = new ActividadHandler();
      AccesoAManejadorCorreos = new CorreoHandler();
    }

    [Authorize]
    public ActionResult CrearActividad() {
      ViewData["Categorias"] = AccesoADatosCategoria.ObtenerCategorias();
      return View();
    }

    [Authorize]
    [HttpPost]
    public ActionResult CrearActividad(ActividadModel actividad) {
      ViewData["Categorias"] = AccesoADatosCategoria.ObtenerCategorias();
      ViewBag.ExitoAlCrear = false;
      actividad.Categoria = Request.Form["categoriaSeleccionada"];
      String topicosSeleccionados = Request.Form["topicosSeleccionados"];
      String publicoMetaSeleccionado = Request.Form["publicosSeleccionados"];
      actividad.NumeroIdentificacionFuncionario = Convert.ToString(User.Identity.Name);
      actividad.Topicos = AccesoADatosActividad.ConvertirHileraTopicosALista(topicosSeleccionados);
      actividad.PublicoMeta = AccesoADatosActividad.ConvertirHileraTopicosALista(publicoMetaSeleccionado);
      if (actividad.Topicos.Count == 0) {
        actividad.Topicos = null;
        ModelState.AddModelError("ErrorTopicos", "Es necesario que seleccione al menos un tópico");
      }
      if (actividad.PublicoMeta.Count == 0) {
        actividad.PublicoMeta = null;
        ModelState.AddModelError("ErrorPublicoMeta", "Es necesario que seleccione al menos un público meta");
      }
      if (actividad.PrecioSugerido == 0) {
        ModelState.AddModelError("ErrorPrecioSugerido", "Debe ingresar un valor entero válido");
      }
      try {
        if (ModelState.IsValid) {
          actividad.Estado = "Pendiente";
          ViewBag.ExitoAlCrear = AccesoADatosActividad.CrearActividad(actividad);
          if (ViewBag.ExitoAlCrear) {
            ViewBag.Message = "La peticion de actividad fue enviada a evaluación con éxito";
            ModelState.Clear();
          }
        }
        return View();
      }
      catch (Exception) {
        ViewBag.Message = "Algo salió mal y no fue posible enviar la petición de actividad";
        return View();
      }
    }

    public JsonResult ObtenerListaTopicos(String categoria) {
      List<String> topicos = AccesoADatosCategoria.ObtenerTopicosCategoria(categoria);
      List<SelectListItem> topicosParseados = new List<SelectListItem>();
      foreach (string topico in topicos) {
        topicosParseados.Add(new SelectListItem { Text = topico, Value = topico });
      }
      return Json(new SelectList(topicosParseados, "Value", "Text"));
    }

    public ActionResult ListaCharlas() {
      ViewBag.ListaCharlas = this.AccesoADatosActividad.ObtenerCharlasAprobadas();
      return View();
    }

    [HttpPost]
    public ActionResult ListaCharlas(string palabraClave) {
      ViewBag.ListaCharlas = this.AccesoADatosActividad.ObtenerCharlasAprobadas(palabraClave);
      return View();
    }

    public ActionResult ListaTalleres() {
      ViewBag.ListaTalleres = this.AccesoADatosActividad.ObtenerTalleresAprobados();
      return View();
    }

    [HttpPost]
    public ActionResult ListaTalleres(string palabraClave) {
      ViewBag.ListaTalleres = this.AccesoADatosActividad.ObtenerTalleresAprobados(palabraClave);
      return View();
    }

    [Authorize]
    public ActionResult AdministrarActividadesPendientes(string mensaje = "") {
      ViewBag.Message = mensaje;
      ViewBag.Actividades = AccesoADatosActividad.ObtenerActividadesPendientesDeAprobacion();
      return View();
    }

    [Authorize]
    [HttpPost]
    public ActionResult AprobarActividad(string titulo, DateTime fecha) {
      AccesoADatosActividad.CambiarEstadoActividad(titulo, fecha, ActividadModel.APROBADA);
      ActividadModel actividad = AccesoADatosActividad.ObtenerActividad(titulo, fecha);
      CalendariosHandler calendario = new CalendariosHandler();
      String baseEnlace = Request.Url.GetLeftPart(UriPartial.Authority);
      String enlace = baseEnlace + @Url.Action("Actividad", "Actividad", new { nombre = titulo, fecha = fecha }, null);
      calendario.AgregarActividadACalendario(actividad, enlace);
      string correoDestinatario = actividad.Correo;
      AccesoAManejadorCorreos.EnviarCorreoDeAprobacion(titulo, correoDestinatario);
      ViewBag.Actividades = AccesoADatosActividad.ObtenerActividadesPendientesDeAprobacion();
      return RedirectToAction("AdministrarActividadesPendientes", new { mensaje = "La actividad fue aprobada exitosamente" });
    }

    [Authorize]
    [HttpPost]
    public ActionResult RechazarActividad(string titulo, DateTime fecha) {
      AccesoADatosActividad.CambiarEstadoActividad(titulo, fecha, ActividadModel.RECHAZADA);
      ActividadModel actividad = AccesoADatosActividad.ObtenerActividad(titulo, fecha);
      string correoDestinatario = actividad.Correo;
      AccesoAManejadorCorreos.EnviarCorreoDeRechazo(titulo, correoDestinatario);
      ViewBag.Actividades = AccesoADatosActividad.ObtenerActividadesPendientesDeAprobacion();
      return RedirectToAction("AdministrarActividadesPendientes", new { mensaje = "La actividad fue rechazada exitosamente" });
    }


    public ActionResult Actividad(String nombre, DateTime fecha) {
      ActividadModel actividadObtenida = AccesoADatosActividad.ObtenerActividad(nombre, fecha);
      String enlaceStream = actividadObtenida.EnlaceStream;
      if (enlaceStream != null && enlaceStream != "") {
        Regex youtubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:(.*)v(/|=)|(.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);
        Match enlaceValidoYoutube = youtubeVideoRegex.Match(enlaceStream);
        if (enlaceValidoYoutube.Success) {
          String identificadorVideo = enlaceValidoYoutube.Groups[enlaceValidoYoutube.Groups.Count - 1].Value;
          String enlaceEmpotrado = "https://www.youtube.com/embed/" + identificadorVideo;
          ViewBag.VideoEmpotrado = enlaceEmpotrado;
        }
      }
      ViewBag.ActividadesRecomendadas = AccesoADatosActividad.ObtenerActividadesSimilares(nombre, fecha);
      ViewBag.Actividad = actividadObtenida;
      return View();
    }

    [HttpPost]
    public ActionResult EditarEnlace(String enlaceNuevo, String titulo, String fecha) {
      DateTime fechaConvertida = Convert.ToDateTime(fecha);
      this.AccesoADatosActividad.EditarEnlace(enlaceNuevo, titulo, fechaConvertida);
      return RedirectToAction("ListaTalleres");
    }
    [HttpPost]
    public ActionResult EnviarDatosDeCompra(String cuposComprados, String titulo , String fecha,String redireccionar ) {
      DateTime fechaConvertida = Convert.ToDateTime(fecha);
      int cuposCompradosConvertidos = Convert.ToInt32(cuposComprados);
      if (redireccionar == "visitanteNuevo") {
        return RedirectToAction("InscribirVisitante", "Visitante", new { cantidadDeCupos = cuposCompradosConvertidos,tituloActividad = titulo,fecha=fechaConvertida });
      } else {
        return RedirectToAction("VerificarInscripcionEnPlanetario", "Visitante", new { cantidadDeCupos = cuposCompradosConvertidos, tituloActividad = titulo, fecha = fechaConvertida });
      }

    }

  }
}