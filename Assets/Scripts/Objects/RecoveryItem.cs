using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryItem : MonoBehaviour {

    public bool RecuperaHP, RecuperaESP;
    public int valorRecuperacao;
	public GameObject Player;

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
				Destroy(gameObject);
            }
            if (RecuperaESP)
            {
				Player = other.gameObject;
				transform.SetParent (other.transform);
				transform.position = transform.parent.position;
				other.GetComponent<UseSpecial> ().SpecialInScreen.Add (gameObject);
				GetComponent<SpecialPos> ().XRef = other.GetComponent<UseSpecial> ();
				GetComponent<SpecialPos> ().PlayerNumber = other.GetComponent<UseSpecial> ().PlayerNumber;
				GetComponent<SpecialPos> ().enabled = true;
				SetCollectedAnimations ();

				Destroy (GetComponent<Collider> ());
				Destroy (GetComponent<Rigidbody> ());
            }
            
        }
    }

	public void SetCollectedAnimations(){
		if(GetComponent<Animator> () != null)
			GetComponent<Animator> ().SetBool ("Collected", true);
	}

	public void ADDSpecial(){
		if (Player.GetComponent<UseSpecial>() != null)
		{
			Player.GetComponent<UseSpecial> ().SpecialItens++;
			Player.GetComponent<UseSpecial> ().UpdateBar ();
		}
	}
}
