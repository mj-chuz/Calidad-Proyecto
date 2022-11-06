using System;
using System.Collections.Generic;
using System.Linq;
using Planetario.Models;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Planetario.Handlers {
  public class NoticiasHandler {
    readonly string Directorio;

    public int CantidadTotalDePaginas { get; set; }
    public int NoticiasPorPagina { get; set; }

    public NoticiasHandler(string directorio) {
      this.Directorio = directorio;
      this.NoticiasPorPagina = 4;
      this.CantidadTotalDePaginas = CalcularTotalDePaginas(this.NoticiasPorPagina);
    }
    public NoticiasHandler() {
      Directorio = AppContext.BaseDirectory + "Noticias/";
      this.NoticiasPorPagina = 4;
      this.CantidadTotalDePaginas = CalcularTotalDePaginas(this.NoticiasPorPagina);
    }
    public NoticiaModel ObtenerNoticia(string nombreArchivo) {
      string contenidoArchivo = File.ReadAllText(Directorio + "/" + nombreArchivo);
      JObject noticiaJson = JObject.Parse(contenidoArchivo);
      NoticiaModel noticia = GuardarNoticia(noticiaJson, nombreArchivo);
      return noticia;
    }

    public NoticiaModel GuardarNoticia(JObject noticiaJson, string nombreArchivo) {
      NoticiaModel noticia = new NoticiaModel() {
        Titulo = noticiaJson["Titulo"].Value<string>(),
        Fecha = DateTime.ParseExact(noticiaJson["Fecha"].Value<string>(), "dd-MM-yyyy", null),
        Contenido = noticiaJson["Contenido"].Value<string>(),
        NombreImagen = noticiaJson["NombreImagen"].Value<string>(),
        NombreArchivo = nombreArchivo
      };
      return noticia;
    }

    FileInfo[] ObtenerArchivosNoticias() {
      DirectoryInfo metaDatosNoticias = new DirectoryInfo(this.Directorio);
      FileInfo[] archivoNoticia = metaDatosNoticias.GetFiles().OrderByDescending(archivo => archivo.Name).ToArray();
      return archivoNoticia;
    }

    public string GenerarNombreArchivo(NoticiaModel noticia) {
      string nombreArchivo = "";
      nombreArchivo += noticia.Fecha.ToString("yyyyMMdd");
      string tituloFormateado = noticia.Titulo;
      tituloFormateado = tituloFormateado.Substring(0, Math.Min(tituloFormateado.Length, 100));
      tituloFormateado = tituloFormateado.Replace(' ', '-');
      nombreArchivo += tituloFormateado;
      nombreArchivo += ".JSON";
      return nombreArchivo;
    }
    public bool GuardarNoticia(NoticiaModel noticia) {
      noticia.NombreArchivo = GenerarNombreArchivo(noticia);
      noticia.NombreImagen = noticia.ArchivoImagen.FileName;
      noticia.ArchivoImagen.SaveAs(Directorio + "/imagenes/" + noticia.NombreImagen);
      JObject archivoJSON = new JObject(
         new JProperty("Titulo", noticia.Titulo),
         new JProperty("Fecha", noticia.Fecha.ToString("dd-MM-yyyy")),
         new JProperty("Contenido", noticia.Contenido),
         new JProperty("NombreImagen", noticia.NombreImagen)
         );
      File.WriteAllText(Directorio + "/" + noticia.NombreArchivo, archivoJSON.ToString());
      bool exito = true;

      return exito;
    }

    public List<NoticiaModel> GetPaginaNoticias(int pagina = 1) {
      FileInfo[] archivosNoticias = this.ObtenerArchivosNoticias();
      int indiceNoticiaInicio = (pagina - 1) * this.NoticiasPorPagina;
      int indiceNoticiaFinal = indiceNoticiaInicio + this.NoticiasPorPagina;
      Tuple<int, int> indicesBusqueda = new Tuple<int, int>(indiceNoticiaInicio, indiceNoticiaFinal);
      return this.ObtenerListaNoticias(indicesBusqueda, archivosNoticias);
    }

    List<NoticiaModel> ObtenerListaNoticias(Tuple<int, int> indicesBusqueda, FileInfo[] archivosNoticias) {
      List<NoticiaModel> noticiasObtenidas = new List<NoticiaModel>();
      for (int indiceNoticia = indicesBusqueda.Item1; indiceNoticia < indicesBusqueda.Item2 && indiceNoticia < archivosNoticias.Length; ++indiceNoticia) {
        string direccionArchivo = archivosNoticias[indiceNoticia].DirectoryName + "/" + archivosNoticias[indiceNoticia].Name;
        string contenidoArchivo = File.ReadAllText(direccionArchivo);
        JObject json = JObject.Parse(contenidoArchivo);
        NoticiaModel noticia = GuardarNoticia(json, archivosNoticias[indiceNoticia].Name);
        noticiasObtenidas.Add(noticia);
      }
      return noticiasObtenidas;
    }

    public int CalcularTotalDePaginas(int noticiasPorPagina) {
      FileInfo[] archivosNoticias = this.ObtenerArchivosNoticias();
      int cantidadTotalDeNoticias = archivosNoticias.Length;
      int cantidadTotalDePaginas = cantidadTotalDeNoticias / noticiasPorPagina;
      if (cantidadTotalDeNoticias % noticiasPorPagina != 0) {
        cantidadTotalDePaginas += 1;
      }
      return cantidadTotalDePaginas;
    }

    public int ValidarNumeroDePagina(int numeroDePagina) {
      if (numeroDePagina < 1) {
        numeroDePagina = 1;
      } else if (numeroDePagina > this.CantidadTotalDePaginas) {
        numeroDePagina = this.CantidadTotalDePaginas;
      }
      return numeroDePagina;
    }

    public Tuple<int, int> CalcularLimitesValidacion(int paginaMostrar) {
      int numeroInicioPaginacion = paginaMostrar - 1;
      numeroInicioPaginacion = this.ValidarNumeroDePagina(numeroInicioPaginacion);
      int numeroFinalPaginacion = paginaMostrar + 1;
      if (numeroInicioPaginacion == 1) {
        numeroFinalPaginacion = numeroInicioPaginacion + 2;
        numeroFinalPaginacion = this.ValidarNumeroDePagina(numeroFinalPaginacion);
      } else if (numeroFinalPaginacion == this.CantidadTotalDePaginas) {
        numeroInicioPaginacion = numeroFinalPaginacion - 2;
        numeroInicioPaginacion = this.ValidarNumeroDePagina(numeroInicioPaginacion);
      }
      numeroFinalPaginacion = this.ValidarNumeroDePagina(numeroFinalPaginacion);
      return new Tuple<int, int>(numeroInicioPaginacion, numeroFinalPaginacion);
    }
  }
}