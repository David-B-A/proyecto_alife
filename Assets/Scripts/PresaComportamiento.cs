using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PresaComportamiento : MonoBehaviour {

	public GameObject presa;
	//public GameObject predador;
	public float vida = 1000;
	public float capacidadDeAlmacenamiento = 800;
	public string caminataActual = "";
	public float vision = 10;
	private float distantanciaParaIrPorUnGrano =8;
	private float distanciaLSystem = 1;
	private float distanciaInternaBoids = 0.5f;
	private int tiempoParaBuscarDir = 1;
	private int vidaParaIrPorComida = 600;
	private int vidaCritica = 300;
	private int vidaSuperCritica = 60;
	public bool fertil = false;
	public int diferenciaMaximaAceptada = 15;

	public bool reproduccionHabilidada = true;
	public bool alimentarse = true;

	public int edad = 0;
	public int edadMaxima = 8000;
	private int tiempoDeMorir;
	public int metabolismo = 1;
	public int tiempoDeEsperaEntreReproducciones = 100;
	public int tiempoActual;
	public int tiempoParaReproducirse = 100;
	public int comidaMinimaParaReproducirse = 300;
	private System.Random random;

	private float parecidoConLaActual;
	// Use this for initialization

	private int tiempoEnDireccionActual=0;
	private int tiempoParaCambioDeDireccionCaminataAleatoria=50;

	public bool metabolismoPorCodigoGenetico = false;

	public BitArray codigoGenetico;

	private Quaternion direccionAPareja;
	private Quaternion direccionCaminataAleatoria;
	private Quaternion direccionAComida;
	private Quaternion direccionBoids;
	private Quaternion direccionEsquivarLSystem;
	private Quaternion direccionEsquivarCompa;
	private Quaternion direccionDepredador;

	private Quaternion[] direccionAmigos;

	//Vector3 direccionAComida;
	float rapidezCambioDir = 0.4f;
	//CHOQUE PARED
	private bool choqueParedOCompa = false;
	private int tiempoDejarDeAlejarse = 0;
	private int intervaloAlejarseDePared = 8;
	private int intervaloAlejarseDeCompa = 2;
	//CHOQUE PARED

	//Estabilizar Reproducción
	public bool recienReproducido = false;
	private int tiempoRecienReproducido = 0;
	//Estabilizar Reproducción

	private int calcularFenotipo(BitArray codGen, int c) {
		int a = c * 4;
		string crom = String.Concat (Convert.ToInt32(codGen [a+0]),Convert.ToInt32(codGen [a+1]),Convert.ToInt32(codGen [a+2]),Convert.ToInt32(codGen [a+3]));
		int fen = Convert.ToInt32 (crom,2);
		return fen;
	}

	void Start () {
		random =  new System.Random ((int) (transform.position.x));

		tiempoDeEsperaEntreReproducciones = tiempoDeEsperaEntreReproducciones + tiempoActual + random.Next (10);
		tiempoParaReproducirse = tiempoDeEsperaEntreReproducciones + tiempoActual;
		vision = calcularFenotipo(codigoGenetico,2)*1.3f;
		edadMaxima = calcularFenotipo(codigoGenetico,3)*300;
		metabolismo = calcularFenotipo(codigoGenetico,4);
		capacidadDeAlmacenamiento =  300 + calcularFenotipo(codigoGenetico,5)*20;

		distantanciaParaIrPorUnGrano = 0.5f*vision;
		if(!metabolismoPorCodigoGenetico){
			vision = 10;
			edadMaxima = 1000;
			metabolismo = 10;
			capacidadDeAlmacenamiento =  1000;
		}
		vida = capacidadDeAlmacenamiento - 200;
	}



	// Update is called once per frame
	void Update () {
		
		if (tiempoRecienReproducido > 0) {
			tiempoRecienReproducido--;
		} else {
			recienReproducido = false;
		}
		edad++;
		tiempoEnDireccionActual++;

		cambiarCaminataAleatoria ();

		tiempoActual = Time.frameCount;

		Quaternion rotacion = transform.rotation;

		if (Time.frameCount % tiempoParaBuscarDir == 0) {
			if (correrleAlDepredador () && vida > vidaSuperCritica) {
				rotacion = direccionDepredador;
				caminataActual = ("escape");
			} else if(choqueParedOCompa && tiempoDejarDeAlejarse > tiempoActual){
				caminataActual = ("alejarse de pared");
			} else if ((tiempoParaReproducirse < Time.frameCount) && (vida > comidaMinimaParaReproducirse) && reproduccionHabilidada && reproducirse ()) {
				rotacion = direccionAPareja;
				caminataActual = ("reproduccion");
			} else if (buscarComida ()&& alimentarse) {
				rotacion = direccionAComida;
				caminataActual = ("Comida");
			} else if (buscarBoids () && vida > vidaCritica) {
				rotacion = direccionBoids;
				caminataActual = ("Boids");
			} else if (tiempoEnDireccionActual >= tiempoParaCambioDeDireccionCaminataAleatoria) {
				rotacion = direccionCaminataAleatoria;
				caminataActual = ("Aleatorio");
			}
			transform.rotation = rotacion;
			//transform.rotation = Quaternion.Slerp (transform.rotation, rotacion,rapidezCambioDir);

			//Barra de vida
			foreach(Transform hijo in transform){
				hijo.localScale = new Vector3 (0.03f*(vida/capacidadDeAlmacenamiento),0.0055f,0.0055f);
			}
			//Barra de vida
		}

		transform.Translate (metabolismo*Vector3.back/20);

		vida = vida - 0.2f*metabolismo;
		if (vida <= 0) {
			Destroy (gameObject);
		}
		if (transform.position.y <= -2) {
			Destroy (gameObject);
		}
		if (edad >= edadMaxima) {
			Destroy (gameObject);
		}
		transform.rotation = Quaternion.Euler (0,transform.rotation.eulerAngles.y,0);
	}

	private bool correrleAlDepredador(){
		bool correr = false;
		float distanciaActual = vision;
		for (int i = 0; i < Animales.todosLosPredadores.Length; i++) {
			float distancia = Vector3.Distance (Animales.todosLosPredadores[i].transform.position, transform.position);
			if (distancia <= vision && distancia < distanciaActual) {
				correr = true;
				Vector3 vectorADepredador = transform.position - Animales.todosLosPredadores[i].transform.position;
				vectorADepredador = new Vector3 (vectorADepredador.x,0,vectorADepredador.z);
				direccionDepredador = Quaternion.FromToRotation (Vector3.back, vectorADepredador);
				distanciaActual = distancia;

			}
		}
		return correr;
	}

	private bool cambiarCaminataAleatoria(){
		if(esquivarLSystem()){
			direccionCaminataAleatoria = Quaternion.Slerp (direccionCaminataAleatoria, direccionEsquivarLSystem,0.6f);
		}
		if (tiempoEnDireccionActual > tiempoParaCambioDeDireccionCaminataAleatoria) {
			direccionCaminataAleatoria = Quaternion.Euler (0, random.Next (360), 0);
			if(esquivarLSystem()){
				direccionCaminataAleatoria = Quaternion.Slerp (direccionCaminataAleatoria, direccionEsquivarLSystem,0.6f);
			}
			tiempoParaCambioDeDireccionCaminataAleatoria = random.Next (10) + (int)Time.frameCount;
			tiempoEnDireccionActual = 0;
			return true;
		}
		else {
			return false;
		}
	}

	private bool buscarComida(){
		float sumxtarget=0,sumztarget=0;

		float xtarget;
		float ztarget;
		Vector3 target;
		Vector3 vectorAComida;

		int granoscercanos=0;
		bool irPorGrano = false;
		Quaternion direccionPorGrano = Quaternion.Euler(0,0,0);
		float distanciaActual = distantanciaParaIrPorUnGrano;
		for (int i = 0; i < DistribuidorDeComida.todosLosGranos.Length; i++) {
			float distancia = Vector3.Distance (DistribuidorDeComida.todosLosGranos[i].transform.position, transform.position);
			if (distancia <= vision) {
				sumxtarget = sumxtarget + DistribuidorDeComida.todosLosGranos [i].transform.position.x;
				sumztarget = sumztarget + DistribuidorDeComida.todosLosGranos [i].transform.position.z;
				if (distancia < distanciaActual) {
					irPorGrano = true;
					Vector3 vectorAGrano = DistribuidorDeComida.todosLosGranos[i].transform.position-transform.position;
					vectorAGrano = new Vector3 (vectorAGrano.x,0,vectorAGrano.z);
					direccionPorGrano = Quaternion.FromToRotation (Vector3.back, vectorAGrano);
					distanciaActual = distancia;
				}
				granoscercanos++;
			}
		}
		if (granoscercanos != 0) {
			xtarget = sumxtarget / granoscercanos;
			ztarget = sumztarget / granoscercanos;
			target = new Vector3 (xtarget, 0, ztarget);

			vectorAComida = target - transform.position;
			Vector3 vectorDireccionAComida = new Vector3 (vectorAComida.x, 0, vectorAComida.z);
			direccionAComida = Quaternion.FromToRotation (Vector3.back, vectorDireccionAComida);

			if(esquivarLSystem()){
				direccionAComida = Quaternion.Slerp (direccionAComida, direccionEsquivarLSystem,0.6f);
			}

			if (irPorGrano) {
				direccionAComida = Quaternion.Slerp (direccionAComida, direccionPorGrano,0.95f);
			}

			return true;
		} else {
			return false;
		}
	}

	private bool buscarBoids(){
		Vector3 boid;
		Vector3 vectorABoid;

		float xgrupal = 0;
		float zgrupal = 0;
		float yagrupal = 0;

		int amigoscercanos=0;
		bool esquivarCompa = false;
		float distancia, distanciaAnterior = distanciaInternaBoids;
		for (int i = 0; i < Animales.todasLasPresas.Length; i++) {
			distancia = Vector3.Distance (Animales.todasLasPresas[i].transform.position, transform.position);

			if (distancia <= vision) {
				amigoscercanos++;
				xgrupal = xgrupal + Animales.todasLasPresas [i].transform.position.x;
				zgrupal = zgrupal + Animales.todasLasPresas [i].transform.position.z;
				yagrupal = yagrupal + Animales.todasLasPresas [i].transform.rotation.eulerAngles.y;

				if (distancia < distanciaAnterior && distancia < distanciaInternaBoids) {
					esquivarCompa = true;
					Vector3 vectorACompa = transform.position - Animales.todasLasPresas [i].transform.position;
					direccionEsquivarCompa = Quaternion.FromToRotation (Vector3.back, vectorACompa);
					direccionEsquivarCompa = Quaternion.Euler (0, direccionEsquivarCompa.eulerAngles.y,0);
					distanciaAnterior = distancia;
				}
			}
		}

		if (amigoscercanos > 1) {
			xgrupal = xgrupal / amigoscercanos;
			zgrupal = zgrupal / amigoscercanos;
			yagrupal = yagrupal / amigoscercanos;

			boid = new Vector3 (xgrupal, 0, zgrupal);

			vectorABoid = boid - transform.position;
			vectorABoid = new Vector3 (vectorABoid.x, 0, vectorABoid.z);
			Quaternion direccionDelBoid =  Quaternion.Euler(0, yagrupal, 0);

			Quaternion direccionAlBoid = Quaternion.FromToRotation (Vector3.back, vectorABoid);

			float preferencia = 0;
			float distanciaPreferencia = Vector3.Distance(transform.position, boid);
			preferencia = 1 - distanciaPreferencia / 10;
			if (preferencia < 0.1f) {
				preferencia = 0.1f;
			} if (preferencia > 0.8f) {
				preferencia = 1;
			}

			direccionBoids = Quaternion.Slerp (direccionAlBoid, direccionDelBoid,preferencia);
			//direccionBoids = direccionDelBoid;
			/*
			if(esquivarCompa){
				direccionBoids = Quaternion.Slerp (direccionBoids, direccionEsquivarCompa,0.9f);
			}
			if(esquivarLSystem()){
				direccionBoids = Quaternion.Slerp (direccionBoids, direccionEsquivarLSystem,0.9f);
			}
			*/
			return true;
		} else {
			return false;
		}
	}

	private bool esquivarLSystem(){
		bool LSystemCercano = false;

		for (int i = 0; i < LSystem.todosLosLSystemPosiciones.Length; i++) {
			float distancia = Vector3.Distance (LSystem.todosLosLSystemPosiciones[i], transform.position);
			if ( distancia<= vision && distancia <= distanciaLSystem) {
				LSystemCercano = true;
				Vector3 vectorALSystem = transform.position - LSystem.todosLosLSystemPosiciones [i];
				direccionEsquivarLSystem = Quaternion.FromToRotation (Vector3.back, vectorALSystem);
				direccionEsquivarLSystem = Quaternion.Euler (0, direccionEsquivarLSystem.eulerAngles.y,0);
			}
		}
		return LSystemCercano;
	}

	private bool reproducirse(){
		fertil = true;
		turing esteObjTuring = this.GetComponent<turing>();
		TransformacionesPrueba esteObjTransform = this.GetComponent<TransformacionesPrueba>();

		bool parejaCercana = false;
		int masParecido = 0;
		float distanciaConElActual = 0;

		distanciaConElActual = 135;

		for (int i = 0; i < Animales.todasLasPresas.Length; i++) {
			float distancia = Vector3.Distance (Animales.todasLasPresas[i].transform.position, transform.position);

			if (distancia <= vision) {
				float distanciaNueva = 0;

				turing nuevaPresaTuring = Animales.todasLasPresas[i].GetComponent<turing>();
				TransformacionesPrueba nuevaPresaTransform = Animales.todasLasPresas[i].GetComponent<TransformacionesPrueba>();

				distanciaNueva = distanciaNueva + Mathf.Abs((esteObjTuring.cantidadRojo - nuevaPresaTuring.cantidadRojo)*10);
				distanciaNueva = distanciaNueva + Mathf.Abs((esteObjTuring.cantidadAzul - nuevaPresaTuring.cantidadAzul)*10);
				distanciaNueva = distanciaNueva + Mathf.Abs((esteObjTuring.cantidadVerde - nuevaPresaTuring.cantidadVerde)*10);
				distanciaNueva = distanciaNueva + Mathf.Abs(esteObjTransform.factorx - nuevaPresaTransform.factorx);
				distanciaNueva = distanciaNueva + Mathf.Abs(esteObjTransform.factory - nuevaPresaTransform.factory);

				if(distanciaConElActual > distanciaNueva && distanciaNueva > 0 && distanciaNueva <= diferenciaMaximaAceptada){
					distanciaConElActual = distanciaNueva;
					masParecido = i;
					parejaCercana = true;
					parecidoConLaActual = distanciaConElActual;
					Vector3 vectorAPareja = Animales.todasLasPresas[i].transform.position - transform.position;
					vectorAPareja = new Vector3 (vectorAPareja.x,0,vectorAPareja.z);
					direccionAPareja = Quaternion.FromToRotation (Vector3.back, vectorAPareja);
					PresaComportamiento pareja1Comp = Animales.todasLasPresas[i].GetComponent<PresaComportamiento>();
					parejaCercana = pareja1Comp.fertil;
				}
			}
		}
		return parejaCercana;
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "comida") {
			if (vida < capacidadDeAlmacenamiento) {
				vida = vida +10;
			}
		}else if (collision.gameObject.tag == "predator") {
			Destroy (gameObject);
		} else if (collision.gameObject.tag == "pared") {
			//transform.rotation = Quaternion.Euler(0,360 - transform.rotation.eulerAngles.y,0);
			choqueParedOCompa = true;
			tiempoDejarDeAlejarse = tiempoActual + intervaloAlejarseDePared;

			//Vector3 reflejo = Vector3.Reflect (Vector3.back, collision.contacts [0].normal);
			//Debug.Log (collision.contacts [0].normal.x + "  "+collision.contacts [0].normal.y + "  "+collision.contacts [0].normal.z + "  ");
			//Debug.Log (transform.rotation.eulerAngles.y);
			//transform.rotation = Quaternion.Euler (0,reflejo.y+180,0);
			float nuevoy, yex = transform.rotation.eulerAngles.y;
			if (collision.contacts [0].normal.x > 0.95f || collision.contacts [0].normal.x < -0.95f) {
				nuevoy = 360 - yex;
			} else{
				nuevoy = 180 - yex;
			}
			transform.rotation = Quaternion.Euler(0,nuevoy,0);

		} else if (collision.gameObject.tag == "presa" && reproduccionHabilidada){
			/////////////////

			turing esteObjTuring = this.GetComponent<turing>();
			TransformacionesPrueba esteObjTransform = this.GetComponent<TransformacionesPrueba>();

			PresaComportamiento pareja1Comp = collision.gameObject.GetComponent<PresaComportamiento>();
			turing pareja1Turing = collision.gameObject.GetComponent<turing>();
			TransformacionesPrueba pareja1Transform = collision.gameObject.GetComponent<TransformacionesPrueba>();

			bool parejaDispuesta = pareja1Comp.fertil;
			bool parejaRecienReproducida = pareja1Comp.recienReproducido;


			float distanciaConElActual = 0;

			distanciaConElActual = distanciaConElActual + Mathf.Abs((esteObjTuring.cantidadRojo - pareja1Turing.cantidadRojo)*10);
			distanciaConElActual = distanciaConElActual + Mathf.Abs((esteObjTuring.cantidadAzul - pareja1Turing.cantidadAzul)*10);
			distanciaConElActual = distanciaConElActual + Mathf.Abs((esteObjTuring.cantidadVerde - pareja1Turing.cantidadVerde)*10);
			distanciaConElActual = distanciaConElActual + Mathf.Abs(esteObjTransform.factorx - pareja1Transform.factorx);
			distanciaConElActual = distanciaConElActual + Mathf.Abs(esteObjTransform.factory - pareja1Transform.factory);

			if (distanciaConElActual <= diferenciaMaximaAceptada && fertil && parejaDispuesta) {
				BitArray codigoGenActual = esteObjTuring.codigoGenetico;
				BitArray codigoGenPareja = pareja1Turing.codigoGenetico;

				int splitCode = 1 + random.Next (35);
				BitArray codigoGen = new BitArray(36);
				for (int a = 0; a < splitCode; a++) {
					codigoGen[a] = codigoGenPareja[a];
				}
				for (int a = splitCode; a < codigoGen.Length; a++) {
					codigoGen[a] = codigoGenActual[a];
				}
				
				//Animales.instanciarPresa (transform.position.x, transform.position.y, transform.position.z, codigoGen);
			
				GameObject hijo = presa;

				/*
				//turing nuevaPresaTuring = hijo.GetComponent<turing>();
				hijo.GetComponent<turing>().codigoGenetico = codigoGen;
				//PresaComportamiento nuevaPresaComp = hijo.GetComponent<PresaComportamiento>();
				hijo.GetComponent<PresaComportamiento>().codigoGenetico = codigoGen;
				//TransformacionesPrueba nuevaPresaTransform = hijo.GetComponent<TransformacionesPrueba>();
				hijo.GetComponent<TransformacionesPrueba>().codigoGenetico = codigoGen;
				*/

				GameObject nuevaPresa = Instantiate(Animales.presaInst, new Vector3 (transform.position.x,transform.position.y,transform.position.z), Quaternion.identity);
				turing nuevaPresaTuring = nuevaPresa.GetComponent<turing>();
				nuevaPresaTuring.codigoGenetico = codigoGen;
				PresaComportamiento nuevaPresaComp = nuevaPresa.GetComponent<PresaComportamiento>();
				nuevaPresaComp.codigoGenetico = codigoGen;
				nuevaPresaComp.tiempoActual = tiempoActual;
				TransformacionesPrueba nuevaPresaTransform = nuevaPresa.GetComponent<TransformacionesPrueba>();
				nuevaPresaTransform.codigoGenetico = codigoGen;

				fertil = false;
				recienReproducido = true;
				vida = vida / 2;
				tiempoParaReproducirse = tiempoActual + tiempoDeEsperaEntreReproducciones;
				transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y + 180,0);
				choqueParedOCompa = true;
				tiempoDejarDeAlejarse = tiempoActual + intervaloAlejarseDeCompa;
				tiempoRecienReproducido = 2;
			}
			if (distanciaConElActual <= diferenciaMaximaAceptada && fertil && parejaRecienReproducida) {
				fertil = false;
				vida = vida / 2;
				recienReproducido = true;
				tiempoRecienReproducido = 2;
			}
			/////////////////
		}
	}

	void generarCria(GameObject pareja){
		
	}

}
