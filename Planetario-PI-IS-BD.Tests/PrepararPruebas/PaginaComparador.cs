using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace Planetario_PI_IS_BD.Tests.PrepararPruebas{
  public class PaginaComparador{
    public IWebDriver DriverChrome;
    public String DireccionaPaginaComparador = "http://pruebaproyectopi.azurewebsites.net/Comparador/CuerposCelestes";
    public By PrimerCuerpo = By.XPath("//*[@id='main']/div[2]/select");
    public By SegundoCuerpo = By.XPath("//*[@id='main']/div[3]/select");
    public String ValorPrimerCuerpo = "sun";
    public String ValorSegundoCuerpo = "jupiter";
    public List<String> ValoresPlanetas = new List<String>(){
      "jupiter",
      "saturn",
      "uranus",
      "neptune",
      "earth",
      "venus",
      "mars",
      "mercury",
      "moon"
    };

    public PaginaComparador(IWebDriver driver){
      this.DriverChrome = driver;
    }

    public void IniciarPagina(){
      DriverChrome.Manage().Window.Maximize();
      DriverChrome.Navigate().GoToUrl(DireccionaPaginaComparador);
    }

    public void CompararDosCuerpos(){
      SelectElement seleccionarPrimerCuerpo = new SelectElement(DriverChrome.FindElement(PrimerCuerpo));
      SelectElement seleccionarSegundoCuerpo = new SelectElement(DriverChrome.FindElement(SegundoCuerpo));
      seleccionarPrimerCuerpo.SelectByValue(ValorPrimerCuerpo);
      seleccionarSegundoCuerpo.SelectByValue(ValorSegundoCuerpo);
    }

    public void CompararCuerpos(){
      SelectElement seleccionarPrimerCuerpo = new SelectElement(DriverChrome.FindElement(PrimerCuerpo));
      SelectElement seleccionarSegundoCuerpo = new SelectElement(DriverChrome.FindElement(SegundoCuerpo));
      seleccionarPrimerCuerpo.SelectByValue(ValorPrimerCuerpo);
      for (int indice = 0; indice < ValoresPlanetas.Count; indice++){
        seleccionarSegundoCuerpo.SelectByValue(ValoresPlanetas[indice]);
      }
    }
  }
}
