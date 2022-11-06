using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;


namespace Planetario_PI_IS_BD.Tests.PrepararPruebas {
  public class PaginaJuegos {
    public IWebDriver DriverChrome;
    public String DireccionPaginaInicio = "http://pruebaproyectopi.azurewebsites.net/";
    By BotonMaterialesEducativos = By.Id("dropdown-materiales");
    By BotonJuegos = By.XPath("//*[@id='navbarColor01']/ul/li[6]/ul/li[3]/a");
    By PrimeraCartaJuego = By.XPath("/html/body/div/div/div/div/div/div[1]/div/div/p/a");
    By BotonEmpezar = By.Id("btnComenzar");


    public PaginaJuegos(IWebDriver driver) {
      this.DriverChrome = driver;
    }
    public void IniciarPagina() {
      DriverChrome.Manage().Window.Maximize();
      DriverChrome.Navigate().GoToUrl(DireccionPaginaInicio);
    }

    public void AccederAJuego() {
      DriverChrome.FindElement(BotonMaterialesEducativos).Click();
      DriverChrome.FindElement(BotonJuegos).Click();
      DriverChrome.FindElement(PrimeraCartaJuego).Click();
    }

    public void LlenarFilaCrucigrama() {
      AccederAJuego();
      new WebDriverWait(DriverChrome,TimeSpan.FromTicks(1000000000)).Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("frame-juego")));
      WebDriverWait esperar = new WebDriverWait(DriverChrome, TimeSpan.FromTicks(100000000));
      esperar.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(BotonEmpezar));
      DriverChrome.FindElement(BotonEmpezar).Click();
      DriverChrome.FindElement(By.Id("c05_00")).Click();
      DriverChrome.FindElement(By.Id("control")).SendKeys("hanslippershey");
    }
  }
}
