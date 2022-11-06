function filtrarTopicos(idContenedorUrl) {
    $("#listaTopicos").empty();
    idContenedorUrl = "#" + idContenedorUrl;
        if ($("#categoriaSeleccionada").val() != '') {
            $.ajax({
                type: 'POST',
                url: $(idContenedorUrl).data("request-url"),
                dataType: 'json',
                data: { categoria: $("#categoriaSeleccionada").val() },
                success: function (topicos) {
                    $("#listaTopicos").append('<option value="---"></option>');
                    $.each(topicos, function (i, topico) {
                        $("#listaTopicos").append('<option value=' + topico.Value.replace(" ", "-") + '>' + topico.Text + '</option>');
                    });
                },
                error: function (excepcion) {
                    alert('No se pudieron recuperar los tópicos correspondientes' + excepcion);
                }
            });
            return false;
        } else {
            $("#listaTopicos").empty();
        }
}

function limpiarContenedorTopicos() {
    $("#contenedor-topicos-seleccionados").empty();
    $("#topicosSeleccionados").val("");
}

function filtrarTopicosActividad(idContenedorUrl) {
    limpiarContenedorTopicos();
    filtrarTopicos(idContenedorUrl);
}

