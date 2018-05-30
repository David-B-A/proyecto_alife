using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Animales : MonoBehaviour {
	
	public int numerodepresas = 20;
	public int numerodepredadores = 2;
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
	public bool metabolismoPorCodigoGenetico = true;
	public bool turingHabilitadoPresa = true;
	public int edadMaximaPresa = 2000;

	public int diferenciaMaximaAceptadaParaReproducirse = 15;
	public GameObject Texto;

	public float capacidadDeAlmacenamientoPromedio = 0;
	public float metabolismoPromedio = 0;
	public float metabolismoMaximo = 0;
	public float metabolismoMinimo = 15;
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
			nuevaPresaComp.metabolismoPorCodigoGenetico = metabolismoPorCodigoGenetico;
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
		metabolismoMaximo = 0;
		metabolismoMinimo = 15;
		todasLasPresas = GameObject.FindGameObjectsWithTag ("presa");
		todosLosPredadores = GameObject.FindGameObjectsWithTag ("predator");
		numerodepresas = todasLasPresas.Length;
		numerodepredadores = todosLosPredadores.Length;
		float sumaMetabolismo = 0;
		float sumaCapAlmac = 0;
		foreach (GameObject presa in todasLasPresas) {
			PresaComportamiento pc = presa.GetComponent<PresaComportamiento>();
			sumaMetabolismo = sumaMetabolismo + pc.metabolismo;
			sumaCapAlmac = sumaCapAlmac + pc.capacidadDeAlmacenamiento;
			if (pc.metabolismo > metabolismoMaximo) {
				metabolismoMaximo = pc.metabolismo;
			}
			if (pc.metabolismo < metabolismoMinimo) {
				metabolismoMinimo = pc.metabolismo;
			}
		}
		metabolismoPromedio = sumaMetabolismo / todasLasPresas.Length;
		capacidadDeAlmacenamientoPromedio = sumaCapAlmac / todasLasPresas.Length;

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
		nuevaPresaComp.edadMaxima = edadMaximaPresa;
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
