class CreadorDeCuestionarios {

    constructor() {
        this.contadorPreguntas = 0;
    }

    inicializarCuestionario() {
        let contenedorCreacionCuestionario = document.getElementById("contenedor-creacion-cuestionario");
        this._agregarContenedorInicial(contenedorCreacionCuestionario);
        this._agregarBotonesCreacionCuestionario();
    }

    _agregarContenedorInicial(contenedorCreacionCuestionario) {
        const contenedorInicial = document.createElement('div');
        contenedorInicial.classList.add(...["card", "card-formulario-creacion-cuestionario", "seccion-nombre-dificultad"]);

        const rowCardBody = this._crearRowCardBody();
        const divInputNombre = this._crearInputNombreCuestionario();
        const divSelectDificultad = this._crearSelectNivelDeDificultad();

        rowCardBody.appendChild(divInputNombre);
        rowCardBody.appendChild(divSelectDificultad);
        contenedorInicial.appendChild(rowCardBody);
        contenedorCreacionCuestionario.appendChild(contenedorInicial);
    }

    _crearRowCardBody(listaClases = []) {
        const rowCardBody = document.createElement('div');
        rowCardBody.classList.add("row", "card-body");
        rowCardBody.classList.add(...listaClases);
        return rowCardBody;
    }


    _crearInputNombreCuestionario() {
        const divInputNombre = document.createElement('div');
        divInputNombre.classList.add("col");
        let labelInputNombre = document.createElement('label');
        labelInputNombre.innerHTML = "Nombre";
        const inputText = this._crearInputText(['form-control'], 'Cuestionario 1', 'nombre-cuestionario');
        divInputNombre.appendChild(labelInputNombre);
        labelInputNombre.appendChild(inputText);

        return divInputNombre;
    }

    _crearInputText(listaClases, placeholder = '', id = '') {
        const inputText = document.createElement('input');
        inputText.classList.add(...listaClases);
        if (id != '') {
            inputText.id = id;
        }
        if (placeholder != '') {
            inputText.placeholder = placeholder;
        }
        return inputText;
    }

    _crearSelectNivelDeDificultad() {
        const divSelectDificultad = document.createElement('div');
        divSelectDificultad.classList.add('col');
        let labelSelectDificultad = document.createElement('label');
        labelSelectDificultad.innerHTML = 'Nivel de dificultad';
        const selectDificultad = this._crearSelect(["Principiante", "Intermedio", "Avanzado"],
            ["Principiante", "Intermedio", "Avanzado"],
            ["form-select"], "Dificultad", "dificultad-cuestionario"
        );
        labelSelectDificultad.appendChild(selectDificultad);
        divSelectDificultad.appendChild(labelSelectDificultad);
        return divSelectDificultad;
    }

    _crearSelect(listaOpciones, listaValores, listaClases = [], opcionDefault = '', id = '') {
        const select = document.createElement('select');
        if (listaValores == undefined || listaValores.length != listaOpciones.length) {
            listaValores = listaOpciones;
        }
        select.classList.add(...listaClases);
        if (opcionDefault != '') {
            const elementoOpcion = document.createElement('option');
            elementoOpcion.hidden = true;
            elementoOpcion.selected = true;
            elementoOpcion.disabled = true;
            elementoOpcion.value = '';
            elementoOpcion.innerHTML = opcionDefault;
            select.appendChild(elementoOpcion);
        }
        for (let i = 0; i < listaOpciones.length; ++i) {
            const elementoOpcion = document.createElement('option')
            elementoOpcion.value = listaValores[i];
            elementoOpcion.innerHTML = listaOpciones[i];
            select.appendChild(elementoOpcion);
        };
        if (id != '') {
            select.id = id;
        }
        return select;
    }

    _agregarBotonesCreacionCuestionario() {
        const contenedorBotones = document.createElement('div');
        contenedorBotones.classList.add(...["botones-creacion-cuestionario", "justify-content-center"]);
        contenedorBotones.appendChild(this._crearBotonAñadirPregunta());
        contenedorBotones.appendChild(document.createElement('br'));
        const formularioEnviar = document.getElementById("formularioEnvio");

        contenedorBotones.appendChild(this._crearBotonEnviarCuestionario());
        formularioEnviar.appendChild(contenedorBotones);
    }

    _crearBotonAñadirPregunta() {
        const botonAñadirPregunta = this._crearBoton(["btn", "btn-primary"], "Añadir pregunta");
        botonAñadirPregunta.id = "boton-añadir-pregunta";
        botonAñadirPregunta.addEventListener('click', this._añadirPregunta.bind(this));

        return botonAñadirPregunta;
    }

    _crearBotonEnviarCuestionario() {
        const botonEnviarCuestionario = document.createElement('input');
        botonEnviarCuestionario.type = 'submit';
        botonEnviarCuestionario.classList.add(...["btn", "btn-primary", "float-end"]);
        botonEnviarCuestionario.value = "Enviar";
        botonEnviarCuestionario.addEventListener("click", generarCuestionario, false)
        return botonEnviarCuestionario;
    }

