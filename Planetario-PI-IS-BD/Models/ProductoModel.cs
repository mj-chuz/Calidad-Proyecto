using System;
using System.Collections.Generic;

namespace Planetario.Models {
  public class ProductoModel {
    public String IdentificadorProducto { get; set; }
    public int UnidadesDisponibles { get; set; }
    public String Descripcion { get; set; }
    public String Categoria { get; set; }
    public String Nombre { get; set; }
    public float Precio { get; set; }
    public float Peso { get; set; }
    public String Color { get; set; }
    public String NombreFoto { get; set; }

    public override bool Equals(object obj) {
      return obj is ProductoModel model &&
             IdentificadorProducto == model.IdentificadorProducto;
    }

    public override int GetHashCode() {
      return -1161671498 + EqualityComparer<string>.Default.GetHashCode(IdentificadorProducto);
    }

    public static bool operator ==(ProductoModel left, ProductoModel right) {
      return EqualityComparer<ProductoModel>.Default.Equals(left, right);
    }

    public static bool operator !=(ProductoModel left, ProductoModel right) {
      return !(left == right);
    }
  }
}