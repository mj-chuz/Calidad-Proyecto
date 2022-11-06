class VerificadorCamposSeleccionFormulario {
    constructor(campoRelleno, camposSeleccionados, parrafoError) {
        this.campoRelleno = campoRelleno;
        this.camposSeleccionados = camposSeleccionados;
        this.parrafoError = parrafoError;
    }

    revisarCampoSeleccionMultiple() {
        let tagRelleno = document.getElementById(this.campoRelleno);
        let elementosSeleccionados = document.getElementById(this.camposSeleccionados);
        if (elementosSeleccionados.getAttribute("value") != "") {
            tagRelleno.setAttribute("value", "--");
        } else {
            let parrafoError = document.getElementById(this.parrafoError);
            parrafoError.textContent = "Es necesario que seleccione al menos una opción";
            parrafoError.style.color = "#E85050";
        }
    }
    revisarCampoSeleccionSimple() {
        let tagRelleno = document.getElementById(this.campoRelleno);
        let elementoSeleccionado = document.getElementById(this.camposSeleccionados).value;
        if (elementoSeleccionado != "" && elementoSeleccionado != undefined) {
            tagRelleno.setAttribute("value", "--");
        } else {
            let parrafoError = document.getElementById(this.parrafoError);
            parrafoError.textContent = "Es necesario que seleccione una opción";
            parrafoError.style.color = "#E85050";
        }
    }
}