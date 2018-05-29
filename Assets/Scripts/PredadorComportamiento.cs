using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PredadorComportamiento : MonoBehaviour {

	public float vida = 140;
	private float vision = 20;
	private float distantanciaParaIrPorUnaPresa = 20;
	public string caminataActual = "";

	private System.Random random;
	// Use this for initialization

	private int tiempoEnDireccionActual=0;
	private int tiempoParaCambioDeDireccionCaminataAleatoria=30;



	private Quaternion direccionCaminataAleatoria;
	private Quaternion direccionAComida;
	private Quaternion direccionEsquivarCompa;

	// Use this for initialization
	void Start () {
		random =  new System.Random ((int) (transform.position.x));
		transform.rotation = Quaternion.Euler (0,random.Next(360),0);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.Translate (Vector3.forward);

		tiempoEnDireccionActual++;

		if (buscarComida () && vida <90) {
			transform.rotation = direccionAComida;
			caminataActual = "comida";
		}
		else {
			if (cambiarCaminataAleatoria()) {
				transform.rotation = direccionCaminataAleatoria;
				caminataActual = "aleatorio";

			}
		}
		transform.Translate (Vector3.forward/2);
		vida = vida - 0.5f;
		if (vida <= 0) {
			Destroy (gameObject);
		}
		if (transform.position.y <= -2) {
			Destroy (gameObject);
		}
		transform.rotation = Quaternion.Euler (0,transform.rotation.eulerAngles.y,0);
	}

	private bool cambiarCaminataAleatoria(){
		if (tiempoEnDireccionActual > tiempoParaCambioDeDireccionCaminataAleatoria) {
			direccionCaminataAleatoria = Quaternion.Euler (0, random.Next (360), 0);
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

		int presascercanas=0;
		bool irPorPresa = false;
		Quaternion direccionPorPresa = Quaternion.Euler(0,0,0);
		float distanciaActual = distantanciaParaIrPorUnaPresa;
		for (int i = 0; i < Animales.todasLasPresas.Length; i++) {
			float distancia = Vector3.Distance (Animales.todasLasPresas[i].transform.position, transform.position);
			if (distancia <= vision) {
				sumxtarget = sumxtarget + Animales.todasLasPresas [i].transform.position.x;
				sumztarget = sumztarget + Animales.todasLasPresas [i].transform.position.z;
				if (distancia < distanciaActual) {
					irPorPresa = true;
					Vector3 vectorAPresa = transform.position - Animales.todasLasPresas[i].transform.position;
					vectorAPresa = new Vector3 (vectorAPresa.x,0,vectorAPresa.z);
					direccionPorPresa = Quaternion.FromToRotation (Vector3.back, vectorAPresa);
					distanciaActual = distancia;
				}

				presascercanas++;
			}
		}
		if (presascercanas != 0) {
			xtarget = sumxtarget / presascercanas;
			ztarget = sumztarget / presascercanas;
			target = new Vector3 (xtarget, 0, ztarget);

			vectorAComida = transform.position - target ;
			Vector3 vectorDireccionAComida = new Vector3 (vectorAComida.x, 0, vectorAComida.z);
			direccionAComida = Quaternion.FromToRotation (Vector3.back, vectorDireccionAComida);

			if (irPorPresa) {
				direccionAComida = Quaternion.Slerp (direccionAComida, direccionPorPresa,0.95f);
			}

			return true;
		} else {
			return false;
		}
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "presa") {
			vida = vida + 70;
		}
		if (collision.gameObject.tag == "pared") {
			float nuevoy, yex = transform.rotation.eulerAngles.y;
			if (collision.contacts [0].normal.x > 0.95f || collision.contacts [0].normal.x < -0.95f) {
				nuevoy = 360 - yex;
			} else{
				nuevoy = 180 - yex;
			}
			transform.rotation = Quaternion.Euler(0,nuevoy,0);
		}
	}
}
