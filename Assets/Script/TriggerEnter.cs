using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnter : MonoBehaviour {

	public bool triggerInFlag=false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag=="GameElement"){
			triggerInFlag=true;
		}
	}
		
	void OnTriggerExit2D(Collider2D other) {
		if(other.gameObject.tag=="GameElement"){
			triggerInFlag=false;
		}
	}
}
