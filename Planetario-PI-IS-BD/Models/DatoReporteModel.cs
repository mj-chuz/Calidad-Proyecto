using System;

namespace Planetario.Models {
  public class DatoReporteModel {
    public DateTime FechaCompra { get; set; }
    public int CantidadDeUnidadesVendidas { get; set; }
    public String IdentificadorProducto { get; set; }
    public String NombreProducto { get; set; }
    public String Categoria { get; set; }
    public String Genero { get; set; }
    public String NivelEducativo { get; set; }
    public String DiaSemana { get; set; }
  }
}