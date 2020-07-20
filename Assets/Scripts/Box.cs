using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

	public float delay, delayDelta;
	float initTime;
	bool isDying;
	public int fadeScore;
	public enum BoxState { normal, fading, dying };
	public BoxState state;
	Animator animator;
	public GameObject scoreDeltaPrefab;

	void Start () {
		initTime = Time.time;
		isDying = false;
		animator = GetComponent<Animator>();
		InputManager.touchEvent.AddListener(SetDying);
		Score.scoreEvent.AddListener(ChangeDelay);
//		GameUIManager.changeDelayScaleEvent.AddListener(SetNewDelay);
	}
	
	void Update () {

		if (Time.time - initTime > delay && state != BoxState.dying) {
			StartFading();
		}
	}

	public void SetDying() {
		isDying = true;
		state = BoxState.dying;
		if(animator != null)
			animator.SetTrigger("die");
	}

	public void SetFading() {
		if (Utilites.scoreMode == Utilites.ScoreMode.fadeEqDie) {
			isDying = true;
		}
		state = BoxState.fading;
		if (animator != null) {
			animator.SetTrigger("fade");
		}
	}

	public BoxState GetState() {
		return state;
	}

	public bool IsDying() {
		return isDying;
	}

	void ChangeDelay() {
		if (delay + delayDelta > 0) {
			delay += delayDelta;
			if(delayDelta > -3.4e38 * 1.5f)
				delayDelta /= 1.5f;
		}
	}

	void SetNewDelay(float newDelay) {
		delay = newDelay * 3f;
	}

	void StartFading() {
		StartCoroutine(SetFadingCourutine());
	}

	IEnumerator SetFadingCourutine() {
		if (GetState() != BoxState.fading) {
			SetFading();

			if (Utilites.scoreMode == Utilites.ScoreMode.fadeEqDie) {

				Vector3 posWorld = this.transform.position;
				GameObject scoreDeltaObject = Instantiate(scoreDeltaPrefab, posWorld, Quaternion.identity);
				scoreDeltaObject.transform.SetParent(GameObject.FindGameObjectsWithTag("canvas")[0].transform, false);
				DisplayScoreDelta scoreDelta = (DisplayScoreDelta)(scoreDeltaObject.GetComponent<DisplayScoreDelta>());
				yield return new WaitForSeconds(0.1f);
				InputManager.touchEventScore.Invoke(fadeScore);
				scoreDelta.SetScore(fadeScore, posWorld);
			}

			yield return new WaitForSeconds(0.8f);
			if (Utilites.scoreMode == Utilites.ScoreMode.fadeIsNotTheEnd) {
				Vector3 posWorld = this.transform.position;
				GameObject scoreDeltaObject = Instantiate(scoreDeltaPrefab, posWorld, Quaternion.identity);
				scoreDeltaObject.transform.SetParent(GameObject.FindGameObjectsWithTag("canvas")[0].transform, false);
				DisplayScoreDelta scoreDelta = (DisplayScoreDelta)(scoreDeltaObject.GetComponent<DisplayScoreDelta>());
				if (!IsDying()) {
					scoreDelta.SetScore(fadeScore, posWorld);
					InputManager.touchEventScore.Invoke(fadeScore);
				}
			}
			
			InputManager.touchEventCoord.Invoke(this.gameObject.transform.position);
			
			Destroy(this.gameObject, 0.00001f);	
		}
	}

}
