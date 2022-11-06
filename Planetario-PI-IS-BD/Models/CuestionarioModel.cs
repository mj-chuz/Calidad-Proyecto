using System;
using System.Collections.Generic;

namespace Planetario.Models {
  public class CuestionarioModel {
    public String Nombre { get; set; }
    public String Dificultad { get; set; }
    public List<PreguntaCuestionarioModel> Preguntas { get; set; }
    public CuestionarioModel() {
      Preguntas = new List<PreguntaCuestionarioModel>();
    }
  }
}