    _crearBoton(listaClases, texto) {
        const boton = document.createElement("button");
        boton.type = "button";
        boton.classList.add(...listaClases);
        boton.innerHTML = texto;
        return boton;
    }



    _añadirPregunta() {
        const contenedorFormularioCreacionCuestionarios = document.getElementById("contenedor-creacion-cuestionario");
        contenedorFormularioCreacionCuestionarios.appendChild(this._crearPregunta());

    }

    _eliminarPregunta() {
        this.remove();
    }

    _crearPregunta() {
        const preguntaCompleta = document.createElement("div");
        preguntaCompleta.classList.add(...["card", "card-formulario-creacion-cuestionario", "subformulario-creacion-pregunta"]);
        const headerPregunta = this._crearHeaderPregunta();
        preguntaCompleta.appendChild(headerPregunta);
        const cuerpoPregunta = this._сrearCuerpoVacio();
        preguntaCompleta.appendChild(cuerpoPregunta);
        preguntaCompleta.id = this.contadorPreguntas++;

        const botonEliminarPregunta = this._crearBoton(["btn", "btn-primary", "float-end", "boton-eliminar-pregunta"], "Eliminar pregunta");
        botonEliminarPregunta.addEventListener("click", this._eliminarPregunta.bind(preguntaCompleta));
        preguntaCompleta.appendChild(botonEliminarPregunta);
        return preguntaCompleta;
    }

    _сrearCuerpoVacio() {
        const cuerpoPregunta = document.createElement("div");
        cuerpoPregunta.classList.add(...["card-body", "cuerpo-subformulario-creacion-pregunta", "pregunta-vacia"]);
        return cuerpoPregunta;
    }

    _crearCuerpoAsocie() {
        const cuerpoAsocie = document.createElement("div");
        cuerpoAsocie.classList.add(...["card-body", "cuerpo-subformulario-creacion-pregunta", "pregunta-asocie"]);

        const etiquetaRespuestasPosibles = document.createElement("label");
        etiquetaRespuestasPosibles.innerHTML = "Respuestas posibles";

       
        cuerpoAsocie.appendChild(etiquetaRespuestasPosibles);
        const listaRespuestasPosibles = document.createElement("div");
        listaRespuestasPosibles.classList.add(...["lista-respuestas-posibles"]);
        cuerpoAsocie.appendChild(listaRespuestasPosibles);
        cuerpoAsocie.appendChild(this._crearGrupoAgregarRespuestasAsocie());

        const etiquetaOpcionesAsocie = document.createElement("label");
        etiquetaOpcionesAsocie.innerHTML = "Asociaciones";
        cuerpoAsocie.appendChild(etiquetaOpcionesAsocie);

        const contenedorOpcionesAsocie = document.createElement("div");
        contenedorOpcionesAsocie.classList.add("contenedor-opciones-asocie");
        cuerpoAsocie.appendChild(contenedorOpcionesAsocie);

        const botonAgregarOpcion = this._crearBoton(["btn", "btn-primary", "boton-agregar-opciones"], "Agregar opción");
        botonAgregarOpcion.addEventListener("click", this._agregarOpcionAsocie, false);
        cuerpoAsocie.appendChild(botonAgregarOpcion);
        return cuerpoAsocie;
    }

    _crearGrupoAgregarRespuestasAsocie() {
        const grupoAgregarRespuesta = document.createElement("div");
        grupoAgregarRespuesta.classList.add(...["input-group", "mb-3", "agregar-respuesta"])
        const campoRespuestaPosible = this._crearInputText(["form-control", "campo-respuesta-posible"]);
        const botonAgregarRespuesta = this._crearBoton(["btn", "btn-primary", "boton-agregar-respuesta"], "Agregar respuesta");
        botonAgregarRespuesta.addEventListener("click", this._agregarRespuestaPosibleAsocie, false);
        grupoAgregarRespuesta.appendChild(campoRespuestaPosible);
        grupoAgregarRespuesta.appendChild(botonAgregarRespuesta);
        return grupoAgregarRespuesta;
    }

    _agregarRespuestaPosibleAsocie(event) {
        const botonAgregar = event.currentTarget;
        const listaDeRespuestas = botonAgregar.parentElement.parentElement.getElementsByClassName("lista-respuestas-posibles")[0];
        const campoDeTexto = botonAgregar.parentElement.getElementsByClassName("campo-respuesta-posible")[0];
        let creador = new CreadorDeCuestionarios();
        if (campoDeTexto.value != '') {
            listaDeRespuestas.appendChild(creador._crearNuevaRespuestaPosibleAsocie(campoDeTexto, botonAgregar));
        }
    }

