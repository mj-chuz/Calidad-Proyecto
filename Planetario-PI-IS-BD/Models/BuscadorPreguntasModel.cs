using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planetario.Models {
  public class BuscadorPreguntasModel {

    public List<String> topicosFiltrados { get; set; }
    public FuncionarioModel FuncionarioCrearPregunta { get; set; }
  }
}