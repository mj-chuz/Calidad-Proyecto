using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Planetario.Handlers {
  public class CargadorArchivoHandler : HttpPostedFileBase{
    private readonly byte[] _bytesDeArchivo;

    public CargadorArchivoHandler(byte[] bytesDeArchivo) {
      this._bytesDeArchivo = bytesDeArchivo;
      this.InputStream = new MemoryStream(bytesDeArchivo);
    }

    public override int ContentLength => _bytesDeArchivo.Length;

    public override Stream InputStream { get; }
  }
}