    _crearNuevaRespuestaPosibleAsocie(campoDeTexto, botonAgregar) {
        const elementoNuevo = document.createElement("div");
        elementoNuevo.classList.add(...["input-group", "mb-3"]);
        const campoDeshabilitado = this._crearInputText(["form-control", "respuesta-posible-asocie"]);
        campoDeshabilitado.disabled = true;
        campoDeshabilitado.value = campoDeTexto.value;
        const botonBorrarOpcion = creador._crearBoton(["btn", "btn-primary", "boton-eliminar-opcion"], "Eliminar respuesta");
        botonBorrarOpcion.addEventListener("click", this._eliminarRespuestaAsocie);
        elementoNuevo.appendChild(campoDeshabilitado);
        elementoNuevo.appendChild(botonBorrarOpcion);
        let contenidoCampoDeTexto = campoDeTexto.value
        campoDeTexto.value = "";
        creador._agregarRespuestaSelectsAsocie(contenidoCampoDeTexto, botonAgregar.parentElement.parentElement)
        return elementoNuevo;
    }

    _crearCuerpoSeleccionMultiple() {
        return this._crearCuerpoSeleccion("SeleccionMultiple");
    }

    _crearCuerpoSeleccionUnica() {
        return this._crearCuerpoSeleccion("SeleccionUnica");
    }

    _crearCuerpoSeleccion(tipo) {
        let clasePregunta = "pregunta-seleccion-multiple";
        if (tipo == "SeleccionUnica") {
            clasePregunta = "pregunta-seleccion-unica";
        }

        const cuerpoSeleccion = document.createElement("div");
        cuerpoSeleccion.classList.add(...["card-body", "cuerpo-subformulario-creacion-pregunta", clasePregunta]);

        const botonAgregarOpcion = this._crearBoton(["btn", "btn-primary", "boton-agregar-opcion"], "Agregar opcion");
        if (tipo == "SeleccionUnica") {
            botonAgregarOpcion.addEventListener("click", this._agregarOpcionSeleccionUnica, false);
        }
        else if (tipo == "SeleccionMultiple") {
            botonAgregarOpcion.addEventListener("click", this._agregarOpcionSeleccionMultiple, false);
        }
        cuerpoSeleccion.appendChild(botonAgregarOpcion);

        return cuerpoSeleccion;
    }

    _agregarOpcionSeleccionUnica(event) {
        const botonEliminarPregunta = event.currentTarget;
        let creadorCuestionarios = new CreadorDeCuestionarios();
        let id = botonEliminarPregunta.parentElement.parentElement.id;
        botonEliminarPregunta.parentElement.insertBefore(creadorCuestionarios._crearOpcionSeleccionUnica(id), botonEliminarPregunta);
    }

    _agregarOpcionSeleccionMultiple(event) {
        const botonEliminarPregunta = event.currentTarget;
        let creadorCuestionarios = new CreadorDeCuestionarios();
        botonEliminarPregunta.parentElement.insertBefore(creadorCuestionarios._crearOpcionSeleccionMultiple(), botonEliminarPregunta);
    }

    _agregarOpcionAsocie() {
        const botonAgregarOpcionAsocie = event.currentTarget;
        let creadorCuestionarios = new CreadorDeCuestionarios();
        const contenedorOpcionesAsocie = botonAgregarOpcionAsocie.parentElement.getElementsByClassName("contenedor-opciones-asocie")[0];
        const pregunta = contenedorOpcionesAsocie.parentElement;
        contenedorOpcionesAsocie.appendChild(creadorCuestionarios._crearOpcionAsocie(pregunta));
    }

    _crearOpcionAsocie(pregunta) {
        const contenedorOpcion = document.createElement("div");
        contenedorOpcion.classList.add(...["input-group", "mb-3"]);

        const campoDeTexto = this._crearInputText(["form-control", "pregunta-asocie"]);
        contenedorOpcion.appendChild(campoDeTexto);

        const listaOpcionesRespuesta = pregunta.getElementsByClassName("lista-respuestas-posibles")[0];
        let listaValoresOpcionesRespuesta = this._obtenerRespuestasPosiblesAsocie(listaOpcionesRespuesta);
        const selectOpciones = this._crearSelect(listaValoresOpcionesRespuesta, listaValoresOpcionesRespuesta,
            ["form-select", "opcion-asocie"]);
        selectOpciones.style.marginLeft = "5px";
        contenedorOpcion.appendChild(selectOpciones);

        const botonEliminarOpcion = this._crearBoton(["btn", "btn-primary", "boton-eliminar-opcion"], "Eliminar opción");
        botonEliminarOpcion.addEventListener("click", this._eliminarOpcion, false)
        contenedorOpcion.appendChild(botonEliminarOpcion);
        return contenedorOpcion;
    }

    _obtenerRespuestasPosiblesAsocie(listaOpcionesDOM) {
        let listaCamposDeTextoRespuestas = listaOpcionesDOM.getElementsByClassName("respuesta-posible-asocie");
        let listaValoresRespuestas = [];
        for (let respuesta of listaCamposDeTextoRespuestas) {
            listaValoresRespuestas.push(respuesta.value);
        }
        return listaValoresRespuestas;
    }

    _crearOpcionSeleccionMultiple() {
        return this._crearOpcion("SeleccionMultiple")
    }

    _crearOpcionSeleccionUnica(nombre) {
        return this._crearOpcion("SeleccionUnica", nombre)
    }

