using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RAIN.Core;
using UnityEngine.UI;

public class Life : MonoBehaviour {


	public enum LifeType {Player, Enemy, Boss, Object};
	public LifeType LifeOF;
	public float LifeQuant;
    public GameObject[] Loot;
    [Range(0,100)]
    public int LootChance;


    #region Variables For Enemys Life
    AIRig ai;
    #endregion

    #region Variables For Objects Life
    public GameObject ObjDestruido;
    #endregion

    #region Variables For Players Life
    public List<GameObject> ListOfImg;
	private int PlayerNumber;
	private GameObject LifeOBJ;
	private GameObject Container;
	private int Division;
	private int QuantImgInScene;
	[SerializeField] private int QuantImg;
	[SerializeField] private GameObject LifeSpritePrefab;
    #endregion

    void Start () {
        if (LifeOF == LifeType.Player)
        {
            PlayerNumber = GetComponent<Movimentacao3D>().PlayerNumber;
            LifeOBJ = GameObject.Find("UI").transform.FindChild("LifeP" + PlayerNumber).gameObject;
            Container = LifeOBJ.transform.FindChild("Container").gameObject;
			//mudar a quantidade de vida para imagem aqui
            Division = 30;
            UpdateLife();
        }
	}

	void Update(){
		if (QuantImgInScene < QuantImg && LifeOF == LifeType.Player) {
			UpdateLife ();
		}
	}

    
	public void UpdateLife () {
		switch (LifeOF) {
            case LifeType.Object:
                ObjectLife();
                break;
            case LifeType.Player:
			    PlayerLife ();
			    break;
		    case LifeType.Enemy:
			    EnemyLife();
			    break;
		    case LifeType.Boss:
			    BossLife();
			    break;
		}
	}

    void ObjectLife()
    {
        if (LifeQuant <= 0)
        {
            GameObject bo = Instantiate(ObjDestruido, transform.position, Quaternion.identity);
            DropLoot();
            Destroy(bo, 3f);
            Destroy(this);
            Destroy(gameObject);
        }
    }
    void DestroyObject()
    {
        
    }

	void PlayerLife(){
		LifeOBJ.SetActive (true);
		QuantImg = (int)LifeQuant / Division;
		if (QuantImgInScene < QuantImg) {
				GameObject gb = GameObject.Instantiate (LifeSpritePrefab);
				gb.transform.SetParent (Container.transform);
				QuantImgInScene++;
				ListOfImg.Add (gb);
		} else if (QuantImgInScene > QuantImg) {
			Destroy (ListOfImg [QuantImgInScene - 1]);
			ListOfImg.RemoveAt (QuantImgInScene - 1);
			QuantImgInScene--;
		}

		if (Input.GetButtonDown ("LB P" + PlayerNumber)) {
			LifeQuant -= Random.Range (20, 50);
		}
		if (Input.GetButtonDown ("RB P" + PlayerNumber)) {
			LifeQuant += Random.Range (20, 50);
		}
	}

	void EnemyLife(){
        ai = GetComponentInChildren<AIRig>();
        ai.AI.WorkingMemory.SetItem("takingDamage", true);
        ai.AI.WorkingMemory.SetItem("vida", LifeQuant);
    }

	void BossLife(){
	
	}

    void DropLoot()
    {
        float random = Random.Range(0, 100);
        if (random <= LootChance)
        {
            if (Loot[0] != null)
            {
                int drop = Random.Range(0, Loot.Length);
                Debug.Log(drop);
                Instantiate(Loot[drop],transform.position,Quaternion.identity);
            }
        }
    }
}
