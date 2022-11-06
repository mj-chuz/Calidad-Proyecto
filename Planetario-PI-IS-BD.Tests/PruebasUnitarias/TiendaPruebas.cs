using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetario.Controllers;
using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Planetario.Handlers;
using System.Collections.Generic;
using Planetario.Models;

namespace Planetario_PI_IS_BD.Tests.PruebasUnitarias
{
    [TestClass]
    public class TiendaPruebas
    {
        ProductoHandler ProductoHandler;
        public TiendaPruebas()
        {
            ProductoHandler = new ProductoHandler();
        }

        [TestMethod]
        public void PruebaObtenerProductosPorPagina()
        {
            int cantidadProductosFinal = ProductoHandler.ObtenerProductosPorPagina();
            List<ProductoModel> productos = ProductoHandler.ObtenerListaProductos(ProductoHandler.ObtenerTablaProductos());
            Assert.AreEqual(cantidadProductosFinal, productos.Count);
        }

    }
}
