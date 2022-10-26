using System;


namespace Planetario.Models {
  public class CuponModel {
    public String Codigo { get; set; }
    public double DescuentoRelativo { get; set; }

    public CuponModel(String codigo = "", double descuentoRelativo = 0) {
      Codigo = codigo;
      DescuentoRelativo = descuentoRelativo;
    }
  }
}