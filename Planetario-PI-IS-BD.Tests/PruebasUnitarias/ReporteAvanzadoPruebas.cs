using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetario.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Planetario_PI_IS_BD.Tests.PruebasUnitarias {

  [TestClass]
  public class ReporteAvanzadoPruebas {
    public ReporteCompraController AccesoReporteAvanzadoController;

    public ReporteAvanzadoPruebas() {
      AccesoReporteAvanzadoController = new ReporteCompraController();
    }

    [TestMethod]
    public void VerReporteCompraAvanzadaSinFiltrar() {
      ViewResult paginaReporte = (ViewResult)AccesoReporteAvanzadoController.ReporteCompraAvanzado(1);
      Assert.IsNotNull(paginaReporte);
    }

    [TestMethod]

    public void VerReporteCompraAvanzadoConFechaCompleta() {
      ViewResult paginaReporte = (ViewResult)AccesoReporteAvanzadoController.ReporteCompraAvanzado(1, null, null, null, null, null, DateTime.Now.AddDays(-2), DateTime.Now);
      Assert.AreNotEqual("Por favor seleccione un formato de fechas válido.", paginaReporte.ViewBag.MensajeError);
    }

    [TestMethod]
    public void VerReporteCompraAvanzadoConFechaInvalida() {
      ViewResult paginaReporte = (ViewResult)AccesoReporteAvanzadoController.ReporteCompraAvanzado(1, null, null, null, null, null, DateTime.Now, Convert.ToDateTime("12/2/2021"));
      Assert.AreEqual("Por favor seleccione un formato de fechas válido.", paginaReporte.ViewBag.MensajeError);
    }

    [TestMethod]
    public void VerReporteCompraAvanzadoSinUnaFecha() {
      ViewResult paginaReporte = (ViewResult)AccesoReporteAvanzadoController.ReporteCompraAvanzado(1,null,null,null,null,null, Convert.ToDateTime("11/2/2021"));
      Assert.IsNotNull(paginaReporte);
    }
    
    [TestMethod]
    public void VerReporteCompraAvanzadoConjuntoCorrecto() {
      ViewResult paginaReporte = (ViewResult)AccesoReporteAvanzadoController.ReporteCompraAvanzado(1, "comprasConjunto", "1A", null, null, null, Convert.ToDateTime("11/2/2021"));
      Assert.IsNotNull(paginaReporte);
    }

    [TestMethod]
    public void VerReporteCompraAvanzadoTipoClienteCorrecto() {
      ViewResult paginaReporte = (ViewResult)AccesoReporteAvanzadoController.ReporteCompraAvanzado(1, "comprasTipoCliente", null, "Primaria terminada", "Hombre", "Martes", Convert.ToDateTime("11/2/2021"));
      Assert.IsNotNull(paginaReporte);
    }
  }
}
