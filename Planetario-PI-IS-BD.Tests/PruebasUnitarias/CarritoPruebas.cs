using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Planetario.Controllers;
using Planetario.Models;
using System;
using System.Web.Mvc;

namespace Planetario_PI_IS_BD.Tests.PruebasUnitarias
{

    [TestClass]
    public class CarritoPruebas
    {
        CarritoController carritoController;
        public CarritoPruebas()
        {
            carritoController = new CarritoController();
        }

        [TestMethod]
        public void PruebaAgregarProducto()
        {
            var contexto = new Mock<ControllerContext>();
            String identificadorProducto = "1Ll";
            contexto.Setup(carritoMock => carritoMock.HttpContext.Session["carrito"]).Returns(new CarritoModel());
            carritoController.ControllerContext = contexto.Object;
            carritoController.AgregarProducto(identificadorProducto);
            CarritoModel carrito = (CarritoModel)carritoController.Session["carrito"];

            Assert.IsTrue(carrito.ProductosEnCarrito.ContainsKey(identificadorProducto));
        }

        [TestMethod]
        public void PruebaAgregarProductoInvalido()
        {
            var contexto = new Mock<ControllerContext>();
            String identificadorProducto = "idInvalido";
            contexto.Setup(carritoMock => carritoMock.HttpContext.Session["carrito"]).Returns(new CarritoModel());
            carritoController.ControllerContext = contexto.Object;
            carritoController.AgregarProducto(identificadorProducto);


            ActionResult vistaRetornada = carritoController.AgregarProducto(identificadorProducto);
            CarritoModel carrito = (CarritoModel)carritoController.Session["carrito"];
            Assert.AreEqual("RedirectToRouteResult", vistaRetornada.GetType().Name);
            Assert.IsFalse(carrito.ProductosEnCarrito.ContainsKey(identificadorProducto));
        }

        [TestMethod]
        public void PruebaEliminarProducto()
        {
            var contexto = new Mock<ControllerContext>();
            String identificadorProducto = "1Ll";
            CarritoModel carritoMock = new CarritoModel();
            carritoMock.ProductosEnCarrito.Add(identificadorProducto, 1);
            contexto.Setup(carritoMockSet => carritoMockSet.HttpContext.Session["carrito"]).Returns(carritoMock);
            carritoController.ControllerContext = contexto.Object;
            carritoController.EliminarProducto(identificadorProducto);
            CarritoModel carrito = (CarritoModel)carritoController.Session["carrito"];

            Assert.IsFalse(carrito.ProductosEnCarrito.ContainsKey(identificadorProducto));
        }
    }
}
