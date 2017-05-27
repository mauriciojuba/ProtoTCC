using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public GameObject SFX;


	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {

        if(Input.anyKeyDown)
            SFX.GetComponent<AudioManager>().playSound("Mosquito_Flying", gameObject);
    }
}
