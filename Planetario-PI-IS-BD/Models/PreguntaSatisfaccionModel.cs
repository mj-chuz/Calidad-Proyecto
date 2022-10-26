using System;
using System.Collections.Generic;

namespace Planetario.Models {
  public class PreguntaSatisfaccionModel {
    public String IdPregunta { get; set; }
    public String Pregunta { get; set; }
    public String Categoria { get; set; }
    public int Respuesta { get; set; }

    public override bool Equals(object obj) {
      return obj is PreguntaSatisfaccionModel model &&
             IdPregunta == model.IdPregunta &&
             Pregunta == model.Pregunta &&
             Categoria == model.Categoria &&
             Respuesta == model.Respuesta;
    }

    public static bool operator ==(PreguntaSatisfaccionModel left, PreguntaSatisfaccionModel right) {
      return EqualityComparer<PreguntaSatisfaccionModel>.Default.Equals(left, right);
    }

    public static bool operator !=(PreguntaSatisfaccionModel left, PreguntaSatisfaccionModel right) {
      return !(left == right);
    }
  }
}