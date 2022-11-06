class SeleccionadorMultiple {

    constructor(identificadorContenedorSeleccionados, identificadorDatosSeleccionados) {
        this.contenedorSeleccionados = document.querySelector(identificadorContenedorSeleccionados);
        this.datosSeleccionados = document.querySelector(identificadorDatosSeleccionados);
        this.listaTopicos = [];
    }

    agregarSeleccion(elementoSeleccionado) {
        let datosObtenidos = this.datosSeleccionados.getAttribute("value");
        if (!datosObtenidos.includes(elementoSeleccionado)) {
            let tagSeleccion = document.createElement("BUTTON");
            tagSeleccion.setAttribute("id", String(elementoSeleccionado).replaceAll(" ", "-"));
            tagSeleccion.textContent = String(elementoSeleccionado).replace("-", " ")
            tagSeleccion.classList.add("btn", "btn-dark")
            tagSeleccion.style.marginRight = "5px";
            tagSeleccion.addEventListener("click", (tagTrigger) => {
                let tagParaBorrar = tagTrigger.target;
                let textoTag = tagParaBorrar.textContent;
                this.borrarDatoSeleccionado(textoTag);
                tagParaBorrar.parentNode.removeChild(tagParaBorrar);
            });
            this.contenedorSeleccionados.appendChild(tagSeleccion);
            this.agregarDatoSeleccionado(String(elementoSeleccionado));
        }
    }

    eliminarTopicosSegunCategoria(listaTopicos) {
        for (let i = 0; i < listaTopicos.length; i++) {
            let topico = listaTopicos[i].replaceAll(" ", "-");
            this.borrarDatoSeleccionado(topico);
            let tagParaBorrar = document.getElementById(topico);
            tagParaBorrar.parentNode.removeChild(tagParaBorrar);
        }
    }

    async agregarSeleccionEstadistica(elementoSeleccionado) {
        let datosObtenidos = this.datosSeleccionados.getAttribute("value");
        if (!datosObtenidos.includes(elementoSeleccionado)) {
            let tagSeleccion = document.createElement("BUTTON");
            tagSeleccion.textContent = String(elementoSeleccionado).replaceAll("-", " ")
            tagSeleccion.setAttribute("id", String(elementoSeleccionado).replaceAll(" ", "-"));
            tagSeleccion.classList.add("btn", "btn-dark", "btn-sm")
            tagSeleccion.style.marginRight = "5px";
            tagSeleccion.style.marginBottom = "5px";
            tagSeleccion.addEventListener("click", async (tagTrigger) => {
                let tagParaBorrar = tagTrigger.target;
                let textoTag = tagParaBorrar.textContent;
                this.borrarDatoSeleccionado(textoTag);
                tagParaBorrar.parentNode.removeChild(tagParaBorrar);

                if (this.datosSeleccionados.getAttribute("id") == "categoriasSeleccionado") {
                    let listaTopicos = await this.obtenerTopicosSegunCategoria(elementoSeleccionado);
                    this.eliminarTopicosSegunCategoria(listaTopicos);
                }
            });

            this.contenedorSeleccionados.appendChild(tagSeleccion);
            this.agregarDatoSeleccionado(String(elementoSeleccionado));
        }
    }

    async obtenerTopicosSegunCategoria(nombreCategoria) {
        let listaTopicos = [];
        await $.ajax({

            type: 'POST',
            url: $("#estadisticasController").data("request-url"),
            dataType: 'json',

            data: { categoria: nombreCategoria },

            success: function (topicos) {

                $.each(topicos, function (i, topico) {
                    listaTopicos.push(topico.Text);
                });
            },

            error: function (excepcion) {
                alert('No se pudieron recuperar los tópicos correspondientes' + excepcion);
            }

        });
        return listaTopicos;
    }

    borrarDatoSeleccionado(nombreSeleccion) {
        let datosObtenidos = this.datosSeleccionados.getAttribute("value");
        let espacioAReemplazar = nombreSeleccion + ";";
        if (datosObtenidos.includes(nombreSeleccion)) {
            datosObtenidos = datosObtenidos.replace(espacioAReemplazar, "");
        }
        this.datosSeleccionados.value = datosObtenidos;
        this.datosSeleccionados.setAttribute("value", datosObtenidos);
    }

    agregarDatoSeleccionado(nombreSeleccion) {
        let datosObtenidos = this.datosSeleccionados.getAttribute("value");
        if (!datosObtenidos.includes(nombreSeleccion)) {
            datosObtenidos = datosObtenidos + nombreSeleccion + ";";
        }
        this.datosSeleccionados.value = datosObtenidos;
        this.datosSeleccionados.setAttribute("value", datosObtenidos);
    }

}

function agregarTagTopico(elementoSeleccionado) {
    if (elementoSeleccionado != "") {
        selectorMultipleTopicos.agregarSeleccion(elementoSeleccionado);
    }
}

function agregarTagPublicoMeta(elementoSeleccionado) {
    if (elementoSeleccionado != "") {
        selectorMultiplePublicoMeta.agregarSeleccion(elementoSeleccionado);
    }
}

function agregarTagTopicoEstadistica(elementoSeleccionado) {
    if (elementoSeleccionado != "") {
        selectorMultipleTopicos.agregarSeleccionEstadistica(elementoSeleccionado);
    }
}

function agregarTagCategoria(elementoSeleccionado) {
    if (elementoSeleccionado != "") {
        selectorMultipleCategorias.agregarSeleccionEstadistica(elementoSeleccionado);
    }
}

function agregarTagIdioma(elementoSeleccionado) {
    if (elementoSeleccionado != "") {
        selectorMultipleIdiomas.agregarSeleccion(elementoSeleccionado);
    }
}

function agregarTagIdiomaEstadistica(elementoSeleccionado) {
    if (elementoSeleccionado != "") {
        selectorMultipleIdiomas.agregarSeleccionEstadistica(elementoSeleccionado);
    }
}