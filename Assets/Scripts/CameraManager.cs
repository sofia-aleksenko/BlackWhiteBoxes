using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	void Start() {
		transform.position.Set(0, 0, 0);
		transform.Translate(Utilites.GetCameraWidth() - 0.5f,
			Utilites.GetCameraHeight() - 0.5f,
			0);
	}

	void Update() {

	}


}
