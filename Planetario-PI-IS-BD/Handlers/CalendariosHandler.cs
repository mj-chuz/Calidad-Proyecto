using System;
using System.Collections.Generic;
using Planetario.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using TimeZoneConverter;

namespace Planetario.Handlers {
  public class CalendariosHandler {
    private Dictionary<string, string> _idsCalendarios;
    public CalendariosHandler() {
      _idsCalendarios = new Dictionary<string, string>();
      _idsCalendarios.Add("FenomenoAstronomico", "");
      _idsCalendarios.Add("Charla", "76g9m80gc5k2kecctbc1os2i1g@group.calendar.google.com");
      _idsCalendarios.Add("Taller", "6q9o35lkdooaddbcev6qdvbt1c@group.calendar.google.com");
      _idsCalendarios.Add("Telescopiada", "g1isfqe5ngi1gm6mp04rv8u8gg@group.calendar.google.com");
      _idsCalendarios.Add("Película", "km5345raduhdabepcvp1emse4s@group.calendar.google.com");
    }

    private UserCredential GenerarCredenciales() {
      UserCredential credencial;
      string[] extension = { CalendarService.Scope.Calendar };
      using (var archivo = new FileStream(AppContext.BaseDirectory + "Credentials/credentials.json", FileMode.Open, FileAccess.Read)) {
        string caminoCredencial = AppContext.BaseDirectory + "token.json";
        credencial = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(archivo).Secrets,
            extension,
            "user",
            CancellationToken.None,
            new FileDataStore(caminoCredencial, true)).Result;
      }
      return credencial;
    }

    private Event CrearActividad(ActividadModel actividad, String enlaceActividad) {
      DateTime fechaFinalizacion = actividad.Fecha.AddMinutes(actividad.Duracion);
      return new Event() {
        Summary = actividad.Titulo,
        Description = GenerarDescripcionCalendario(actividad.Descripcion) + " " + "<a href = " + enlaceActividad + " >Ver más</a>",
        Start = new EventDateTime() {
          DateTime = actividad.Fecha,
          TimeZone = TZConvert.WindowsToIana(TimeZone.CurrentTimeZone.StandardName),
        },
        End = new EventDateTime() {
          DateTime = fechaFinalizacion,
          TimeZone = TZConvert.WindowsToIana(TimeZone.CurrentTimeZone.StandardName),
        },
      };
    }

    private String GenerarDescripcionCalendario(String descripcionCompleta) {
      int tamanoMaximoDescripcion = 150;
      if (descripcionCompleta.Length > tamanoMaximoDescripcion) {
        descripcionCompleta = descripcionCompleta.Substring(0, tamanoMaximoDescripcion - 20) + "...";
      }
      return descripcionCompleta;
    }

    public void AgregarActividadACalendario(ActividadModel actividad, String enlaceActividad) {
      Event eventoCalendario = CrearActividad(actividad, enlaceActividad);
      _idsCalendarios.TryGetValue(actividad.TipoDeActividad, out string idCalendario);
      string nombreAplicacion = "PlanetarioPI";
      UserCredential credencial = GenerarCredenciales();
      var servicio = new CalendarService(new BaseClientService.Initializer() {
        HttpClientInitializer = credencial,
        ApplicationName = nombreAplicacion,
      });
      EventsResource.InsertRequest peticion = servicio.Events.Insert(eventoCalendario, idCalendario);
      peticion.Execute();
    }
    private Event CrearTelescopiadaPelicula(TelescopiadaPeliculaModel telescopiadaPelicula, String enlace) {
      DateTime fechaFinalizacion = telescopiadaPelicula.Fecha.AddMinutes(telescopiadaPelicula.Duracion);
      return new Event() {
        Summary = telescopiadaPelicula.Titulo,
        Description = GenerarDescripcionCalendario(telescopiadaPelicula.Descripcion) + " " + "<a href = " + enlace + " >Ver más</a>",
        Start = new EventDateTime() {
          DateTime = telescopiadaPelicula.Fecha,
          TimeZone = TZConvert.WindowsToIana(TimeZone.CurrentTimeZone.StandardName),
        },
        End = new EventDateTime() {
          DateTime = fechaFinalizacion,
          TimeZone = TZConvert.WindowsToIana(TimeZone.CurrentTimeZone.StandardName),
        },
      };
    }
    public void AgregarTelescopiadaPeliculaACalendario(TelescopiadaPeliculaModel telescopiadaPelicula, String enlace) {
      Event eventoCalendario = CrearTelescopiadaPelicula(telescopiadaPelicula, enlace);
      _idsCalendarios.TryGetValue(telescopiadaPelicula.Tipo, out string idCalendario);
      string nombreAplicacion = "PlanetarioPI";
      UserCredential credencial = GenerarCredenciales();
      var servicio = new CalendarService(new BaseClientService.Initializer() {
        HttpClientInitializer = credencial,
        ApplicationName = nombreAplicacion,
      });
      EventsResource.InsertRequest peticion = servicio.Events.Insert(eventoCalendario, idCalendario);
      peticion.Execute();
    }


  }
}