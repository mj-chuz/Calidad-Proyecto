using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Planetario.Models;
using Planetario.Handlers;
using System.IO;
using System.Linq;


namespace Planetario.Controllers {
  public class CuestionariosController : Controller {
    public ActionResult Cuestionario() {
      ViewBag.ExitoAlCargar = false;
      return View();
    }
    [HttpGet]
    public ActionResult Cuestionario(String dificultad) {
      try { 
      List<String> dificultades = new List<String>(){ "Principiante", "Intermedio", "Avanzado" };
      if (!dificultades.Contains(dificultad)) { 
        ViewBag.ExitoAlCargar = false;
      } else {
        CuestionariosHandler creadorCuestionario = new CuestionariosHandler();
        String direccionCarpeta = AppContext.BaseDirectory + "/Cuestionarios/" + dificultad;
        DirectoryInfo metadatosCuestionarios = new DirectoryInfo(direccionCarpeta);
        FileInfo[] metadatosCuestionariosIterable = metadatosCuestionarios.GetFiles().ToArray();
        if (metadatosCuestionariosIterable.Length > 0) { 
          Random generadorDeNumeros = new Random();
          int indiceGenerado = generadorDeNumeros.Next(metadatosCuestionariosIterable.Length);
          String direccionCuestionario = metadatosCuestionariosIterable[indiceGenerado].DirectoryName.ToString() 
                                          + "/" + metadatosCuestionariosIterable[indiceGenerado].Name;
          ViewBag.Cuestionario = creadorCuestionario.CargarArchivo(System.IO.File.ReadAllText(direccionCuestionario));
          ViewBag.ExitoAlCargar = true;
        } else {
          ViewBag.ExitoAlCargar = false;
        }
      }
      }catch(Exception) {
        ViewBag.ExitoAlCargar = false;
      }
      return View();
    }

    public ActionResult PaginaPrincipal(String dificultad) {

      return View();    
    }

    [Authorize]
    public ActionResult CreacionCuestionario() {
      CuestionariosHandler creadorCuestionario = new CuestionariosHandler();
      ViewBag.NombresCuestionariosPorDificultad = creadorCuestionario.ObtenerNombreCuestionariosPorDificultad();
      return View();
    }

    [Authorize]
    [HttpPost]
    public ActionResult CreacionCuestionario(String cuestionario) {
      CuestionariosHandler creadorCuestionario = new CuestionariosHandler();
      ViewBag.NombresCuestionariosPorDificultad = creadorCuestionario.ObtenerNombreCuestionariosPorDificultad();
      String cuestionarioObtenido = Request.Form["jsonCuestionario"];
      CuestionariosHandler cuestionariosHandler = new CuestionariosHandler();
      CuestionarioModel cuestionarioCargado = cuestionariosHandler.CargarArchivo(cuestionarioObtenido);
      cuestionariosHandler.GuardarArchivo(cuestionarioCargado);
      return View();
    }

    [Authorize]
    public ActionResult AdministrarCuestionarios() {
      CuestionariosHandler creadorCuestionario = new CuestionariosHandler();
      ViewBag.Cuestionarios = creadorCuestionario.ObtenerNombreCuestionariosPorDificultad();
      return View();
    }

    public ActionResult EliminarCuestionario(String nombreCuestionario, String dificultad) {
      string path = AppContext.BaseDirectory + "/Cuestionarios/" + dificultad + "/" + nombreCuestionario;
      if (System.IO.File.Exists(path)) {
        System.IO.File.Delete(path);
      }
      return Redirect("~/Cuestionarios/AdministrarCuestionarios");
    }
  }
 
}