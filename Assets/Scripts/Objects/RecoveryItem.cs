using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryItem : MonoBehaviour {

    public bool RecuperaHP, RecuperaESP;
    public int valorRecuperacao;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1_3D") || other.CompareTag("Player2_3D") || other.CompareTag("Player3_3D") || other.CompareTag("Player4_3D")) {
            if (RecuperaHP)
            {
                if (other.gameObject.GetComponent<Life>() != null)
                {
                    //Aqui deve ser chamado o método(função) que substituirá o Update do script Life.
                    other.gameObject.GetComponent<Life>().LifeQuant += valorRecuperacao;
                }
            }
            if (RecuperaESP)
            {
                //completar quando estiver com barra de especial implementada
            }
            Destroy(gameObject);
        }
    }
}
