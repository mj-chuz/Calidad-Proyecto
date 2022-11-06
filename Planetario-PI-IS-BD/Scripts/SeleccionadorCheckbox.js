class SeleccionadorCheckbox {
    constructor(claseCategoria, identificadorDatosSeleccionados) {
        this.nombreClaseCategoria = claseCategoria;
        this.datosSeleccionados = document.querySelector(identificadorDatosSeleccionados);
        this.datosSeleccionados.value = '';
    }

    obtenerListaMarcados(nombreClaseCategoria) {
        let claseCategoria = document.getElementsByClassName(nombreClaseCategoria);
        let listaMarcados = [];
        for (let i = 0; i < claseCategoria.length; ++i) {
            localStorage.setItem(claseCategoria[i].value, claseCategoria[i].checked);
            if (claseCategoria[i].checked) {
                listaMarcados.push(claseCategoria[i]);
            }
        }
        return listaMarcados
    }

    generarStringSeleccionados() {
        let listaMarcados = this.obtenerListaMarcados(this.nombreClaseCategoria);
        let stringCompleta = this.datosSeleccionados.getAttribute("value");
        if (stringCompleta != '') stringCompleta += ':';
        for (let i = 0; i < listaMarcados.length; ++i) {
            stringCompleta += listaMarcados[i].value;
            if (i != listaMarcados.length - 1) stringCompleta += ',';
        }
        this.datosSeleccionados.value = stringCompleta;
        this.datosSeleccionados.setAttribute("value", stringCompleta);
    }
}

function guardarFiltrosEstadisticasInvolucramiento() {
    selectorCheckboxDia.generarStringSeleccionados();
    selectorCheckboxPublicos.generarStringSeleccionados();
    selectorCheckboxDificultad.generarStringSeleccionados();
}

function load(nombreClaseCategoria) {
    let checkboxes = document.querySelectorAll(nombreClaseCategoria);
    for (let i = 0; i < checkboxes.length; i++) {
        checkboxes[i].checked = localStorage.getItem(checkboxes[i].value) === 'true' ? true : false;
    }
}

