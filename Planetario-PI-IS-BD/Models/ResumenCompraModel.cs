using System;
using System.Web;

namespace Planetario.Models {
  public abstract class ResumenCompraModel {
    public abstract IHtmlString GenerarResumen();
    public abstract String SerializarAJson();
  }
}