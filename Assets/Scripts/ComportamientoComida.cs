using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoComida : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "presa") {
			Destroy (gameObject);
			DistribuidorDeComida.cuenta = DistribuidorDeComida.cuenta - 1;
		}

	}
}
