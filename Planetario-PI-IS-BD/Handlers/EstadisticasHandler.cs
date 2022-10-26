using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Planetario.Handlers {
  public class EstadisticasHandler : BaseDeDatosHandler {

    private DataTable RecuperarPublicoDeActividadesParaPublicoMetaYNivel(String datos) {
      List<String[]> datosDeFiltroProcesados = ProcesarDatosDeFiltroParaPublicoMetaYNivel(datos);
      List<bool> condicionesParaWhere = ObtenerCondicionesParaWhereParaPublicoMetaYNivel(datosDeFiltroProcesados);
      List<String> selectYGroupBy = ConstruirSelectYGroupByParaPublicoMetaYNivel();
      String consulta = "SET LANGUAGE Spanish;";
      consulta += selectYGroupBy[0];
      consulta += ArmarConsultaCompletaParaPublicoMetaYNivel(datosDeFiltroProcesados, condicionesParaWhere);
      consulta += selectYGroupBy[1];
      consulta += "ORDER BY 'Publico Meta', 'Nivel de complejidad',  DATEPART(WEEKDAY, A.fechaPK) ASC ";
      consulta += "SET LANGUAGE us_english;";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable resultado = CrearTablaConsulta(comandoParaConsulta);
      return resultado;
    }

    private List<String[]> ProcesarDatosDeFiltroParaPublicoMetaYNivel(String datos) {
      List<String[]> datosDeFiltroProcesados = new List<String[]>();
      String[] datosSeparadosPorFiltro = datos.Split(':');
      String[] datosDia = { "" };
      String[] datosPublicoMeta = { "" };
      String[] datosNivelComplejidad = { "" };
      if (datosSeparadosPorFiltro[0] != "") {
        datosDia = datosSeparadosPorFiltro[0].Split(',');
      }
      if (datosSeparadosPorFiltro.Length>1 && datosSeparadosPorFiltro[1] != "") {
        datosPublicoMeta = datosSeparadosPorFiltro[1].Split(',');
      }
      if (datosSeparadosPorFiltro.Length > 2 &&  datosSeparadosPorFiltro[2] != "") {
        datosNivelComplejidad = datosSeparadosPorFiltro[2].Split(',');
      }
      datosDeFiltroProcesados.Add(datosDia);
      datosDeFiltroProcesados.Add(datosPublicoMeta);
      datosDeFiltroProcesados.Add(datosNivelComplejidad);
      return datosDeFiltroProcesados;
    }

    private List<bool> ObtenerCondicionesParaWhereParaPublicoMetaYNivel(List<String[]> datosDeFiltroProcesados) {
      bool hayCondicionDia = datosDeFiltroProcesados[0][0] != "";
      bool hayCondicionPublicoMeta = datosDeFiltroProcesados[1][0] != "";
      bool hayCondicionNivelComplejidad = datosDeFiltroProcesados[2][0] != "";
      List<bool> condicionesParaWhere = new List<bool>();
      condicionesParaWhere.Add(hayCondicionDia);
      condicionesParaWhere.Add(hayCondicionPublicoMeta);
      condicionesParaWhere.Add(hayCondicionNivelComplejidad);
      return condicionesParaWhere;
    }

    private List<String> ConstruirSelectYGroupByParaPublicoMetaYNivel() {
      String groupBy = "GROUP BY DATENAME(WEEKDAY,A.fechaPK), PM.publicoMeta , A.nivelDeComplejidad,DATEPART(WEEKDAY, A.fechaPK)  ";
      String select = " SELECT DATENAME(WEEKDAY,A.fechaPK)  AS 'Dia de la semana',COUNT(*) AS 'Cantidad de personas',PM.publicoMeta AS 'Publico meta'" +
      ", A.nivelDeComplejidad AS 'Nivel de complejidad' ";
      List<String> selectYGroupBy = new List<String> {
        select,
        groupBy
      };
      return selectYGroupBy;
    }
    private String ArmarConsultaCompletaParaPublicoMetaYNivel(List<String[]> datosDeFiltroProcesados, List<bool> condicionesParaWhere) {
      String consulta = "FROM Actividad A JOIN PublicoMeta PM " +
      " ON A.tituloPK=PM.tituloActividadFK AND A.fechaPK=PM.fechaActividadFK " +
      " JOIN ReservaActividad I  " +
      " ON I.tituloActividadFK = A.tituloPK AND I.fechaActividadFK = A.fechaPK ";
      bool hayCondicionDia = condicionesParaWhere[0];
      bool hayCondicionPublicoMeta = condicionesParaWhere[1];
      bool hayCondicionNivelComplejidad = condicionesParaWhere[2];
      if (hayCondicionDia || hayCondicionPublicoMeta || hayCondicionNivelComplejidad) {
        consulta += "WHERE ";
        if (hayCondicionDia) {
          consulta += " ( ";
          foreach (String dia in datosDeFiltroProcesados[0]) {
            if (dia != datosDeFiltroProcesados[0][0]) {
              consulta += " OR ";
            }
            consulta += " (DATENAME(WEEKDAY,A.fechaPK) = \'" + dia + "\') ";
          }
          consulta += " ) ";
          if (hayCondicionPublicoMeta || hayCondicionNivelComplejidad) {
            consulta += " AND ";
          }
        }
        if (hayCondicionPublicoMeta) {
          consulta += " ( ";
          foreach (String publicoMeta in datosDeFiltroProcesados[1]) {
            if (publicoMeta != datosDeFiltroProcesados[1][0]) {
              consulta += " OR ";
            }
            consulta += " (PM.publicoMeta = \'" + publicoMeta + "\') ";
          }
          consulta += " ) ";
          if (hayCondicionNivelComplejidad) {
            consulta += " AND ";
          }
        }
        if (hayCondicionNivelComplejidad) {
          consulta += " ( ";
          foreach (String complejidad in datosDeFiltroProcesados[2]) {
            if (complejidad != datosDeFiltroProcesados[2][0]) {
              consulta += " OR ";
            }
            consulta += " (A.nivelDeComplejidad = \'" + complejidad + "\') ";
          }
          consulta += " ) ";
        }
      }
      return consulta;
    }

    public String ObtenerPublicoDeActividadesParaPublicoMetaYNivel(String datos) {
      if (datos != null && datos != "") return JsonConvert.SerializeObject(RecuperarPublicoDeActividadesParaPublicoMetaYNivel(datos));
      return "";
    }

    public DataTable ObtenerParticipacionParaCategorias(String filtroCategoria = "") {
      filtroCategoria = filtroCategoria.Replace('-', ' ');
      String[] categoriasFiltradas = filtroCategoria.Split(';');
      String consulta = ConstruirConsultaCompletaCategorias(categoriasFiltradas);
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable resultado = CrearTablaConsulta(comandoParaConsulta);
      return resultado;
    }

    private String ConstruirConsultaCompletaCategorias(String[] categoriasFiltradas) {
      bool hayFiltroCategorias = categoriasFiltradas[0] != "";
      String consulta = "SELECT AC.categoriaFK AS 'Nombre categoria', COUNT(*) AS 'Cantidad de personas'  " +
       " FROM (    SELECT DISTINCT AP.tituloActividadFK, AP.fechaActividadFK, T.categoriaFK " +
       " FROM ActividadPerteneceA AP " +
       " JOIN TopicoPregunta T ON AP.nombreTopicoFK = T.nombreTopicoPK ";
      if (hayFiltroCategorias) {
        consulta += " WHERE ";
        foreach (String categoria in categoriasFiltradas) {
          if (categoria != null && categoria != "") {
            if (categoria != categoriasFiltradas[0]) {
              consulta += " OR ";
            }
            consulta += " (T.categoriaFK = \'" + categoria + "\') ";
          }
        }
      }
      consulta += "  ) AC JOIN ReservaActividad I ON I.fechaActividadFK = AC.fechaActividadFK AND I.tituloActividadFK = I.tituloActividadFK  ";
      consulta += " GROUP BY AC.categoriaFK ORDER BY 'Cantidad de personas' DESC ";
      return consulta;
    }

    public DataTable ObtenerParticipacionParaTopicos(String filtroTopico = "") {
      filtroTopico = filtroTopico.Replace('-', ' ');
      String[] topicosFiltrados = filtroTopico.Split(';');
      String consulta = ConstruirConsultaCompletaTopicos(topicosFiltrados);
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable resultado = CrearTablaConsulta(comandoParaConsulta);
      return resultado;
    }

    private String ConstruirConsultaCompletaTopicos(String[] topicosFiltrados) {
      bool hayFiltroTopicos = topicosFiltrados[0] != "";
      String consulta = "SELECT T.nombreTopicoPK as 'Nombre topico', COUNT(*) as 'Cantidad de personas' " +
        "FROM TopicoPregunta T " +
        "JOIN ActividadPerteneceA AP ON T.nombreTopicoPK = AP.nombreTopicoFK " +
        "JOIN ReservaActividad I ON I.tituloActividadFK = AP.tituloActividadFK AND I.fechaActividadFK = AP.fechaActividadFK ";
      if (hayFiltroTopicos) {
        consulta += "WHERE ";
        foreach (String topico in topicosFiltrados) {
          if (topico != null && topico != "") {
            if (topico != topicosFiltrados[0]) {
              consulta += " OR ";
            }
            consulta += " (T.nombreTopicoPK = \'" + topico + "\') ";
          }
        }
      }
      consulta += " GROUP BY T.nombreTopicoPK ";
      consulta += " ORDER BY 'Cantidad de personas' DESC";
      return consulta;
    }

    public String ObtenerPublicoPorTopicos(String datos) {
      if (datos != null && datos != "") return JsonConvert.SerializeObject(ObtenerParticipacionParaTopicos(datos));
      return ObtenerTopTopicos(5);
    }

    public String ObtenerPublicoPorCategoria(String datos) {
      if (datos != null && datos != "") return JsonConvert.SerializeObject(ObtenerParticipacionParaCategorias(datos));
      return ObtenerTopCategorias(3);
    }

    public List<SelectListItem> ObtenerIdiomasHablados(){
      List<String> idiomasHablados = new List<String>();
      DataTable idiomasRecuperados = RecuperarIdiomasHablados();
      foreach(DataRow fila in idiomasRecuperados.Rows) {
        idiomasHablados.Add(Convert.ToString(fila["Idioma"]));
      }
      return TransformarListaView(idiomasHablados);
    }

    private DataTable RecuperarIdiomasHablados() {
      String consulta = "SELECT DISTINCT I.idioma as Idioma " +
                        "FROM IdiomasFuncionario I ";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable resultado = CrearTablaConsulta(comandoParaConsulta);
      return resultado;
    }

    public String ObtenerFuncionariosPorIdioma(String filtroIdioma) {
      if (filtroIdioma != null && filtroIdioma != "") return JsonConvert.SerializeObject(ObtenerFuncionariosSegunIdioma(filtroIdioma));
      return ObtenerTodosFuncionariosConIdioma();
    }

    private DataTable ObtenerFuncionariosSegunIdioma(String filtroIdioma) {
      String[] idiomasFiltrados = filtroIdioma.Split(';');
      String consulta = "DECLARE @idiomas ListaIdiomas;";
      if (idiomasFiltrados[0] != "") {
        consulta += "INSERT INTO @idiomas VALUES ";
        foreach (String idioma in idiomasFiltrados) {
          if (idioma != "") {
            if (idioma != idiomasFiltrados[0]) {
              consulta += " , ";
            }
            consulta += "(\'" + idioma + "\')";
          }
        }
        consulta += ";";
      }
      consulta += "EXEC ObtenerFuncionarioIdioma @idiomas;";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable funcionarios = CrearTablaConsulta(comandoParaConsulta);
      return AgregarIdiomasHabladosFuncionarios(funcionarios);
    }

    private DataTable AgregarIdiomasHabladosFuncionarios(DataTable funcionarios) {
      funcionarios.Columns.Add("idiomasHablados", typeof(String));
      Dictionary<int, String> idiomasHabladosPorFuncinario = ObtenerIdiomasHabladosPorFuncionarios(funcionarios);
      foreach (DataRow filaFuncionario in funcionarios.Rows) {
        filaFuncionario["idiomasHablados"] = idiomasHabladosPorFuncinario[Convert.ToInt32(filaFuncionario["numeroDeIdentificacionPK"])];
      }
      return funcionarios;
    }

    private Dictionary<int, String> ObtenerIdiomasHabladosPorFuncionarios(DataTable funcionarios) {
      Dictionary<int, String> idiomasHablados = new Dictionary<int, String>();
      foreach (DataRow filaFuncionario in funcionarios.Rows) {
        DataTable idiomasFuncionario = ObtenerIdiomasFuncionario(Convert.ToInt32(filaFuncionario["numeroDeIdentificacionPK"]));
        String idiomasHabladosFuncionario = "";
        foreach (DataRow filaIdioma in idiomasFuncionario.Rows) {
          if (filaIdioma != idiomasFuncionario.Rows[0]) {
            idiomasHabladosFuncionario += ",";
          }
          idiomasHabladosFuncionario += Convert.ToString(filaIdioma["idioma"]);
        }
        idiomasHablados[Convert.ToInt32(filaFuncionario["numeroDeIdentificacionPK"])] = idiomasHabladosFuncionario;
      }
      return idiomasHablados;
    }

    private DataTable ObtenerIdiomasFuncionario(int numeroIdentificacionFuncionario) {
      String consulta = "SELECT idioma FROM IdiomasFuncionario WHERE numeroDeIdentificacionFK = @numeroIdentificacion";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@numeroIdentificacion", numeroIdentificacionFuncionario);
      DataTable resultado = CrearTablaConsulta(comandoParaConsulta);
      return resultado;
    }

    public String ObtenerTodosFuncionariosConIdioma() {
      return JsonConvert.SerializeObject(ObtenerTodosFuncionariosSegunIdioma());
    }

    private DataTable ObtenerTodosFuncionariosSegunIdioma() {
      String consulta = "SELECT DISTINCT numeroDeIdentificacionPK, nombre, primerApellido, segundoApellido, correo, telefono FROM Funcionario";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable funcionarios = CrearTablaConsulta(comandoParaConsulta);
      return AgregarIdiomasHabladosFuncionarios(funcionarios);
    }

    public String ObtenerTopCategorias(int cantidad) {
      DataTable resultadoCompleto = ObtenerParticipacionParaCategorias();
      DataTable resultadoAcotado = resultadoCompleto.AsEnumerable().Take(cantidad)
                                                                 .CopyToDataTable();
      return JsonConvert.SerializeObject(resultadoAcotado);
    }

    public String ObtenerTopTopicos(int cantidad) {
      DataTable resultadoCompleto = ObtenerParticipacionParaTopicos();
      DataTable resultadoAcotado = resultadoCompleto.AsEnumerable().Take(cantidad)
                                                                 .CopyToDataTable();
      return JsonConvert.SerializeObject(resultadoAcotado);
    }

  }
}