using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectController : MonoBehaviour {

	public GameObject inputManager;
	public GameObject blackBox, whiteBox;
	int MIN_X = 0;
	int MAX_X = Utilites.targetScreenWidth / Utilites.pixelPerUnit;
	int MIN_Y = 2;
	int MAX_Y = Utilites.targetScreenHeight / Utilites.pixelPerUnit - 3;

	enum CellState {normal, deleting, free};

	enum GameState { play, pause, stop };


	float timeLastGen;
	CellState[,] cellState;
	GameState gameState;
	public float delay = 2.0f;
	public float delayDelta;
	float delayScale = 1;


	void Start() {

		cellState = new CellState[MAX_X - MIN_X, MAX_Y - MIN_Y];

		for (int i = MIN_X; i < MAX_X; i++) {
			for (int j = MIN_Y; j < MAX_Y; j++) {
				SetCellState(i, j, CellState.free);
			}
		}

		if(InputManager.touchEventCoord != null)
		InputManager.touchEventCoord.AddListener(StartDeleteCell);
		Score.scoreEvent.AddListener(ChangeDelay);
		GameUIManager.changeDelayScaleEvent.AddListener(SetDelayScale);
		Score.staticGameOverEvent.AddListener(SetStop);

		timeLastGen = Time.time;
		SetPlay();

	}

	void Update() {
		if(gameState == GameState.play)
		if (Time.time - timeLastGen > delay*delayScale && !IsFieldFull()) {
			Generate();
		}

	}

	void Generate() {
		Vector2 pos = GenerateFreePos();
		int color = Random.Range(0, 11);
		if (0 <= color && color <= 2) {
			Instantiate(blackBox, pos, Quaternion.identity);
		}
		else {
			Instantiate(whiteBox, pos, Quaternion.identity);			
		}

		timeLastGen = Time.time;
		SetCellState(pos, CellState.normal);

	}

	Vector2 GenerateFreePos() {

		int x = Random.Range(MIN_X, MAX_X);
		int y = Random.Range(MIN_Y, MAX_Y);
		if (GetCellState(x, y) == CellState.free)
			return new Vector2((float)x, (float)y);
		else return GenerateFreePos();
	}

	bool IsFieldFull() {
		for (int i = 0; i < MAX_X-MIN_X; i++)
			for (int j = 0; j < MAX_Y-MIN_Y; j++)
				if (cellState[i, j] == CellState.free)
					return false;
		return true;
	}

	void DeleteCell(int x, int y) {
		if (MIN_X <= x && x < MAX_X && MIN_Y <= y && y < MAX_Y) {

			if (IsFieldFull()) {
				timeLastGen = Time.time;
			}	
			
			float timeStartDeleting = Time.time;
			SetCellState(x, y, CellState.free);
		}
	}

	void DeleteCell(Vector2 v) {
		int x = (int)v.x;
		int y = (int)v.y;		
		DeleteCell(x, y);
	}

	IEnumerator DeleteCellCoroutine(Vector2 vector2) {
		if (GetCellState(vector2) != CellState.deleting) {
			SetCellState(vector2, CellState.deleting);
			yield return new WaitForSeconds(2);
			DeleteCell(vector2);
		}
		
	}

	void StartDeleteCell(Vector2 vector2) {
		if (GetCellState(vector2) != CellState.free) {
			StartCoroutine(DeleteCellCoroutine(vector2));
		}

	}

	void ChangeDelay() {
		if (delay + delayDelta > 0) {
			delay += delayDelta;
			if (delayDelta > -3.4e38 * 1.5f)
				delayDelta /= 1.5f;
		}
	}

	float GetDelay() { return delay; }

	void SetNewDelay(float newDelay) {
		if (newDelay > 0)
			delay = newDelay;
	}

	void SetDelayScale(float scaleFactor) {
		if(scaleFactor > 0)
			delayScale = scaleFactor;
	}

	void SetCellState(float x, float y) {
		SetCellState((int)x, (int)y);
	}
	
	void SetCellState(Vector2 screenPos, CellState state) {
		int x = (int)screenPos.x;
		int y = (int)screenPos.y;
		SetCellState(x, y, state);
	}
	
	void SetCellState(int x, int y, CellState state) {
		cellState[x-MIN_X, y-MIN_Y] = state;
	}

	CellState GetCellState(Vector2 screenPos) {
		int x = (int)screenPos.x;
		int y = (int)screenPos.y;
		return GetCellState(x, y);		
	}

	CellState GetCellState(int x, int y) {
		return cellState[x-MIN_X, y-MIN_Y];
	}

	void SetGameState(GameState newState) {
		gameState = newState;
	}

	public void SetPause(){
		SetGameState(GameState.pause);
		Time.timeScale = 0;
		inputManager.SetActive(false);
	}

	public void SetPlay() {
		SetGameState(GameState.play);
		Time.timeScale = 1;
		inputManager.SetActive(true);
	}

	public void SetStop() {
		SetGameState(GameState.stop);
		Time.timeScale = 0;
		inputManager.SetActive(false);
	}
	
}
