function ActualizarFiltrosReporte(tipoReporte) {
    if (tipoReporte == "comprasConjunto") {
        $("#nombre-producto").show();
        $("#nivel-educativo").hide();
        $("#genero").hide();
        $("#dia-semana").hide();
    } else if (tipoReporte == "comprasTipoCliente") {
        $("#nombre-producto").hide();
        $("#nivel-educativo").show();
        $("#genero").show();
        $("#dia-semana").show();
    }
}
