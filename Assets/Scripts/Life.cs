using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RAIN.Core;
using UnityEngine.UI;

public class Life : MonoBehaviour {


	public enum LifeType {Player, Enemy, Boss, Object};
    public enum EnemyType {Mosquito};
	public enum ObjectType {Box, Barricade};
    public LifeType LifeOF;
    public EnemyType TypeEnemy;
	public ObjectType TypeOfObject;
	public float LifeQuant;
    public GameObject[] Loot;
    [Range(0,100)]
    public int LootChance;


    #region Variables For Enemys Life



    #endregion

    #region Variables For Objects Life
    public GameObject ObjDestruido;
    #endregion

    #region Variables For Players Life
    public List<GameObject> ListOfImg;
	[SerializeField] private int PlayerNumber;
	private GameObject LifeOBJ;
	private GameObject Container;
	private int Division;
	private int QuantImgInScene;
	[SerializeField] private int QuantImg;
	[SerializeField] private GameObject LifeSpritePrefab;
	[SerializeField] private GameObject ScreenGlass;
	[SerializeField] private CameraControl DollyCam;
	public float X, InitialX;
    #endregion
	[SerializeField] private bool UpdateL;
    private SFX sfx;

     void Start()
    {
        if(GetComponent<SFX>() != null)
        {
           sfx = GetComponent<SFX>();
        }
    }

    void Awake () {

        

        if (LifeOF == LifeType.Player)
        {
            PlayerNumber = GetComponent<Movimentacao3D>().PlayerNumber;
			//mudar a quantidade de vida para imagem aqui
            Division = 50;
			if (PlayerNumber == 1) {
				InitialX = 0.06f;
				X = InitialX;
			} else if (PlayerNumber == 2) {
				InitialX = 0.06f;
				X = 0.70f;
			}
        }
        UpdateLife();
    }

	void Update(){
		if (UpdateL) {
			UpdateLife ();
			UpdateL = false;
		}

		if (QuantImgInScene < QuantImg && LifeOF == LifeType.Player) {
			UpdateLife ();
		}
		if (LifeOF == LifeType.Object) {
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
			switch(TypeOfObject){
			case ObjectType.Box:
				GameObject GB = GameObject.Instantiate (ObjDestruido, transform.position, Quaternion.identity) as GameObject;
				Component[] RBGB;
				RBGB = GB.transform.GetComponentsInChildren<Rigidbody> ();
				foreach (Rigidbody rb in RBGB) {
					rb.velocity = gameObject.GetComponent<Rigidbody> ().velocity;
				}
				if (sfx != null) {
					sfx.PlaySoundSfxGrupo ("Caixa Quebrando");
				}
				break;
			case ObjectType.Barricade:
				Instantiate (ObjDestruido, transform.position, Quaternion.identity);
				break;
			}
            DropLoot();
            Destroy(gameObject);
        }
    }
    void DestroyObject()
    {
        
    }

	void PlayerLife(){
		if (LifeQuant <= Division) {
			GetComponent<Movimentacao3D> ().CanMove = false;
			GetComponent<Movimentacao3D> ().Stunned = true;
			if (!DollyCam.StunnedPlayers.Contains (gameObject))
				DollyCam.StunnedPlayers.Add (gameObject);
		}
		QuantImg = (int)LifeQuant / Division;
		if (QuantImgInScene < QuantImg) {
			GameObject gb = GameObject.Instantiate (LifeSpritePrefab,Camera.main.ViewportToWorldPoint(new Vector3(X,1.5f,1)),ScreenGlass.transform.rotation);
			gb.transform.SetParent (ScreenGlass.transform);
			QuantImgInScene++;
			gb.GetComponent<Rigidbody> ().useGravity = false;
			gb.GetComponent<Rigidbody> ().isKinematic = true;
			gb.gameObject.GetComponent<LifePos> ().PlayerNumber = PlayerNumber;
			gb.gameObject.GetComponent<LifePos> ().X = X;
			X += InitialX;
			ListOfImg.Add (gb);
		} else if (QuantImgInScene > QuantImg) {
			Destroy (ListOfImg [QuantImgInScene - 1],15);
			ListOfImg [QuantImgInScene - 1].GetComponent<Rigidbody> ().freezeRotation = false;
			ListOfImg [QuantImgInScene - 1].GetComponent<Rigidbody> ().isKinematic = false;
			ListOfImg [QuantImgInScene - 1].GetComponent<Rigidbody> ().AddForce (ListOfImg [QuantImgInScene - 1].transform.up * 100);
			ListOfImg [QuantImgInScene - 1].GetComponent<Rigidbody> ().useGravity = true;
			ListOfImg [QuantImgInScene - 1].GetComponent<LifePos> ().StartCoroutine (ListOfImg [QuantImgInScene - 1].GetComponent<LifePos> ().SetScale ());
			ListOfImg.RemoveAt (QuantImgInScene - 1);
			QuantImgInScene--;
			X -= InitialX;
		}

	}

	void EnemyLife(){
        switch (TypeEnemy)
        {
            case EnemyType.Mosquito:
               gameObject.GetComponent<FSMMosquito>().Life = LifeQuant;
                break;
        }
        //ai.AI.WorkingMemory.SetItem("takingDamage", true);
        //ai.AI.WorkingMemory.SetItem("vida", LifeQuant);
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
