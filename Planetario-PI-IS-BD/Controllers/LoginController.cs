using Planetario.Handlers;
using Planetario.Models;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web.Security;

namespace Planetario.Controllers {
  public class LoginController : Controller {
    SHA256 EncriptadorContrasena;
    FuncionariosHandler AccesoDatosFuncionario;
    public LoginController() {
      AccesoDatosFuncionario = new FuncionariosHandler();
      EncriptadorContrasena = SHA256.Create();
    }
    public ActionResult Login() {
      return View();
    }

    [HttpPost]
    public ActionResult Login(LoginModel usuario) {
      if (ModelState.IsValid) {

        LoginModel usuarioValido = ValidarUsuario(usuario);
        if (usuarioValido != null) {
          FormsAuthentication.SetAuthCookie(usuario.NumeroIdentificacion.ToString(), false);
          return Redirect("/Home");
        } else {
          ModelState.AddModelError("Error", "Número de identificación o contraseña incorrecta.");
          return View();
        }
      } else {
        return View(usuario);
      }
    }

    public LoginModel ValidarUsuario(LoginModel usuario) {
      FuncionarioModel funcionario = AccesoDatosFuncionario.ObtenerListaFuncionarios().Find(funcionarioModel => funcionarioModel.NumeroIdentificacion == usuario.NumeroIdentificacion);
      LoginModel usuarioVerificado = new LoginModel();
      usuario.Contrasena = AccesoDatosFuncionario.ObtenerHash(EncriptadorContrasena, usuario.Contrasena);
      if (funcionario != null) {
        if (funcionario.Contrasena == usuario.Contrasena) {
          return usuario;
        } else {
          return null;
        }
      } else {
        return null;
      }
    }

    public ActionResult Logout() {
      FormsAuthentication.SignOut();
      Session.Abandon();
      return Redirect("/Home");
    }

  }
}