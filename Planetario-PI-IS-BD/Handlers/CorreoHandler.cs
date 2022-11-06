using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MimeKit;
using MailKit.Net.Smtp;

namespace Planetario.Handlers {
  public class CorreoHandler {
    private readonly MailboxAddress CorreoRemitente; 
    public CorreoHandler() {
      CorreoRemitente = new MailboxAddress("Planetario Dajam", "planetariodajam@gmail.com");
    }

    public void EnviarCorreo(MimeMessage correo) {
      using (var client = new SmtpClient()) {
        client.Connect("smtp.gmail.com", 587, false);
        client.Authenticate("planetariodajam", "PI3UnArbo");
        client.Send(correo);
        client.Disconnect(true);
      }
    }

    public void EnviarCorreoDeAprobacion(string tituloActividad, string correoDestinatario) {
      MimeMessage correo = new MimeMessage();
      correo.From.Add(CorreoRemitente);
      correo.To.Add(new MailboxAddress("Educador", correoDestinatario));
      correo.Subject = "Su actividad " + tituloActividad + " ha sido aprobada";
      EnviarCorreo(correo);
    }
    public void EnviarCorreoDeRechazo(string tituloActividad, string correoDestinatario) {
      MimeMessage correo = new MimeMessage();
      correo.From.Add(CorreoRemitente);
      correo.To.Add(new MailboxAddress("Educador", correoDestinatario));
      correo.Subject = "Su actividad " + tituloActividad + " ha sido rechazada";
      EnviarCorreo(correo);
    }
  }
}