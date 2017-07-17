using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruir : MonoBehaviour {


    public bool SelfDestruct;
    [SerializeField] private float Tempo;

    private void Start()
    {
        if (SelfDestruct)
            ApagarDaExistenciaTimer(Tempo);
    }

    public void ApagarDaExistencia()
    {
        Destroy(gameObject);
    }

    public void ApagarAlvoDaExistencia(GameObject Target) { }

    public void ApagarDaExistenciaTimer(float Timer)
    {
        Destroy(gameObject, Timer);
    }

    public void ApagarDaExistenciaLentamente() { }

}
