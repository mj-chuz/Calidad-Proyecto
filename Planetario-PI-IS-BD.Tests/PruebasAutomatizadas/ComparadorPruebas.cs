using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Planetario_PI_IS_BD.Tests.PrepararPruebas;


namespace Planetario_PI_IS_BD.Tests.PruebasAutomatizadas{
  [TestClass]
  public class ComparadorPruebas{
    public PaginaComparador AccesoPaginaComparador;
    IWebDriver DriverChrome;

    public ComparadorPruebas(){
      DriverChrome = new ChromeDriver();
      AccesoPaginaComparador = new PaginaComparador(DriverChrome);
    }

    [TestMethod]
    public void CompararCuerpos(){
      AccesoPaginaComparador.IniciarPagina();
      AccesoPaginaComparador.CompararDosCuerpos();
    }

    [TestMethod]
    public void CompararConjuntoCuerpos(){
      AccesoPaginaComparador.IniciarPagina();
      AccesoPaginaComparador.CompararCuerpos();
    }
  }
}
