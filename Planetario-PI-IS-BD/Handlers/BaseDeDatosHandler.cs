using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;

namespace Planetario.Handlers {
  public class BaseDeDatosHandler {
    protected readonly SqlConnection ConexionPlanetario;
    public BaseDeDatosHandler() {
      String RutaConexionActividad = ConfigurationManager.ConnectionStrings["PlanetarioConnection"].ToString();
      ConexionPlanetario = new SqlConnection(RutaConexionActividad);
    }
    public DataTable CrearTablaConsulta(SqlCommand comandoParaConsulta) {
      SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
      DataTable consultaFormatoTabla = new DataTable();
      SqlConnection conexionSQL = comandoParaConsulta.Connection;
      conexionSQL.Open();
      adaptadorParaTabla.Fill(consultaFormatoTabla);
      conexionSQL.Close();
      return consultaFormatoTabla;
    }
    public void ComprobarCambiosEnTabla(int exitoCambiarFilas) {
      if (exitoCambiarFilas == 0) {
        throw new Exception("No se cambio ninguna fila");
      }
    }
    public List<SelectListItem> TransformarListaView(List<string> lista) {
      List<SelectListItem> listaView = new List<SelectListItem>();
      foreach (string valor in lista) {
        listaView.Add(new SelectListItem() { Text = valor, Value = valor });
      }
      return listaView;
    }

    public String generarID() {
      String consulta = "Select CONVERT(varchar(255), NEWID())";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta,ConexionPlanetario);
      DataTable IDGenerada = CrearTablaConsulta(comandoParaConsulta);
      String IDConvertida = Convert.ToString(IDGenerada.Rows[0][0]);
      return IDConvertida;
    }
  }
}