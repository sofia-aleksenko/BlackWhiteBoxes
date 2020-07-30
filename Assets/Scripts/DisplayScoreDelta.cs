using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScoreDelta : MonoBehaviour {

	UnityEngine.UI.Text text;
	float initTime;
	enum ScoreState { normal, fading };
	ScoreState state;
	public float delay;
	Animator animator;

	void Start () {
		
	}
	
	void Update () {
		// if it is time to start fading
		if (Time.time - initTime > delay && state != ScoreState.fading) {
			StartFading();
		}
	}

	void Awake() {
		text = this.GetComponent<UnityEngine.UI.Text>();
		initTime = Time.time;
		state = ScoreState.normal;
		animator = GetComponent<Animator>();
		// puts the object to the front (the last element to be drawn)
		transform.SetAsFirstSibling();
	}

	public void SetScore(int score, Vector2 pos) {
		// positive score
		if (score > 0) {
			text.text = "+" + score.ToString();
			text.color = Color.green;
		}
		//negative score
		else if (score < 0) {
			text.color = Color.red;
			text.text = score.ToString();
		}
		this.transform.position = Camera.main.WorldToScreenPoint(pos);
	}

	void StartFading() {
		state = ScoreState.fading;
		if (animator != null)
			animator.SetTrigger("fade");
		Destroy(this.gameObject, 1.5f);
	}
}
