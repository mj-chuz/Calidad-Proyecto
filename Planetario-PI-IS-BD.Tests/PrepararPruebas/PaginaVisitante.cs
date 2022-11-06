using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Planetario_PI_IS_BD.Tests.PrepararPruebas
{
  public class PaginaVisitante
  {
    public struct VisitanteNuevo
    {
      public String Correo { get; set; }
      public String Nombre { get; set; }
      public String PrimerApellido { get; set; }
      public String SegundoApellido { get; set; }
      public String Genero { get; set; }
      public String Pais { get; set; }
      public String FechaNacimiento { get; set; }

      public void AgregarDatos()
      {
        this.Correo = "jonathan@gmail.com";
        this.Nombre = "Oscar";
        this.PrimerApellido = "Lopez";
        this.SegundoApellido = "Aguilar";
        this.Genero = "Femenino";
        this.Pais = "Costa_Rica";
        this.FechaNacimiento = "10/10/2000";
      }
    }

    public IWebDriver DriverChrome;
    By NumeroIdentificacion = By.Id("NumeroIdentificacion");
    By BotonContinuarAPago = By.Id("continuarAPago");
    By EntradaNumeroTarjeta = By.Id("numeroTarjeta");
    By NombreTarjeta = By.Id("nombreTarjeta");
    By MesExpiracion = By.Id("mes-expiracion");
    By AnhoExpiracion = By.Id("anho-expiracion");
    By Cvc = By.Id("cvc");
    By BotonPagar = By.Id("boton-pagar");
    By EncontrarCuposReservadosFinal = By.XPath("/html/body/div/div/div/div/div[4]/div[2]/p");
    By BotonContinuarPagoVisitanteNuevo = By.XPath("/html/body/div/form/div/input");
    By InputCedula = By.Id("NumeroIdentificacion");
    By InputNombre = By.Id("Nombre");
    By InputPrimerApellido = By.Id("PrimerApellido");
    By InputSegundoApellido = By.Id("SegundoApellido");
    By InputCorreo = By.Id("Correo");
    By InputGenero = By.Id("Genero");
    By InputNacionalidad = By.Id("paisSeleccionado");
    By InputFechaNacimiento = By.Id("FechaDeNacimiento");

    public PaginaVisitante(IWebDriver driverChrome)
    {
      this.DriverChrome = driverChrome;
    }

    public IWebDriver ingresarIdentificacion(String identificacion)
    {
      DriverChrome.FindElement(NumeroIdentificacion).SendKeys(identificacion);
      DriverChrome.FindElement(BotonContinuarAPago).Click();
      return DriverChrome;
    }

    public IWebDriver ingresarVisitanteNuevo(String identificacion)
    {
      VisitanteNuevo datosVisitante = new VisitanteNuevo();
      IJavaScriptExecutor js = (IJavaScriptExecutor)DriverChrome;
      js.ExecuteScript("window.scrollTo(0, 954)");
      datosVisitante.AgregarDatos();
      DriverChrome.FindElement(InputCedula).SendKeys(identificacion);
      DriverChrome.FindElement(InputCorreo).SendKeys(datosVisitante.Correo);
      DriverChrome.FindElement(InputNombre).SendKeys(datosVisitante.Nombre);
      DriverChrome.FindElement(InputPrimerApellido).SendKeys(datosVisitante.PrimerApellido);
      DriverChrome.FindElement(InputSegundoApellido).SendKeys(datosVisitante.SegundoApellido);
      SelectElement seleccionarGenero = new SelectElement(DriverChrome.FindElement(InputGenero));
      seleccionarGenero.SelectByValue(datosVisitante.Genero);
      SelectElement seleccionarPais = new SelectElement(DriverChrome.FindElement(InputNacionalidad));
      seleccionarPais.SelectByValue(datosVisitante.Pais);
      IWebElement fecha = DriverChrome.FindElement(InputFechaNacimiento);
      fecha.SendKeys(datosVisitante.FechaNacimiento);
      DriverChrome.FindElement(BotonContinuarPagoVisitanteNuevo).Click();
      return DriverChrome;
    }

    public void ingresarNumeroTarjeta(String numeroTarjeta, String codigoSeguridad)
    {
      IJavaScriptExecutor js = (IJavaScriptExecutor)DriverChrome;
      js.ExecuteScript("window.scrollTo(0, 1005)");
      DriverChrome.FindElement(EntradaNumeroTarjeta).SendKeys(numeroTarjeta);
      DriverChrome.FindElement(Cvc).SendKeys(codigoSeguridad);
    }

    public void ingresarFechaVencimiento(String mes, String anyo)
    {
      SelectElement seleccionarMes = new SelectElement(DriverChrome.FindElement(MesExpiracion));
      seleccionarMes.SelectByValue(mes);
      SelectElement seleccionarAnho = new SelectElement(DriverChrome.FindElement(AnhoExpiracion));
      seleccionarAnho.SelectByValue(anyo);
    }

    public void ingresarNombreTarjeta(String nombreCompleto)
    {
      IJavaScriptExecutor js = (IJavaScriptExecutor)DriverChrome;
      js.ExecuteScript("window.scrollTo(0, 1005)");
      DriverChrome.FindElement(NombreTarjeta).SendKeys(nombreCompleto);
      DriverChrome.FindElement(BotonPagar).Click();
    }

    public IWebElement verificarCuposVentanaDePago()
    {
      IWebElement cuposEncontrados = DriverChrome.FindElement(EncontrarCuposReservadosFinal);
      return cuposEncontrados;
    }
  }
}