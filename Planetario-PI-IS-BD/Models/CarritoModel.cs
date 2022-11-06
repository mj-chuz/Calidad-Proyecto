using System;
using System.Collections.Generic;
using Planetario.Handlers;

namespace Planetario.Models {
  public class CarritoModel {
    public Dictionary<String, int> ProductosEnCarrito { get; set; }

    public CarritoModel() {
      ProductosEnCarrito = new Dictionary<String, int>(); 
    }

    public Dictionary<ProductoModel,int> ObtenerCarritoConModelos() {
      Dictionary<ProductoModel, int> respuesta = new Dictionary<ProductoModel, int>();
      ProductoHandler productoHandler = new ProductoHandler();
      foreach (KeyValuePair<String, int> elementoCarrito in ProductosEnCarrito) {
        ProductoModel producto = productoHandler.ObtenerProductoModel(elementoCarrito.Key);
        respuesta.Add(producto, elementoCarrito.Value);
      }
      return respuesta;
    }
    public void AgregarProducto(String producto, int cantidad = 1) {
      if (ProductosEnCarrito.ContainsKey(producto)) {
        ProductosEnCarrito[producto] = ProductosEnCarrito[producto]+cantidad;
      } else ProductosEnCarrito.Add(producto, cantidad);
    }

    public int EliminarProducto(String producto) {
      int cantidadProducto = ProductosEnCarrito[producto];
      ProductosEnCarrito.Remove(producto);
      return cantidadProducto;
    }

    public double CalcularSubtotal() {
      Double subtotal = 0;
      ProductoHandler productoHandler = new ProductoHandler();
      foreach (KeyValuePair<String, int> elementoCarrito in ProductosEnCarrito) {
        ProductoModel productoModel = productoHandler.ObtenerProductoModel(elementoCarrito.Key);
        Double precio = productoModel.Precio;
        int cantidad = elementoCarrito.Value;
        subtotal += (precio * cantidad);
      }
      return subtotal;
    }


  }
}