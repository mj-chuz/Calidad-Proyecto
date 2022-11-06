using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Planetario.Models;
using System.Web.Mvc;
using System.Text.RegularExpressions;


namespace Planetario.Handlers {
  public class ActividadHandler : BaseDeDatosHandler{

    public bool CrearActividad(ActividadModel actividad) {
      bool comprobarAgregarElemento;
      String consulta = "INSERT INTO Actividad " + "(tituloPK,fechaPK,descripcion,precioSugerido,duracion,estado,correo,nivelDeComplejidad,tipoDeActividad,modalidad,numeroDeIdentificacionFuncionarioFK, enlaceStream,cupos)" +
         " VALUES (@titulo,@fecha,@descripcion,@precioSugerido,@duracion, " +
         "@estado,@correo,@nivelDeComplejidad,@tipoActividad,@modalidad,@numeroDeIdentificacionFuncionarioFK, @enlaceStream,@cupos)";
      comprobarAgregarElemento = AgregarActividad(consulta, actividad);
      if (!comprobarAgregarElemento) {
        return comprobarAgregarElemento;
      }
      comprobarAgregarElemento = AgregarPublicoMeta(actividad);
      if (!comprobarAgregarElemento) {
        return comprobarAgregarElemento;
      }
      comprobarAgregarElemento = AgregarTopicosDeActividad(actividad);
      return comprobarAgregarElemento;
    }

    public bool AgregarActividad(String consulta, ActividadModel actividad) {
      bool exito;
      String enlace = "";
      ConexionPlanetario.Open();
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@titulo", actividad.Titulo);
      comandoParaConsulta.Parameters.AddWithValue("@fecha", actividad.Fecha);
      comandoParaConsulta.Parameters.AddWithValue("@descripcion", actividad.Descripcion);
      comandoParaConsulta.Parameters.AddWithValue("@precioSugerido", actividad.PrecioSugerido);
      comandoParaConsulta.Parameters.AddWithValue("@duracion", actividad.Duracion);
      comandoParaConsulta.Parameters.AddWithValue("@tipoActividad", actividad.TipoDeActividad);
      comandoParaConsulta.Parameters.AddWithValue("@nivelDeComplejidad", actividad.NivelDeComplejidad);
      comandoParaConsulta.Parameters.AddWithValue("@correo", actividad.Correo);
      comandoParaConsulta.Parameters.AddWithValue("@estado", actividad.Estado);
      comandoParaConsulta.Parameters.AddWithValue("@modalidad", actividad.Modalidad);
      comandoParaConsulta.Parameters.AddWithValue("@numeroDeIdentificacionFuncionarioFK", actividad.NumeroIdentificacionFuncionario);
      comandoParaConsulta.Parameters.AddWithValue("@cupos", actividad.CuposDisponibles);
      if (actividad.EnlaceStream != null) {
        enlace = actividad.EnlaceStream;
      }
      comandoParaConsulta.Parameters.AddWithValue("@enlaceStream", enlace);
      exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
      ConexionPlanetario.Close();
      return exito;
    }

    public bool AgregarPublicoMeta(ActividadModel actividad) {
      String consulta = "INSERT INTO PublicoMeta VALUES (@tituloFK,@fechaFK,@publicoMeta)";
      bool exito = true;
      ConexionPlanetario.Open();
      foreach (String publicoMeta in actividad.PublicoMeta) {
        SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
        comandoParaConsulta.Parameters.AddWithValue("@tituloFK", actividad.Titulo);
        comandoParaConsulta.Parameters.AddWithValue("@fechaFK", actividad.Fecha);
        comandoParaConsulta.Parameters.AddWithValue("@publicoMeta", publicoMeta);
        exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
        if (!exito) {
          ConexionPlanetario.Close();
          return exito;
        }
      }
      ConexionPlanetario.Close();
      return exito;
    }

