"use strict";

const lightSpeed = 299792458; // meter (per second)
const solarMass = 1.98855 * Math.pow(10, 30); // kilograms
const gravity = 6.67408 * Math.pow(10, -11);

function calcSchwarzschildRadius(nbSolarMass) {
	return (2 * gravity * nbSolarMass * solarMass) / (lightSpeed * lightSpeed);
}

const Space = {
	blackholes: {
		name: "Agujeros Negros",
		objects: {
            ton_618: { name: "TON 618", nbSolarMass: 66000000000, color: "#000000", type: "black-hole", aka:"Es probable que contenga el agujero negro supermasivo más grande descubierto hasta ahora, quizás con una masa 66 mil millones de veces la masa del sol" },
			h1821_643: { name: "H1821+643", nbSolarMass: 30000000000, color: "#000000", type: "black-hole",aka:"Es el agujero negro masivo medido con la mayor presición hasta la fecha." },
			hercules_a: { name: "Hércules A", nbSolarMass: 4000000000, color: "#000000", type: "black-hole", aka:"Es un millar de veces más másivo que el agujero negro de nuestra galaxia, la Vía Láctea." },
			ngc_3115: { name: "NGC 3115", nbSolarMass: 2000000000, color: "#000000", type: "black-hole", aka:"Fue el cuarto agujero negro supermasivo descubierto." },
			messier_84: { name: "Messier 84", nbSolarMass: 1500000000, color: "#000000", type: "black-hole", aka:"Se encuentra en la galaxia lenticular M84, pesa alrededor de 180 millones de masas solares. " },
			sombrero_galaxy: { name: "Galaxia del Sombrero", nbSolarMass: 1000000000, color: "#000000", type: "black-hole" , aka:"Debido a su inclinación, desde la Tierra tiene forma de sombrero. "},
			messier_49: { name: "Messier 49", nbSolarMass: 560000000, color: "#000000", type: "black-hole",aka:"Su massa estimada es de 2600 millones de masas solares." },
			messier_59: { name: "Messier 59", nbSolarMass: 270000000, color: "#000000", type: "black-hole",  aka:"Se encuentra dentro de una galaxia elítica-lenticular, siendo esta una de las más grandes del Cúmulo de Virgo."},
			messier_81: { name: "Messier 81", nbSolarMass: 70000000, color: "#000000", type: "black-hole", aka: "Se conoce como la galaxia de Bode, alrededor giran más de 250.000 millones de estrellas." },
			sagittarius_astar: { name: "Sagitario A*", nbSolarMass: 4300000, color: "#000000", type: "black-hole", aka: "Se localiza en el centro de la Vía Láctea, en la actualiza se considera que es el agujero negro central de nuestra galaxia." },
		}
	},
	solarSystem: {
		name: "Sistema Solar",
		objects: {
			sun: { name: "Sol", diameter: 1391016, color: "#fff5f1", type: "yellow-dwarf-star", hasImg: true, aka:"El Sol tiene más masa que el 90% de las estrellas en promedio." },
			jupiter: { name: "Júpiter", diameter: 139822, color: "#a4906f", type: "gaz-planet", hasImg: true , aka:"Es el planeta más grande del sistema solar y el quinto más lejano al Sol."},
			saturn: { name: "Saturno", diameter: 116464, color: "#e2cb84", type: "gaz-planet", hasImg: true , aka:"Un año en saturno dura un total de 29..4 años terrestres."},
			uranus: { name: "Urano", diameter: 50724, color: "#c1e7ea", type: "gaz-planet" , aka:"Es el planeta más frío de todo el sistema solar."},
			neptune: { name: "Neptuno", diameter: 49244, color: "#3d66fa", type: "gaz-planet", hasImg: true , aka:"Este fue el primer planeta en ser predicho de manera matemática, es el más pequeño de los gigantes gaseosos." },
			earth: { name: "Tierra", diameter: 12742, color: "#2c3789", type: "rocky-planet", hasImg: true , aka: "Es el planeta más denso de todo el sistema solar, a pesar de ser el quinto de mayor tamaño."},
			venus: { name: "Venus", diameter: 12104, color: "#d79f3c", type: "rocky-planet", hasImg: true , aka:"Es el planeta más brillante del sistema solar."},
			mars: { name: "Marte", diameter: 6779, color: "#ee7f5a", type: "rocky-planet", hasImg: true , aka:"Posee la montaña más alta del sistema solar, que se conoce como el Monte Olimpo, la cual mide 27km."},
			ganymede: { name: "Ganimedes (Luna de Júpiter)", diameter: 5268, color: "#8d7c69", type: "satellite", hasImg: true , aka:"Es la luna más grande del sistema solar, es tan grande que podría ser considerado un planeta." },
			titan: { name: "Titán (Luna de Saturno)", diameter: 5151, color: "#d6c359", type: "satellite", hasImg: true , aka:"En Titán hay volcanes de hielo, conocidos como criovolcanes."},
			mercury: { name: "Mercurio", diameter: 4879, color: "#828086", type: "iron-planet", hasImg: true , aka:"Es el planeta con más cráteres del sistema solar." },
			callisto: { name: "Calisto (Luna de Júpiter)", diameter: 4820, color: "#91775e", type: "satellite", hasImg: true, aka:"Está compuesto un 40% por hielo y un 60% por hierro y roca." },
			io: { name: "Io (Luna de Júpiter)", diameter: 3643, color: "#fbf680", type: "satellite", hasImg: true , aka:"Es el cuerpo celeste más activo de todo el sistema solar." },
			moon: { name: "La Luna", diameter: 3474, color: "#7f7978", type: "satellite", hasImg: true , aka:"Cada año, la Luna se aleja 3.8 cm de la Tierra." },
			europa: { name: "Europa (Luna de Júpiter)", diameter: 3122, color: "#9c7047", type: "satellite", hasImg: true , aka:"Bajo su corteza de hielo, se esconde un enorme océano de agua salada que podría albergar vida." },
			triton: { name: "Triton (Luna de Neptuno)", diameter: 2707, color: "#aea5a6", type: "satellite", hasImg: true , aka:"Esta gira en el sentido contrario a los demás satélites de Neptuno, se conoce como satélite invasor debido a que originalmente no orbitaba Neptuno." },
			pluto: { name: "Plutón", diameter: 2377, color: "#866143", type: "dwarf-planet", hasImg: true ,aka:"Es el único planeta enano del sistema solar que presenta atmósfera."},
			titania: { name: "Titania (Luna de Urano)", diameter: 1577, color: "#c3b7ad", type: "satellite", hasImg: true , aka:"Está compuesto por un 50% de hielo y otro 50% de roca, con un manto de agua líquida."},
			rhea: { name: "Rea (Luna de Saturno)", diameter: 1528, color: "#b2b2b2", type: "satellite", hasImg: true ,aka:"Su atmósfera está compuesta de una delgada capa de oxígeno."},
            oberon: { name: "Oberon (Luna de Urano)", diameter: 1523, color: "#ae9b94", type: "satellite", hasImg: true, aka: "Es el satélite de urano más alejado del planeta" },
            iapetus: { name: "Jápeto (Luna de Saturno)", diameter: 1469, color: "#c3c1b2", type: "satellite", hasImg: true, aka: "Uno de los hemisferios del satélite es mucho más oscuro que el otro, peculiar característica que se podría deber a una composición distinta del material de la superficie" },
            umbriel: { name: "Umbriel (Luna de Úrano)", diameter: 1169, color: "#3d3d3d", type: "satellite", hasImg: true, aka: "La superficie es la más oscura de los satélites principales de Urano (con esto, su nombre recuerda a la palabra latina «umbra», sombra en español) " },
            ariel: { name: "Ariel (Luna de Urano)", diameter: 1158, color: "#b6b6b6", type: "satellite", hasImg: true, aka: "Ariel tiene rotación síncrona (Igual que nuestra luna); es decir, tarda lo mismo en girar sobre sí mismo que alrededor de Urano." },
            dione: { name: "Dione (Luna de Saturno)", diameter: 1123, color: "#c1c1c1", type: "satellite", hasImg: true, aka: "Principalmente compuesto de agua congelada, pero por su densidad seguramente oculta algún material más denso en su interior" },
            tethys: { name: "Tetis (Luna de Saturno)", diameter: 1062, color: "#b0afaf", type: "satellite", hasImg: true, aka: "Tiene la densidad más baja entre los mayores satélites del sistema solar, lo que indica que está compuesto por hielo de agua con una muy pequeña fracción de roca." },
            ceres: { name: "Ceres (Asteroide del cinturón de asteroides)", diameter: 946, color: "#898790", type: "asteroid", hasImg: true, aka: "Originalmente fue considerado un planeta, pero se catalogó como asteroide en la década de 1850 cuando se empezaron a descubrir otros objetos en órbitas similares." },
            enceladus: { name: "Encelado (Luna de Saturno)", diameter: 504, color: "#aeaeae", type: "satellite", hasImg: true, aka: "Probablemente es calentado por muchas fuentes hidrotermales, lo que despierta gran interés al existir las condiciones necesarias para la vida" },
            miranda: { name: "Miranda (Luna de Urano)", diameter: 472, color: "#bbbbbb", type: "satellite", hasImg: true, aka: "Recibe la nombre de la hija del mago Próspero, de la obre de Shakespeare La Tempestad" },
            mimas: { name: "Mimas (Luna de Saturno)", diameter: 396, color: "#8b8b8b", type: "satellite", hasImg: true, aka: "Es reconocida por el gran crater en su superficie" },
        }
    },
    alphaCentauri: {
        name: "Alfa Centauri",
        objects: {
            alpha_centauri_a: { name: "α Centauri A", diameter: 1701769, color: "#fff5f1", type: "yellow-dwarf-star", aka: "Es la cuarta estrella más brillante de nuestro cielo" },
            alpha_centauri_b: { name: "α Centauri B", diameter: 1200725, color: "#ffe0bb", type: "yellow-dwarf-star", aka: "Es menos luminosa que Alpha Centauri A, pero emite mayor energía" },
            alpha_centauri_c: { name: "α Centauri C", diameter: 214495, color: "#ffbf68", type: "red-dwarf-star", aka: "Es la estrella más cercana al sol, pero es demasiado débil para poder ser vista por el ojo" },
        }
    },
    kepler10System: {
        name: "Sistema Kepler-10 ",
        objects: {
            kepler_10: { name: "Kepler-10", diameter: 1481841, color: "#fff5f1", type: "yellow-dwarf-star", aka: "Ligeramente menos masiva, más grande, y más fría que el sol." },
            kepler_10c: { name: "Kepler-10c", diameter: 29944, color: "#f7f7f7", type: "rocky-planet", aka: "Tiene aproximadamente 7 veces la masa de la tierra" },
            kepler_10b: { name: "Kepler-10b", diameter: 18731, color: "#f7f7f7", type: "rocky-planet", aka: 'Es el exoplaneta más pequeño conocido hasta ahora descubierto por el método de "tránsito".' },
        }
    },
    siriusSystem: {
        name: "Sistema Sirio",
        objects: {
            sirius_a: { name: "Sirio A", diameter: 2380685, color: "#eeeefa", type: "am-star", aka: "Es la séptima estrella más cercana respecto al Sol." },
            sirius_b: { name: "Sirio B", diameter: 11688, color: "#d5dff5", type: "white-dwarf-star", aka: "Su tamaño es como el de la tierra" },
        }
    },
    others: {
        name: "Otros",
        objects: {
            uy_scuti: { name: "UY Scuti", diameter: 2376511200, color: "#ffe2c8", type: "red-supergiant-star", aka: "Si esta estrella fuera nuestro Sol, englobaría todos los planetas hasta cerca de Saturno" },
            betelgeuse: { name: "Betelgeuse", diameter: 1234171800, color: "#fca653", type: "red-supergiant-star", aka: "Estallará como supernova dentro de los próximos 100.000 años" },
            antares: { name: "Antáres", diameter: 946152000, color: "#fcaf29", type: "red-supergiant-star", aka: "La más brillante de la constelación de Escorpio" },
            rigel_a: { name: "Rigel A", diameter: 109781460, color: "#bbf1ff", type: "blue-white-supergiant-star", aka: "Está situada en el pie izquierdo de la constelación de Orión" },
            aldebaran: { name: "Aldebarán", diameter: 61402482, color: "#fc8d2e", type: "orange-giant-star", aka: "Es la estrella más brillante de la constelación de Tauro" },
            arcturus: { name: "Arcturus", diameter: 35341560, color: "#df822d", type: "red-giant-star", aka: "Es la tercera estrella más brillante del cielo nocturno" },
            pollux: { name: "Pólux", diameter: 12244320, color: "#eabf80", type: "orange-giant-star", aka: "Es la estrella más brillante de la constelación de Géminis " },
        }
    },
};

const ObjectByName =
    Object.values(Space).reduce((obj, cate) => (
        Object.entries(cate.objects).reduce(
            (obj, [id, spaceObj]) => {
                spaceObj.id = id;
                obj[id] = spaceObj;
                return obj;
            },
            obj)
    ), {});

Object.values(Space.blackholes.objects).forEach(bh => {
    bh.diameter = Math.round(2 * calcSchwarzschildRadius(bh.nbSolarMass) / 1000);
});