using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleControl : MonoBehaviour {


	public Movimentacao3D Move3D;
	public Movimentacao2D Move2D;
	public GameObject Player3D;
	public GameObject Player2D;

	public bool In3D;

    void Start()
    {
        ChangeStyle();
    }	
	void Update () {
		if (Input.GetButtonDown("B")) ChangeStyle();
    }

	void ChangeStyle(){
		//Muda o estilo de 3D pra 2D e vice-versa
		In3D = !In3D;

		//Quando esta em 3D, desativa o 2D.
		if (In3D) {
            ComponentsHandler_3D(true);
            ComponentsHandler_2D(false);
            
			//Mantem o personagem 3D na mesma posição visual que o personagem 2D está.
			Player3D.transform.position = new Vector3 (Player2D.transform.position.x, Player3D.transform.position.y, Player2D.transform.position.y);
		} else {
            ComponentsHandler_3D(false);
            ComponentsHandler_2D(true);

			//Mantem o personagem 2D na mesma posição visual que o personagem 3D está.
			Player2D.transform.position = new Vector2 (Player3D.transform.position.x, Player3D.transform.position.z);
		}
	}

    //Desativa/Ativa o codigo de movimentação 3D.
    //Desativa/Ativa as Renders do 3D, o collider e a gravidade.
    void ComponentsHandler_3D(bool active)
    {
		if(active == false)
			Player3D.GetComponent<Rigidbody>().velocity = transform.forward * 0;
		
        Move3D.enabled = active;
        Player3D.GetComponent<MeshRenderer>().enabled = active;
        Player3D.GetComponent<CapsuleCollider>().enabled = active;
        Player3D.GetComponent<Rigidbody>().useGravity = active;
    }

    //Destiva/Ativa o codigo de movimentação 2D.
    //Desativa/Ativa a Sprite 2D
    void ComponentsHandler_2D(bool active)
	{
        Move2D.enabled = active;
        Player2D.GetComponent<SpriteRenderer>().enabled = active;
    }
}