    public bool AgregarTopicosDeActividad(ActividadModel actividad) {
      String consulta = "INSERT INTO ActividadPerteneceA VALUES (@titulo,@fecha,@topico)";
      bool exito = true;
      ConexionPlanetario.Open();
      foreach (String topico in actividad.Topicos) {
        SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
        comandoParaConsulta.Parameters.AddWithValue("@titulo", actividad.Titulo);
        comandoParaConsulta.Parameters.AddWithValue("@fecha", actividad.Fecha);
        comandoParaConsulta.Parameters.AddWithValue("@topico", topico);
        exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
        if (!exito) {
          ConexionPlanetario.Close();
          return exito;
        }
      }
      ConexionPlanetario.Close();
      return exito;
    }

    public void CambiarEstadoActividad(string titulo, DateTime fecha, string estado) {
      ConexionPlanetario.Open();
      String consulta = "UPDATE Actividad " +
        "SET estado = @estado " +
        "WHERE tituloPK = @tituloActividad AND fechaPK = @fechaActividad";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@tituloActividad", titulo);
      comandoParaConsulta.Parameters.AddWithValue("@fechaActividad", fecha);
      comandoParaConsulta.Parameters.AddWithValue("@estado", estado);
      int cantidadDeFilasCambiadas = comandoParaConsulta.ExecuteNonQuery();
      ComprobarCambiosEnTabla(cantidadDeFilasCambiadas);
      ConexionPlanetario.Close();
    }


    private ActividadModel ObtenerActividad(DataRow filaActividad) {
      ActividadModel actividad = new ActividadModel {
        Fecha = Convert.ToDateTime(filaActividad["fechaPK"]),
        Titulo = Convert.ToString(filaActividad["tituloPK"]),
        Descripcion = Convert.ToString(filaActividad["descripcion"]),
        PrecioSugerido = Convert.ToInt32(filaActividad["precioSugerido"]),
        Duracion = Convert.ToInt32(filaActividad["duracion"]),
        NivelDeComplejidad = Convert.ToString(filaActividad["nivelDeComplejidad"]),
        TipoDeActividad = Convert.ToString(filaActividad["tipoDeActividad"]),
        Correo = Convert.ToString(filaActividad["correo"]),
        Estado = Convert.ToString(filaActividad["estado"]),
        Modalidad = Convert.ToString(filaActividad["modalidad"]),
        EnlaceStream = Convert.ToString(filaActividad["enlaceStream"]),
        CuposDisponibles = Convert.ToInt32(filaActividad["cupos"])
      };
      actividad.Topicos = ObtenerListaTopicosActividad(actividad);
      actividad.PublicoMeta = ObtenerListaPublicoMeta(actividad);
      return actividad;
    }
    
    public List<String> ObtenerListaTopicosActividad(ActividadModel actividad) {
      List<String> topicos = new List<String>();
      DataTable tablaDeActividades = ObtenerTablaTopicosActividad(actividad);
      actividad.Categoria = Convert.ToString(tablaDeActividades.Rows[0]["categoriaFK"]);
      foreach (DataRow filaActividad in tablaDeActividades.Rows) {
        String topico = Convert.ToString(filaActividad["nombreTopicoFK"]);
        topicos.Add(topico);
      }

      return topicos;
    }

    public DataTable ObtenerTablaTopicosActividad(ActividadModel actividad) {
      String consulta = " SELECT AP.nombreTopicoFK, T.categoriaFK " +
      " FROM ActividadPerteneceA AP " +
      " JOIN TopicoPregunta T ON T.nombreTopicoPK = AP.nombreTopicoFK " +
      " WHERE tituloActividadFK = @tituloActividad AND fechaActividadFK = @fechaActividad";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@tituloActividad", actividad.Titulo);
      comandoParaConsulta.Parameters.AddWithValue("@fechaActividad", actividad.Fecha);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      return tablaResultado;
    }

    public List<String> ObtenerListaPublicoMeta(ActividadModel actividad) {
      List<String> publicosMeta = new List<String>();
      DataTable tablaDeActividades = ObtenerTablaPublicoMeta(actividad);
      foreach (DataRow filaActividad in tablaDeActividades.Rows) {
        String publicoMeta = Convert.ToString(filaActividad["publicoMeta"]);
        publicosMeta.Add(publicoMeta);
      }
      return publicosMeta;
    }