    _crearOpcion(tipo, nombre = " ") {
        let clase = "opcion-seleccion-multiple";
        let tipoBoton = "checkbox";
        if (tipo == "SeleccionUnica") {
            clase = "opcion-seleccion-unica";
            tipoBoton = "radio";
        }

        const contenedorOpcion = document.createElement("div");
        contenedorOpcion.classList.add(...["input-group", "mb-3", clase]);

        contenedorOpcion.appendChild(this._crearBotonSeleccion(tipoBoton,nombre));

        const campoDeTexto = this._crearInputText(["form-control"]);
        contenedorOpcion.appendChild(campoDeTexto);

        const botonEliminarOpcion = this._crearBoton(["btn", "btn-primary", "boton-eliminar-opcion"], "Eliminar opción");
        botonEliminarOpcion.addEventListener("click", this._eliminarOpcion, false)
        contenedorOpcion.appendChild(botonEliminarOpcion);

        return contenedorOpcion;
    }

    _crearBotonSeleccion(tipoBoton,nombre) {
        const contenedorBotonRadio = document.createElement("div");
        contenedorBotonRadio.classList.add(...["input-group-text"]);
        const botonSeleccion = document.createElement("input");
        botonSeleccion.type = tipoBoton;
        botonSeleccion.name = nombre;
        botonSeleccion.classList.add(...["form-check-input", "input-lg"]);
        contenedorBotonRadio.appendChild(botonSeleccion);
        return contenedorBotonRadio;
    }

    _eliminarRespuestaAsocie(evento) {

        const opcion = evento.target.parentElement.getElementsByClassName("respuesta-posible-asocie")[0].value;
        const pregunta = evento.currentTarget.parentElement.parentElement.parentElement;
        let creador = new CreadorDeCuestionarios();
        creador._eliminarOpcion(evento);
        creador._borrarRespuestaSelectsAsocie(opcion, pregunta);
    }

    _borrarRespuestaSelectsAsocie(opcionAEliminar, pregunta) {
        const asocies = pregunta.getElementsByClassName("opcion-asocie");
        for (let asocie of asocies) {
            for (let opcion of asocie.options) {
                if (opcion.value == opcionAEliminar) {
                    opcion.remove();
                }
            }
        }
    }

    _agregarRespuestaSelectsAsocie(opcionAAgregar, pregunta) {
        const asocies = pregunta.getElementsByClassName("opcion-asocie");
        for (let asocie of asocies) {
            const elementoOpcion = document.createElement('option')
            elementoOpcion.value = opcionAAgregar;
            elementoOpcion.innerHTML = opcionAAgregar;
            asocie.appendChild(elementoOpcion);
        }
    }

    _eliminarOpcion(evento) {
        const boton = evento.currentTarget;
        const grupoOpcion = boton.parentElement;
        grupoOpcion.remove();
    }

    _crearHeaderPregunta() {
        const headerPregunta = document.createElement('div');
        headerPregunta.classList.add(...["row", "card-body", "header-subformulario-creacion-pregunta"]);
        headerPregunta.appendChild(this._crearColumnaTipoPregunta());
        headerPregunta.appendChild(this._crearColumnaEnunciado());
        return headerPregunta;
    }

    _crearColumnaTipoPregunta() {
        const columnaTipo = document.createElement('div');
        columnaTipo.classList.add('col');
        const etiquetaTipo = document.createElement('label');
        etiquetaTipo.innerHTML = "Tipo de pregunta";
        const selectTipo = this._crearSelect(['Seleccion Unica', 'Selección Múltiple', 'Asocie'],
            ["SeleccionUnica", "SeleccionMultiple", "Asocie"], ["form-select", "creacion-tipo-pregunta"], "Tipo de pregunta"
        );
        selectTipo.addEventListener("change", this._cambiarCuerpo.bind(selectTipo));
        etiquetaTipo.appendChild(selectTipo);
        columnaTipo.appendChild(etiquetaTipo);
        return columnaTipo;
    }

    _crearColumnaEnunciado() {
        const columnaEnunciado = document.createElement('div');
        columnaEnunciado.classList.add('col');
        const etiquetaEnunciado = document.createElement('label');
        etiquetaEnunciado.style.width = "100%";
        etiquetaEnunciado.innerHTML = "Enunciado";
        etiquetaEnunciado.appendChild(this._crearInputText(["form-control", "campo-enunciado-pregunta"], "¿Cuál es la distancia de la tierra al sol?"));
        columnaEnunciado.appendChild(etiquetaEnunciado);
        return columnaEnunciado;
    }

    _cambiarCuerpo() {
        const select = this;
        let valorSeleccionado = select.value;
        const headerPregunta = select.parentElement.parentElement.parentElement;
        const contenedorPregunta = headerPregunta.parentElement;
        const cuerpoAnterior = contenedorPregunta.getElementsByClassName("cuerpo-subformulario-creacion-pregunta")[0];
        cuerpoAnterior.remove();
        let creador = new CreadorDeCuestionarios();
        creador._ponerNuevoCuerpoPregunta(contenedorPregunta,valorSeleccionado)
    }

