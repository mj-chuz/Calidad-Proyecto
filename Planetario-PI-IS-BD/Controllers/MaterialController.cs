using System;
using Planetario.Handlers;
using Planetario.Models;
using System.Web.Mvc;

namespace Planetario.Controllers {
  public class MaterialController : Controller {
    public CategoriaHandler AccesoADatosCategoria;
    public MaterialHandler AccesoDatosMaterial;
    public ActividadHandler AccesoActividades;
    public MaterialController() {
      AccesoADatosCategoria = new CategoriaHandler();
      AccesoDatosMaterial = new MaterialHandler();
      AccesoActividades = new ActividadHandler();
    }

    public ActionResult RecuperarMateriales() {
      ViewBag.Materiales = AccesoDatosMaterial.ObtenerMaterialesPorPalabraClave();
      ViewBag.MaterialesRecomendados = AccesoDatosMaterial.ObtenerMaterialSimilarDeCadaMaterial(ViewBag.Materiales);
      return View();
    }


    [HttpPost]
    public ActionResult RecuperarMateriales(string palabraClave) {
      ViewBag.Materiales = AccesoDatosMaterial.ObtenerMaterialesPorPalabraClave(palabraClave);
      ViewBag.MaterialesRecomendados = AccesoDatosMaterial.ObtenerMaterialSimilarDeCadaMaterial(ViewBag.Materiales);
      return View();
    }

    public ActionResult RecuperarMaterialesActividad(string titulo = "", DateTime fecha = new DateTime()) {
      ActividadModel actividad = AccesoActividades.ObtenerActividad(titulo, fecha);
      ViewBag.Materiales = AccesoDatosMaterial.ObtenerMaterialesDeActividad(actividad);
      ViewBag.Actividad = actividad;
      ViewBag.MaterialesRecomendados = AccesoDatosMaterial.ObtenerMaterialSimilarDeCadaMaterial(ViewBag.Materiales);
      return View();
    }
    [Authorize]
    public ActionResult CrearMaterial() {
      ViewBag.ListaActividades= AccesoActividades.ObtenerListaActividadesView(); 
      ViewBag.ListaCategorias = AccesoADatosCategoria.ObtenerListaCategoriasView();
      ViewBag.ExitoAlCrear = true;

      return View();
    }
    [Authorize]
    [HttpPost]
    public ActionResult CrearMaterial(MaterialModel material) {
      ViewBag.ListaActividades = AccesoActividades.ObtenerListaActividadesView();
      ViewBag.ListaCategorias = AccesoADatosCategoria.ObtenerListaCategoriasView();
      ViewBag.ExitoAlCrear = false;
      try {
        if (ModelState.IsValid) {
          material.FechaDePublicacion = DateTime.Now;
          AccesoDatosMaterial.AgregarMaterial(material);
          ViewBag.Message = "El material fue creada con éxito";
          ViewBag.ExitoAlCrear = true;
          ModelState.Clear();
        }
        return View();
      }
      catch(Exception) {
        ViewBag.ExitoAlCrear = false;
        ViewBag.Message = "Algo salió mal y no fue posible enviar el material";
        return View();
      }
    }
    byte[] ObtenerBytesArchivo(String direccion) {
      System.IO.FileStream contenidoArchivo = System.IO.File.OpenRead(direccion);
      byte[] data = new byte[contenidoArchivo.Length];
      int ultimoByteLeido = contenidoArchivo.Read(data, 0, data.Length);
      if (ultimoByteLeido != contenidoArchivo.Length)
        throw new System.IO.IOException(direccion);
      return data;
    }

    public FileResult DescargarMaterial(String direccionMaterial) {
      String caminoCompleto = Server.MapPath(direccionMaterial);
      byte[] bytesArchivo = System.IO.File.ReadAllBytes(caminoCompleto);
      return File(bytesArchivo, System.Net.Mime.MediaTypeNames.Application.Octet, direccionMaterial);
    }
  }
}