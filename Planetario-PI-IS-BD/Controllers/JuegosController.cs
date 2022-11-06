using System;
using System.Web.Mvc;
using Planetario.Handlers;

namespace Planetario.Controllers
{
  public class JuegosController : Controller {

    private JuegosHandler AccesoMetodosJuegos;

    public JuegosController() {
      AccesoMetodosJuegos = new JuegosHandler();
    }

    public ActionResult PaginaPrincipal() {
      ViewBag.ListaJuegos = AccesoMetodosJuegos.CargarListaJuegos();
      return View();
    }


    public ActionResult Juego(String nombre, String linkAlJuego) {
      ViewBag.NombreJuego = nombre;
      ViewBag.LinkJuego = linkAlJuego;
      return View();
    }
  }
    
}