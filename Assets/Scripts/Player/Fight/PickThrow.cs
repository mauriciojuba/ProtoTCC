using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickThrow : MonoBehaviour {


	[SerializeField] private float Radius;
	[SerializeField] private Collider[] colliders;
	[SerializeField] List<Collider> CollidersInRange;
	Ray ray;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		colliders = Physics.OverlapSphere (transform.position, Radius);

		for(int i = 0; i < colliders.Length; i ++){
			if(!colliders [i].GetComponent<IgnoreRangeCast>()){
				if (!CollidersInRange.Contains (colliders [i]))
					CollidersInRange.Add (colliders [i]);
			}
		}


	}

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, Radius);
	}
}
