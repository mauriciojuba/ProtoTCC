using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePos : MonoBehaviour {

	public float X,Y,Z;
	public float Speed;
	public int PlayerNumber;
    public float ScaleToSet;
    public float MultiplierScaleSpeed;
    private bool SetS;


	void Start () {
		if (PlayerNumber == 1) {
			Y = 0.8f;
		}

	}

    // Update is called once per frame
    void Update()
    {
        if (!SetS)
        {
            if (transform.position != Camera.main.ViewportToWorldPoint(new Vector3(X, Y, Z)))
            {
                transform.position = Vector3.MoveTowards(transform.position, Camera.main.ViewportToWorldPoint(new Vector3(X, Y, Z)), Speed * Time.deltaTime);
                transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(0, 90, 90), Speed * Time.deltaTime * 5);
            }

            if (transform.localScale != new Vector3(ScaleToSet, ScaleToSet, ScaleToSet))
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(ScaleToSet, ScaleToSet, ScaleToSet), Speed * Time.deltaTime * MultiplierScaleSpeed);
            }
        }
        
    }

	public IEnumerator SetScale(){
		yield return new WaitForSeconds (0.2f);
		SetS = true;
	}
}
