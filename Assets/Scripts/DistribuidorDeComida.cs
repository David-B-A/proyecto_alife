using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class DistribuidorDeComida : MonoBehaviour {

	public GameObject azucar;
	public int maximaComida = 500;
	public int contador;
	public int tiempoActual;
	public int numeroDePilas=3;
	public int moduloActual;
	public static int cuenta = 0;
	public static GameObject[] todosLosGranos;
	public int[] x;
	public int[] z;
	public GameObject Texto;

	private System.Random random;
	// Use this for initialization
	void Start () {
		random = new System.Random ();
		x = new int[numeroDePilas];
		z = new int[numeroDePilas];
		for (int i = 0; i < numeroDePilas; i++) {
			x [i] = random.Next (-20, 20);
			z [i] = random.Next (-20, 20);
		}
	}
	
	// Update is called once per frame
	void Update () {
		tiempoActual++;
		int tiempoCambio = 500;
		if (contador < maximaComida) {
			if ((moduloActual = tiempoActual % tiempoCambio) < tiempoCambio/2) {
				int posi = -20;
				int posf = 0;
				for (int i = 0; i < numeroDePilas; i++) {
					
					Instantiate (azucar, new Vector3 (random.Next (posi, posf), 20, random.Next (posi, posf)), Quaternion.identity);
					Instantiate (azucar, new Vector3 (random.Next (posi, posf), 20, random.Next (posi, posf)), Quaternion.identity);
					Instantiate (azucar, new Vector3 (random.Next (posi, posf), 20, random.Next (posi, posf)), Quaternion.identity);
					Instantiate (azucar, new Vector3 (random.Next (posi, posf), 20, random.Next (posi, posf)), Quaternion.identity);
					Instantiate (azucar, new Vector3 (random.Next (posi, posf), 20, random.Next (posi, posf)), Quaternion.identity);
					//Instantiate (azucar, new Vector3 (x[i], 20, z[i]), Quaternion.identity);
					cuenta++;
				}
			} else {
				int posi = 0;
				int posf = 20;
				for (int i = 0; i < numeroDePilas; i++) {

					Instantiate (azucar, new Vector3 (random.Next (posi, posf), 20, random.Next (posi, posf)), Quaternion.identity);
					Instantiate (azucar, new Vector3 (random.Next (posi, posf), 20, random.Next (posi, posf)), Quaternion.identity);
					Instantiate (azucar, new Vector3 (random.Next (posi, posf), 20, random.Next (posi, posf)), Quaternion.identity);
					Instantiate (azucar, new Vector3 (random.Next (posi, posf), 20, random.Next (posi, posf)), Quaternion.identity);
					Instantiate (azucar, new Vector3 (random.Next (posi, posf), 20, random.Next (posi, posf)), Quaternion.identity);
					//Instantiate (azucar, new Vector3 (x[i], 20, z[i]), Quaternion.identity);
					cuenta++;
				}
			}
		}
		if (Time.frameCount % (tiempoCambio/2) <= 5 ) {
			todosLosGranos = GameObject.FindGameObjectsWithTag ("comida");
			for (int i = 0; i < todosLosGranos.Length; i++) {
				Destroy (todosLosGranos[i]);
			}
		} 
		todosLosGranos = GameObject.FindGameObjectsWithTag ("comida");
		contador = todosLosGranos.Length;
		Texto.GetComponent<Text> ().text += String.Concat("\nComida: \t\t\t",contador);
	}


}
