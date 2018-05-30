using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reporte : MonoBehaviour {
	public GameObject grafica;
	private Texture2D texture;
	float[] valores;
	float[] maximos;
	float[] minimos;
	private int ancho = 200;
	private int alto = 150;
	private int margen = 10;
	public int tiempoActual;
	// Use this for initialization
	void Start () {
		texture = new Texture2D(ancho, alto);
		Renderer renderer = grafica.GetComponent<Renderer>();
		renderer.material.mainTexture = texture;

		valores = new float[50];
		maximos = new float[50];
		minimos = new float[50];
		for (int i = 1; i < valores.Length; i++) {
			maximos [i] = 15;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		tiempoActual = Time.frameCount;
		if(Time.frameCount % 4 == 0){
			Borrar (Color.white);

			int xp, yp;
			int xM, yM;
			int xmi, ymi;

			float metabolismoMaximo = 15;
			float mayor = 0, menor = metabolismoMaximo;

			int xpA = (int)margen/2;
			int ypA = (int) (margen/2 + valores [0] / metabolismoMaximo * (alto - margen/2));

			int xMA = (int)margen/2;
			int yMA = (int) (margen/2 + maximos [0] / metabolismoMaximo * (alto - margen/2));

			int xmiA = (int)margen/2;
			int ymiA = (int) (margen/2 + minimos [0] / metabolismoMaximo * (alto - margen/2));

			for (int i = 1; i < valores.Length; i++) {
				xp = (int)(margen/2 + ((float)i/(float)valores.Length) * (ancho-margen/2));
				yp = (int) (margen/2 + valores [i] / metabolismoMaximo * (alto - margen/2));
				xM = (int)(margen/2 + ((float)i/(float)maximos.Length) * (ancho-margen/2));
				yM = (int) (margen/2 + maximos [i] / metabolismoMaximo * (alto - margen/2));
				xmi = (int)(margen/2 + ((float)i/(float)minimos.Length) * (ancho-margen/2));
				ymi = (int) (margen/2 + minimos [i] / metabolismoMaximo * (alto - margen/2));
				DibujarRecta (xpA,ypA,xp,yp,Color.black,2);
				DibujarRecta (xMA,yMA,xM,yM,Color.green,2);
				DibujarRecta (xmiA,ymiA,xmi,ymi,Color.green,2);
				valores [i - 1] = valores [i];
				maximos [i - 1] = maximos [i];
				minimos [i - 1] = minimos [i];
				xpA = xp;
				ypA = yp;
				xMA = xM;
				yMA = yM;
				xmiA = xmi;
				ymiA = ymi;
			}
			Animales animales = gameObject.GetComponent<Animales>();
			valores [valores.Length - 1] = animales.metabolismoPromedio;
			maximos [maximos.Length - 1] = animales.metabolismoMaximo;
			minimos [minimos.Length - 1] = animales.metabolismoMinimo;
			texture.Apply();
		}
	}
	void Borrar(Color color){
		for (int i = 0; i < ancho; i++) {
			for (int j = 0; j < alto; j++) {
				try{
					texture.SetPixel(i,j, color);
				} catch{}
			}
		}
	}
	void DibujarRecta(int xi, int yi, int xf, int yf, Color color, int ancho){
		float m = (yf - yi) / (xf - xi);
		float b = yf - m * xf;
		int npixels = (int)Mathf.Sqrt (Mathf.Pow((yf - yi),2)+Mathf.Pow((xf - xi),2));
		float xact, yact;
		int pixX, pixY;
		for (int i = 0; i < npixels; i++) {
			xact = xi + ((float)i/(float)npixels)*(xf - xi);
			yact = m * xact + b;
			pixX = (int)xact;
			pixY = (int)yact;
			texture.SetPixel(pixX, pixY, color);
			for (int j = 0; j < ancho; j++) {
				try{
					texture.SetPixel(pixX+j, pixY+j, color);
					texture.SetPixel(pixX+j, pixY, color);
					texture.SetPixel(pixX+j, pixY-j, color);
					texture.SetPixel(pixX, pixY+j, color);
					texture.SetPixel(pixX, pixY-j, color);
					texture.SetPixel(pixX-j, pixY+j, color);
					texture.SetPixel(pixX-j, pixY, color);
					texture.SetPixel(pixX-j, pixY-j, color);
				} catch{}
			}
		}
	}
}
