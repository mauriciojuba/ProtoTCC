using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruir : MonoBehaviour {

    //Algumas Maneiras de Destruir Um Objeto.

    //Apaga Da Existencia o Objeto Intantaneamente.
    public void ApagarDaExistencia()
    {
        Destroy(gameObject);
    }

    public void ApagarAlvoDaExistencia(GameObject Target) { }

    public void ApagarDaExistenciaTimer() { }

    public void ApagarDaExistenciaLentamente() { }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
