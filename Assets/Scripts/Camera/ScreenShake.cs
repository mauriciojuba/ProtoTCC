using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

	    public float _amplitude = 0.1f;
    private Vector3 initialPosition;
    private bool isShaking = false;
    public static ScreenShake Instance;

	void Start () {
        initialPosition = transform.localPosition;
        Instance = this;
    }
    
    public void Shake(float amplitude, float duration)
    {
		initialPosition = transform.localPosition;
        amplitude = _amplitude;
        isShaking = true;
        CancelInvoke();
        Invoke("StopShaking", duration);
    }
    public void StopShaking()
    {
        isShaking = false;
    }

    void Update () {
        if (isShaking)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * _amplitude;
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            Shake(0.2f,0.5f);
        }
		
	
	}
}
