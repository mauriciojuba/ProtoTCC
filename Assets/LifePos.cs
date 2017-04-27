using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePos : MonoBehaviour {

	public float X,Y,Z;
	public float Speed;
	public int PlayerNumber;
	private bool SetS;
	void Start () {
		if (PlayerNumber == 1) {
			Y = 0.9f;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (!SetS) {
			if (transform.position != Camera.main.ViewportToWorldPoint (new Vector3 (X, Y, Z))) {
				transform.position = Vector3.MoveTowards (transform.position, Camera.main.ViewportToWorldPoint (new Vector3 (X, Y, Z)), Speed * Time.deltaTime);
			}
		} else {
			transform.localScale = Vector3.MoveTowards(transform.localScale,new Vector3 (1, 1, 1),Speed * Time.deltaTime * 2);
			if (transform.localScale == new Vector3 (1, 1, 1)) {
				transform.position = Vector3.MoveTowards (transform.position, Camera.main.ViewportToWorldPoint (new Vector3 (-1, transform.position.y, transform.position.z)), Speed * Time.deltaTime);

			}
		}
	}

	public IEnumerator SetScale(){
		yield return new WaitForSeconds (0.2f);
		SetS = true;
	}
}
