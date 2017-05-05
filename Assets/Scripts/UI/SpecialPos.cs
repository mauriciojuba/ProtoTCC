using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPos : MonoBehaviour {

	public float X,Y,Z;
	public float Speed;
	public int PlayerNumber;
	public float ScaleToSet;
	public bool SetPos;
	public float MultiplierRotationSpeed;
	public float MultiplierScaleSpeed;
	public Animator Anim;
	public UseSpecial XRef;
	void OnEnable () {
		X = 0.05f;
		Anim = GetComponent<Animator> ();
		if (PlayerNumber == 1) {
			Y = 0.75f;
			Z = 1;
			X = X * (XRef.SpecialInScreen.Count + 1);
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A))
			SetGoToScreenAnimation ();
		if (SetPos) {
			if (transform.position != Camera.main.ViewportToWorldPoint (new Vector3 (X, Y, Z))) {
				transform.position = Vector3.MoveTowards (transform.position, Camera.main.ViewportToWorldPoint (new Vector3 (X, Y, Z)), Speed * Time.deltaTime);
				transform.LookAt (Camera.main.ViewportToWorldPoint (new Vector3 (X, Y, Z)));
			} else {
				SetReachAnimation ();
				transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, transform.localEulerAngles.z);

				if(transform.localEulerAngles.y != 180)
					transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles ,new Vector3 (transform.localEulerAngles.x, 180, transform.localEulerAngles.z), Speed * Time.deltaTime * MultiplierRotationSpeed);

				if(transform.localEulerAngles.z != 0)
					transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles ,new Vector3 (transform.localEulerAngles.x, transform.localEulerAngles.y, 0), Speed * Time.deltaTime * MultiplierRotationSpeed);
			}
			if (transform.localScale != new Vector3 (ScaleToSet, ScaleToSet, ScaleToSet)) {
				transform.localScale = Vector3.MoveTowards (transform.localScale, new Vector3 (ScaleToSet, ScaleToSet, ScaleToSet), Speed * Time.deltaTime * MultiplierScaleSpeed);
			}
		}
	}

	void SetReachAnimation(){
		if (Anim != null) {
			Anim.SetBool ("ReachedScreen", true);
		}
	}

	public void SetGoToScreenAnimation(){
		if (Anim != null) {
			Anim.SetBool ("GoToScreen", true);
		}
		transform.parent.GetComponent<UseSpecial> ().SpecialInScreen.Add (gameObject);
		transform.SetParent (transform.parent.GetComponent<UseSpecial> ().ScreenGlass);
		SetPos = true;
	}
}
