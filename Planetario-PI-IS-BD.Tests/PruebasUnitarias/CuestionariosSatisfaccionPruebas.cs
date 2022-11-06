using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetario.Controllers;
using System.Web.Mvc;

namespace Planetario_PI_IS_BD.Tests.PruebasUnitarias{
    [TestClass]
    public class CuestionariosSatisfaccionPruebas{
        public PreguntaSatisfaccionController AccesoAPreguntasSatisfaccion;

        public CuestionariosSatisfaccionPruebas(){
            AccesoAPreguntasSatisfaccion = new PreguntaSatisfaccionController();
        }

        [TestMethod]
        public void EnviarActividadEnNull(){
            ActionResult paginaSatisfaccionCompra = AccesoAPreguntasSatisfaccion.CuestionarioSatisfaccionCompra("1234",null);
            Assert.AreEqual("RedirectToRouteResult", paginaSatisfaccionCompra.GetType().Name);
        }

        [TestMethod]
        public void EnviarIdentificacionEnNull(){
            ActionResult paginaSatisfaccionCompra = AccesoAPreguntasSatisfaccion.CuestionarioSatisfaccionCompra(null, "actividad compra");
            Assert.AreEqual("RedirectToRouteResult", paginaSatisfaccionCompra.GetType().Name);
        }

        [TestMethod]
        public void InsertarCategoriaIncorrecta(){
            ActionResult paginaSatisfaccionCompra = AccesoAPreguntasSatisfaccion.CuestionarioSatisfaccionCompra("1234", "datos inválidos");
            Assert.AreEqual("RedirectToRouteResult", paginaSatisfaccionCompra.GetType().Name);
        }
    }
}
