using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Planetario.Models;
using System.Globalization;



namespace Planetario.Handlers {
  public class MaterialHandler : BaseDeDatosHandler{

    readonly String Directorio;
    public MaterialHandler() {
      Directorio = AppContext.BaseDirectory + "Materiales\\";
    }

    public void AgregarMaterial(MaterialModel material) {
      String consulta = "INSERT INTO Material VALUES (@nombreArchivoPK,@fechaDePublicacion,@nombreDeMaterial" +
      ",@numeroDeFuncionario)";
      try {
        CrearMaterialEnBD(consulta, material);
        AgregarTopicos(material);
        RelacionarMaterialAActividad(material);
        GuardarMaterial(material);
      }
      catch (Exception) {
        if (ConexionPlanetario.State != ConnectionState.Open) {
          ConexionPlanetario.Close();
        }
        throw;
      }
    }

    public void CrearMaterialEnBD(String consulta, MaterialModel material) {
      ConexionPlanetario.Open();
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@nombreDeMaterial", material.Titulo);
      comandoParaConsulta.Parameters.AddWithValue("@numeroDeFuncionario",1234);
      comandoParaConsulta.Parameters.AddWithValue("@fechaDePublicacion", material.FechaDePublicacion);
      comandoParaConsulta.Parameters.AddWithValue("@nombreArchivoPK", material.Archivo.FileName);
      int exitoCambiarFilas = comandoParaConsulta.ExecuteNonQuery();
      ComprobarCambiosEnTabla(exitoCambiarFilas);
      ConexionPlanetario.Close();
    }
     
    public void AgregarTopicos(MaterialModel material) {
      String consulta = "INSERT INTO MaterialPerteneceA " + 
        "VALUES (@nombreArchivo,@nombreTopico)";
      ConexionPlanetario.Open();
      foreach (String topico in material.Topicos) {
        SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
        comandoParaConsulta.Parameters.AddWithValue("@nombreArchivo", material.Archivo.FileName);
        comandoParaConsulta.Parameters.AddWithValue("@nombreTopico", topico);
        int exitoCambiarFilas = comandoParaConsulta.ExecuteNonQuery();
        ComprobarCambiosEnTabla(exitoCambiarFilas);
      }
      ConexionPlanetario.Close();
    }

    public void RelacionarMaterialAActividad(MaterialModel material) {
      String consulta = "INSERT INTO TieneMaterial " +
        "VALUES (@nombreArchivo, @tituloActividad, @fechaActividad)";
      ConexionPlanetario.Open();
      String[] datosActividad = material.ActividadCodificada.Split(new[] { ' ' }, 2);
      DateTime fechaParseada = DateTime.ParseExact(datosActividad[0], "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@nombreArchivo", material.Archivo.FileName);
      comandoParaConsulta.Parameters.AddWithValue("@tituloActividad", datosActividad[1]);
      comandoParaConsulta.Parameters.AddWithValue("@fechaActividad", fechaParseada);
      int exitoCambiarFilas = comandoParaConsulta.ExecuteNonQuery();
      ComprobarCambiosEnTabla(exitoCambiarFilas);
      ConexionPlanetario.Close();
    }

    public void GuardarMaterial(MaterialModel material) {
      String nombreArchivo = material.Archivo.FileName;
      material.Archivo.SaveAs(Directorio +"\\"+nombreArchivo);
    }

    public List<MaterialModel> ObtenerMaterialesDeActividad(ActividadModel actividad) {
      String consulta = "SELECT M.nombreDeMaterial, M.nombreArchivoPK, M.fechaDePublicacion, A.tituloPK , A.fechaPK" +
        " FROM Material M JOIN TieneMaterial TM " +
        " ON M.nombreArchivoPK = TM.nombreDeArchivoFK JOIN Actividad A " +
        " ON A.tituloPK = TM.tituloActividadFK AND A.fechaPK=TM.fechaActividadFK " +
        " WHERE (A.tituloPK= @tituloActividad) AND (A.fechaPK = @fechaActividad) ";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@tituloActividad", actividad.Titulo);
      comandoParaConsulta.Parameters.AddWithValue("@fechaActividad", actividad.Fecha);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      List<MaterialModel> materiales = this.FormarListaDeMaterialesDeTabla(tablaResultado);
      return materiales;
    }


