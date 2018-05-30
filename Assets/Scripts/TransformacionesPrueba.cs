using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TransformacionesPrueba : MonoBehaviour {

	public int factorx;
	public int factory;
	public BitArray codigoGenetico;

	// Use this for initialization
	private int calcularFenotipo(BitArray codGen, int c) {
		int a = c * 4;
		string crom = String.Concat (Convert.ToInt32(codGen [a+0]),Convert.ToInt32(codGen [a+1]),Convert.ToInt32(codGen [a+2]),Convert.ToInt32(codGen [a+3]));
		int fen = Convert.ToInt32 (crom,2);
		return fen;
	}
	void Start () {
		factorx = calcularFenotipo(codigoGenetico,0);
		factory = calcularFenotipo(codigoGenetico,1);
		TransformacionAfin (factorx,factory);
	}


	private void TransformacionAfin (int factorx, int factory) {
		factory = factory - 5;
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		float minz = vertices [0].z;
		float maxz = vertices [0].z;
		for (int i = 0; i < vertices.Length; i++) {
			if (vertices [i].z < minz) {
				minz = vertices [i].z;
			} if (vertices [i].z > maxz) {
				maxz = vertices [i].z;
			}
		}

		int j = 0;
		while (j < vertices.Length) {
			vertices[j].y = vertices[j].y * (0.2f*factory*Mathf.Sin(Mathf.PI*(vertices[j].z - minz)/(2*(maxz - minz)))+1);
			vertices[j].x = vertices[j].x * (0.2f*factorx*Mathf.Sin(Mathf.PI*(vertices[j].z - minz)/(2*(maxz - minz)))+1);
			j++;
		}
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}

}
