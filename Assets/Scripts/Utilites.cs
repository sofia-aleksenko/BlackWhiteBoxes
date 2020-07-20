using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilites : MonoBehaviour {

	public static int pixelPerUnit = 128;
	public static int targetScreenHeight = 1280;
	public static int targetScreenWidth = 640;
	public static float cameraHeight = targetScreenHeight / pixelPerUnit / 2;
	public enum ScoreMode { fadeEqDie, fadeIsNotTheEnd };
	public static ScoreMode scoreMode;

	void Awake() {
		DontDestroyOnLoad(this);
		scoreMode = ScoreMode.fadeIsNotTheEnd;
	}

	public static float GetCameraWidth() {
		return (float)targetScreenWidth / (float)pixelPerUnit / 2.0f;
	}

	public static float GetCameraHeight() {
		return (float)targetScreenHeight / (float)pixelPerUnit / 2.0f;
	}

	public void SetFadeEqDieMode() {
		scoreMode = ScoreMode.fadeEqDie;
	}

	public void SetFadeIsNotTheEnd() {
		scoreMode = ScoreMode.fadeIsNotTheEnd;
	}


}
