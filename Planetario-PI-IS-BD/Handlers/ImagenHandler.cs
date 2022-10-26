using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Planetario.Models;

namespace Planetario.Handlers {
  public class ImagenHandler : BaseDeDatosHandler{

    public ImagenModel ObtenerImagen(string nombrePK) {
      string consulta = "SELECT * FROM Imagen WHERE nombrePK = @nombrePK";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, this.ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@nombrePK", nombrePK);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      DataRow filaImagen = tablaResultado.Rows[0];
      ImagenModel imagen = new ImagenModel {
        Nombre = Convert.ToString(filaImagen["nombrePK"]),
        Extension = Convert.ToString(filaImagen["extension"])
      };
      imagen.ArchivoImagen = new CargadorArchivoHandler((byte[])filaImagen["archivo"]);
      return imagen;
    }
  }
}
