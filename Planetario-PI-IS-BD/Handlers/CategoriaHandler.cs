using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Planetario.Models;
using System.Web.Mvc;

namespace Planetario.Handlers {
  public class CategoriaHandler : BaseDeDatosHandler {

    public DataTable ObtenerTablaCategorias(List<string> topicos = null) {
      string consulta = "SELECT DISTINCT C.nombreCategoriaPK FROM CategoriaPregunta C " +
        "JOIN TopicoPregunta T ON T.categoriaFK = C.nombreCategoriaPK";
      if (topicos != null && topicos.Count() != 0) {
        consulta += " WHERE ";
        foreach (string topico in topicos) {
          if (topico != topicos.First()) {
            consulta += " OR ";
          }
          consulta += " (T.nombreTopicoPK = \'" + topico + "\') ";
        }
      }
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      return tablaResultado;
    }

    public List<string> ObtenerTopicosCategoria(string nombreCategoria) {
      string consulta = "SELECT nombreTopicoPK FROM TopicoPregunta" + " WHERE categoriaFK = @nombreCategoria";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@nombreCategoria", nombreCategoria);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      List<string> topicos = new List<string>();

      foreach (DataRow filaTopico in tablaResultado.Rows) {
        topicos.Add(Convert.ToString(filaTopico["nombreTopicoPK"]));
      }

      return topicos;
    }

    public CategoriaModel CrearCategoria(string nombreCategoria) {
      List<string> topicos = new List<string>();
      topicos = this.ObtenerTopicosCategoria(nombreCategoria);
      CategoriaModel categoria = new CategoriaModel() {
        Nombre = nombreCategoria,
        Topicos = topicos
      };

      return categoria;
    }

    public List<CategoriaModel> ObtenerListaCategorias(List<string> topicos = null) {
      if (topicos == null) {
        topicos = new List<String>();
      }
      List<CategoriaModel> categorias = new List<CategoriaModel>();
      DataTable tablaResultado = this.ObtenerTablaCategorias(topicos);

      foreach (DataRow filaCategoria in tablaResultado.Rows) {
        CategoriaModel categoria = CrearCategoria(Convert.ToString(filaCategoria["nombreCategoriaPK"]));
        categorias.Add(categoria);
      }

      return categorias;
    }

    public List<String> ObtenerNombreCategorias() {

      List<String> categorias = new List<String>();
      DataTable tablaResultado = this.ObtenerTablaCategorias();

      foreach (DataRow filaCategoria in tablaResultado.Rows) {
        String categoria = Convert.ToString(filaCategoria["nombreCategoriaPK"]);
        categorias.Add(categoria);
      }

      return categorias;
    }

    public List<Tuple<CategoriaModel, List<SelectListItem>>> ObtenerListaCategoriasView()
    {
      List<CategoriaModel> listaCategorias = this.ObtenerListaCategorias();
      List<Tuple<CategoriaModel, List<SelectListItem>>> listaCategoriasView = new List<Tuple<CategoriaModel, List<SelectListItem>>>();
      foreach (CategoriaModel categoria in listaCategorias) {
        List<SelectListItem> topicosView = TransformarListaView(categoria.Topicos);
        listaCategoriasView.Add(new Tuple<CategoriaModel, List<SelectListItem>>(categoria, topicosView));
      }
      return listaCategoriasView;
    }



    public List<SelectListItem> ObtenerCategorias() {

      List<String> categorias = ObtenerNombreCategorias();
      List<SelectListItem> categoriasParseadas = new List<SelectListItem>();
      foreach (string categoria in categorias) {
        categoriasParseadas.Add(new SelectListItem { Text = categoria, Value = categoria });
      }
      return categoriasParseadas;
    }
  }
}