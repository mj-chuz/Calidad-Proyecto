using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Planetario_PI_IS_BD.Tests.PrepararPruebas;

namespace Planetario_PI_IS_BD.Tests.PruebasAutomatizadas
{
  [TestClass]
  public class TiendaPruebas
  {
    public PaginaTienda AccederPaginaTienda;
    IWebDriver DriverChrome;

    public TiendaPruebas()
    {
      DriverChrome = new ChromeDriver();
      AccederPaginaTienda = new PaginaTienda(DriverChrome);
      AccederPaginaTienda.Iniciar();
    }

    [TestMethod]
    public void PruebaOrdenamiento()
    {
     
      IWebElement primerProducto = AccederPaginaTienda.CompararOrdenamiento("Nombre-Asc");
      IWebElement segundoProducto = AccederPaginaTienda.CompararOrdenamiento("Nombre-Desc");
      DriverChrome.Quit();
      Assert.AreNotEqual(primerProducto, segundoProducto);
    }

    [TestMethod]
    public void AccederCategorias()
    {
      AccederPaginaTienda.AgregarElementosDiccionarios();
      AccederPaginaTienda.CompararCategorias("Tazas");
      AccederPaginaTienda.CompararCategorias("Peluches");
      AccederPaginaTienda.CompararCategorias("Posters");
      AccederPaginaTienda.CompararCategorias("Llaveros");
      DriverChrome.Quit();
    }
  }
}
