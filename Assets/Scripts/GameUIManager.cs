using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameUIManager : MonoBehaviour {

	public UnityEngine.UI.Button pauseBtn, resumeBtn;
	public UnityEngine.UI.Text modeText, delayFactorText;
	public GameObject gameOverMessage;
	public UnityEngine.UI.Slider delayFactorSlider;
	public GameObject delayFactor;

	public class ChangeDelayScaleEvent : UnityEvent<float> { };
	public static ChangeDelayScaleEvent changeDelayScaleEvent;
	
	void Start () {
		gameOverMessage.gameObject.SetActive(false);
		Score.staticGameOverEvent.AddListener(GameOver);
		SetResume();
		SetMode();
	}
	
	void Update () {
		
	}

	void Awake() {
		changeDelayScaleEvent = new ChangeDelayScaleEvent();
	}

	public void SetPause() {
		pauseBtn.gameObject.SetActive(false);
		resumeBtn.gameObject.SetActive(true);
		delayFactor.gameObject.SetActive(true);
	}

	public void SetResume() {
		pauseBtn.gameObject.SetActive(true);
		resumeBtn.gameObject.SetActive(false);
		delayFactor.gameObject.SetActive(false);
		changeDelayScaleEvent.Invoke(delayFactorSlider.value);
	}

	public void ExitToManu() {
		SceneManager.LoadScene("Menu");
	}

	public void SetMode() {
		if (Utilites.scoreMode == Utilites.ScoreMode.fadeEqDie) {
			Utilites.scoreMode = Utilites.ScoreMode.fadeIsNotTheEnd;
			modeText.GetComponent<UnityEngine.UI.Text>().text = "fading is not the end";
		}
		else if (Utilites.scoreMode == Utilites.ScoreMode.fadeIsNotTheEnd) {
			Utilites.scoreMode = Utilites.ScoreMode.fadeEqDie;
			modeText.GetComponent<UnityEngine.UI.Text>().text = "fading = the end";
		}
	}

	public void SetDelayText() {
		delayFactorText.text = delayFactorSlider.value.ToString();
	}

	public void GameOver() {
		gameOverMessage.gameObject.SetActive(true);
		StartCoroutine(GameOverCoroutine());
		
	}

	IEnumerator GameOverCoroutine() {
		Debug.Log("GameOverCouroutine");
		yield return new WaitForSeconds(1);
		Debug.Log("Waited");
		ExitToManu();
		
	}
}
