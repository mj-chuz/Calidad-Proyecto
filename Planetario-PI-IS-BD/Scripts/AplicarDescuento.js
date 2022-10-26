let codigoAplicado = false
async function aplicarDescuento() {
    let campoIngresoCodigo = document.getElementById("campo-ingreso-codigo");
    let codigo = campoIngresoCodigo.value;
    await $.ajax({
        type: 'POST',
        url: $("#boton-aplicar-cupon").data("request-url"),
        dataType: 'json',

        data: { codigoCupon: codigo },

        success: function (datosPrecio) {
            $("espacios-descuentos").show();
            datosPrecio = JSON.parse(datosPrecio)[0];
            let descuento = datosPrecio["Descuento"]
            if (descuento > 0) {
                console.log(datosPrecio)
                let campoDescuento = document.getElementById("descuento-aplicado")
                document.getElementById("nombre-cupon").innerHTML = "Cupón";
                campoDescuento.innerHTML = "₡" + datosPrecio["Descuento"];
                document.getElementById("precio-total").innerHTML = "₡" + datosPrecio["PrecioTotal"];
                campoIngresoCodigo.disabled = true;
                campoIngresoCodigo.placeholder = "Ya se aplicó un cupón";
                document.getElementById("boton-aplicar-cupon").disabled = true;
                if (codigoAplicado) {
                    campoIngresoCodigo.disabled = true;
                    campoIngresoCodigo.placeholder = "Ya se aplicó un cupón"
                }
                document.getElementById("errorCodigoDescuento").innerHTML = "";
            } else {
                document.getElementById("errorCodigoDescuento").innerHTML = "Cupón no válido";
                document.getElementById("errorCodigoDescuento").style.color = 'red';
                document.getElementById("errorCodigoDescuento").style.fontSize = 'small';
            }
        }
    })
}