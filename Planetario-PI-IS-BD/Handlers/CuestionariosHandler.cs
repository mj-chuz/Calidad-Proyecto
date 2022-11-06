using System;
using System.Collections.Generic;
using Planetario.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace Planetario.Handlers {
  public class CuestionariosHandler {
    public void GuardarArchivo(CuestionarioModel cuestionario) {
      JsonSerializer serializador = new JsonSerializer();
      using (StreamWriter stream = new StreamWriter(AppContext.BaseDirectory + "/Cuestionarios/" + cuestionario.Dificultad + "/" + cuestionario.Nombre + ".json"))
      using (JsonWriter writer = new JsonTextWriter(stream)) {
        serializador.Serialize(writer, cuestionario);
      }
    }
    public PreguntaCuestionarioModel ProcesarPregunta(JToken preguntaJSON) {
      PreguntaCuestionarioModel resultado;
      switch (preguntaJSON["Tipo"].ToString()) {
        case "SeleccionUnica":
          resultado = new SeleccionUnica() {
            Pregunta = preguntaJSON["Pregunta"].ToString(),
            Opciones = preguntaJSON["Opciones"].ToObject<List<String>>(),
            Respuesta = preguntaJSON["Respuesta"].ToString()
          };
          return resultado;
        case "Asocie":
          resultado = new Asocie() {
            Pregunta = preguntaJSON["Pregunta"].ToString(),
            Preguntas = preguntaJSON["Preguntas"].ToObject<List<String>>(),
            Respuesta = preguntaJSON["Respuesta"].ToObject<List<String>>(),
            Opciones = preguntaJSON["Opciones"].ToObject<List<String>>()
          };
          return resultado;
        case "SeleccionMultiple":
          resultado = new SeleccionMultiple() {
            Pregunta = preguntaJSON["Pregunta"].ToString(),
            Opciones = preguntaJSON["Opciones"].ToObject<List<String>>(),
            Respuesta = preguntaJSON["Respuesta"].ToObject<List<String>>()
          };
          return resultado;
      }
      return new PreguntaCuestionarioModel();
    }
    public CuestionarioModel CargarArchivo(String cuestionarioJSON) {
      CuestionarioModel cuestionario = new CuestionarioModel();
      JObject cuestionarioObjetoJSON = JObject.Parse(cuestionarioJSON);
      cuestionario.Nombre = cuestionarioObjetoJSON["Nombre"].ToString();
      cuestionario.Dificultad = cuestionarioObjetoJSON["Dificultad"].ToString();
      JToken preguntas = cuestionarioObjetoJSON["Preguntas"];
      foreach(JToken pregunta in (preguntas as JArray)) {
        cuestionario.Preguntas.Add(ProcesarPregunta(pregunta));
      }
      return cuestionario;
    }

    public Dictionary<String, List<String>>  ObtenerNombreCuestionariosPorDificultad() {
      List<String> dificultades = new List<String>() { "Principiante", "Intermedio", "Avanzado" };
      Dictionary<String, List<String>> nombresCuestionariosPorDificultad = new Dictionary<String, List<String>>();
      foreach (String dificultad in dificultades) {
        String direccionCarpeta = AppContext.BaseDirectory + "/Cuestionarios/" + dificultad;
        List<String> cuestionarios = new List<String>();
        DirectoryInfo metadatosCuestionarios = new DirectoryInfo(direccionCarpeta);
        FileInfo[] metadatosCuestionariosIterable = metadatosCuestionarios.GetFiles();
        foreach (FileInfo metaDatoCuestionario in metadatosCuestionariosIterable) {
          cuestionarios.Add(metaDatoCuestionario.Name);
        }
        nombresCuestionariosPorDificultad[dificultad] = cuestionarios;
      }
      return nombresCuestionariosPorDificultad;
    }
  }
}