    _ponerNuevoCuerpoPregunta(contenedorPregunta,valorSeleccionado) {
        const botonEliminarPregunta = contenedorPregunta.getElementsByClassName("boton-eliminar-pregunta")[0];
        if (valorSeleccionado == "SeleccionUnica") {
            contenedorPregunta.insertBefore(this._crearCuerpoSeleccionUnica(), botonEliminarPregunta);
        } else if (valorSeleccionado == "SeleccionMultiple") {
            contenedorPregunta.insertBefore(this._crearCuerpoSeleccionMultiple(), botonEliminarPregunta);
        } else if (valorSeleccionado == "Asocie") {
            contenedorPregunta.insertBefore(this._crearCuerpoAsocie(), botonEliminarPregunta);
        } else {
            contenedorPregunta.insertBefore(this._сrearCuerpoVacio(), botonEliminarPregunta);
        }
    }


    _crearInputNombreCuestionario() {
        const divInputNombre = document.createElement('div');
        divInputNombre.classList.add("col");
        let labelInputNombre = document.createElement('label');
        labelInputNombre.innerHTML = "Nombre";
        const inputText = this._crearInputText(['form-control'], 'Cuestionario 1', 'nombre-cuestionario');
        divInputNombre.appendChild(labelInputNombre);
        labelInputNombre.appendChild(inputText);
        
        return divInputNombre;
    }

    _crearInputText(listaClases, placeholder = '', id = '') {
        const inputText = document.createElement('input');
        inputText.classList.add(...listaClases);
        if (id != '') {
            inputText.id = id;
        }
        if (placeholder != '') {
            inputText.placeholder = placeholder;
        }
        return inputText;
    }

    _crearSelectNivelDeDificultad() {
        const divSelectDificultad = document.createElement('div');
        divSelectDificultad.classList.add('col');
        let labelSelectDificultad = document.createElement('label');
        labelSelectDificultad.innerHTML = 'Nivel de dificultad';
        const selectDificultad = this._crearSelect(["Principiante", "Intermedio", "Avanzado"],
            ["Principiante", "Intermedio", "Avanzado"],
            ["form-select"], "Dificultad", "dificultad-cuestionario"
        );
        labelSelectDificultad.appendChild(selectDificultad);
        divSelectDificultad.appendChild(labelSelectDificultad);
        return divSelectDificultad;
    }

    _crearSelect(listaOpciones, listaValores, listaClases = [], opcionDefault = '', id = '') {
        const select = document.createElement('select');
        if (listaValores == undefined || listaValores.length != listaOpciones.length) {
            listaValores = listaOpciones;
        }
        select.classList.add(...listaClases);
        if (opcionDefault != '') {
            const elementoOpcion = document.createElement('option');
            elementoOpcion.hidden = true;
            elementoOpcion.selected = true;
            elementoOpcion.disabled = true;
            elementoOpcion.value = '';
            elementoOpcion.innerHTML = opcionDefault;
            select.appendChild(elementoOpcion);
        }
        for (let i = 0; i < listaOpciones.length; ++i) {
            const elementoOpcion = document.createElement('option')
            elementoOpcion.value = listaValores[i];
            elementoOpcion.innerHTML = listaOpciones[i];
            select.appendChild(elementoOpcion);
        };
        if (id != '') {
            select.id = id;
        }
        return select;
    }

    _validarCampo(evento) {
        if (evento.currentTarget.value == '') {
            evento.currentTarget.setCustomValidity("Por favor rellene este campo")
        }
        else {
            evento.currentTarget.setCustomValidity("")
        }
    }

}

function recuperarRespuestasAsocie(preguntaObjeto, cuerpoPregunta) {
    preguntaObjeto.Respuesta = [];
    preguntaObjeto.Preguntas = [];
    preguntaObjeto.Opciones = [];
    const opciones = cuerpoPregunta.getElementsByClassName("respuesta-posible-asocie");
    for (let opcion of opciones) {
        preguntaObjeto.Opciones.push(opcion.value);
    }
    const preguntas = cuerpoPregunta.getElementsByClassName("pregunta-asocie");
    for (let pregunta of preguntas) {
        preguntaObjeto.Preguntas.push(pregunta.value);
    }
    const respuestas = cuerpoPregunta.getElementsByClassName("opcion-asocie");
    for (let respuesta of respuestas) {
        preguntaObjeto.Respuesta.push(respuesta.value);
    }
    if (preguntaObjeto.Respuesta.length == 1) preguntaObjeto.Respuesta = preguntaObjeto.Respuesta[0];
}

function recuperarRespuestasSeleccionUnica(preguntaObjeto, cuerpoPregunta) {
    preguntaObjeto.Respuesta = [];
    preguntaObjeto.Opciones = [];
    const opciones = cuerpoPregunta.getElementsByClassName("opcion-seleccion-unica");
    for (let opcion of opciones) {
        let checkbox = opcion.getElementsByClassName("form-check-input")[0];
        let textbox = opcion.getElementsByClassName("form-control")[0];
        preguntaObjeto.Opciones.push(textbox.value);
        if (checkbox.checked) {
            preguntaObjeto.Respuesta.push(textbox.value);
        }
    }
    if (preguntaObjeto.Respuesta.length == 1) preguntaObjeto.Respuesta = preguntaObjeto.Respuesta[0];
}

