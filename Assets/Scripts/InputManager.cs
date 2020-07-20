using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour {

	[System.Serializable]
	public class TouchEventCoord : UnityEvent<Vector2> { };
	public class TouchEventScore : UnityEvent<int> { };
	public static TouchEventCoord touchEventCoord;
	public static TouchEventScore touchEventScore;
	public static UnityEvent touchEvent;

	public int plusPoints, minusPoints;
	public GameObject scoreDeltaPrefab;

	
	void Start () {
		
	}
	
	void Update () {
		// touch on object
		if (touchEventCoord != null && Input.GetMouseButtonDown(0)) {			
			Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
			Vector2 touchPos2D = new Vector2(touchPos.x, touchPos.y);

			RaycastHit2D hit = Physics2D.Raycast(touchPos2D, Vector2.zero);
			if (hit.collider != null) {
				GameObject hitGameObject = hit.collider.gameObject;
				if (hitGameObject.CompareTag("whiteBox") || hitGameObject.CompareTag("blackBox")) {					
					touchEventCoord.Invoke(hitGameObject.transform.position);
					Box box = hitGameObject.GetComponent<Box>();
					Vector3 posWorld = hitGameObject.gameObject.transform.position;
					if (!box.IsDying()) {
						box.SetDying();
						Destroy(hitGameObject, 1f);

						GameObject scoreDeltaObject = Instantiate(scoreDeltaPrefab, posWorld, Quaternion.identity);
						scoreDeltaObject.transform.SetParent(GameObject.FindGameObjectsWithTag("canvas")[0].transform, false);
						DisplayScoreDelta scoreDelta = (DisplayScoreDelta)(scoreDeltaObject.GetComponent<DisplayScoreDelta>());

						switch (hitGameObject.tag) {
							case "whiteBox":
								touchEventScore.Invoke(plusPoints);								
								scoreDelta.SetScore(plusPoints, posWorld);
								
								break;
							case "blackBox":
								touchEventScore.Invoke(minusPoints);
								scoreDelta.SetScore(minusPoints, posWorld);
								break;
						}
					}

				}				
			}
		}
	}

	void Awake() {
		touchEventCoord = new TouchEventCoord();
		touchEventScore = new TouchEventScore();
		touchEvent = new UnityEvent();
	}
}
