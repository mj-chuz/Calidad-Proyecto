using Planetario.Handlers;
using System.Web.Mvc;
using Planetario.Models;
using System.Security.Cryptography;
using System;

namespace Planetario.Controllers {
  public class FuncionariosController : Controller {
    FuncionariosHandler AccesoDatosFuncionario;
    SHA256 EncriptadorContrasena;

    public FuncionariosController() {
      AccesoDatosFuncionario = new FuncionariosHandler();
      EncriptadorContrasena = SHA256.Create();
    }
    public ActionResult ListaDeFuncionarios() {
      ViewBag.Funcionarios = AccesoDatosFuncionario.ObtenerListaFuncionarios();
      return View();
    }

    public ActionResult CrearFuncionario() {
      return View();
    }
    [HttpPost]
    public ActionResult CrearFuncionario(FuncionarioModel funcionario) {
      funcionario.Pais = Request.Form["paisSeleccionado"];
      funcionario.Idiomas = AccesoDatosFuncionario.ConvertirHileraTopicosALista(Request.Form["idiomasSeleccionados"]);
      funcionario.Contrasena = AccesoDatosFuncionario.GenerarPrimeraContrasena(funcionario);
      funcionario.Contrasena = AccesoDatosFuncionario.ObtenerHash(EncriptadorContrasena, funcionario.Contrasena);
      ViewBag.ExitoAlCrear = false;
      if (funcionario.Idiomas.Count == 0) {
        funcionario.Idiomas = null;
        ModelState.AddModelError("ErrorIdiomas", "Es necesario que seleccione al menos un idioma");
      }
      try {
        if (ModelState.IsValid) {
          ViewBag.ExitoAlCrear = AccesoDatosFuncionario.CrearFuncionario(funcionario);
          if (ViewBag.ExitoAlCrear) {
            ViewBag.Message = "El funcionario " + funcionario.Nombre + " fue creado con éxito";
            ModelState.Clear();
          }
        }
        return View();
      }
      catch {
        ViewBag.Message = "Algo salió mal y no fue posible crear el funcionario";
        return View();
      }
    }

    [Authorize]
    [HttpGet]
    public ActionResult EditarFuncionario(string identificador) {
      ActionResult vista;
      try {
        FuncionarioModel modificarFuncionario = AccesoDatosFuncionario.ObtenerListaFuncionarios().Find(funcionarioModel => funcionarioModel.NumeroIdentificacion == identificador);
        if (modificarFuncionario == null) {
          vista = RedirectToAction("ListaDeFuncionarios");
        } else {
          vista = View(modificarFuncionario);
        }
      }
      catch {
        vista = RedirectToAction("ListaDeFuncionarios");
      }
      return vista;
    }
    [Authorize]
    [HttpPost]
    public ActionResult EditarFuncionario(FuncionarioModel funcionario) {
      try {
        AccesoDatosFuncionario.ModificarFuncionario(funcionario);
        return RedirectToAction("ListaDeFuncionarios", "Funcionarios");
      }
      catch {
        return View();
      }
    }

    public JsonResult ObtenerPaises() {
      String paises = AccesoDatosFuncionario.ObtenerPaises();
      return Json(paises);
    }

    public JsonResult ObtenerIdiomas() {
      String idiomas = AccesoDatosFuncionario.ObtenerIdiomas();
      return Json(idiomas);
    }

  }

}