function validarFormulario(evento) {

    let formularioLlenoCorrectamente = true;
    

    borrarMensajesTemporales();

    formularioLlenoCorrectamente = validarNombreCuestionario();
    formularioLlenoCorrectamente = validarDificultadCuestionario() && formularioLlenoCorrectamente;
    formularioLlenoCorrectamente = validarCantidadPreguntas() && formularioLlenoCorrectamente;
    formularioLlenoCorrectamente = validarPreguntas() && formularioLlenoCorrectamente;
    
    evento.returnValue = formularioLlenoCorrectamente;
}

function validarNombreCuestionario() {
    let validacionNombre = true;
    const elementoNombreCuestionario = document.getElementById("nombre-cuestionario");
    const elementoDificultadCuestionario = document.getElementById("dificultad-cuestionario")
    if (elementoNombreCuestionario.value == '') {
        let mensajeError = crearMensajeError("Por favor inserte un nombre")
        elementoNombreCuestionario.parentElement.appendChild(mensajeError);
        elementoNombreCuestionario.addEventListener("change", function () { mensajeError.remove() })
        validacionNombre = false;
    }
    let nombreArchivoGenearado = elementoNombreCuestionario.value + ".json";
    if (nombresCuestionariosPorDificultad[elementoDificultadCuestionario.value] != undefined && nombresCuestionariosPorDificultad[elementoDificultadCuestionario.value].includes(nombreArchivoGenearado)) {
        let mensajeError = crearMensajeError("El nombre ya ha sido usado")
        elementoNombreCuestionario.parentElement.appendChild(mensajeError);
        elementoNombreCuestionario.addEventListener("change", function () { mensajeError.remove() })
        validacionNombre = false;
    }
    return validacionNombre;
}

function validarDificultadCuestionario() {
    let validacionDificultad = true;
    const elementoDificultadCuestionario = document.getElementById("dificultad-cuestionario")
    if (elementoDificultadCuestionario.value == '') {
        let mensajeError = crearMensajeError("Por favor seleccione una dificultad")
        elementoDificultadCuestionario.parentElement.appendChild(mensajeError);
        elementoDificultadCuestionario.addEventListener("change", function () { mensajeError.remove() })
        validacionDificultad = false;
    }
    return validacionDificultad;
}

function validarCantidadPreguntas() {
    let validacionCantidadPreguntas = true;
    const contenedorCuestionario = document.getElementById("contenedor-creacion-cuestionario");
    let listaPreguntas = contenedorCuestionario.getElementsByClassName("subformulario-creacion-pregunta");
    if (listaPreguntas.length == 0) {
        validacionCantidadPreguntas = false;
        const botonAñadirPregunta = document.getElementById("boton-añadir-pregunta");
        let mensajeError = crearMensajeError("Debe agregar al menos una pregunta");
        botonAñadirPregunta.parentElement.appendChild(mensajeError);
        botonAñadirPregunta.addEventListener("click", function () { mensajeError.remove() });
    }
    return validacionCantidadPreguntas;
}

function validarPreguntas() {
    let validacionPreguntas = true;
    const contenedorCuestionario = document.getElementById("contenedor-creacion-cuestionario");
    let listaPreguntas = contenedorCuestionario.getElementsByClassName("subformulario-creacion-pregunta");
    for (let pregunta of listaPreguntas) {
        let preguntaEsCorrecta = true;
        let headerPregunta = pregunta.getElementsByClassName("header-subformulario-creacion-pregunta")[0];
        let cuerpoPregunta = pregunta.getElementsByClassName("cuerpo-subformulario-creacion-pregunta")[0];
        let tipoPregunta = headerPregunta.getElementsByClassName("creacion-tipo-pregunta")[0].value;
        let headerEsCorrecto = validarHeaderPregunta(headerPregunta);
        if (tipoPregunta == "Asocie") {
            preguntaEsCorrecta = validarRespuestasAsocie(cuerpoPregunta, headerPregunta)
        } else if (tipoPregunta == "SeleccionMultiple") {
            preguntaEsCorrecta = validarRespuestasSeleccion(cuerpoPregunta, headerPregunta, tipoPregunta);
        } else if (tipoPregunta == "SeleccionUnica") {
            preguntaEsCorrecta = validarRespuestasSeleccion(cuerpoPregunta, headerPregunta, tipoPregunta);
        }
        if (!preguntaEsCorrecta || !headerEsCorrecto) validacionPreguntas = false;
    }
    return validacionPreguntas
}

function validarHeaderPregunta(headerPregunta) {
    let preguntaEsCorrecta = true;
    const seleccionadorTipoPregunta = headerPregunta.getElementsByClassName("creacion-tipo-pregunta")[0];
    const enunciadoPregunta = headerPregunta.getElementsByClassName("campo-enunciado-pregunta")[0];
    preguntaEsCorrecta = validarTipoPregunta(seleccionadorTipoPregunta) && preguntaEsCorrecta;
    preguntaEsCorrecta = validarEnunciadoPregunta(enunciadoPregunta) && preguntaEsCorrecta;
    return preguntaEsCorrecta;
}

