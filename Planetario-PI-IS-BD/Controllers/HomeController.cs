using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetario.Handlers;

namespace Planetario.Controllers {
  public class HomeController : Controller {
    public ActionResult Index() {
      NoticiasHandler noticias = new NoticiasHandler();
      ViewBag.Noticias = noticias.GetPaginaNoticias();
      return View();
    }

    public ActionResult Nosotros() {
      return View();
    }

  }
}