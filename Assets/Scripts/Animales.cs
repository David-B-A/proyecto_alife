using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Animales : MonoBehaviour {
	
	public int numerodepresas = 9;
	public int numerodepredadores = 1;
	public GameObject presa;
	public static GameObject presaInst;
	//public static GameObject presaInst;
	public GameObject predador;
	public static GameObject[] todasLasPresas;
	public static GameObject[] todosLosPredadores;
	private System.Random random = new System.Random ();
	// Use this for initialization
	public bool reproduccionHabilidadaParaPresas = true;
	public bool presasPuedenAlimentarse = true;
	public bool asignarMetabolismoEstandarPresas = false;
	public bool turingHabilitadoPresa = true;

	public int diferenciaMaximaAceptadaParaReproducirse = 15;
	public GameObject Texto;
	void Start () {



		//string uno = String.Concat (Convert.ToInt32(codGen [0]),Convert.ToInt32(codGen [1]),Convert.ToInt32(codGen [2]),Convert.ToInt32(codGen [3]));
		//int aa = Convert.ToInt32 (uno,2);
		//Debug.Log (uno);
		//Debug.Log (aa);

		presaInst = presa;
		for (int i = 0; i < numerodepresas; i++) {
			BitArray codigoGen = generarCodigoGenetico();
			GameObject nuevaPresa = Instantiate(presa, new Vector3 (random.Next(-20,20),0,random.Next(-20,20)), Quaternion.Euler(0,random.Next(360),0));
			turing nuevaPresaTuring = nuevaPresa.GetComponent<turing>();
			nuevaPresaTuring.codigoGenetico = codigoGen;
			nuevaPresaTuring.turingHabilitado = turingHabilitadoPresa;
			PresaComportamiento nuevaPresaComp = nuevaPresa.GetComponent<PresaComportamiento>();
			nuevaPresaComp.codigoGenetico = codigoGen;
			nuevaPresaComp.reproduccionHabilidada = reproduccionHabilidadaParaPresas; 
			nuevaPresaComp.diferenciaMaximaAceptada = diferenciaMaximaAceptadaParaReproducirse;
			nuevaPresaComp.alimentarse = presasPuedenAlimentarse;
			nuevaPresaComp.asignarMetabolismoEstandar = asignarMetabolismoEstandarPresas;
			TransformacionesPrueba nuevaPresaTransform = nuevaPresa.GetComponent<TransformacionesPrueba>();
			nuevaPresaTransform.codigoGenetico = codigoGen;
		}
		for (int i = 0; i < numerodepredadores; i++) {
			BitArray codigoGen = generarCodigoGenetico();
			GameObject nuevoPredador = Instantiate(predador, new Vector3 (random.Next(-20,20),0,random.Next(-20,20)), Quaternion.identity);
			foreach (Transform t in nuevoPredador.transform) {
				if(t.gameObject.tag == "pielPredador"){
					turing turingPredador = t.gameObject.GetComponent<turing>();
					turingPredador.codigoGenetico = codigoGen;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		todasLasPresas = GameObject.FindGameObjectsWithTag ("presa");
		todosLosPredadores = GameObject.FindGameObjectsWithTag ("predator");
		numerodepresas = todasLasPresas.Length;
		numerodepredadores = todosLosPredadores.Length;
		Texto.GetComponent<Text> ().text = String.Concat("Presas: \t\t\t",numerodepresas);
		Texto.GetComponent<Text> ().text += String.Concat("\nPredadores: \t",numerodepredadores);
	}

	public void instanciarPresa(float x, float y, float z, BitArray codigoGen, int tiempo){
		GameObject nuevaPresa = Instantiate(presa, new Vector3 (x,y,z), Quaternion.identity);
		turing nuevaPresaTuring = nuevaPresa.GetComponent<turing>();
		nuevaPresaTuring.codigoGenetico = codigoGen;
		PresaComportamiento nuevaPresaComp = nuevaPresa.GetComponent<PresaComportamiento>();
		nuevaPresaComp.codigoGenetico = codigoGen;
		nuevaPresaComp.tiempoActual = tiempo;
		TransformacionesPrueba nuevaPresaTransform = nuevaPresa.GetComponent<TransformacionesPrueba>();
		nuevaPresaTransform.codigoGenetico = codigoGen;
	}
	private BitArray generarCodigoGenetico(){
		BitArray codGen = new BitArray(36);
		for (int i = 0; i < codGen.Length; i++) {
			if (random.Next(100) < 50) {
				codGen [i] = false;
			} else {
				codGen [i] = true;
			}
		}
		return codGen;
	}
}
