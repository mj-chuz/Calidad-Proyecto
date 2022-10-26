using System;
using System.Web.Mvc;

namespace Planetario.Controllers {
  public class ComparadorController : Controller {

    public ActionResult CuerposCelestes() {
      return View("CuerposCelestes");
    }
  }
}