    public DataTable ObtenerTablaPublicoMeta(ActividadModel actividad) {
      String consulta = "SELECT publicoMeta FROM PublicoMeta WHERE " +
        "(tituloActividadFK= @tituloActividadFK) AND (fechaActividadFK=@fechaActividadFK)";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@tituloActividadFK", actividad.Titulo);
      comandoParaConsulta.Parameters.AddWithValue("@fechaActividadFK", actividad.Fecha);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      return tablaResultado;
    }


    public ActividadModel ObtenerActividad(string titulo, DateTime fecha) {
      string consulta = "SELECT * FROM Actividad" + " WHERE tituloPK = @titulo AND fechaPK = @fecha";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@titulo", titulo);
      comandoParaConsulta.Parameters.AddWithValue("@fecha", fecha);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      if (tablaResultado.Rows.Count > 0) {

        return this.ObtenerActividad(tablaResultado.Rows[0]);
      }
      return null;
    }
      private List<ActividadModel> ObtenerListaDeActividadesPorEstadoPorTipo(String estado, String tipoActividad = "", String palabraClave = null) {
      DataTable tablaResultado = new DataTable();
      List<ActividadModel> listaActividades = new List<ActividadModel>();
      String consulta = "SELECT * FROM Actividad WHERE estado = @estado";
      if (tipoActividad != "") {
        consulta += " AND tipoDeActividad = @tipoActividad ";
      }
      if (palabraClave != null) {
        consulta += " AND (tituloPK LIKE '%" + palabraClave + "%' OR descripcion LIKE '%" + palabraClave + "%')";
      }
      consulta += " ORDER BY fechaPK DESC";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@estado", estado);
      if (tipoActividad != "") {
        comandoParaConsulta.Parameters.AddWithValue("@tipoActividad",tipoActividad);
      }
      tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      foreach (DataRow filaFuncionario in tablaResultado.Rows) {
        ActividadModel actividad = ObtenerActividad(filaFuncionario);
        actividad.Categoria = ObtenerCategoriaActividad(actividad.Titulo, actividad.Fecha);
        actividad.Topicos = ObtenerListaTopicosActividad(actividad);
         listaActividades.Add(actividad);
      }
      return listaActividades;
    }

    private String ObtenerCategoriaActividad(String tituloActividad, DateTime fechaActividad) {
      String consulta = "SELECT categoriaFK FROM vw_ActividadCategoria " +
        "WHERE tituloActividadFK = @tituloActividad AND fechaActividadFK = @fechaActividad";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@tituloActividad", tituloActividad);
      comandoParaConsulta.Parameters.AddWithValue("@fechaActividad", fechaActividad);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      String categoria = Convert.ToString(tablaResultado.Rows[0]["categoriaFK"]);
      return categoria;
    }

    public List<ActividadModel> ObtenerListaDeActividadesPorEstado(String estado) {
      return this.ObtenerListaDeActividadesPorEstadoPorTipo(estado);
    }

    public List<ActividadModel> ObtenerActividadesPendientesDeAprobacion() {
      return ObtenerListaDeActividadesPorEstado(ActividadModel.PENDIENTE);
    }
    public List<ActividadModel> ObtenerActividadesAprobadas() {
      return ObtenerListaDeActividadesPorEstado(ActividadModel.APROBADA);
    }
    public List<SelectListItem> ObtenerListaActividadesView() {
      String consulta = "SELECT tituloPK,fechaPK  FROM Actividad";
      List<SelectListItem> actividades = new List<SelectListItem>();

      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable tablaConsultada = CrearTablaConsulta(comandoParaConsulta);
      foreach (DataRow filaActividad in tablaConsultada.Rows) {
        ActividadModel.IdentificadorActividad actividad = new ActividadModel.IdentificadorActividad {
          Nombre = Convert.ToString(filaActividad["tituloPK"]),
          Fecha = Convert.ToDateTime(filaActividad["fechaPK"])
        };
        actividades.Add(new SelectListItem() {Text=actividad.Fecha.ToString() + " " + actividad.Nombre,Value=actividad.ToString() });
       }

      return actividades;
    }


