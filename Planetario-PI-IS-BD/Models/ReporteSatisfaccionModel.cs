using System;

namespace Planetario.Models {
  public class ReporteSatisfaccionModel {
    public String Pregunta { get; set; }
    public String Categoria { get; set; }
    public String Genero { get; set; }
    public String NivelEducativo { get; set; }
    public double PromedioRespuestas { get; set; }
    public double PorcentajeSatisfaccion { get; set; }
    public int CantidadRespuestas { get; set; }
  }
}