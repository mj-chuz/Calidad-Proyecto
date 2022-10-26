using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Planetario_PI_IS_BD.Tests.PrepararPruebas;

namespace Planetario_PI_IS_BD.Tests.PruebasAutomatizadas
{
  [TestClass]
  public class VentaEntradasPrueba
  {
    public PaginaActividad AccederPaginaActividad;
    public PaginaVisitante AccederPaginaVisitante;
    IWebDriver ChromeDriver;

    public VentaEntradasPrueba()
    {
      ChromeDriver = new ChromeDriver();
    }

    public String ReservarTiquetes()
    {
      String cantidadCupos = "2";
      AccederPaginaActividad = new PaginaActividad(ChromeDriver);
      AccederPaginaActividad.Iniciar();
      AccederPaginaActividad.ReservarTiquetesVisitante(cantidadCupos);
      return cantidadCupos;
    }


    [TestMethod]
    public void PruebaCompraEntradaVisitanteRecurrente()
    {
      String identificacion = "1234";
      String cantidadCupos = this.ReservarTiquetes();
      ChromeDriver = AccederPaginaActividad.ReservarVisitanteRecurrente();
      AccederPaginaVisitante = new PaginaVisitante(ChromeDriver);
      ChromeDriver = AccederPaginaVisitante.ingresarIdentificacion(identificacion);
      this.RealizarPago(ChromeDriver, cantidadCupos);
    }

    [TestMethod]
    public void PruebaCompraEntradaVisitanteNuevo()
    {
      String identificacion = "24415956";
      String cantidadCupos = this.ReservarTiquetes();
      ChromeDriver = AccederPaginaActividad.ReservarVisitanteNuevo();
      AccederPaginaVisitante = new PaginaVisitante(ChromeDriver);
      ChromeDriver = AccederPaginaVisitante.ingresarVisitanteNuevo(identificacion);
      this.RealizarPago(ChromeDriver, cantidadCupos);
    }

    public void RealizarPago(IWebDriver chromeDriver, String cantidadCupos)
    {
      AccederPaginaVisitante = new PaginaVisitante(chromeDriver);
      String numeroTarjeta = "1234123412341234";
      String codigoSeguridad = "413";
      String nombreCompleto = "Jonathan Alfonso Navarro";
      String mes = "12";
      String anyo = "2025";
      AccederPaginaVisitante.ingresarNumeroTarjeta(numeroTarjeta, codigoSeguridad);
      AccederPaginaVisitante.ingresarFechaVencimiento(mes, anyo);
      AccederPaginaVisitante.ingresarNombreTarjeta(nombreCompleto);
      this.VerificarCuposVentanaPago(cantidadCupos);
    }

    public void VerificarCuposVentanaPago(String cantidadCupos)
    {
      IWebElement cuposEncontrados = AccederPaginaVisitante.verificarCuposVentanaDePago();
      Assert.AreEqual(cuposEncontrados.Text, "Cantidad de espacios reservados: " + cantidadCupos);
      ChromeDriver.Quit();
    }
  }
}

