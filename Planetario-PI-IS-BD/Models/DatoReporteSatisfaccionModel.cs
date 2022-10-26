using System;

namespace Planetario.Models {
  public class DatoReporteSatisfaccionModel {
    public DateTime FechaReporte { get; set; }
    public double PorcentajeSatisfaccion { get; set; } 
    public String Pregunta { get; set; }
    public String Categoria { get; set; }
    public double Promedio { get; set; }
    public double CantidadDeRespuestas { get; set; }
    public String Genero { get; set; }
    public String NivelEducativo { get; set; }
  }
}