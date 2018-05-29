using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class ConstructorLSystem : MonoBehaviour 
	{
		private GameObject palito;
		private float longitud;
		private float ancho;
		private System.Random random = new System.Random ();
		private int nObjetos;
		private GameObject[] vecG;

		private string caminoTortuga;

		private float[] x;
		private float[] y;
		private float[] z;
		private float[] angle;

		private float xinicial;
		private float yinicial;
		private float zinicial;
		private float anguloDeGiro;

		private int nivelesGlobal;

		public ConstructorLSystem (float xi, float yi, float zi, string partida, string regla, int iteraciones, float agiro, float longitudRama, float anchoRama, GameObject gameObject)
		{
			longitud = longitudRama;
			ancho = anchoRama;
			palito = gameObject;

			xinicial = xi;
			yinicial = yi;
			zinicial = zi;
			anguloDeGiro = agiro;

			//iterar para encontrar el camino
			#region
			encontrarCamino (partida, regla, iteraciones);

			#endregion


			//Conseguir el numero de objetos y niveles necesarios
			#region
			nObjetos = 0;
			int niveles = 1, contnivtemp = 1;

			for (int i = 0; i < caminoTortuga.Length; i++) {
				if (caminoTortuga [i] == 'F') {
					nObjetos++;
				}
				if (caminoTortuga [i] == '[') {
					contnivtemp++;
					if (contnivtemp > niveles) {
						niveles = contnivtemp;
					}
				}
				if (caminoTortuga [i] == ']') {
					contnivtemp--;
				}
			}
			#endregion

			nivelesGlobal = niveles;

			//inicializar variables
			#region
			x = new float[niveles];
			y = new float[niveles];
			z = new float[niveles];
			angle = new float[niveles];

			float desv =  random.Next (-3,3)+(float) random.NextDouble();
			vecG = new GameObject[nObjetos +1];

			int nobj = 0, nivel = 0;
			x [0] = xi;
			y [0] = yi;
			z [0] = zi;
			angle [0] = 0;
			#endregion

			//Dibujar
			#region
			for (int i = 0; i < caminoTortuga.Length; i++) {
				if (caminoTortuga [i] == 'F') {
					desv =  random.Next (-5,5)+(float) random.NextDouble();
					InstanciarPalito (nivel, longitud, ancho, desv, nobj);
					nobj = nobj + 1;
				}
				if (caminoTortuga [i] == '+') {
					cambiarAngulo (agiro, nivel);
				}
				if (caminoTortuga [i] == '-') {
					cambiarAngulo (-agiro, nivel); 
				}
				if (caminoTortuga [i] == '[') {
					nivel++;
					x [nivel] = x [nivel - 1];
					y [nivel] = y [nivel - 1];
					z [nivel] = z [nivel - 1];
					angle [nivel] = angle [nivel - 1];
				}
				if (caminoTortuga [i] == ']') {
					nivel--;
				}
			}


			#endregion
		}

		void encontrarCamino(string partida,string regla,int iteraciones){
			if (iteraciones == 0) {
				return;
			}
			string temp = "";
			for (int i = 0; i < partida.Length; i++) {
				if (partida [i] != 'F') {
					temp = string.Concat (temp, partida [i]);
				} else {
					temp = string.Concat (temp, regla);
				}
			}
			caminoTortuga = temp;
			encontrarCamino (temp, regla, iteraciones - 1);
		}

		void InstanciarPalito(int nivel, float l, float w, float desv, int nobj){

			float angleRad = (float) (Mathf.PI*(angle [nivel]+desv) / 180);
			float nuevaX = x[nivel]+(float) (l/2*Mathf.Sin(-angleRad));
			float nuevaY = y[nivel]+(float) (l/2*Mathf.Cos(-angleRad));
			vecG[nobj]=Instantiate(palito, new Vector3 (nuevaX, nuevaY, z[nivel]), Quaternion.identity);

			vecG[nobj].transform.Rotate (new Vector3(0,0,angle[nivel]));
			vecG[nobj].transform.localScale = new Vector3(w,l,w);

			x[nivel] = x[nivel]+l * Mathf.Sin (-angleRad);
			y[nivel] = y[nivel]+l * Mathf.Cos (-angleRad);
		}

		void cambiarAngulo(float cambio, int nivel){
			angle[nivel] = angle[nivel] + cambio;
		}

		public void destruir(){
			for (int i = 0; i <= nObjetos; i++) {
				Destroy (vecG[i]);		
			}
		}


		public void MoverArbol(float longitudRama, float anchoRama){
			

			longitud = longitudRama;
			ancho = anchoRama;
			float agiro = anguloDeGiro;

			#region

			x = new float[nivelesGlobal];
			y = new float[nivelesGlobal];
			z = new float[nivelesGlobal];
			angle = new float[nivelesGlobal];

			float desv =  random.Next (-3,3)+(float) random.NextDouble();

			int nobj = 0, nivel = 0;
			x [0] = xinicial;
			y [0] = yinicial;
			z [0] = zinicial;
			angle [0] = 0;
			#endregion

			//Dibujar
			#region
			for (int i = 0; i < caminoTortuga.Length; i++) {
				if (caminoTortuga [i] == 'F') {
					desv =  random.Next (-5,5)+(float) random.NextDouble();
					MoverPalito (nivel, longitud, ancho, desv, nobj);
					nobj = nobj + 1;

				}
				if (caminoTortuga [i] == '+') {
					cambiarAngulo (agiro, nivel);
				}
				if (caminoTortuga [i] == '-') {
					cambiarAngulo (-agiro, nivel); 
				}
				if (caminoTortuga [i] == '[') {
					nivel++;
					x [nivel] = x [nivel - 1];
					y [nivel] = y [nivel - 1];
					z [nivel] = z [nivel - 1];
					angle [nivel] = angle [nivel - 1];
				}
				if (caminoTortuga [i] == ']') {
					nivel--;
				}
			}
			#endregion
		}

		void MoverPalito(int nivel, float l, float w, float desv, int nobj){

			float angleRad = (float) (Mathf.PI*(angle [nivel]+desv) / 180);
			float nuevaX = x[nivel]+(float) (l/2*Mathf.Sin(-angleRad));
			float nuevaY = y[nivel]+(float) (l/2*Mathf.Cos(-angleRad));
			vecG [nobj].transform.position = new Vector3 (nuevaX, nuevaY, z[nivel]);
			vecG[nobj].transform.rotation = Quaternion.Euler(0,0,angle[nivel]);
			vecG[nobj].transform.localScale = new Vector3(w,l,w);

			x[nivel] = x[nivel]+l * Mathf.Sin (-angleRad);
			y[nivel] = y[nivel]+l * Mathf.Cos (-angleRad);

		}

	}
}

