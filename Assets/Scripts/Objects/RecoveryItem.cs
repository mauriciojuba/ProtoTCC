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
                    other.gameObject.GetComponent<Life>().LifeQuant += valorRecuperacao;
					other.gameObject.GetComponent<Life> ().UpdateLife ();
                }
            }
            if (RecuperaESP)
            {
				if (other.gameObject.GetComponent<UseSpecial>() != null)
				{
					other.gameObject.GetComponent<UseSpecial>().SpecialBar += valorRecuperacao;
					other.gameObject.GetComponent<UseSpecial> ().UpdateBar ();
				}
            }
            Destroy(gameObject);
        }
    }
}
