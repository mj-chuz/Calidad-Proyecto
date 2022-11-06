

const coloresDefault = ['rgba(255, 99, 132, 0.2)',
'rgba(54, 162, 235, 0.2)',
    'rgba(255, 206, 86, 0.2)',
    'rgba(75, 192, 192, 0.2)',
    'rgba(153, 102, 255, 0.2)',
    'rgba(255, 159, 64, 0.2)',
    'rgba(204, 245, 0, 0.2)'
]
function obtenerColoresGraficoDiasDeLaSemada(listaDias) {
    let listaColores = [];
    let diccionarioDiaColor = {
        Lunes : 'rgba(255, 99, 132, 0.2)',
        Martes : 'rgba(54, 162, 235, 0.2)',
        Miércoles : 'rgba(255, 206, 86, 0.2)',
        Jueves : 'rgba(75, 192, 192, 0.2)',
        Viernes : 'rgba(153, 102, 255, 0.2)',
        Sábado : 'rgba(255, 159, 64, 0.2)',
        Domingo : 'rgba(204, 245, 0, 0.2)'
    }
    for (let dia of listaDias) {
        listaColores.push(diccionarioDiaColor[dia]);
    }
    return listaColores;
}

function crearGrafico(idChart, nombreGrafico, data, etiquetas, listaColores, maximoSugerido = 0) {
    let elementoDOM=document.getElementById(idChart).getContext('2d');
    let grafico = new Chart(elementoDOM, {
        type: 'bar',
        data: {
            labels: etiquetas,
            datasets: [{
                label: "Visitantes",
                data: data,
                backgroundColor: 
                    listaColores
                ,
            borderColor: 
                    listaColores
                ,
            }]
        },
        options: {
            plugins: {
                title: {
                    display: true,
                    text: "Inscripciones " + nombreGrafico,
                },
                legend : false
            },
            scale: {
                y: {
                    beginAtZero: true,
                    suggestedMax: maximoSugerido,
                    
                },
                ticks: {
                    precision: 0
                },
            },
            responsive: true,
            maintainAspectRatio: false,
        }
    });
}

function generarGraficos(tablaInvolucramiento) {
    let publicoAnterior = "";
    let dificultadAnterior = "";
    let data = [];
    let etiquetas = [];
    let nombreGrafico = tablaInvolucramiento[0]["Publico meta"] + " " + tablaInvolucramiento[0]["Nivel de complejidad"] 
    let idChart = nombreGrafico.replaceAll(" ", "-");
    let todosLosValores = [];
    for (let fila of tablaInvolucramiento) {
        todosLosValores.push(fila["Cantidad de personas"]);
    }
    const valorMaximo = Math.max(...todosLosValores);
    for (let i = 0; i < tablaInvolucramiento.length; ++i){
        let fila = tablaInvolucramiento[i];
        let publico = fila["Publico meta"]
        let dificultad = fila["Nivel de complejidad"]
        let llegaATablaNueva = publico != publicoAnterior || dificultad != dificultadAnterior;  
        if (llegaATablaNueva) {
            if (i != 0) {
                let colores = obtenerColoresGraficoDiasDeLaSemada(etiquetas);
                crearGrafico(idChart, nombreGrafico, data, etiquetas, colores, valorMaximo);
                data = []
                etiquetas = []
            }
            nombreGrafico = publico + " " + dificultad
            idChart = (nombreGrafico).replaceAll(' ', '-');
        }
        data.push(fila["Cantidad de personas"]);
        etiquetas.push(fila['Dia de la semana']);
        if (i == tablaInvolucramiento.length - 1) {
            let colores = obtenerColoresGraficoDiasDeLaSemada(etiquetas);
            crearGrafico(idChart, nombreGrafico, data, etiquetas, colores, valorMaximo);
            data = []
            etiquetas = []
        }
        publicoAnterior = publico;
        dificultadAnterior = dificultad;
    }
}

function generarGraficoPublicoCategorias(informacionPublicoCategorias) {
    let nombreGrafico = "Publico por categorias"
    let idGraficos = "graficoPublicoCategoria";
    let cantidadesDeVisitantes = [];
    let etiquetas = [];
    informacionPublicoCategorias.forEach(fila => {
        cantidadesDeVisitantes.push(fila["Cantidad de personas"]);
        etiquetas.push(fila["Nombre categoria"]);
    });
    crearGrafico(idGraficos, nombreGrafico, cantidadesDeVisitantes, etiquetas, coloresDefault);
}

function generarGraficoPublicoTopicos(informacionPublicoTopicos) {
    let nombreGrafico = "Publico por topicos"
    let idGraficos = "graficoPublicoTopico";
    let cantidadesDeVisitantes = [];
    let etiquetas = [];
    informacionPublicoTopicos.forEach(fila => {
        cantidadesDeVisitantes.push(fila["Cantidad de personas"]);
        etiquetas.push(fila["Nombre topico"]);
    });
    crearGrafico(idGraficos, nombreGrafico, cantidadesDeVisitantes, etiquetas, coloresDefault);
}