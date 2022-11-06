function ActivarOpcionesDireccion() {
    $("#contenedor-direccion-domicilio").show();
}

function DesactivarOpcionesDireccion() {
    $("#contenedor-direccion-domicilio").hide();
}

async function CargarArchivoJSON(nombreArchivo) {
    let url = "../JSON/" + nombreArchivo + ".json";
    try {
        let res = await fetch(url);
        return await res.json();
    } catch (error) {
        console.log(error);
    }
}

function LimpiarSelecciones(identificador, tipo) {
    $(identificador).empty();
    $(identificador).append("<option selected>" + tipo + "</option>");
}

function CrearElementoOpcion(nombre) {
    let opcion = document.createElement("option");
    opcion.setAttribute('value', nombre);
    opcion.innerHTML = nombre;
    return opcion;
}

async function CargarCantones(provincia) {
    let todosLosCantones = await CargarArchivoJSON("Cantones");
    todosLosCantones = todosLosCantones.Cantones[0];
    let cantonesProvincia = todosLosCantones[provincia];
    LimpiarSelecciones("#cantones", "Cantón");
    LimpiarSelecciones("#distritos", "Distrito");
    for (let canton = 0; canton < cantonesProvincia.length; canton++) {
        let nombreCanton = cantonesProvincia[canton]
        $("#cantones").append(CrearElementoOpcion(nombreCanton));
    }
}

function ObtenerDistritosArchivo(canton, distritosProvincia) {
    for (let indiceCanton = 0; indiceCanton < distritosProvincia.length; indiceCanton++) {
        if (canton == distritosProvincia[indiceCanton]["canton"]) {
            return distritosProvincia[indiceCanton]["distritos"];
        }
    }
}

async function CargarDistritos(canton) {
    let todosLosCantones = await CargarArchivoJSON("Distritos");
    todosLosCantones = todosLosCantones.Distritos[0];
    let provinciaActual = $("#provincias").val();
    let distritos = todosLosCantones[provinciaActual];
    distritos = ObtenerDistritosArchivo(canton, distritos);
    LimpiarSelecciones("#distritos", "Distrito");
    for (let distrito = 0; distrito < distritos.length; distrito++) {
        let nombreDistrito = distritos[distrito];
        $("#distritos").append(CrearElementoOpcion(nombreDistrito));
    }
}
