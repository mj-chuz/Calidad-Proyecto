using System;
using System.Collections.Generic;

namespace Planetario.Models {
  public class PreguntaCuestionarioModel {
    public String Pregunta;
    public String Tipo;
  }

  public class SeleccionUnica : PreguntaCuestionarioModel {
    public List<String> Opciones { get; set; }
    public String Respuesta { get; set; }
    public SeleccionUnica() {
      Tipo = "SeleccionUnica";
    }
  }

  public class Asocie : PreguntaCuestionarioModel {
    public List<String> Preguntas { get; set; }
    public List<String> Respuesta { get; set; }
    public List<String> Opciones { get; set; }
    public Asocie() {
      Tipo = "Asocie";
    }
  }

  public class SeleccionMultiple : PreguntaCuestionarioModel {
    public List<String> Opciones { get; set; }
    public List<String> Respuesta { get; set; }
    public SeleccionMultiple() {
      Tipo = "SeleccionMultiple";
    }
  }
}