function validarTipoPregunta(seleccionadorTipoPregunta) {
    let validacionTipoPregunta = true;
    if (seleccionadorTipoPregunta.value == '') {
        let mensajeError = crearMensajeError("Por favor seleccione un tipo de pregunta");
        seleccionadorTipoPregunta.parentElement.appendChild(mensajeError);
        seleccionadorTipoPregunta.addEventListener("change", function () { mensajeError.remove() });
        validacionTipoPregunta = false;
    }
    return validacionTipoPregunta;
}

function validarEnunciadoPregunta(enunciadoPregunta) {
    let validacionEnunciadoPregunta = true;
    if (enunciadoPregunta.value == '') {
        let mensajeError = crearMensajeError("Por favor escriba el enunciado de la pregunta");
        enunciadoPregunta.parentElement.appendChild(mensajeError);
        enunciadoPregunta.addEventListener("change", function () { mensajeError.remove() })
        validacionEnunciadoPregunta = false;
    }
    return validacionEnunciadoPregunta;
}

function validarRespuestasSeleccion(cuerpoPregunta, headerPregunta, tipoSeleccion) {
    let claseSeleccion = "opcion-seleccion-multiple";
    if (tipoSeleccion === "SeleccionUnica")
        claseSeleccion = "opcion-seleccion-unica";
    let preguntaEsCorrecta = true;
    const opciones = cuerpoPregunta.getElementsByClassName(claseSeleccion);
    let opcionSeleccionada = false;
    let mensajeErrorCheckbox = crearMensajeError("Debe marcar al menos una opcion");
    for (let opcion of opciones) {
        let checkbox = opcion.getElementsByClassName("form-check-input")[0];
        checkbox.addEventListener("change", function () { mensajeErrorCheckbox.remove() });
        let textbox = opcion.getElementsByClassName("form-control")[0];
        if (textbox.value == '' || textbox.value == null) {
            let mensajeError = crearMensajeError("Debe rellenar este campo o eliminarlo");
            textbox.parentElement.parentElement.insertBefore(mensajeError, textbox.parentElement);
            textbox.addEventListener("change", function () { mensajeError.remove() })
            const botonDeEliminar = textbox.parentElement.getElementsByClassName("boton-eliminar-opcion")[0];
            botonDeEliminar.addEventListener("click", function () { mensajeError.remove() })
            preguntaEsCorrecta = false;
        }
        if (checkbox.checked) {
            opcionSeleccionada = true;
        }
    }
    if (!opcionSeleccionada && opciones.length > 0) {
        headerPregunta.appendChild(mensajeErrorCheckbox);
        preguntaEsCorrecta = false;
    }
    if (opciones.length == 0) {
        let mensajeErrorPocasOpciones = crearMensajeError("Debe agregar al menos una opcion");
        headerPregunta.appendChild(mensajeErrorPocasOpciones);
        const botonAgregar = cuerpoPregunta.getElementsByClassName("boton-agregar-opcion")[0];
        botonAgregar.addEventListener("click", function () { mensajeErrorPocasOpciones.remove() })
        preguntaEsCorrecta = false;
    }
    return preguntaEsCorrecta;
}

function crearMensajeError(mensaje) {
    const grupoMensajeError = document.createElement("div");
    grupoMensajeError.classList.add("mensaje-temporal-formulario");
    const spanError = document.createElement("span");
    spanError.classList.add("badge", "bg-danger");
    spanError.innerHTML = mensaje;
    grupoMensajeError.appendChild(document.createElement("br"));
    grupoMensajeError.appendChild(spanError);
    return grupoMensajeError;
}

function borrarMensajesTemporales() {
    let mensajesTemporales = document.getElementsByClassName("mensaje-temporal-formulario");
    while (mensajesTemporales.length > 0) {
        mensajesTemporales[0].remove();
    }
}

function recuperarRespuestasSeleccionMultiple(preguntaObjeto, cuerpoPregunta) {
    preguntaObjeto.Respuesta = [];
    preguntaObjeto.Opciones = [];
    const opciones = cuerpoPregunta.getElementsByClassName("opcion-seleccion-multiple");
    let opcionSeleccionada = false;
    for (let opcion of opciones) {
        let checkbox = opcion.getElementsByClassName("form-check-input")[0];
        let textbox = opcion.getElementsByClassName("form-control")[0];
        if (textbox.value == '' || textbox.value == null) {
            let mensajeError = document.createElement("div");
            mensajeError.classList.add("invalid-feedback", "texto-temporal-formulario");
            mensajeError.innerHTML = "Por favor rellene este campo o eliminelo";
            textbox.addEventListener("change", function () { eliminarElemento(mensajeError) }, false);
            textbox.parentElement.insertBefore(mensajeError, textbox);
        }
        preguntaObjeto.Opciones.push(textbox.value);
        if (checkbox.checked) {
            preguntaObjeto.Respuesta.push(textbox.value);
            opcionSeleccionada = true;
        }
    }
}

