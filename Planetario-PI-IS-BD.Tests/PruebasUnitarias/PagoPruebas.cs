using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetario.Controllers;
using Planetario.Models;
using Moq;
using System.Web.Mvc;


namespace Planetario_PI_IS_BD.Tests.PruebasUnitarias
{
    [TestClass]
    public class PagoPruebas
    {
        private PagoController ObtenerPagoController()
        {
            PagoController controlador = new PagoController();
            var controllerContext = new Mock<ControllerContext>();
            double subtotal = 10000;
            double IVA = 1.13;
            controllerContext.SetupGet(p => p.HttpContext.Session["resumenDeCompra"]).Returns(new ResumenCompraProductosModel()
            {
                PrecioTotal = subtotal * IVA,
                SubTotal = subtotal,
                Impuestos = subtotal * IVA - subtotal,
                Descuento = 0
            });
            controlador.ControllerContext = controllerContext.Object;
            return controlador;
        }

        [TestMethod]
        public void TestCuponAplicado() {
            PagoController controlador = ObtenerPagoController();

            controlador.AplicarDescuento("123456");
            ResumenCompraProductosModel resumenCompra = (ResumenCompraProductosModel)controlador.Session["resumenDeCompra"];

            Assert.IsTrue(resumenCompra.Descuento > 0);
        }

        [TestMethod]
        public void CuponCorrectoAplicado()
        {
            PagoController controlador = ObtenerPagoController();

            ResumenCompraProductosModel resumenCompra = (ResumenCompraProductosModel)controlador.Session["resumenDeCompra"];
            double descuentoAAplicar = resumenCompra.SubTotal * 0.2;
            JsonResult resultadoAplicarCupon = controlador.AplicarDescuento("123456");
            resumenCompra = (ResumenCompraProductosModel)controlador.Session["resumenDeCompra"];


            Assert.AreEqual(resumenCompra.Descuento, descuentoAAplicar);
        }
    }
}
