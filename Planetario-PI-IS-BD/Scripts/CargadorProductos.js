

let categoriaActual = '';
let idProductoSeleccionado = '';
let ordenamientoActual = ''
let columnaOrdenamientoActual = '';
let paginaActual = 0;
let paginaMinima = 0;
let paginaMaxima = 0;
let paginasTotales = 0;
let cantidadProductos = 0;
const PRODUCTOS_POR_PAGINA = 6;


async function obtenerProductos2(categoria, columnaOrdenamiento, direccionOrdenamiento) {
    console.log(paginaActual)
    $("#contenedorProductos").html("")
    $("#contenedor-catalogo").show();
    $("#producto-individual").hide();
    $("#contenedor-baner-catalogo").show();
    categoriaActual = categoria;
    ordenamientoActual = direccionOrdenamiento;
    columnaOrdenamientoActual = columnaOrdenamiento;
    actualizarCategoria();
    await $.ajax({
        type: 'POST',
        url: $("#cantidadProductosController").data("request-url"),
        dataType: 'json',

        data: { categoria: categoria, columnaOrdenamiento: columnaOrdenamiento},

        success: function (cantidadProductosEncontrados) {
            cantidadProductosEncontrados = JSON.parse(cantidadProductosEncontrados)[0]['Cantidad de Productos'];
            cantidadProductos = parseInt(cantidadProductosEncontrados);
        }
    })
    await $.ajax({

        type: 'POST',
        url: $("#productoController").data("request-url"),
        dataType: 'json',

        data: { categoria: categoria, columnaOrdenamiento: columnaOrdenamiento, direccionOrdenamiento: direccionOrdenamiento, pagina: paginaActual },

        success: function (productos) {
            productos = JSON.parse(productos);
            console.log(paginaActual)
            console.log(productos)
            let fila = crearFila();
            for (let producto of productos) {
                let columna = crearColumna();
                let carta = crearCarta();
                let cuerpoCarta = crearCuerpoCarta();
                let imagenProducto = document.createElement("img");
                imagenProducto.classList.add(...["card-img-top", "imagen-producto"]);
                imagenProducto.src = "/imagenes/productos/" + producto.NombreFoto;
                imagenProducto.onclick = function () { obtenerProductoIndividual(producto.IdentificadorProducto) }
                let nombreProducto = document.createElement("h5");
                nombreProducto.classList.add(...["card-title","titulo-producto"]);
                nombreProducto.innerHTML = producto.Nombre;
                let precio = document.createElement("p");
                precio.innerHTML = "₡" + producto.Precio;

                let botonComprar = document.createElement("button");
                botonComprar.classList.add(...["btn", "btn-primary", "boton-comprar-producto"]);
                botonComprar.textContent = "Añadir al carrito";
                botonComprar.onclick = function (e) {
                    fetch('/Carrito/AgregarProducto?idItem=' + producto.IdentificadorProducto);
                };
                let br = document.createElement("br");
                cuerpoCarta.appendChild(imagenProducto);
                cuerpoCarta.appendChild(nombreProducto);
                cuerpoCarta.appendChild(precio);
                cuerpoCarta.appendChild(botonComprar);
                cuerpoCarta.appendChild(br);
                carta.appendChild(cuerpoCarta);
                columna.appendChild(carta);
                fila.appendChild(columna);
            }
            $("#contenedorProductos").append(fila);
            actualizarOpcionesPaginas();
        },

        error: function (excepcion) {
            alert('Algo salió mal, vuelve pronto :(' + excepcion);
        }

    });
}

async function obtenerProductoIndividual(idProducto) {
    $("#producto-individual").show();
    $("#contenedor-catalogo").hide();
    $("#contenedor-baner-catalogo").hide();
    await $.ajax({

        type: 'POST',
        url: $("#productoControllerIndividual").data("request-url"),
        dataType: 'json',

        data: { identificadorProducto: idProducto },

        success: function (producto) {
            producto = JSON.parse(producto)[0];
            console.log(producto);
            $("#imagen-producto-individual").attr("src", "/imagenes/productos/" + producto.nombreArchivoImagen);
            $("#nombre-producto-individual").html(producto.nombre);
            $("#precio-producto-individual").html("₡" + producto.precio);
            $("#descripcion-producto-individual").html(producto.descripcion);
            $("#productos-comprados").val(1);
            $("#productos-comprados").attr({ "min": 1 });
            $("#productos-comprados").attr({ "max": producto.unidadesDisponibles });
            $("#boton-carrito-producto-individual").off();
            $("#boton-carrito-producto-individual").click(function (e) {
                fetch('/Carrito/AgregarProducto?idItem=' + producto.idProductoPK + '&cantidad=' + $("#productos-comprados").val());
            });
        },

        error: function (excepcion) {
            alert('Algo salió mal, vuelve pronto :(' + excepcion);
        }

    });
}

function crearFila() {
    let fila = document.createElement("div");
    fila.classList.add("row");
    fila.classList.add("justify-content-center");
    return fila;
}

function crearColumna() {
    let columna = document.createElement("div");
    columna.classList.add(...["col-lg-4"]);
    return columna
}