function generarCuestionario() {
    const contenedorCuestionario = document.getElementById("contenedor-creacion-cuestionario");
    let cuestionario = {};
    cuestionario.Nombre = document.getElementById("nombre-cuestionario").value;

    cuestionario.Dificultad = document.getElementById("dificultad-cuestionario").value;
    cuestionario.Preguntas = [];
    let listaPreguntas = contenedorCuestionario.getElementsByClassName("subformulario-creacion-pregunta");
    for (let pregunta of listaPreguntas) {
        let preguntaObjeto = {};
        let headerPregunta = pregunta.getElementsByClassName("header-subformulario-creacion-pregunta")[0];
        let cuerpoPregunta = pregunta.getElementsByClassName("cuerpo-subformulario-creacion-pregunta")[0];
        preguntaObjeto.Tipo = headerPregunta.getElementsByClassName("creacion-tipo-pregunta")[0].value;
        preguntaObjeto.Pregunta = headerPregunta.getElementsByClassName("form-control")[0].value;
        if (preguntaObjeto.Tipo == "Asocie") {
            recuperarRespuestasAsocie(preguntaObjeto, cuerpoPregunta)
        } else if (preguntaObjeto.Tipo == "SeleccionMultiple") {
            recuperarRespuestasSeleccionMultiple(preguntaObjeto, cuerpoPregunta);
        } else if (preguntaObjeto.Tipo == "SeleccionUnica") {
            recuperarRespuestasSeleccionUnica(preguntaObjeto, cuerpoPregunta);
        }
        cuestionario.Preguntas.push(preguntaObjeto);
    }
    const campoCuestionario = document.getElementById("jsonCuestionario");
    campoCuestionario.value = JSON.stringify(cuestionario);
}

function validarRespuestasAsocie(cuerpoPregunta) {
    let preguntaEsCorrecta = true;
    const opciones = cuerpoPregunta.getElementsByClassName("respuesta-posible-asocie");
    const preguntas = cuerpoPregunta.getElementsByClassName("pregunta-asocie");
    const respuestas = cuerpoPregunta.getElementsByClassName("opcion-asocie");

    preguntaEsCorrecta = validarCantidadOpciones(cuerpoPregunta,opciones) && preguntaEsCorrecta;
    preguntaEsCorrecta = validarCantidadPreguntasAsocie(cuerpoPregunta,preguntas) && preguntaEsCorrecta;
    preguntaEsCorrecta = validarAsocieRespuestaOpcion(preguntas, respuestas) && preguntaEsCorrecta;
    return preguntaEsCorrecta;
}

function validarCantidadOpciones(cuerpoPregunta,opciones) {
    let validacionCantidadOpciones = true;
    if (opciones.length == 0) {
        const listaRespuestasPosible = cuerpoPregunta.getElementsByClassName("lista-respuestas-posibles")[0];
        const botonAgregarRespuesta = cuerpoPregunta.getElementsByClassName("boton-agregar-respuesta")[0];
        let mensajeError = crearMensajeError("Debe agregar al menos una respuesta posible");
        listaRespuestasPosible.parentElement.insertBefore(mensajeError, listaRespuestasPosible);
        botonAgregarRespuesta.addEventListener("click", function () { mensajeError.remove() });
        validacionCantidadOpciones = false;
    }
    return validacionCantidadOpciones;
}

function validarCantidadPreguntasAsocie(cuerpoPregunta,preguntas) {
    let validacionCantidadPreguntas = true;
    if (preguntas.length == 0) {
        const listaAsocies = cuerpoPregunta.getElementsByClassName("contenedor-opciones-asocie")[0];
        const botonAgregarOpciones = cuerpoPregunta.getElementsByClassName("boton-agregar-opciones")[0];
        let mensajeError = crearMensajeError("Debe agregar al menos una opcion");
        listaAsocies.parentElement.insertBefore(mensajeError, listaAsocies);
        botonAgregarOpciones.addEventListener("click", function () { mensajeError.remove() });
        validacionCantidadPreguntas = false;
    }
    return validacionCantidadPreguntas;
}

function validarAsocieRespuestaOpcion(preguntas, respuestas) {
    let validacionAsocieRespuestaOpcion = true;
    for (let indice = 0; indice < preguntas.length && indice < respuestas.length; indice++) {
        if (preguntas[indice].value == '' || respuestas[indice].value == '') {
            let mensajeError = crearMensajeError("Debe rellenar todos los campos de la opcion");
            preguntas[indice].parentElement.parentElement.insertBefore(mensajeError, preguntas[indice].parentElement);
            const botonEliminarOpcion = preguntas[indice].parentElement.getElementsByClassName("boton-eliminar-opcion")[0];
            botonEliminarOpcion.addEventListener("click", function () { mensajeError });
            validacionAsocieRespuestaOpcion = false;
        }
    }
    return validacionAsocieRespuestaOpcion;
}