using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Planetario_PI_IS_BD.Tests.PrepararPruebas;

namespace Planetario_PI_IS_BD.Tests.PruebasAutomatizadas{
  [TestClass]
  public class PreguntasSatisfaccionPruebas{

    public PaginaPreguntasSatisfaccion AccederPaginaPreguntasSatisfaccion;
    public PaginaActividad AccederPaginaActividad;
    public PaginaVisitante AccederPaginaVisitante;

    IWebDriver ChromeDriver;

    public PreguntasSatisfaccionPruebas(){
      ChromeDriver = new ChromeDriver();
      AccederPaginaPreguntasSatisfaccion = new PaginaPreguntasSatisfaccion(ChromeDriver);
      AccederPaginaActividad = new PaginaActividad(ChromeDriver);
      AccederPaginaVisitante = new PaginaVisitante(ChromeDriver);
    }

    [TestMethod]
    public void LlenarCuestionarioProductos(){
      AccederPaginaPreguntasSatisfaccion.Iniciar();
      IWebElement tituloPaginaPrincipal = AccederPaginaPreguntasSatisfaccion.IngresarRespuestasDeSatisfaccion();
      Assert.AreEqual("PLANETARIO", tituloPaginaPrincipal.Text);
      ChromeDriver.Quit();
    }


    [TestMethod]
    public void LlenarCuestionarioTiquetes(){
      String numeroTarjeta = "1234123412341234";
      String codigoSeguridad = "413";
      String nombreCompleto = "Jonathan Alfonso Navarro";
      String mes = "12";
      String anyo = "2025";
      AccederPaginaActividad.Iniciar();
      AccederPaginaActividad.ReservarTiquetesVisitante("1");
      ChromeDriver = AccederPaginaActividad.ReservarVisitanteRecurrente();
      ChromeDriver = AccederPaginaVisitante.ingresarIdentificacion("1234");
      AccederPaginaVisitante.ingresarNumeroTarjeta(numeroTarjeta, codigoSeguridad);
      AccederPaginaVisitante.ingresarFechaVencimiento(mes, anyo);
      AccederPaginaVisitante.ingresarNombreTarjeta(nombreCompleto);
      AccederPaginaPreguntasSatisfaccion.IngresarACuestionarioDeSatisfaccion();
      IWebElement tituloPaginaPrincipal = AccederPaginaPreguntasSatisfaccion.IngresarRespuestasDeSatisfaccion();
      Assert.AreEqual("PLANETARIO", tituloPaginaPrincipal.Text);
      ChromeDriver.Quit();

    }
  }
}
