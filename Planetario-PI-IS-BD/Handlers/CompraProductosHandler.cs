using System;
using System.Collections.Generic;
using Planetario.Models;
using System.Data.SqlClient;

namespace Planetario.Handlers {
  public class CompraProductosHandler : BaseDeDatosHandler {

    public void ActualizarTablasCompra(ResumenCompraProductosModel resumenDeCompra, String numeroIdentificacionComprador) {
      ActualizarTablaProductos(resumenDeCompra);
      CrearFactura(resumenDeCompra, numeroIdentificacionComprador);
    }

    private SqlCommand GenerarComandoParaRestarUnidades(int cantidadComprada, String idProducto) {
      String consulta = "UPDATE Producto SET unidadesDisponibles = unidadesDisponibles - @unidadesCompradas " +
          "WHERE idProductoPK = @idProducto";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@unidadesCompradas", cantidadComprada);
      comandoParaConsulta.Parameters.AddWithValue("@idProducto", idProducto);
      return comandoParaConsulta;
    }

    private bool ActualizarTablaProductos(ResumenCompraProductosModel resumenDeCompra) {
      ProductoHandler productoHandler = new ProductoHandler();
      bool exitoActualizar = true;
      ConexionPlanetario.Open();
      foreach (KeyValuePair<String, int> elementoCarrito in resumenDeCompra.Carrito.ProductosEnCarrito) {
        ProductoModel productoComprado = productoHandler.ObtenerProductoModel(elementoCarrito.Key);
        int cantidadComprada = elementoCarrito.Value;
        SqlCommand comandoParaConsulta = GenerarComandoParaRestarUnidades(cantidadComprada, productoComprado.IdentificadorProducto);
        exitoActualizar = comandoParaConsulta.ExecuteNonQuery() >= 1;
        if (!exitoActualizar) {
          ConexionPlanetario.Close();
          return exitoActualizar;
        }
      }
      ConexionPlanetario.Close();
      return exitoActualizar;
    }

    private void CrearFactura(ResumenCompraProductosModel resumenDeCompra, String numeroIdentificacionComprador) {
      DateTime fechaDeCompra = DateTime.Now;
      String consulta = "INSERT INTO Factura(fechaCompraPK,numeroIdentificacionVisitanteFK, subTotal, impuesto, descuentoAplicado, total, cuponAplicadoFK) " +
        "VALUES(@fechaDeCompra, @numeroIdentificacionComprador, @subTotal, @impuesto, @descuento, @total, @cupon)";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@fechaDeCompra", fechaDeCompra);
      comandoParaConsulta.Parameters.AddWithValue("@numeroIdentificacionComprador", numeroIdentificacionComprador);
      comandoParaConsulta.Parameters.AddWithValue("@subTotal", resumenDeCompra.SubTotal);
      comandoParaConsulta.Parameters.AddWithValue("@impuesto", resumenDeCompra.Impuestos);
      comandoParaConsulta.Parameters.AddWithValue("@descuento", resumenDeCompra.Descuento);
      comandoParaConsulta.Parameters.AddWithValue("@total", resumenDeCompra.PrecioTotal);
      comandoParaConsulta.Parameters.AddWithValue("@cupon", resumenDeCompra.CodigoCupon);
      ConexionPlanetario.Open();
      comandoParaConsulta.ExecuteNonQuery();
      ConexionPlanetario.Close();
      CrearDetallesFactura(fechaDeCompra, numeroIdentificacionComprador, resumenDeCompra);
    }

    private SqlCommand GenerarComandoParaDetalleFactura(ProductoModel productoComprado, DateTime fechaDeCompra, 
                                                        String numeroIdentificacionComprador, int cantidadComprada) {
      String consulta = "INSERT INTO DetalleFactura VALUES(@fechaDeCompra, @numeroIdVisitante, @idProducto, @cantidadComprada, @precioTotalProducto)";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@fechaDeCompra", fechaDeCompra);
      comandoParaConsulta.Parameters.AddWithValue("@numeroIdVisitante", numeroIdentificacionComprador);
      comandoParaConsulta.Parameters.AddWithValue("@idProducto", productoComprado.IdentificadorProducto);
      comandoParaConsulta.Parameters.AddWithValue("@cantidadComprada", cantidadComprada);
      comandoParaConsulta.Parameters.AddWithValue("@precioTotalProducto", productoComprado.Precio);
      return comandoParaConsulta;
    }

    private bool CrearDetallesFactura(DateTime fechaDeCompra, String numeroIdentificacionComprador, ResumenCompraProductosModel resumenDeCompra) {
      bool exitoActualizar = true;
      ProductoHandler productoHandler = new ProductoHandler();
      ConexionPlanetario.Open();
      foreach (KeyValuePair<string, int> elementoCarrito in resumenDeCompra.Carrito.ProductosEnCarrito) {
        ProductoModel productoComprado = productoHandler.ObtenerProductoModel(elementoCarrito.Key);
        int cantidadComprada = elementoCarrito.Value;
        SqlCommand comandoParaConsulta = GenerarComandoParaDetalleFactura(productoComprado,fechaDeCompra,numeroIdentificacionComprador,cantidadComprada);
        exitoActualizar = comandoParaConsulta.ExecuteNonQuery() >= 1;
        if (!exitoActualizar) {
          ConexionPlanetario.Close();
          return exitoActualizar;
        }
      }
      ConexionPlanetario.Close();
      return exitoActualizar;
    }
  }
}