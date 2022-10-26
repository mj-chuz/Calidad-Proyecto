using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Planetario.Models;

namespace Planetario.Handlers {
  public class JuegosHandler {

    private String _direccionArchivo = AppContext.BaseDirectory + "/Juegos/juegos.json";

    public List<JuegoModel> CargarListaJuegos() {
      List<JuegoModel> juegos = new List<JuegoModel>();
      JToken juegosJSON = JObject.Parse(System.IO.File.ReadAllText(_direccionArchivo))["Juegos"];
      foreach(JToken juego in (juegosJSON as JArray)) {
        JuegoModel juegoLeido = new JuegoModel(juego["Nombre"].ToString(), juego["Link"].ToString(),juego["Imagen"].ToString());
        juegos.Add(juegoLeido);
      }
      return juegos;
    }

  }
}