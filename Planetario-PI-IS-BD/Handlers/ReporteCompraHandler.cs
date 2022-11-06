using Planetario.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Planetario.Handlers
{
  public class ReporteCompraHandler : BaseDeDatosHandler
  {



    public List<DatoReporteModel> ObtenerInformacionReporteCompraSimple(String fechaInicio = "", String fechaFinal = "", String ordenamiento = "")
    {
      List<DatoReporteModel> informacionReporteCompraSimple = new List<DatoReporteModel>();
      DataTable informacionRecuperada = ObtenerTablaReporteCompraSimple(fechaInicio, fechaFinal, ordenamiento);
      foreach (DataRow filaDeInformacion in informacionRecuperada.Rows)
      {
        DatoReporteModel datoReporteSimple = new DatoReporteModel
        {
          FechaCompra = Convert.ToDateTime(filaDeInformacion["fechaCompraFK"]),
          CantidadDeUnidadesVendidas = Convert.ToInt32(filaDeInformacion["Cantidad vendidos"]),
          IdentificadorProducto = Convert.ToString(filaDeInformacion["idProductoPK"]),
          NombreProducto = Convert.ToString(filaDeInformacion["nombre"]),
          Categoria = Convert.ToString(filaDeInformacion["categoria"]),
          Genero = Convert.ToString(filaDeInformacion["genero"]),
          NivelEducativo = Convert.ToString(filaDeInformacion["nivelEducativo"])
        };
        informacionReporteCompraSimple.Add(datoReporteSimple);
      }
      return informacionReporteCompraSimple;
    }

    private DataTable ObtenerTablaReporteCompraSimple(String fechaInicio = "", String fechaFinal = "", String ordenamiento = "")
    {
      bool existenFiltrosFecha = (fechaInicio != "" && fechaFinal != "");
      String direccionOrden = " ASC ";
      String consulta = "SELECT DF.fechaCompraFK, SUM(DF.cantidadUnidades) as 'Cantidad vendidos', P.idProductoPK, P.nombre, " +
        "P.categoria, V.genero, V.nivelEducativo FROM Producto P JOIN DetalleFactura DF ON P.idProductoPK = DF.idProductoFK " +
        "JOIN Visitante V ON V.numeroIdentificacionPK = DF.numeroIdentificacionVisitanteFK ";
      if (existenFiltrosFecha)
      {
        consulta += "WHERE DF.fechaCompraFK BETWEEN @fechaInicio AND @fechaFinal ";
      }
      consulta += "GROUP BY DF.fechaCompraFK,P.idProductoPK, P.nombre, P.categoria, V.genero, V.nivelEducativo ";
      if (ordenamiento == "" || ordenamiento == null)
      {
        ordenamiento = "'Cantidad vendidos'";
      }
      if (ordenamiento == "'Cantidad vendidos'")
      {
        direccionOrden = " DESC ";
      }
      consulta += "ORDER BY " + ordenamiento + direccionOrden;
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      if (existenFiltrosFecha)
      {
        comandoParaConsulta.Parameters.AddWithValue("@fechaInicio", Convert.ToDateTime(fechaInicio));
        comandoParaConsulta.Parameters.AddWithValue("@fechaFinal", Convert.ToDateTime(fechaFinal));
      }
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      return tablaResultado;
    }

    public List<DatoReporteModel> ObtenerInformacionReporteCompraAvanzado(String tipoConsulta, String idProducto = null, String nivelEducativo = null,
                                                                          String genero = null, String diaSemana = null, String ordenamiento = "",
                                                                          DateTime? fechaInicio = null, DateTime? fechaFinal = null)
    {
      List<DatoReporteModel> informacionReporteAvanzado = new List<DatoReporteModel>();
      String fechaInicioParseada = Convert.ToString(fechaInicio);
      String fechaFinalParseada = Convert.ToString(fechaFinal);
      if (tipoConsulta == "comprasConjunto")
      {
        informacionReporteAvanzado = ObtenerInformacionProductosCompradosEnConjunto(idProducto, ordenamiento, fechaInicioParseada, fechaFinalParseada);
      }
      else if (tipoConsulta == "comprasTipoCliente")
      {
        if (diaSemana == "Todos")
        {
          informacionReporteAvanzado = ObtenerInformacionProductoCompradoTipoCliente(genero, nivelEducativo, ordenamiento, fechaInicioParseada, fechaFinalParseada);
        }
        else
        {
          informacionReporteAvanzado = ObtenerInformacionProductoCompradoTipoClienteDiaSemana(genero, nivelEducativo, diaSemana, ordenamiento, fechaInicioParseada, fechaFinalParseada);
        }
      }
      return informacionReporteAvanzado;
    }

    public List<DatoReporteModel> ObtenerInformacionProductosCompradosEnConjunto(String idProducto, String ordenamiento = "", String fechaInicio = "", String fechaFinal = "")
    {
      List<DatoReporteModel> informacionProductosCompradosEnConjunto = new List<DatoReporteModel>();
      DataTable informacionRecuperada = ObtenerTablaProductosCompradosEnConjunto(idProducto, ordenamiento, fechaInicio, fechaFinal);
      foreach (DataRow filaDeInformacion in informacionRecuperada.Rows)
      {
        DatoReporteModel datoReporteSimple = new DatoReporteModel
        {
          CantidadDeUnidadesVendidas = Convert.ToInt32(filaDeInformacion["Cantidad vendidos"]),
          IdentificadorProducto = Convert.ToString(filaDeInformacion["identificacion producto"]),
          NombreProducto = Convert.ToString(filaDeInformacion["Producto comprado"])
        };
        informacionProductosCompradosEnConjunto.Add(datoReporteSimple);
      }
      return informacionProductosCompradosEnConjunto;
    }

    private DataTable ObtenerTablaProductosCompradosEnConjunto(String idProducto, String ordenamiento = "", String fechaInicio = "", String fechaFinal = "")
    {
      bool existenFiltrosFecha = (fechaInicio != "" && fechaFinal != "");
      String consulta = ConstruirConsultaProductosCompradosEnConjunto(existenFiltrosFecha, ordenamiento);
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      if (existenFiltrosFecha)
      {
        comandoParaConsulta.Parameters.AddWithValue("@fechaInicio", Convert.ToDateTime(fechaInicio));
        comandoParaConsulta.Parameters.AddWithValue("@fechaFinal", Convert.ToDateTime(fechaFinal));
      }
      comandoParaConsulta.Parameters.AddWithValue("@idProducto", idProducto);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      return tablaResultado;
    }

    private String ConstruirConsultaProductosCompradosEnConjunto(bool existenFiltrosFecha, String ordenamiento = "")
    {
      String direccionOrden = " ASC ";
      String consulta = "SELECT P2.nombre as 'Producto comprado', P2.idProductoPk as 'identificacion producto', SUM(DF2.cantidadUnidades) as 'Cantidad vendidos' " +
                        "FROM Producto P " +
                        "JOIN DetalleFactura DF ON DF.idProductoFK = P.idProductoPK " +
                        "JOIN Factura F ON  DF.fechaCompraFK = F.fechaCompraPK " +
                        "JOIN DetalleFactura DF2 ON DF2.fechaCompraFK = F.fechaCompraPK " +
                        "JOIN Producto P2 ON DF2.idProductoFK = P2.idProductoPK " +
                        "WHERE ";
      if (existenFiltrosFecha)
      {
        consulta += "DF.fechaCompraFK BETWEEN @fechaInicio AND @fechaFinal AND ";
      }
      consulta += "P.idProductoPk = @idProducto AND NOT(P2.idProductoPk = @idProducto) " +
                  "GROUP BY P2.nombre, P2.idProductoPk ";
      if (ordenamiento == "" || ordenamiento == null)
      {
        ordenamiento = "'Cantidad vendidos'";
      }
      if (ordenamiento == "'Cantidad vendidos'")
      {
        direccionOrden = " DESC ";
      }
      consulta += "ORDER BY " + ordenamiento + direccionOrden;
      return consulta;
    }

    public List<DatoReporteModel> ObtenerInformacionProductoCompradoTipoCliente(String genero, String nivelEducativo, String ordenamiento = "",
                                                                                String fechaInicio = "", String fechaFinal = "")
    {
      List<DatoReporteModel> informacionProductoCompradoGeneroNivelEducativo = new List<DatoReporteModel>();
      DataTable informacionRecuperada = ObtenerTablaProductoCompradoTipoCliente(genero, nivelEducativo, ordenamiento, fechaInicio, fechaFinal);
      foreach (DataRow filaDeInformacion in informacionRecuperada.Rows)
      {
        DatoReporteModel datoReporteSimple = new DatoReporteModel
        {
          CantidadDeUnidadesVendidas = Convert.ToInt32(filaDeInformacion["Cantidad vendidos"]),
          IdentificadorProducto = Convert.ToString(filaDeInformacion["idProductoPK"]),
          NombreProducto = Convert.ToString(filaDeInformacion["nombre"]),
          Genero = Convert.ToString(filaDeInformacion["genero"]),
          NivelEducativo = Convert.ToString(filaDeInformacion["nivelEducativo"])
        };
        informacionProductoCompradoGeneroNivelEducativo.Add(datoReporteSimple);
      }
      return informacionProductoCompradoGeneroNivelEducativo;
    }

    private DataTable ObtenerTablaProductoCompradoTipoCliente(String genero, String nivelEducativo, String ordenamiento = "",
                                                                       String fechaInicio = "", String fechaFinal = "")
    {
      bool existenFiltrosFecha = (fechaInicio != "" && fechaFinal != "");
      bool existenFiltrosGenero = (genero != null && genero != "");
      bool existenFiltrosNivelEducativo = (nivelEducativo != null && nivelEducativo != "");
      String consulta = ConstruirConsultaProductoCompradoTipoCliente(existenFiltrosFecha, existenFiltrosGenero, existenFiltrosNivelEducativo, ordenamiento);
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      if (existenFiltrosFecha)
      {
        comandoParaConsulta.Parameters.AddWithValue("@fechaInicio", Convert.ToDateTime(fechaInicio));
        comandoParaConsulta.Parameters.AddWithValue("@fechaFinal", Convert.ToDateTime(fechaFinal));
      }
      if (existenFiltrosGenero)
        comandoParaConsulta.Parameters.AddWithValue("@Genero", genero);
      if (existenFiltrosNivelEducativo)
        comandoParaConsulta.Parameters.AddWithValue("@NivelEducativo", nivelEducativo);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      return tablaResultado;
    }

    private String ConstruirConsultaProductoCompradoTipoCliente(bool existenFiltrosFecha, bool existenFiltrosGenero, bool existenFiltrosNivelEducativo, String ordenamiento = "")
    {
      String direccionOrden = " ASC ";
      String consulta = "SELECT P.nombre, P.idProductoPk, V.genero, V.nivelEducativo, SUM(DF.cantidadUnidades) as 'Cantidad vendidos' " +
                        "FROM Producto P " +
                        "JOIN DetalleFactura DF ON DF.idProductoFK = P.idProductoPK " +
                        "JOIN Factura F ON  DF.fechaCompraFK = F.fechaCompraPK " +
                        "JOIN Visitante V ON V.numeroIdentificacionPK = F.numeroIdentificacionVisitanteFK ";
      if (existenFiltrosNivelEducativo || existenFiltrosGenero)
      {
        consulta += "WHERE ";
      }
      if (existenFiltrosFecha)
      {
        consulta += "DF.fechaCompraFK BETWEEN @fechaInicio AND @fechaFinal AND ";
      }
      if (existenFiltrosGenero)
      {
        consulta += "V.genero = @Genero ";
      }
      if (existenFiltrosGenero && existenFiltrosNivelEducativo)
      {
        consulta += "AND ";
      }
      if (existenFiltrosNivelEducativo)
      {
        consulta += "V.nivelEducativo = @NivelEducativo ";
      }
      consulta += "GROUP BY P.nombre, P.idProductoPk, V.genero, V.nivelEducativo ";
      if (ordenamiento == "" || ordenamiento == null)
      {
        ordenamiento = "'Cantidad vendidos'";
      }
      if (ordenamiento == "'Cantidad vendidos'")
      {
        direccionOrden = " DESC ";
      }
      consulta += "ORDER BY " + ordenamiento + direccionOrden;
      return consulta;
    }

    private List<DatoReporteModel> ObtenerInformacionProductoCompradoTipoClienteDiaSemana(String genero, String nivelEducativo, String diaSemana,
                                                                                         String ordenamiento = "", String fechaInicio = "",
                                                                                         String fechaFinal = "")
    {
      List<DatoReporteModel> informacionProductoCompradoGeneroNivelEducativoDiaSemana = new List<DatoReporteModel>();
      DataTable informacionRecuperada = ObtenerTablaProductoCompradoTipoClienteDiaSemana(genero, nivelEducativo, diaSemana, ordenamiento, fechaInicio, fechaFinal);
      foreach (DataRow filaDeInformacion in informacionRecuperada.Rows)
      {
        DatoReporteModel datoReporteSimple = new DatoReporteModel
        {
          CantidadDeUnidadesVendidas = Convert.ToInt32(filaDeInformacion["Cantidad vendidos"]),
          IdentificadorProducto = Convert.ToString(filaDeInformacion["idProductoPK"]),
          NombreProducto = Convert.ToString(filaDeInformacion["nombre"]),
          Genero = Convert.ToString(filaDeInformacion["genero"]),
          NivelEducativo = Convert.ToString(filaDeInformacion["nivelEducativo"]),
          DiaSemana = Convert.ToString(filaDeInformacion["Día de la semana"])
        };
        informacionProductoCompradoGeneroNivelEducativoDiaSemana.Add(datoReporteSimple);
      }
      return informacionProductoCompradoGeneroNivelEducativoDiaSemana;
    }

    private DataTable ObtenerTablaProductoCompradoTipoClienteDiaSemana(String genero, String nivelEducativo, String diaSemana,
                                                                       String ordenamiento = "", String fechaInicio = "", String fechaFinal = "")
    {
      bool existenFiltrosFecha = (fechaInicio != "" && fechaFinal != "");
      bool existenFiltrosGenero = (genero != null && genero != "");
      bool existenFiltrosNivelEducativo = (nivelEducativo != null && nivelEducativo != "");
      String consulta = ConstruirConsultaProductoCompradoTipoClienteDiaSemana(existenFiltrosFecha, existenFiltrosGenero, existenFiltrosNivelEducativo, ordenamiento);
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      if (existenFiltrosFecha)
      {
        comandoParaConsulta.Parameters.AddWithValue("@fechaInicio", Convert.ToDateTime(fechaInicio));
        comandoParaConsulta.Parameters.AddWithValue("@fechaFinal", Convert.ToDateTime(fechaFinal));
      }
      if (existenFiltrosGenero)
        comandoParaConsulta.Parameters.AddWithValue("@Genero", genero);
      if (existenFiltrosNivelEducativo)
        comandoParaConsulta.Parameters.AddWithValue("@NivelEducativo", nivelEducativo);
      comandoParaConsulta.Parameters.AddWithValue("@Dia", diaSemana);
      DataTable tablaResultado = CrearTablaConsulta(comandoParaConsulta);
      return tablaResultado;
    }

    private String ConstruirConsultaProductoCompradoTipoClienteDiaSemana(bool existenFiltrosFecha, bool existenFiltrosGenero, bool existenFiltrosNivelEducativo,
                                                                         String ordenamiento = "")
    {
      String direccionOrden = " ASC ";
      String consulta = "SET LANGUAGE Spanish " +
                        "SELECT P.nombre, P.idProductoPk, DATENAME(WEEKDAY, F.fechaCompraPK) AS 'Día de la semana', V.genero, V.nivelEducativo, " +
                        "SUM(DF.cantidadUnidades) as 'Cantidad vendidos'" +
                        "FROM Producto P " +
                        "JOIN DetalleFactura DF ON DF.idProductoFK = P.idProductoPK " +
                        "JOIN Factura F ON  DF.fechaCompraFK = F.fechaCompraPK " +
                        "JOIN Visitante V ON V.numeroIdentificacionPK = F.numeroIdentificacionVisitanteFK ";
      consulta += "WHERE DATENAME(WEEKDAY, F.fechaCompraPK) = @Dia ";
      if (existenFiltrosNivelEducativo || existenFiltrosGenero)
        consulta += "AND ";
      if (existenFiltrosFecha)
      {
        consulta += "DF.fechaCompraFK BETWEEN @fechaInicio AND @fechaFinal AND ";
      }
      if (existenFiltrosGenero)
      {
        consulta += "V.genero = @Genero ";
      }
      if (existenFiltrosGenero && existenFiltrosNivelEducativo)
      {
        consulta += "AND ";
      }
      if (existenFiltrosNivelEducativo)
      {
        consulta += "V.nivelEducativo = @NivelEducativo ";
      }
      consulta += "GROUP BY P.nombre, P.idProductoPk, DATENAME(WEEKDAY, F.fechaCompraPK), V.genero, V.nivelEducativo ";
      if (ordenamiento == "" || ordenamiento == null)
      {
        ordenamiento = "'Cantidad vendidos'";
      }
      if (ordenamiento == "'Cantidad vendidos'")
      {
        direccionOrden = " DESC ";
      }
      consulta += "ORDER BY " + ordenamiento + direccionOrden;
      return consulta;
    }

  }
}