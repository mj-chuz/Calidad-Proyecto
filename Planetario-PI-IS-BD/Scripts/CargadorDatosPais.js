window.onload = function () {
    llenarSelect("obtenedor-paises","#paisSeleccionado");
    llenarSelect("obtenedor-idiomas", "#listaIdiomas");
};
async function obtenerInformacionJson(idCargador) {
    idCargador = "#" + idCargador;
    let datosEncontrados = "";
    await $.ajax({
        type: 'POST',
        url: $(idCargador).data("request-url"),
        dataType: 'json',

        data: { },

        success: function (datos) {
            console.log(datos)
            datosEncontrados = JSON.parse(datos);
            console.log(datosEncontrados);
        }
    })
    return datosEncontrados;
}

async function llenarSelect(idCargador, idContenedor) {
    let paises = await obtenerInformacionJson(idCargador);
    paises = paises.Datos;
    for (let indicePais = 0; indicePais < paises.length; indicePais++) {
        $(idContenedor).append('<option value=' + paises[indicePais].replace(" ", "_") + '>' + paises[indicePais] + '</option>');
    }
}

