using System;
using System.Collections.Generic;
using Planetario.Models;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Planetario.Handlers {
  public class PreguntaSatisfaccionHandler : BaseDeDatosHandler {

    public PreguntaSatisfaccionModel ObtenerPregunta(String idPregunta, int respuesta = -1) {
      String consulta = "SELECT * FROM PreguntaSatisfaccion WHERE idPreguntaPK = @idPregunta";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@idPregunta", idPregunta);
      DataTable tablaConsultada = CrearTablaConsulta(comandoParaConsulta);
      return ConstruirPregunta(tablaConsultada.Rows[0], respuesta);
    }

    public List<PreguntaSatisfaccionModel> ObtenerPreguntaCategoria(String categoria) {
      String consulta = "SELECT * FROM PreguntaSatisfaccion WHERE categoria = @categoria";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@categoria", categoria);
      DataTable tablaConsultada = CrearTablaConsulta(comandoParaConsulta);
      List<PreguntaSatisfaccionModel> preguntasRecuperadas = new List<PreguntaSatisfaccionModel>();
      foreach (DataRow preguntaEnTabla in tablaConsultada.Rows) {
        preguntasRecuperadas.Add(ConstruirPregunta(preguntaEnTabla));
      }
      return preguntasRecuperadas;
    }

        private PreguntaSatisfaccionModel ConstruirPregunta(DataRow filaTabla, int respuesta = -1) {
      PreguntaSatisfaccionModel preguntaRecuperada = new PreguntaSatisfaccionModel() {
        IdPregunta = filaTabla["idPreguntaPK"].ToString(),
        Pregunta = filaTabla["pregunta"].ToString(),
        Categoria = filaTabla["categoria"].ToString()
      };
      if (respuesta != -1) {
        preguntaRecuperada.Respuesta = respuesta;
      }
      return preguntaRecuperada;
    }

    public void AgregarRespuestas(List<PreguntaSatisfaccionModel> preguntas, String idVisitante) {
      foreach (PreguntaSatisfaccionModel pregunta in preguntas) {
        AgregarRespuestaPregunta(pregunta, idVisitante);
      }
    }

    private void AgregarRespuestaPregunta(PreguntaSatisfaccionModel pregunta, String identificacionVisitante) {
      String consulta = "INSERT INTO RespuestaPregunta(fechaPK, idPreguntaFK, numeroIdentificacionFK, respuesta) VALUES " +
                        "(@fecha, @idPregunta, @numeroIdentificacion, @respuesta)";
      ConexionPlanetario.Open();
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@fecha", DateTime.Now);
      comandoParaConsulta.Parameters.AddWithValue("@idPregunta", pregunta.IdPregunta);
      comandoParaConsulta.Parameters.AddWithValue("@numeroIdentificacion", identificacionVisitante);
      comandoParaConsulta.Parameters.AddWithValue("@respuesta", pregunta.Respuesta);
      int exitoCambiarFilas = comandoParaConsulta.ExecuteNonQuery();
      ComprobarCambiosEnTabla(exitoCambiarFilas);
      ConexionPlanetario.Close();
    }


    public List<ReporteSatisfaccionModel> ObtenerReporteSatisfaccionPreguntaCategoria(String categoria = "", String genero = "", String nivelEducativo = "", String ordenamiento = "",  String fechaInicio ="", String fechaFinal ="") {
      bool existenFiltrosFecha = (fechaInicio != "" && fechaFinal != "");
      String consulta = "SELECT P.idPreguntaPK, P.pregunta, P.categoria, AVG(R.respuesta) as 'Promedio Respuestas', " +
                        "(CAST(COUNT(case when R.respuesta >= 4 then 1 else null end) AS float)/CAST(COUNT(*) AS float)) * 100 as 'Porcentaje satisfacción', " +
                        "COUNT(*) as 'Cantidad respuestas' ";
      if(genero != "" && genero != null)
        consulta += ", V.genero ";
      if (nivelEducativo != "" && nivelEducativo != null)
        consulta += ", V.nivelEducativo ";
        consulta += "FROM PreguntaSatisfaccion P " +
                  "JOIN RespuestaPregunta R ON R.idPreguntaFK = P.idPreguntaPK " +
                  "JOIN Visitante V ON V.numeroIdentificacionPK = R.numeroIdentificacionFK ";
      if (categoria != "" && categoria != null) {
        consulta += "WHERE P.categoria = @categoria ";
      }
      if (existenFiltrosFecha) {
        consulta += "AND R.fechaPK BETWEEN @fechaInicio AND @fechaFinal ";
      }
      SqlCommand comandoParaConsulta = AgregarFiltrosAConsulta(consulta, genero, nivelEducativo, ordenamiento);
      if (categoria != "" && categoria != null) {
        comandoParaConsulta.Parameters.AddWithValue("@categoria", categoria);
      }
      if (existenFiltrosFecha) {
        comandoParaConsulta.Parameters.AddWithValue("@fechaInicio", Convert.ToDateTime(fechaInicio));
        comandoParaConsulta.Parameters.AddWithValue("@fechaFinal", Convert.ToDateTime(fechaFinal));
      }

      DataTable tablaConsultada = CrearTablaConsulta(comandoParaConsulta);
      return ObtenerConjuntoDeReportes(tablaConsultada, genero, nivelEducativo);
    }

    private SqlCommand AgregarFiltrosAConsulta(String consulta, String genero = "", String nivelEducativo = "", String ordenamiento = "") {
      bool existeFiltroGenero = (genero != "" && genero != null && genero != "todos");
      bool existeFiltroNivelEducativo = (nivelEducativo != "" && nivelEducativo != null && nivelEducativo != "todos");
      if (existeFiltroGenero) {
        consulta += "AND V.genero = @genero ";
      }
      if (existeFiltroNivelEducativo) {
        consulta += "AND V.nivelEducativo = @nivelEducativo ";
      }
      consulta += "GROUP BY P.idPreguntaPK, P.pregunta, P.categoria";
      if (genero != "" && genero != null)
        consulta += ", V.genero";
      if (nivelEducativo != "" && nivelEducativo != null)
        consulta += ", V.nivelEducativo";
      if (ordenamiento != "" && ordenamiento != null) {
        consulta += " ORDER BY " + ordenamiento;
        if (ordenamiento != "'Promedio Respuestas'" && ordenamiento != "'Porcentaje satisfacción'" && ordenamiento != "'Cantidad respuestas'")
          consulta += " ASC";
        else
          consulta += " DESC";
      }
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      if (existeFiltroGenero)
        comandoParaConsulta.Parameters.AddWithValue("@genero", genero);
      if (existeFiltroNivelEducativo)
        comandoParaConsulta.Parameters.AddWithValue("@nivelEducativo", nivelEducativo);
      return comandoParaConsulta;
    }

    public List<ReporteSatisfaccionModel> ObtenerConjuntoDeReportes(DataTable tablaConsultada, String genero = "", String nivelEducativo = "") {
      List<ReporteSatisfaccionModel> reportesPreguntas = new List<ReporteSatisfaccionModel>();
      foreach (DataRow filaResultados in tablaConsultada.Rows) {
        ReporteSatisfaccionModel reporte = new ReporteSatisfaccionModel() {
          Pregunta = filaResultados["pregunta"].ToString(),
          PromedioRespuestas = Convert.ToDouble(filaResultados["Promedio Respuestas"]),
          Categoria = filaResultados["categoria"].ToString(),
          PorcentajeSatisfaccion = Convert.ToDouble(filaResultados["Porcentaje satisfacción"]),
          CantidadRespuestas = Convert.ToInt32(filaResultados["Cantidad respuestas"])
        };
        if (genero == "todos") {
          reporte.Genero = filaResultados["genero"].ToString();
        } else {
          reporte.Genero = genero;
        }
        if (nivelEducativo == "todos") {
          reporte.NivelEducativo = filaResultados["nivelEducativo"].ToString();
        } else {
          reporte.NivelEducativo = nivelEducativo;
        }
        reportesPreguntas.Add(reporte);
      }
      return reportesPreguntas;
    }

    public List<SelectListItem> ObtenerCategorias() {
      String consulta = "SELECT DISTINCT categoria FROM PreguntaSatisfaccion";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      List<SelectListItem> informacionCategorias = new List<SelectListItem>();
      foreach (DataRow filaInformacion in tablaResultado.Rows) {
        String categoria = Convert.ToString(filaInformacion["categoria"]);
        informacionCategorias.Add(new SelectListItem { Text = categoria, Value = categoria });
      }
      return informacionCategorias;
    }
  }
}