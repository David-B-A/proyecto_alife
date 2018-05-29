using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specie1 : MonoBehaviour {
	float[] maxycabeza = new float[] {0,0.3f,0.4f,0.5f,0.5f,0.5f,0.5f,0.3f};
	float[] maxycuerpo = new float[] {0.5f,0.6f,0.7f,0.75f,0.8f,0.85f,0.9f,0.95f,1,1,1,1,1,1,1,1,1,1,1,1,1,0.95f,0.9f,0.85f,0.8f,0.75f,0.7f,0.6f,0.45f,0.35f,0.15f,0};

	int width = 256;
	int heigth = 256;

	float scale = 20f;

	float[] maxy;
	float[] maxyorig;

	public Material material;
	public int transformAfin;

	Mesh mesh;
	MeshRenderer meshRenderer;
	MeshFilter meshFilter;

	Vector3[] vertices;
	int[] triangles;

	float xi = 0;
	float yi = 0.5f;
	float zi = 0;
	float tamanox = 5;
	float tamanoz = 1;
	// Use this for initialization
	void Start () {
		maxy = new float[maxycabeza.Length + maxycuerpo.Length];
		for (int i = 0; i < maxycabeza.Length; i++) {
			maxy [i] = maxycabeza [i];
		}
		for (int i = 0; i < maxycuerpo.Length; i++) {
			maxy [i + maxycabeza.Length] = maxycuerpo [i];
		}

		maxyorig = maxy;
		if (transformAfin>0) {
			for (int i = 1; i <= transformAfin; i++) {
				transformacionAfin ();		
			
			}	
		}
		crearAnimal ();
	}
	


	public void crearAnimal(){

		#region inicializar malla
		meshFilter = gameObject.AddComponent<MeshFilter>();
		meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Renderer renderer = GetComponent<Renderer>();
		meshRenderer.material = material;
		renderer.material.mainTexture = GenerateTexture();

		mesh = new Mesh();
		meshFilter.mesh = mesh;
		#endregion

		#region encontrar puntos

		int pasosx = maxycabeza.Length + maxycuerpo.Length;
		int pasosz = 10;
		vertices = new Vector3[(pasosx)*(pasosz+1)];




		float x,y,z;

		for (int i = 0; i < pasosz + 1; i++) {
			z = zi - tamanoz / 2 + i * tamanoz / pasosz;

			for (int j = 0; j < pasosx; j++) {
				x = xi - tamanox / 2 + j *tamanox / pasosx;
				y = maxy[j]*Mathf.Sqrt (1-Mathf.Pow(((z-zi)/(tamanoz/2)),2))+yi;
				vertices [j+(pasosx)*i] = new Vector3 (x,y,z);

			}
		}
		#endregion

		#region hacer triangulos
		triangles = new int[(pasosx)*(pasosz+1)*2*3];
		int posVecTriangles = 0;
		for (int i = 0; i < vertices.Length - pasosx-1;i++){
			triangles[posVecTriangles] = i;
			posVecTriangles++;
			triangles[posVecTriangles] = i + pasosx;
			posVecTriangles++;
			triangles[posVecTriangles] = i + pasosx + 1;
			posVecTriangles++;
			triangles[posVecTriangles] = i;
			posVecTriangles++;
			triangles[posVecTriangles] = i + pasosx + 1;
			posVecTriangles++;
			triangles[posVecTriangles] = i + 1;
			posVecTriangles++;
		}

		#endregion
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		transform.Rotate(new Vector3 (0, 90, 0));
	}

	public void transformacionAfin(){
		for (int i = 0; i < maxy.Length; i++) {
			float xnorm = (float)i / (float)maxy.Length;
			maxy[i] = maxy[i]*(Mathf.Cos(Mathf.PI*xnorm)/2+1);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	Texture2D GenerateTexture()
	{
		Texture2D texture = new Texture2D(width, heigth);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < heigth; y++)
			{
				Color color = setColor(x, y);
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
		return texture;
	}

	Color setColor(int x, int y)
	{
		float xcoord = (float)x / width * scale;
		float ycoord = (float)y / heigth * scale;

		float sample = Mathf.PerlinNoise(xcoord, ycoord);

		return new Color(sample, sample, sample);
	}
}
