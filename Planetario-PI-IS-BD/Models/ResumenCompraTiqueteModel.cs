using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planetario.Models {
  public class ResumenCompraTiqueteModel : ResumenCompraModel{

    public String NombreActividad { get; set; }
    public String FechaActividad { get; set; }
    public int CantidadCupos { get; set; }
    public double PrecioTotal { get; set; }
    public ResumenCompraTiqueteModel(String nombreActividad, String fechaActividad, int cantidadCupos, double precioTotal) {
      this.NombreActividad = nombreActividad;
      this.FechaActividad = fechaActividad;
      this.CantidadCupos = cantidadCupos;
      this.PrecioTotal = precioTotal;
    }
    public override IHtmlString GenerarResumen() {
      String resumen = "";
      resumen += "<p class=\"card-text\">Actividad: {0}</p>\n";
      resumen += "<p class=\"card-text\">Fecha: {1}</p>\n";
      resumen += "<p class=\"card-text\">Total de entradas: {2}</p>\n";
      resumen += "<p class=\"card-text\">Precio final+IVA: {3}</p>\n";
      resumen = String.Format(resumen, NombreActividad, FechaActividad, CantidadCupos, PrecioTotal);
      IHtmlString resumenConvertido = new HtmlString(resumen);
      return resumenConvertido;
    }

    public override string SerializarAJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
  }
}