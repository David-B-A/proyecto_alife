
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class LSystem : MonoBehaviour {
	// Use this for initialization

	public static Vector3[] todosLosLSystemPosiciones;

	public GameObject palito;
	public float longitud;
	public float ancho;
	private int nObjetos;
	private System.Random random = new System.Random ();

	private GameObject[] vecG;

	private string caminoTortuga;

	private float[] x;
	private float[] y;
	private float[] z;
	private float[] angle;


	ConstructorLSystem arbol1;
	ConstructorLSystem arbol2;
	ConstructorLSystem arbol3;
	ConstructorLSystem arbol4;
	ConstructorLSystem arbol5;
	ConstructorLSystem arbol6;
	ConstructorLSystem arbol7;
	ConstructorLSystem arbol8;

	void Start () {
		string partida = "F";
		string regla = "F[+F]F[-F][F]";
		int iteraciones = 5;
		float agiro = 20;



		arbol1 = new ConstructorLSystem (0, 0, 5, partida, regla, iteraciones, agiro, longitud, ancho, palito);
		arbol2 = new ConstructorLSystem (10, 0, 2, partida, regla, iteraciones - 1, agiro, longitud, ancho, palito);
		arbol3 = new ConstructorLSystem (-10, 0, 4, partida, regla, iteraciones - 2, agiro, longitud, ancho, palito);
		arbol4 = new ConstructorLSystem (5, 0, -3, partida, regla, iteraciones - 3, agiro, longitud, ancho, palito);
		arbol5 = new ConstructorLSystem (10, 0, -11, partida, regla, iteraciones - 3, agiro, longitud, ancho, palito);
		arbol6 = new ConstructorLSystem (-11, 0, -8, partida, regla, iteraciones - 3, agiro, longitud, ancho, palito);
		arbol7 = new ConstructorLSystem (-7, 0, 7, partida, regla, iteraciones - 3, agiro, longitud, ancho, palito);
		arbol8 = new ConstructorLSystem (8, 0, 12, partida, regla, iteraciones - 3, agiro, longitud, ancho, palito);

		todosLosLSystemPosiciones = new Vector3[8];

		todosLosLSystemPosiciones[0] = new Vector3(0, 0, 5);
		todosLosLSystemPosiciones[1] = new Vector3(10, 0, 2);
		todosLosLSystemPosiciones[2] = new Vector3(-10, 0, 4);
		todosLosLSystemPosiciones[3] = new Vector3(5, 0, -3);
		todosLosLSystemPosiciones[4] = new Vector3(10, 0, -11);
		todosLosLSystemPosiciones[5] = new Vector3(-11, 0, -8);
		todosLosLSystemPosiciones[6] = new Vector3(-7, 0, 7);
		todosLosLSystemPosiciones[7] = new Vector3(8, 0, 12);
	}

	void Update (){
		if (Time.frameCount % 3 == 0) {
			arbol1.MoverArbol (longitud, ancho);
			arbol2.MoverArbol (longitud, ancho);
			arbol3.MoverArbol (longitud, ancho);
			arbol4.MoverArbol (longitud, ancho);
			arbol5.MoverArbol (longitud, ancho);
			arbol6.MoverArbol (longitud, ancho);
			arbol7.MoverArbol (longitud, ancho);
			arbol8.MoverArbol (longitud, ancho);
		}

		/*
		if (Time.frameCount % 5 == 0) {
			for (int i = 0; i <= nObjetos; i++) {
				arbol1.destruir();		
				arbol2.destruir();		
				arbol3.destruir();		
				arbol4.destruir();		
				arbol5.destruir();		
				arbol6.destruir();		
				arbol7.destruir();		
				arbol8.destruir();		
			}
			string partida = "F";
			string regla = "F[+F]F[-F][F]";
			int iteraciones = 5;
			float agiro = 20;
			arbol1 = new ConstructorLSystem (0, 0, 5, partida, regla, iteraciones, agiro, longitud, ancho, palito);
			arbol2 = new ConstructorLSystem (10, 0, 2, partida, regla, iteraciones - 1, agiro, longitud, ancho, palito);
			arbol3 = new ConstructorLSystem (-10, 0, 4, partida, regla, iteraciones - 2, agiro, longitud, ancho, palito);
			arbol4 = new ConstructorLSystem (5, 0, -3, partida, regla, iteraciones - 3, agiro, longitud, ancho, palito);

			partida = "F";
			regla = "FF-[-F+F+F]+[+F-F-F]";
			iteraciones = 5;
			agiro = 22.5f;
			arbol5 = new ConstructorLSystem (10, 0, -11, partida, regla, iteraciones-1, agiro, longitud/2, ancho, palito);
			arbol6 = new ConstructorLSystem (-11, 0, -8, partida, regla, iteraciones - 2, agiro, longitud, ancho, palito);
			arbol7 = new ConstructorLSystem (-7, 0, 7, partida, regla, iteraciones - 1, agiro, longitud, ancho, palito);
			arbol8 = new ConstructorLSystem (8, 0, 12, partida, regla, iteraciones-2, agiro, longitud, ancho, palito);
		}
		*/

	}

}
