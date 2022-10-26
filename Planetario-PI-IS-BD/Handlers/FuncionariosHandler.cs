using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Planetario.Models;
using Planetario.Controllers;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace Planetario.Handlers {
  public class FuncionariosHandler : BaseDeDatosHandler {
    private readonly ImagenController ControladorDeImagenes;
    public FuncionariosHandler() {
      ControladorDeImagenes = new ImagenController();
    }

    public bool CrearFuncionario(FuncionarioModel funcionario) {
      ConsultarSQLFoto(funcionario);
      bool exitoAlCrear = ConsultarSQLFuncionario(funcionario);
      if (exitoAlCrear) {
        exitoAlCrear = AgregarIdiomasFuncionario(funcionario);
      }
      return exitoAlCrear;
    }

    public FuncionarioModel ObtenerFuncionario(DataRow filaFuncionario) {
      FuncionarioModel funcionario = new FuncionarioModel {
        NumeroIdentificacion = Convert.ToString(filaFuncionario["numeroDeIdentificacionPK"]),
        Nombre = Convert.ToString(filaFuncionario["nombre"]),
        PrimerApellido = Convert.ToString(filaFuncionario["primerApellido"]),
        SegundoApellido = Convert.ToString(filaFuncionario["segundoApellido"]),
        Correo = Convert.ToString(filaFuncionario["correo"]),
        Telefono = Convert.ToString(filaFuncionario["telefono"]),
        Descripcion = Convert.ToString(filaFuncionario["descripcion"]),
        Pais = Convert.ToString(filaFuncionario["pais"]),
        TituloAcademico = Convert.ToString(filaFuncionario["tituloAcademico"]),
        Ocupacion = Convert.ToString(filaFuncionario["ocupacion"]),
        Genero = Convert.ToString(filaFuncionario["genero"]),
        FechaDeNacimiento = Convert.ToDateTime(filaFuncionario["fechaDeNacimiento"]),
        IdentificadorFoto = Convert.ToString(filaFuncionario["fotoFK"]),
        Rol = Convert.ToString(filaFuncionario["rol"]),
        Contrasena = Convert.ToString(filaFuncionario["contrasena"])
      };
      this.ObtenerIdiomaFuncionario(ref funcionario);
      return funcionario;
    }

    public List<FuncionarioModel> ObtenerListaFuncionarios() {
      string consulta = "SELECT * FROM Funcionario";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      List<FuncionarioModel> listaFuncionarios = new List<FuncionarioModel>();
      foreach (DataRow filaFuncionario in tablaResultado.Rows) {
        listaFuncionarios.Add(ObtenerFuncionario(filaFuncionario));
      }
      return listaFuncionarios;
    }

    public string ObtenerExtensionFoto(FuncionarioModel funcionario) {
      string[] partesDeFoto = funcionario.archivoFoto.ContentType.Split('/');
      string extensionDeFoto = "." + partesDeFoto[1];
      return extensionDeFoto;
    }


    public bool ModificarFuncionario(FuncionarioModel funcionario) {
      string consultaFoto = "UPDATE Imagen SET archivo=@archivo, extension=@extension";
      consultaFoto+=" WHERE nombrePK=@nombrePK";
      InsertarFotoFuncionario(consultaFoto, funcionario);
      string consulta = "UPDATE Funcionario SET pais=@pais, descripcion=@descripcion, telefono=@telefono, " +
        " tituloAcademico=@tituloAcademico, nombre=@nombre, primerApellido=@primerApellido, segundoApellido=@segundoApellido, "+
        " fechaDeNacimiento=@fechaDeNacimiento, correo=@correo, ocupacion=@ocupacion, genero=@genero, fotoFK=@fotoFK WHERE "+
        " numeroDeIdentificacionPK=@numeroDeIdentificacionPK, rol=@rol, contrasena=@contrasena";
      bool exitoInsertarDatos = InsertarDatosFuncionario(consulta, funcionario);
      return exitoInsertarDatos;
    }
    public bool ConsultarSQLFoto(FuncionarioModel funcionario) {
      string consultaFoto = " INSERT INTO Imagen (nombrePK, extension, archivo) "
  + " VALUES (@nombrePK, @extension, @archivo) ";
      bool exitoInsertarFoto = InsertarFotoFuncionario(consultaFoto, funcionario);
      return exitoInsertarFoto;
    }

    public bool ConsultarSQLFuncionario(FuncionarioModel funcionario) {
      string consulta = "INSERT INTO Funcionario (numeroDeIdentificacionPK, pais, descripcion , telefono , " +
          " tituloAcademico, nombre, primerApellido, segundoApellido, " +
          " fechaDeNacimiento, correo, ocupacion, genero, fotoFK, rol, contrasena ) " +
          "VALUES (@numeroDeIdentificacionPK, @pais, @descripcion, @telefono, @tituloAcademico, @nombre" +
          ", @primerApellido, @segundoApellido, @fechaDeNacimiento, @correo, @ocupacion, @genero, @fotoFK, @rol, @contrasena) ";
      bool exitoInsertarFuncionario = InsertarDatosFuncionario(consulta, funcionario);
      return exitoInsertarFuncionario;
    }

    public bool InsertarFotoFuncionario(string consulta, FuncionarioModel funcionario) {
      string nombreFoto = "foto" + funcionario.NumeroIdentificacion;
      SqlCommand comandoParaConsultaFoto = new SqlCommand(consulta, ConexionPlanetario);
      string[] partesDeFoto = funcionario.archivoFoto.ContentType.Split('/');
      string extensionDeFoto = "." + partesDeFoto[1];
      comandoParaConsultaFoto.Parameters.AddWithValue("@archivo", ControladorDeImagenes.ObtenerBytes(funcionario.archivoFoto));
      comandoParaConsultaFoto.Parameters.AddWithValue("@extension", extensionDeFoto);
      comandoParaConsultaFoto.Parameters.AddWithValue("@nombrePK", nombreFoto);
      ConexionPlanetario.Open();
      bool exitoInsertarFotoFuncionario = comandoParaConsultaFoto.ExecuteNonQuery() >= 1;
      ConexionPlanetario.Close();
      return exitoInsertarFotoFuncionario;
    }

    public bool InsertarDatosFuncionario(string consulta, FuncionarioModel funcionario) {
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      string[] separadorFecha = Convert.ToString(funcionario.FechaDeNacimiento).Split(' ');
      string fechaReal = separadorFecha[0];
      string nombreFoto = "foto" + funcionario.NumeroIdentificacion;
      comandoParaConsulta.Parameters.AddWithValue("@numeroDeIdentificacionPK", funcionario.NumeroIdentificacion);
      comandoParaConsulta.Parameters.AddWithValue("@pais", funcionario.Pais);
      comandoParaConsulta.Parameters.AddWithValue("@descripcion", funcionario.Descripcion);
      comandoParaConsulta.Parameters.AddWithValue("@telefono", funcionario.Telefono);
      comandoParaConsulta.Parameters.AddWithValue("@tituloAcademico", funcionario.TituloAcademico);
      comandoParaConsulta.Parameters.AddWithValue("@nombre", funcionario.Nombre);
      comandoParaConsulta.Parameters.AddWithValue("@primerApellido", funcionario.PrimerApellido);
      comandoParaConsulta.Parameters.AddWithValue("@segundoApellido", funcionario.SegundoApellido);
      comandoParaConsulta.Parameters.AddWithValue("@fechaDeNacimiento", fechaReal);
      comandoParaConsulta.Parameters.AddWithValue("@correo", funcionario.Correo);
      comandoParaConsulta.Parameters.AddWithValue("@ocupacion", funcionario.Ocupacion);
      comandoParaConsulta.Parameters.AddWithValue("@genero", funcionario.Genero);
      comandoParaConsulta.Parameters.AddWithValue("@fotoFK", nombreFoto);
      comandoParaConsulta.Parameters.AddWithValue("@contrasena", funcionario.Contrasena);
      comandoParaConsulta.Parameters.AddWithValue("@rol", funcionario.Rol);
      ConexionPlanetario.Open();
      bool exitoInsertarDatosFuncionario = comandoParaConsulta.ExecuteNonQuery() >= 1;
      ConexionPlanetario.Close();
      return exitoInsertarDatosFuncionario;
    }

    public List<String> ConvertirHileraTopicosALista(String topicosSeleccionados) {
      topicosSeleccionados = Regex.Replace(topicosSeleccionados, "-", " ");
      String[] topicosSeparados = topicosSeleccionados.Split(';');
      List<String> topicos = new List<String>();
      foreach (String topico in topicosSeparados) {
        topicos.Add(topico);
      }
      topicos.Remove("");
      return topicos;
    }

    public bool AgregarIdiomasFuncionario(FuncionarioModel funcionario) {
      ConexionPlanetario.Open();
      bool exitoInsertarIdioma = true;
      foreach (String idioma in funcionario.Idiomas) {
        String consulta = "INSERT INTO IdiomasFuncionario(numeroDeIdentificacionFK,idioma) VALUES(@id,@idioma)";
        SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
        comandoParaConsulta.Parameters.AddWithValue("@id", funcionario.NumeroIdentificacion);
        comandoParaConsulta.Parameters.AddWithValue("@idioma", idioma);
        exitoInsertarIdioma = comandoParaConsulta.ExecuteNonQuery() >= 1;
        if (!exitoInsertarIdioma) {
          ConexionPlanetario.Close();
          return exitoInsertarIdioma;
        }
      }
      ConexionPlanetario.Close();
      return exitoInsertarIdioma;
    }

    public void ObtenerIdiomaFuncionario(ref FuncionarioModel funcionario) {
      String consulta = "SELECT idioma FROM IdiomasFuncionario WHERE numeroDeIdentificacionFK=@id";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@id", funcionario.NumeroIdentificacion);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      funcionario.Idiomas = new List<string>();
      foreach (DataRow filaIdioma in tablaResultado.Rows) {
        funcionario.Idiomas.Add(Convert.ToString(filaIdioma["idioma"]));
      }
    }

    public String GenerarPrimeraContrasena(FuncionarioModel funcionario) {
      string idFuncionario = funcionario.NumeroIdentificacion.ToString();
      String constrasena = idFuncionario.Substring(0, Math.Min(idFuncionario.Length,3)) + funcionario.Nombre.Substring(0, Math.Min(funcionario.Nombre.Length,3));
      return constrasena;
    }

    public string ObtenerHash(HashAlgorithm algoritmoDeHash, string entrada) {

      byte[] datos = algoritmoDeHash.ComputeHash(Encoding.UTF8.GetBytes(entrada));

      var constructorString = new StringBuilder();

      for (int i = 0; i < datos.Length; i++) {
        constructorString.Append(datos[i].ToString("x2"));
      }

      return constructorString.ToString();
    }

    public String ObtenerPaises() {
      String direccionArchivo = AppContext.BaseDirectory + "/JSON/paises.json";
      String datoPaises = System.IO.File.ReadAllText(direccionArchivo);
      return datoPaises;
    }

    public String ObtenerIdiomas() {
      String direccionArchivo = AppContext.BaseDirectory + "/JSON/lenguajes.json";
      String datoIdiomas = System.IO.File.ReadAllText(direccionArchivo);
      return datoIdiomas;
    }
  }
}