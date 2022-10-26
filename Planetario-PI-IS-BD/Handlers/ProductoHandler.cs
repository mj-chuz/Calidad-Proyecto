using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Newtonsoft.Json;
using Planetario.Models;

namespace Planetario.Handlers {
  public class ProductoHandler : BaseDeDatosHandler {

    private int _productosPorPagina = 6;

    public int ObtenerProductosPorPagina() {
      return this._productosPorPagina;
    }

    public DataTable ObtenerTablaProductos(String categoria = "", String columnaOrdenamiento = "", String direccionOrdenamiento = "", int pagina = 0) {
      bool existenParametrosOrdenamiento = (columnaOrdenamiento != "" && direccionOrdenamiento != "");
      bool existeCategoria = categoria != "";
      String consulta = "SELECT * FROM Producto ";
      if (existeCategoria) {
        consulta += "WHERE categoria=@categoria ";
      }
      if (existenParametrosOrdenamiento) {
        consulta += "ORDER BY " + columnaOrdenamiento + " " + direccionOrdenamiento;
      } else {
        consulta += "ORDER BY (SELECT NULL) ";
      }

      consulta += " OFFSET " + Convert.ToString(pagina * _productosPorPagina) + " ROWS" +
        " FETCH NEXT " + Convert.ToString(_productosPorPagina) + " ROWS ONLY";

      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      if (existeCategoria) {
        comandoParaConsulta.Parameters.AddWithValue("@categoria", categoria);
      }
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      return tablaResultado;
    }

    public String ObtenerProducto(String identificadorProducto) {
      String consulta = "SELECT * FROM Producto WHERE idProductoPK=@identificadorProducto";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@identificadorProducto", identificadorProducto);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      return JsonConvert.SerializeObject(tablaResultado);
    }

    public String ObtenerProductos(String categoria = "", String columnaOrdenamiento = "", String direccionOrdenamiento = "", int pagina = 0) {
      DataTable tablaProductos = ObtenerTablaProductos(categoria, columnaOrdenamiento, direccionOrdenamiento, pagina);
      return JsonConvert.SerializeObject(ObtenerListaProductos(tablaProductos));
    }

    public ProductoModel ObtenerProductoModel(String identificadorProducto) {
      String consulta = "SELECT * FROM Producto WHERE idProductoPK=@identificadorProducto";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@identificadorProducto", identificadorProducto);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      ProductoModel productoObtenido; 
      if(tablaResultado.Rows.Count != 0) {
        productoObtenido = ConstruirProducto(tablaResultado.Rows[0]);
      } else {
        productoObtenido = new ProductoModel { IdentificadorProducto = "ProductoInvalido" };
      }
      return productoObtenido;
    }

    public List<ProductoModel> ObtenerListaProductos(DataTable tablaProductos) {
      List<ProductoModel> productos = new List<ProductoModel>();
      foreach (DataRow filaProducto in tablaProductos.Rows) {
        ProductoModel producto = ConstruirProducto(filaProducto);
        productos.Add(producto);
      }
      return productos;
    }

    public ProductoModel ConstruirProducto(DataRow fila) {
      ProductoModel producto = new ProductoModel();
      producto.IdentificadorProducto = Convert.ToString(fila["idProductoPK"]);
      producto.Nombre = Convert.ToString(fila["nombre"]);
      producto.Descripcion = Convert.ToString(fila["descripcion"]);
      producto.Categoria = Convert.ToString(fila["categoria"]);
      producto.UnidadesDisponibles = Convert.ToInt32(fila["unidadesDisponibles"]);
      producto.Precio = Convert.ToSingle(fila["precio"]);
      producto.Peso = Convert.ToSingle(fila["peso"]);
      producto.Color = Convert.ToString(fila["color"]);
      producto.NombreFoto = Convert.ToString(fila["nombreArchivoImagen"]);
      return producto;
    }

    public String ObtenerCantidadTotalProductos(String categoria = "") {
      DataTable tablaCantidadProductos = ObtenerTablaCantidadProductos(categoria);
      return "[{\"Cantidad de Productos\":\"" + tablaCantidadProductos.Rows[0]["Cantidad de Productos"] + "\"}]";
    }

    public DataTable ObtenerTablaCantidadProductos(String categoria = "") {
      bool existeCategoria = categoria != "";
      String consulta = "SELECT COUNT(*) as 'Cantidad de Productos' FROM Producto ";
      if (existeCategoria) {
        consulta += "WHERE categoria=@categoria ";
      }
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      if (existeCategoria) {
        comandoParaConsulta.Parameters.AddWithValue("@categoria", categoria);
      }
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      return tablaResultado;
    }

    public List<SelectListItem> ObtenerNombreProductos() {
      String consulta = "SELECT idProductoPK, nombre FROM Producto";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      List<SelectListItem> informacionProductos = new List<SelectListItem>();
      foreach (DataRow filaInformacion in tablaResultado.Rows) {
        String nombreProducto = Convert.ToString(filaInformacion["idProductoPK"]);
        String identificadorProducto = Convert.ToString(filaInformacion["nombre"]);
        informacionProductos.Add(new SelectListItem { Text = identificadorProducto, Value = nombreProducto });
      }
      return informacionProductos;
    }

  }
}