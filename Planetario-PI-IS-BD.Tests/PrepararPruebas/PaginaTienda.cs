using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace Planetario_PI_IS_BD.Tests.PrepararPruebas
{
  public class PaginaTienda
  {
    public IWebDriver DriverChrome;
    public String DireccionPaginaTienda = "http://pruebaproyectopi.azurewebsites.net/Producto/Catalogo";
    public Dictionary<String, String> categorias = new Dictionary<String, String>();

    public By SeleccionOrdenamiento = By.Id("ordenamiento");

  

    public PaginaTienda(IWebDriver driverChrome)
    {
      this.DriverChrome = driverChrome;
    }

    public void Iniciar()
    {
      DriverChrome.Manage().Window.Maximize();
      DriverChrome.Navigate().GoToUrl(DireccionPaginaTienda);
    }

    public void AgregarElementosDiccionarios()
    {
      categorias.Add("Peluches", "//*[@id='navbarColor02']/ul/li[2]/a");
      categorias.Add("Tazas", "//*[@id='navbarColor02']/ul/li[3]/a");
      categorias.Add("Llaveros", "//*[@id='navbarColor02']/ul/li[4]/a");
      categorias.Add("Posters", "//*[@id='navbarColor02']/ul/li[5]/a");
    }

    public IWebElement CompararOrdenamiento(String comparacion)
    {
      SelectElement seleccionarComparacion = new SelectElement(DriverChrome.FindElement(SeleccionOrdenamiento));
      seleccionarComparacion.SelectByValue(comparacion);
      IJavaScriptExecutor js = (IJavaScriptExecutor)DriverChrome;
      js.ExecuteScript("window.scrollBy(0,350)", "");
      By seleccion = By.XPath("//*[@id='contenedorProductos']/div/div[1]/div/div/h5");
      WebDriverWait espera = new WebDriverWait(DriverChrome, TimeSpan.FromTicks(100000000000000000));
      espera.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(seleccion));
      IWebElement producto = DriverChrome.FindElement(seleccion);
      return producto;
    }

    public void CompararCategorias(String categoria)
    {
      
      DriverChrome.FindElement(By.XPath(this.categorias[categoria])).Click();
      IJavaScriptExecutor js = (IJavaScriptExecutor)DriverChrome;
      js.ExecuteScript("window.scrollBy(0,350)", "");
      js.ExecuteScript("window.scrollBy(0,-350)", "");
    }
  }
}
