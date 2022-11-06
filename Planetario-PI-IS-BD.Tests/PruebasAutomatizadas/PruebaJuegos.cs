using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Planetario_PI_IS_BD.Tests.PrepararPruebas;
using Planetario.Models;
using Planetario.Controllers;
using System.Web.Mvc;

namespace Planetario_PI_IS_BD.Tests.PruebasAutomatizadas {
  [TestClass]
  public class PruebaJuegos {

    public PaginaJuegos AccesoPaginaJuegos;
    IWebDriver DriverChrome;

    public PruebaJuegos() {
      DriverChrome = new ChromeDriver();
      AccesoPaginaJuegos = new PaginaJuegos(DriverChrome);
    }

    [TestMethod]
    public void AccederAJuego() {
      AccesoPaginaJuegos.IniciarPagina();
      AccesoPaginaJuegos.AccederAJuego();
    }

    [TestMethod]
    public void LlenarFilaCrucigrama() {
      AccesoPaginaJuegos.IniciarPagina();
      AccesoPaginaJuegos.LlenarFilaCrucigrama();
    }
    [TestMethod]
    public void VerificarCantidadCorrectaDeJuegosCargados() {
        JuegosController controladorJuegos = new JuegosController();
        ViewResult vistaPaginaPrincipalJuegos = controladorJuegos.PaginaPrincipal() as ViewResult;
        Assert.AreEqual(4, vistaPaginaPrincipalJuegos.ViewBag.ListaJuegos.Count);
    }
    [TestMethod]
    public void VerificarJuegoCrucigramaCargado() {
        JuegosController controladorJuegos = new JuegosController();
        ViewResult vistaPaginaPrincipalJuegos = controladorJuegos.PaginaPrincipal() as ViewResult;
        JuegoModel crucigrama = new JuegoModel("Crucigrama astronómico", "https://es.educaplay.com/juego/10975597-terminos_astronomicos.html%22%3E", "crucigrama.png");
        Assert.IsTrue(vistaPaginaPrincipalJuegos.ViewBag.ListaJuegos.Contains(crucigrama));
    }
    [TestMethod]
    public void VerificarJuegoMemoriaCargado(){
        JuegosController controladorJuegos = new JuegosController();
        ViewResult vistaPaginaPrincipalJuegos = controladorJuegos.PaginaPrincipal() as ViewResult;
        JuegoModel memoria = new JuegoModel("Memoria Sistema Solar", "https://es.educaplay.com/juego/4675054-sistema_solar.html", "memoria.jpg");
        Assert.IsTrue(vistaPaginaPrincipalJuegos.ViewBag.ListaJuegos.Contains(memoria));
    }

    [TestMethod]
    public void VerificarJuegoRuletaCargado(){
        JuegosController controladorJuegos = new JuegosController();
        ViewResult vistaPaginaPrincipalJuegos = controladorJuegos.PaginaPrincipal() as ViewResult;
        JuegoModel ruleta = new JuegoModel("Ruleta astronómica", "https://es.educaplay.com/juego/10975780-ruleta_astronomica.html", "ruleta.png");
        Assert.IsTrue(vistaPaginaPrincipalJuegos.ViewBag.ListaJuegos.Contains(ruleta));
    }
    [TestMethod]
    public void VerificarJuegoMinecraftCargado(){
        JuegosController controladorJuegos = new JuegosController();
        ViewResult vistaPaginaPrincipalJuegos = controladorJuegos.PaginaPrincipal() as ViewResult;
        JuegoModel minecraft = new JuegoModel("Minecraft astronómico", "https://classic.minecraft.net", "minecraft.png");
        Assert.IsTrue(vistaPaginaPrincipalJuegos.ViewBag.ListaJuegos.Contains(minecraft));
     }


        
  }

}
