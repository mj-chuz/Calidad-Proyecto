using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Planetario.Models;

namespace Planetario.Handlers {
  public class PreguntaHandler : BaseDeDatosHandler{

    public List<string> ObtenerListaTopicosPregunta(string pregunta) {
      List<string> topicos = new List<string>();
      DataTable tablaDeTopicos = ObtenerTablaTopicosPregunta(pregunta);

      foreach (DataRow filaPregunta in tablaDeTopicos.Rows) {
        string topico = Convert.ToString(filaPregunta["nombreTopicoFK"]);
        topicos.Add(topico);
      }

      return topicos;
    }

    public List<CategoriaModel> ObtenerCategoriasYTopicos(List<string> topicosBuscados = null) {
      if (topicosBuscados == null) {
        topicosBuscados = new List<string>();
      }
      CategoriaHandler accesoADatosCategoria = new CategoriaHandler();

      return accesoADatosCategoria.ObtenerListaCategorias(topicosBuscados);
    }

    public DataTable ObtenerTablaTopicosPregunta(string pregunta) { 
      string consulta = " SELECT nombreTopicoFK "+
      " FROM PreguntaPerteneceA "+
      " WHERE preguntaRealizadaFK = \'" +pregunta+"\'";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      return tablaResultado;
    }

    public List<PreguntaModel> ObtenerListaPreguntas(List<string> topicos = null) {
      if (topicos == null)
        topicos = new List<String>();
      List<PreguntaModel> preguntas = new List<PreguntaModel>();
      DataTable tablaPreguntas = ObtenerTablaPreguntas(topicos); 

      foreach (DataRow filaPregunta in tablaPreguntas.Rows) {
        string pregunta = Convert.ToString(filaPregunta["preguntaRealizadaPK"]);
        string respuesta = Convert.ToString(filaPregunta["respuesta"]);
        string categoria = Convert.ToString(filaPregunta["categoriaFK"]);
        List<string> topicosPregunta = ObtenerListaTopicosPregunta(pregunta);
        PreguntaModel preguntaConsulta = new PreguntaModel() {
          PreguntaHecha = pregunta,
          Respuesta = respuesta,
          Topicos = topicosPregunta,
          Categoria = categoria
        };
        preguntas.Add(preguntaConsulta);
      }

      return preguntas;
    }

    public DataTable ObtenerTablaPreguntas(List<string> topicos) {
      string consulta = "SELECT DISTINCT P.preguntaRealizadaPK, P.respuesta, TP.categoriaFK " +
      " FROM Pregunta P JOIN  PreguntaPerteneceA T " +
      " ON P.PreguntaRealizadaPK = T.PreguntaRealizadaFK " +
      " JOIN TopicoPregunta TP " +
      " ON T.nombreTopicoFK = TP.nombreTopicoPK ";
      if ( topicos.Count() != 0 ) {
        consulta += " WHERE ";
        foreach (string topico in topicos) {
          if (topico != topicos.First()) {
            consulta += " OR ";
          }
          consulta += " (TP.nombreTopicoPK = \'" + topico + "\') ";
        }
      }
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      return tablaResultado;
    }

    public bool AgregarTopicos(PreguntaModel pregunta) {
      string consulta = "INSERT INTO PreguntaPerteneceA(preguntaRealizadaFK, nombreTopicoFK)" + "VALUES (@pregunta, @topico)";
      bool exito = true;
      ConexionPlanetario.Open();

      foreach (string topico in pregunta.Topicos) {
        SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
        SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
        comandoParaConsulta.Parameters.AddWithValue("@pregunta", pregunta.PreguntaHecha);
        comandoParaConsulta.Parameters.AddWithValue("@topico", topico);
        exito = comandoParaConsulta.ExecuteNonQuery() >= 1;

        if (exito == false) {
          ConexionPlanetario.Close();
          return false;
        }
      }
      ConexionPlanetario.Close();

      return exito;
    }

    public bool AgregarPreguntaABase(PreguntaModel pregunta) {
      string consulta = "INSERT INTO Pregunta(preguntaRealizadaPK, respuesta)" + "VALUES (@preguntaRealizadaPK, @respuesta)";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);

      comandoParaConsulta.Parameters.AddWithValue("@preguntaRealizadaPK", pregunta.PreguntaHecha);
      comandoParaConsulta.Parameters.AddWithValue("@respuesta", pregunta.Respuesta);

      ConexionPlanetario.Open();
      bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
      ConexionPlanetario.Close();
      exito = exito && this.AgregarTopicos(pregunta);

      return exito;
    }

    public List<string> ObtenerTodosLosTopicos() {
  
      string consulta = "SELECT nombreTopicoPK FROM TopicoPregunta";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      List<string> listaTopicos = new List<string>();

      foreach (DataRow fila in tablaResultado.Rows) {
        listaTopicos.Add(Convert.ToString(fila["nombreTopicoPK"]));
      }
      return listaTopicos;
    }
  }
}