    public List<MaterialModel> ObtenerMaterialesPorPalabraClave(String palabraClave = null) {
      String consulta = "SELECT DISTINCT M.nombreDeMaterial, M.nombreArchivoPK, M.fechaDePublicacion, A.tituloPK, A.fechaPK " +
        "FROM Material M JOIN MaterialPerteneceA MP ON (M.nombreArchivoPK = MP.nombreDeArchivoFK)  " +
        "JOIN TieneMaterial TM ON TM.nombreDeArchivoFK=M.nombreArchivoPK " +
        "JOIN Actividad A ON A.tituloPK = TM.tituloActividadFK AND A.fechaPK=fechaActividadFK " +
        "AND A.fechaPK=TM.fechaActividadFK ";
      if (palabraClave != null) {
        consulta += " WHERE M.nombreDeMaterial LIKE '%" + palabraClave + "%'";
      }
      consulta += " ORDER BY A.tituloPK";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      List<MaterialModel> materiales = this.FormarListaDeMaterialesDeTabla(tablaResultado);
      return materiales;
    }

    public List<MaterialModel> FormarListaDeMaterialesDeTabla(DataTable tablaMateriales) {
      List<MaterialModel> materiales = new List<MaterialModel>();
      foreach (DataRow filaMaterial in tablaMateriales.Rows) {
        ActividadModel.IdentificadorActividad identificadorActividad = new ActividadModel.IdentificadorActividad {
          Nombre = Convert.ToString(filaMaterial["tituloPK"]),
          Fecha = Convert.ToDateTime(filaMaterial["fechaPK"])
        };
        MaterialModel material = new MaterialModel {
          Titulo = Convert.ToString(filaMaterial["nombreDeMaterial"]),
          Actividad = identificadorActividad,
          FechaDePublicacion = Convert.ToDateTime(filaMaterial["fechaDePublicacion"]),
          NombreArchivo = Convert.ToString(filaMaterial["nombreArchivoPK"])
        };
        this.LeerBytesMaterial(ref material);
        materiales.Add(material);
      }
      return materiales;
    }


    public void LeerBytesMaterial(ref MaterialModel material) {
      String pathMaterial = Directorio + "\\" + material.NombreArchivo;
      material.Archivo = new CargadorArchivoHandler(File.ReadAllBytes(pathMaterial));
    }

    public List<Tuple<String,String>> ObtenerMaterialesSimilares(String nombreArchivo) {
      String consulta = "SELECT DISTINCT TOP 3 APA.nombreDeArchivoFK, MA.nombreDeMaterial,(SELECT COUNT(*)  " +
        "FROM (SELECT AP.nombreDeTopicoFK FROM MaterialPerteneceA AP WHERE APA.nombreDeArchivoFK = AP.nombreDeArchivoFK " +
        "INTERSECT SELECT AP2.nombreDeTopicoFK FROM MaterialPerteneceA AP2 WHERE AP2.nombreDeArchivoFK = @nombreArchivo) " +
        "AS materialesSimilares) AS cantidadTopicos FROM MaterialPerteneceA APA JOIN Material MA ON MA.nombreArchivoPK = APA. nombreDeArchivoFK" +
        " WHERE APA.nombreDeArchivoFK != @nombreArchivo " +
        " ORDER BY cantidadTopicos DESC";

      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@nombreArchivo", nombreArchivo);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      List<Tuple<String,String>> materialesSimilares = FormarListaMaterialesRecomendados(tablaResultado);
      return materialesSimilares;

    }

    public List<Tuple<String,String>> FormarListaMaterialesRecomendados(DataTable tablaMateriales) {
      List<Tuple<String,String>> materiales = new List<Tuple<String,String>>();
      foreach (DataRow filaMaterial in tablaMateriales.Rows) {
        Tuple<String,String> material = new Tuple<String,String>(Convert.ToString(filaMaterial["nombreDeArchivoFK"]),Convert.ToString(filaMaterial["nombreDeMaterial"]));
        materiales.Add(material);
      }
      return materiales;
    }

    public List<List<Tuple<String,String>>> ObtenerMaterialSimilarDeCadaMaterial(List<MaterialModel> materiales) {
      List<List<Tuple<String,String>>> materialesRecomendados = new List<List<Tuple<String,String>>>();
      foreach (MaterialModel material in materiales) {
        materialesRecomendados.Add(ObtenerMaterialesSimilares(material.NombreArchivo));
      }
      return materialesRecomendados;
    }

  }
}