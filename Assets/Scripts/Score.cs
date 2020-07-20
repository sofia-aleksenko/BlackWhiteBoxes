using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Score : MonoBehaviour {

	public long score;
	UnityEngine.UI.Text text;
	public static UnityEvent scoreEvent;
	[SerializeField]
	public UnityEvent gameOverEvent;
	public UnityEvent newGameOverEvent;
	[System.Serializable]
	public class MyUnityEvent : UnityEvent{};
	public static UnityEvent staticGameOverEvent;
	public int scoreStep;
	public int gameOverScore;
	long lastLevelScore = 0;
	int level = 1;
	Color color, colorPositive, colorNegative;

	void Start () {
		InputManager.touchEventScore.AddListener(AddPoints);
		
	}
	
	void Update () {
		
	}

	void AddPoints(int delta) {
		score += delta;
		UpdateScore();

		if (score == scoreStep * level && score > lastLevelScore) {
			scoreEvent.Invoke();
			lastLevelScore = score;
			level++;
		}
		CheckScore();
	}

	void Awake() {		
		newGameOverEvent = new UnityEvent();
		staticGameOverEvent = new UnityEvent();
		text = this.GetComponent<UnityEngine.UI.Text>();
		text.text = score.ToString();
		color = new Color();
		colorPositive = new Color(0, 255, 0);
		colorNegative = new Color(255, 0, 0);
		scoreEvent = new UnityEvent();
		gameOverEvent = new MyUnityEvent();
	}

	void UpdateScore() {
		if (score >= 0)
			color = colorPositive;
		else
			color = colorNegative;
		text.color = color;
		text.text = score.ToString();
	}

	// Check if score is too negative
	void CheckScore() {
		if (score <= gameOverScore) {
			gameOverEvent.Invoke();
			staticGameOverEvent.Invoke();
		}
	}
}
