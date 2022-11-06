using System;
using System.Collections.Generic;

namespace Planetario.Models {
  public class JuegoModel {

    public String Nombre { get; }
    public String Link { get; }
    public String DireccionImagen { get; }

    public JuegoModel(String nombre, String link, String direccionImagen) {
      Nombre = nombre;
      Link = link;
      DireccionImagen = direccionImagen;
    }

    public override bool Equals(object obj) {
      return obj is JuegoModel model &&
             Nombre == model.Nombre &&
             Link == model.Link &&
             DireccionImagen == model.DireccionImagen;
    }

    public override int GetHashCode() {
      int hashCode = -683104575;
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Nombre);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Link);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DireccionImagen);
      return hashCode;
    }
  }
}