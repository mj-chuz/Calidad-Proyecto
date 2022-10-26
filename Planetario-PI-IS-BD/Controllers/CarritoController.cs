using System;
using System.Web.Mvc;
using Planetario.Models;
using Planetario.Handlers;

namespace Planetario.Controllers
{
  public class CarritoController : Controller
  {

    public CarritoModel ObtenerCarrito()
    {
      CarritoModel carrito;
      if (Session["carrito"] == null)
      {
        carrito = new CarritoModel();
        Session["cantidadProductosCarrito"] = 0;
      }
      else carrito = (CarritoModel)Session["carrito"];
      Session["carrito"] = carrito;
      return carrito;
    }

    public ActionResult AgregarProducto(String idItem, int cantidad = 1)
    {
      ProductoHandler accesoAProductos = new ProductoHandler();
      ProductoModel productoAgregado = accesoAProductos.ObtenerProductoModel(idItem);
      CarritoModel carrito;
      if (Session["carrito"] == null)
      {
        carrito = new CarritoModel();
        Session["cantidadProductosCarrito"] = 0;
      }
      else carrito = (CarritoModel)Session["carrito"];
      carrito.AgregarProducto(productoAgregado.IdentificadorProducto, cantidad);
      Session["carrito"] = carrito;
      Session["cantidadProductosCarrito"] = (int)Session["cantidadProductosCarrito"] + cantidad;
      return RedirectToAction("Catalogo", "Producto");
    }
    public ActionResult EliminarProducto(String idItem)
    {
      ProductoHandler accesoAProductos = new ProductoHandler();
      ProductoModel productoAgregado = accesoAProductos.ObtenerProductoModel(idItem);
      CarritoModel carrito;
      if (Session["carrito"] == null)
      {
        carrito = new CarritoModel();
      }
      else carrito = (CarritoModel)Session["carrito"];
      int cantidadProducto = carrito.EliminarProducto(productoAgregado.IdentificadorProducto);
      Session["cantidadProductosCarrito"] = (int)Session["cantidadProductosCarrito"] - cantidadProducto;
      Session["carrito"] = carrito;
      return RedirectToAction("MostrarCarritoCompras", "Pago");
    }
  }
}