using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMsg : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

	void OnEnable() {
		// puts the object to the front (the last element to be drawn)
		transform.SetAsLastSibling();
	}
}
