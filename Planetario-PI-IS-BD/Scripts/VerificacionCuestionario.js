function verificarRespuestas() {
    quiz.checkAnswers();
}

function mostrarResultado() {
    if (quiz.checkAnswers()) {
        var quizNota = quiz.result.scorePercentFormatted;
        var quizResultElement = document.getElementById('quiz-result');
        quizResultElement.style.display = 'block';
        document.getElementById('quiz-percent').innerHTML = quizNota.toString();
        
        quiz.highlightResults(manejarRespuestas);
        mostrarModal(quizNota);
        
    }
}

function manejarRespuestas(quiz, pregunta, numeroPregunta, correcta) {
    if (!correcta) {
        var respuestas = pregunta.getElementsByTagName('input');
        if (respuestas.length == 0) {
            respuestas = pregunta.getElementsByTagName('select');
        }
        for (var i = 0; i < respuestas.length; i++) {
            if (respuestas[i].type === "checkbox" || respuestas[i].type === "radio") {
                if (quiz.answers[numeroPregunta].indexOf(respuestas[i].value) > -1) {
                    respuestas[i].parentNode.classList.add(Quiz.Classes.CORRECT);
                }
            }
            else if (respuestas[i].tagName === "SELECT")
            {
                if (quiz.answers[numeroPregunta][i] !== respuestas[i].value) {
                    var respuestaCorrecta = document.createElement('span');
                    respuestas[i].parentNode.classList.add(Quiz.Classes.CORRECT);
                    respuestaCorrecta.classList.add(Quiz.Classes.TEMP); 
                    respuestaCorrecta.innerHTML = quiz.answers[numeroPregunta][i];
                    respuestaCorrecta.style.marginLeft = '10px';
                    respuestas[i].parentNode.insertBefore(respuestaCorrecta, respuestas[i].nextSibling);

                }
            }
            else {

                var respuestaCorrecta = document.createElement('span');
                respuestaCorrecta.classList.add(Quiz.Classes.CORRECT);
                respuestaCorrecta.classList.add(Quiz.Classes.TEMP);
                respuestaCorrecta.innerHTML = quiz.answers[numeroPregunta];
                respuestaCorrecta.style.marginLeft = '10px';
                respuestas[i].parentNode.insertBefore(respuestaCorrecta, respuestas[i].nextSibling);
            }
        }
    }
}

function salirPopUp() {
    $("#modal-resultado-cuestionario").modal('hide');
}

function mostrarModal(quizNota) {
    $("#modal-resultado-cuestionario").modal('show');
    document.getElementById("imagenMedalla").innerHTML = "";
    document.getElementById("texto-resultado").innerHTML = "";
    let imagenMedalla = crearImagenMedalla(quizNota);
    let elementoTextoResultado = crearTextoResultado(quizNota);
    document.getElementById("imagenMedalla").appendChild(imagenMedalla);
    document.getElementById("texto-resultado").appendChild(elementoTextoResultado);
}

function crearImagenMedalla(quizNota) {
    let imagenMedalla = document.createElement('img');
    let direccionImagen = "../imagenes/medallaOro.png";
    if (quizNota < 90 && quizNota >= 60) {
        direccionImagen = "../imagenes/medallaPlata.png"
    }
    else if (quizNota < 60) {
        direccionImagen = "../imagenes/medallaBronze.png"
    }
    imagenMedalla.src = direccionImagen;
    imagenMedalla.classList.add("img-responsive");
    imagenMedalla.style.width = "50%";
    imagenMedalla.style.height = "auto";
    imagenMedalla.style.marginLeft = "auto"
    imagenMedalla.style.marginRight = "auto"
    return imagenMedalla;
}

function crearTextoResultado(quizNota) {
    let elementoTextoResultado = document.createElement('h3')
    elementoTextoResultado.style.textTransform = "initial"
    let textoResultado = "¡Excelente trabajo! Su nota es:";
    if (quizNota < 90 && quizNota >= 60) {
        textoResultado = "¡Muy Bien! Su nota es:"
    }
    else if (quizNota < 60) {
        textoResultado = "¡Lo puede hacer mejor! Su nota es:"
    }
    textoResultado += " " + quizNota
    elementoTextoResultado.innerHTML = textoResultado;
    return elementoTextoResultado;
}
