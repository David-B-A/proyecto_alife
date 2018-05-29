using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioDeCamara : MonoBehaviour {

	Camera camaraPanoramica;
	Camera camaraPrimeraPersona;

	// Use this for initialization
	void Start () {
		camaraPanoramica = Camera.allCameras [0];
		camaraPrimeraPersona = Camera.allCameras [1];
		camaraPanoramica.enabled = false;
		camaraPrimeraPersona.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("1")) {
			camaraPanoramica.enabled = false;
			camaraPrimeraPersona.enabled = true;
		}
		if (Input.GetKeyDown("2")) {
			camaraPanoramica.enabled = true;
			camaraPrimeraPersona.enabled = false;
		}
	}
}
