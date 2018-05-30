using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioDeCamara : MonoBehaviour {

	Camera camaraPanoramica;
	Camera camaraPrimeraPersona;
	GameObject [] barrasDeVida;

	// Use this for initialization
	void Start () {
		camaraPanoramica = Camera.allCameras [0];
		camaraPrimeraPersona = Camera.allCameras [1];
		camaraPanoramica.enabled = false;
		camaraPrimeraPersona.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		barrasDeVida = GameObject.FindGameObjectsWithTag ("barraVida");
		if (Input.GetKeyDown("1")) {
			camaraPanoramica.enabled = false;
			camaraPrimeraPersona.enabled = true;
		}
		if (Input.GetKeyDown("2")) {
			camaraPanoramica.enabled = true;
			camaraPrimeraPersona.enabled = false;
		}
		if (camaraPanoramica.enabled) {
			for (int i = 0; i < barrasDeVida.Length; i++) {
				barrasDeVida [i].transform.rotation = Quaternion.Euler(0,0,0);
			}
		} else {
			for (int i = 0; i < barrasDeVida.Length; i++) {
				barrasDeVida[i].transform.rotation = Quaternion.LookRotation (camaraPrimeraPersona.transform.position - barrasDeVida [i].transform.position);
			}
		}
	}
}
