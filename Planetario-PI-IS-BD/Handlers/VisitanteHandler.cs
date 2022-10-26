using Planetario.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Planetario.Handlers {
  public class VisitanteHandler : BaseDeDatosHandler{

    public void AgregarVisitante(VisitanteModel visitante) {
      String consulta = "INSERT INTO Visitante VALUES(@nombre,@primerApellido,@segundoApellido,@pais,@correo," +
        "@numeroIdentificacionPK,@genero,@fechaDeNacimiento,@nivelEducativo)";
      ConexionPlanetario.Open();
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@nombre", visitante.Nombre);
      comandoParaConsulta.Parameters.AddWithValue("@primerApellido", visitante.PrimerApellido);
      if(visitante.SegundoApellido == null) {
        comandoParaConsulta.Parameters.AddWithValue("@segundoApellido", "  ");
      } else {
        comandoParaConsulta.Parameters.AddWithValue("@segundoApellido", visitante.SegundoApellido);
      }
      comandoParaConsulta.Parameters.AddWithValue("@pais", visitante.Pais);
      comandoParaConsulta.Parameters.AddWithValue("@correo", visitante.Correo);
      comandoParaConsulta.Parameters.AddWithValue("@numeroIdentificacionPK", visitante.NumeroIdentificacion);
      comandoParaConsulta.Parameters.AddWithValue("@genero", visitante.Genero);
      comandoParaConsulta.Parameters.AddWithValue("@fechaDeNacimiento", visitante.FechaDeNacimiento);
      comandoParaConsulta.Parameters.AddWithValue("@nivelEducativo", visitante.NivelEducativo);
      int exitoCambiarFilas= comandoParaConsulta.ExecuteNonQuery();
      ComprobarCambiosEnTabla(exitoCambiarFilas);
      ConexionPlanetario.Close();
    }

    public String AgregarInscripcion(VisitanteModel visitante, ActividadModel actividadInscrita, int numeroCupos) {
      String consulta = "INSERT INTO ReservaActividad VALUES(@tituloActividadFK, @fechaActividadFK, @cuposReservados, @codigoReserva, @fechaReserva, @numeroIdentificacionVisitanteFK)";
      String codigo = ""+visitante.NumeroIdentificacion + generarID();
      int exitoCambiarFilas = 0;
      ConexionPlanetario.Open();
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@tituloActividadFK", actividadInscrita.Titulo);
      comandoParaConsulta.Parameters.AddWithValue("@fechaActividadFK", actividadInscrita.Fecha);
      comandoParaConsulta.Parameters.AddWithValue("@numeroIdentificacionVisitanteFK", visitante.NumeroIdentificacion);
      comandoParaConsulta.Parameters.AddWithValue("@cuposReservados", numeroCupos);
      comandoParaConsulta.Parameters.AddWithValue("@codigoReserva", codigo);
      comandoParaConsulta.Parameters.AddWithValue("@fechaReserva", DateTime.Now);
      exitoCambiarFilas = comandoParaConsulta.ExecuteNonQuery();
      ComprobarCambiosEnTabla(exitoCambiarFilas);
      ConexionPlanetario.Close();
      return codigo;
    }

    public VisitanteModel RecuperarVisitante(String numeroIdentificacion) {
      String consulta = "SELECT * FROM Visitante WHERE numeroIdentificacionPK = @numeroIdentificacion";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@numeroIdentificacion", numeroIdentificacion);
      DataTable tablaConsultada = CrearTablaConsulta(comandoParaConsulta);
      VisitanteModel visitante = new VisitanteModel();
      foreach (DataRow filaVisitante in tablaConsultada.Rows) {
          visitante.NumeroIdentificacion = Convert.ToString(filaVisitante["numeroIdentificacionPK"]);
          visitante.Nombre = Convert.ToString(filaVisitante["nombre"]);
          visitante.PrimerApellido = Convert.ToString(filaVisitante["primerApellido"]);
          visitante.SegundoApellido = Convert.ToString(filaVisitante["segundoApellido"]);
          visitante.Pais = Convert.ToString(filaVisitante["pais"]);
          visitante.Correo = Convert.ToString(filaVisitante["correo"]);
          visitante.Genero = Convert.ToString(filaVisitante["genero"]);
          visitante.FechaDeNacimiento = Convert.ToDateTime(filaVisitante["fechaDeNacimiento"]);
          visitante.NivelEducativo = Convert.ToString(filaVisitante["nivelEducativo"]);
      }
      return visitante;
    }

    public bool VerificarInscripcion(String numeroIdentificacion) {
      return VerificarInscripcionPlanetario(numeroIdentificacion);
    }

    public bool VerificarInscripcionPlanetario(String numeroIdentificacion) {
      DataTable tablaConsultada = new DataTable();
      String consulta = "SELECT numeroIdentificacionPK FROM Visitante WHERE numeroIdentificacionPK = @numeroIdentificacion";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@numeroIdentificacion", numeroIdentificacion);
      tablaConsultada = CrearTablaConsulta(comandoParaConsulta);
      return tablaConsultada.Rows.Count > 0;
    }
  }
}