function crearCarta() {
    let carta = document.createElement("div");
    carta.classList.add(...["card", "card-inicio", "text-center", "border-light", "mb-3", "shadow", "p-3", "mb-5", "bg-white", "rounded",  "d-flex", "align-items-stretch"])
    return carta;
}

function crearCuerpoCarta() {
    let cuerpoCarta = document.createElement("div");
    cuerpoCarta.classList.add(...["card-body", "justify-content-center"]);
    return cuerpoCarta;
}

function actualizarCategoria() {
    let categoriaMostrar = categoriaActual;
    if (categoriaMostrar == '') {
        categoriaMostrar = "Todos los productos";
    }
    $("#titulo-categoria").html(categoriaMostrar);
}

function actualizarOpcionesPaginas() {

    $("#pagina-tercera-opcion").parent().remove()
    $("#pagina-segunda-opcion").parent().remove()
    $("#pagina-primera-opcion").parent().remove()

    let indiceSiguientePaginacion = document.getElementById("pagina-siguiente").parentElement;
    indiceSiguientePaginacion.parentElement.insertBefore(crearIndicePaginacion("pagina-primera-opcion"), indiceSiguientePaginacion);
    indiceSiguientePaginacion.parentElement.insertBefore(crearIndicePaginacion("pagina-segunda-opcion"), indiceSiguientePaginacion);
    indiceSiguientePaginacion.parentElement.insertBefore(crearIndicePaginacion("pagina-tercera-opcion"), indiceSiguientePaginacion);
    paginasTotales = Math.floor(cantidadProductos / PRODUCTOS_POR_PAGINA) + 1;
    if (cantidadProductos % PRODUCTOS_POR_PAGINA == 0) {
        paginasTotales = Math.max(paginasTotales - 1, 0);
    }
    paginaMinima = Math.max(parseInt(paginaActual) - 1, 0);
    if (paginaActual == paginasTotales - 1) {
        paginaMinima = Math.max(parseInt(paginaActual) - 2, 0);
    }
    paginaMaxima = Math.min(parseInt(paginaMinima) + 2, paginasTotales - 1)
    if (parseInt(paginaMaxima) - parseInt(paginaMinima) < 2) {
        $("#pagina-tercera-opcion").parent().remove()
        if (parseInt(paginaMaxima) - parseInt(paginaMinima) < 1) {
            $("#pagina-segunda-opcion").parent().remove();
            $("#pagina-primera-opcion").addClass("pagina-actual");
        }
        $("#pagina-segunda-opcion").attr("value", paginaMaxima);
        $("#pagina-segunda-opcion").html(parseInt(paginaMaxima) + 1);
        if (paginaActual == paginaMinima) {
            $("#pagina-primera-opcion").addClass("pagina-actual")
        }
        else {
            $("#pagina-segunda-opcion").addClass("pagina-actual");
        }
    }
    else {
        $("#pagina-segunda-opcion").attr("value", parseInt(paginaMinima) + 1);
        $("#pagina-segunda-opcion").html(parseInt(paginaMinima) + 2);
        if (paginaActual == paginaMaxima) {
            $("#pagina-tercera-opcion").addClass("pagina-actual");
        } else if (paginaActual == paginaMinima) {
            $("#pagina-primera-opcion").addClass("pagina-actual");
        } else {
            $("#pagina-segunda-opcion").addClass("pagina-actual");
        }
    }

    $("#pagina-tercera-opcion").attr("value", paginaMaxima);
    $("#pagina-tercera-opcion").html(parseInt(paginaMaxima) + 1);
    $("#pagina-primera-opcion").attr("value", paginaMinima);
    $("#pagina-primera-opcion").html(parseInt(paginaMinima) + 1);

    $(".pagina-actual").first().parent().addClass("active");


    $("#pagina-primera-opcion").click(function () { paginaActual = $("#pagina-primera-opcion").attr("value"); obtenerProductos2(categoriaActual, columnaOrdenamientoActual, ordenamientoActual) });
    $("#pagina-segunda-opcion").click(function () { paginaActual = $("#pagina-segunda-opcion").attr("value"); obtenerProductos2(categoriaActual, columnaOrdenamientoActual, ordenamientoActual) });
    $("#pagina-tercera-opcion").click(function () { paginaActual = $("#pagina-tercera-opcion").attr("value"); obtenerProductos2(categoriaActual, columnaOrdenamientoActual, ordenamientoActual) });
}

function crearIndicePaginacion(id) {
    let indicePaginacion = document.createElement("li")
    indicePaginacion.classList.add("page-item");
    let linkDePaginacion = document.createElement("a")
    linkDePaginacion.classList.add("page-link")
    linkDePaginacion.id = id;
    indicePaginacion.appendChild(linkDePaginacion);
    return indicePaginacion;
}


function revisarUnidades() {
    let unidadesSeleccionadas = $("#productos-comprados").val();
    let unidadesDisponibles = $("#productos-comprados").attr('max');
    if (unidadesDisponibles == unidadesSeleccionadas) {
        $("#unidades-disponibles-aviso").html("*Unidades disponibles: " + unidadesDisponibles);
    } else {
        $("#unidades-disponibles-aviso").html("");
    }
}

function reiniciarPaginaActual() {
    paginaActual = 0; 
}
