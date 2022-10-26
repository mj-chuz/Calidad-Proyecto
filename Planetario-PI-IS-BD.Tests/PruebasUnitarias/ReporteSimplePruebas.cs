using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Planetario.Controllers;
using System.Web.Mvc;
namespace Planetario_PI_IS_BD.Tests.PruebasUnitarias
{
  [TestClass]
  public class ReporteSimplePruebas
  {
    public ReporteCompraController AccesoReporteSimpleController;

    public ReporteSimplePruebas()
    {
      AccesoReporteSimpleController = new ReporteCompraController();
    }

    [TestMethod]
    public void VerReporteCompraSimpleSinFiltrar()
    {
      ViewResult paginaReporte = (ViewResult)AccesoReporteSimpleController.ReporteCompraSimple(1);
      Assert.IsNotNull(paginaReporte);
    }

    [TestMethod]

    public void VerReporteCompraSimpleConFechaCompleta()
    {
      ViewResult paginaReporte = (ViewResult)AccesoReporteSimpleController.ReporteCompraSimple(1, Convert.ToDateTime("12/2/2021"), DateTime.Now);
      Assert.AreNotEqual("Por favor seleccione un formato de fechas válido.", paginaReporte.ViewBag.MensajeError);
    }

    [TestMethod]
    public void VerReporteCompraSimpleConFechaInvalida()
    {
      ViewResult paginaReporte = (ViewResult)AccesoReporteSimpleController.ReporteCompraSimple(1, DateTime.Now, Convert.ToDateTime("12/2/2021"));
      Assert.AreEqual("Por favor seleccione un formato de fechas válido.", paginaReporte.ViewBag.MensajeError);
    }

    [TestMethod]
    public void VerReporteCompraSimpleSinUnaFecha()
    {
      ViewResult paginaReporte = (ViewResult)AccesoReporteSimpleController.ReporteCompraSimple(1, Convert.ToDateTime("11/2/2021"));
      Assert.IsNotNull(paginaReporte);
    }

  }
}
