using System;
using System.Data.SqlClient;
using Planetario.Models;
using System.Data;

namespace Planetario.Handlers {
  public class CuponHandler : BaseDeDatosHandler{

    public CuponModel ObtenerCupon(String codigoCupon) {
      String consulta = "SELECT * FROM Cupon WHERE codigoCuponPK = @codigoCupon";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@codigoCupon", codigoCupon);
      DataTable filaCupon = CrearTablaConsulta(comandoParaConsulta);
      CuponModel cupon;
      if (filaCupon.Rows.Count == 0) {
        cupon = new CuponModel {
          Codigo = "0"
        };
      } else {
        DataRow filaCuponResultado = filaCupon.Rows[0];
        cupon = CrearCupon(filaCuponResultado);
      }
      return cupon;
    }


    private CuponModel CrearCupon(DataRow filaCupon) {
      String codigoCupon = filaCupon["codigoCuponPK"].ToString();
      double descuentoRelativo = Convert.ToDouble(filaCupon["descuento"]);
      return new CuponModel(codigoCupon, descuentoRelativo);
    }
  }
}