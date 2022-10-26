using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planetario.Handlers;

namespace Planetario.Controllers {
    public class PlanetarioController : Controller {

      public ActionResult InformacionPlanetario(){
        return View();
      }
      public ActionResult InformacionHorarios() {
        return InformacionPlanetario();
      }
  }
}