    public List<ActividadModel> ObtenerCharlasAprobadas(String palabraClave = null) {
      return ObtenerListaDeActividadesPorEstadoPorTipo(ActividadModel.APROBADA, ActividadModel.CHARLA, palabraClave);
    }

    public List<ActividadModel> ObtenerTalleresAprobados(String palabraClave = null) {
      return ObtenerListaDeActividadesPorEstadoPorTipo(ActividadModel.APROBADA, ActividadModel.TALLER, palabraClave);
    }

    public List<String> ConvertirHileraTopicosALista(String topicosSeleccionados) {
      topicosSeleccionados = Regex.Replace(topicosSeleccionados,"-", " ");
      String[] topicosSeparados = topicosSeleccionados.Split(';');
      List<String> topicos = new List<String>();
      foreach(String topico in topicosSeparados) {
        topicos.Add(topico);
      }
      topicos.Remove("");
      return topicos;
    }

    public void EditarEnlace(String enlaceNuevo, String tituloActividad, DateTime fechaActividad) {
      ConexionPlanetario.Open();
      string consulta = "UPDATE Actividad " + "SET enlaceStream = @enlaceNuevo " + "WHERE tituloPK = @tituloActividad AND fechaPK = @fechaActividad";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@tituloActividad", tituloActividad);
      comandoParaConsulta.Parameters.AddWithValue("@fechaActividad", fechaActividad);
      comandoParaConsulta.Parameters.AddWithValue("@enlaceNuevo", enlaceNuevo);
      int cantidadDeFilasCambiadas = comandoParaConsulta.ExecuteNonQuery();
      ComprobarCambiosEnTabla(cantidadDeFilasCambiadas);
      ConexionPlanetario.Close();
    }

    public List<Tuple<String, DateTime>> ObtenerActividadesSimilares(String tituloActividad, DateTime fechaActividad) {
      String consulta = "SELECT DISTINCT TOP 3 APA.tituloActividadFK, APA.fechaActividadFK, " +
        "(SELECT COUNT(*) FROM (SELECT AP.nombreTopicoFK FROM ActividadPerteneceA AP WHERE " +
        "APA.tituloActividadFK = AP.tituloActividadFK AND APA.fechaActividadFK = AP.fechaActividadFK " +
        "INTERSECT SELECT AP2.nombreTopicoFK FROM ActividadPerteneceA AP2 WHERE AP2.tituloActividadFK = @tituloActividad " +
        "AND AP2.fechaActividadFK = @fechaActividad) AS actividadesSimilares) AS cantidadTopicos FROM ActividadPerteneceA APA " +
        "WHERE APA.tituloActividadFK != @tituloActividad AND APA.fechaActividadFK != @fechaActividad ORDER BY cantidadTopicos DESC";
      List<Tuple<String, DateTime>> listaActividades = new List<Tuple<String, DateTime>>();
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@tituloActividad", tituloActividad);
      comandoParaConsulta.Parameters.AddWithValue("@fechaActividad", fechaActividad);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      foreach (DataRow filaActividad in tablaResultado.Rows) {
        listaActividades.Add(new Tuple<string, DateTime>(Convert.ToString(filaActividad["tituloActividadFK"]), Convert.ToDateTime(filaActividad["fechaActividadFK"])));
      }
      return listaActividades;
    }

    public void ActualizarCuposDisponibles(ActividadModel actividad, int cupos) {
      String consulta = "UPDATE Actividad SET cupos = cupos- @cupos WHERE tituloPK=@tituloPK AND fechaPK=@fechaPK";
      ConexionPlanetario.Open();
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@tituloPK", actividad.Titulo);
      comandoParaConsulta.Parameters.AddWithValue("@fechaPK", actividad.Fecha);
      comandoParaConsulta.Parameters.AddWithValue("@cupos", cupos);
      int exitoCambiarFilas = comandoParaConsulta.ExecuteNonQuery();
      ComprobarCambiosEnTabla(exitoCambiarFilas);
      ConexionPlanetario.Close();
    }

  }
}