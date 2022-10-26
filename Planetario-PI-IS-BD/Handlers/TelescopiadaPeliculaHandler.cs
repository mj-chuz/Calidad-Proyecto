using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Planetario.Models;


namespace Planetario.Handlers {
  public class TelescopiadaPeliculaHandler : BaseDeDatosHandler{
    private CalendariosHandler CalendarioHandler;

    public TelescopiadaPeliculaHandler() {
      CalendarioHandler = new CalendariosHandler();
    }

    public bool CrearTelescopiadaPelicula(TelescopiadaPeliculaModel telescopiadaPelicula, String enlace) {
      bool exitoAgregarActividad;
      String consulta = "INSERT INTO TelescopiadaYPelicula " + "(tituloPK,fechaPK,descripcion,tipo,duracion,numeroDeIdentificacionFuncionarioFK)" +
         " VALUES (@titulo,@fecha,@descripcion,@tipo,@duracion,@NumeroIdentificacionFuncionario)";
      exitoAgregarActividad = AgregarTelescopiadaPelicula(consulta, telescopiadaPelicula);
      if (exitoAgregarActividad) {
        CalendarioHandler.AgregarTelescopiadaPeliculaACalendario(telescopiadaPelicula, enlace);
      }
      return exitoAgregarActividad;
    }

    public bool AgregarTelescopiadaPelicula(String consulta, TelescopiadaPeliculaModel telescopiadaPelicula) {
      bool exito;
      ConexionPlanetario.Open();
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@titulo", telescopiadaPelicula.Titulo);
      comandoParaConsulta.Parameters.AddWithValue("@fecha", telescopiadaPelicula.Fecha);
      comandoParaConsulta.Parameters.AddWithValue("@descripcion", telescopiadaPelicula.Descripcion);
      comandoParaConsulta.Parameters.AddWithValue("@tipo", telescopiadaPelicula.Tipo);
      comandoParaConsulta.Parameters.AddWithValue("@duracion", telescopiadaPelicula.Duracion);
      comandoParaConsulta.Parameters.AddWithValue("@NumeroIdentificacionFuncionario", 1234);
      exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
      ConexionPlanetario.Close();
      return exito;
    }

    public TelescopiadaPeliculaModel ObtenerTelescopiadaPelicula(String titulo, DateTime fecha) {
      string consulta = "SELECT * FROM TelescopiadaYPelicula" + " WHERE tituloPK = @titulo AND fechaPK = @fecha";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@titulo", titulo);
      comandoParaConsulta.Parameters.AddWithValue("@fecha", fecha);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      return this.ObtenerTelescopiadaPelicula(tablaResultado.Rows[0]);
    }

    private TelescopiadaPeliculaModel ObtenerTelescopiadaPelicula(DataRow filaActividad) {
        TelescopiadaPeliculaModel telescopiadaPelicula = new TelescopiadaPeliculaModel {
        Fecha = Convert.ToDateTime(filaActividad["fechaPK"]),
        Titulo = Convert.ToString(filaActividad["tituloPK"]),
        Descripcion = Convert.ToString(filaActividad["descripcion"]),
        Duracion = Convert.ToInt32(filaActividad["duracion"]),
        Tipo = Convert.ToString(filaActividad["tipo"])
      };
      return telescopiadaPelicula;
    }
  }
}