using System;

namespace Planetario.Handlers {
  public class ValidadorFechasReporte {
    public bool ValidarFechasFiltros(object fechaInicio, object fechaFinal) {
      if (fechaInicio != null && fechaFinal != null) {
        return ValidarInicioSegunFinal((DateTime)fechaInicio, (DateTime)fechaFinal) && 
          ValidarFechaSegunActualidad((DateTime)fechaInicio) && 
          ValidarFechaSegunActualidad((DateTime)fechaFinal);
      } else return true;
    }

    public bool ValidarInicioSegunFinal(DateTime fechaInicio, DateTime fechaFinal) {
      return fechaInicio < fechaFinal;
    }

    public bool ValidarFechaSegunActualidad(DateTime fecha) {
      return fecha <= DateTime.Now;
    }
  }
}