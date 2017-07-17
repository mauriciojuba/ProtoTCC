using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerLife : MonoBehaviour {


    public float life;

    [HideInInspector] public float MaxLife;
    [HideInInspector] public int Division;
    private CameraControl DollyCam;
    private int QuantImg;
    [HideInInspector]public int QuantImgInScene;
    public GameObject LifeSpritePrefab;
    private GameObject ScreenGlass;
    private int PlayerNumber;
    [HideInInspector] public float X, InitialX;
    [HideInInspector]public List<GameObject> ListOfImg;

    private GameObject EnemyHit;

    
    // Use this for initialization
    void Start()
    {
        DollyCam = GameObject.FindWithTag("DollyCam").GetComponent<CameraControl>();
        ScreenGlass = GameObject.Find("ScreenGlass");


        MaxLife = life;
        PlayerNumber = GetComponent<Movimentacao3D>().PlayerNumber;
        //mudar a quantidade de vida para imagem aqui
        Division = 100;
        if (PlayerNumber == 1)
        {
            InitialX = 0.06f;
            X = InitialX;
        }
        else if (PlayerNumber == 2)
        {
            InitialX = 0.06f;
            X = 0.70f;
        }

        LifeFunc();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (QuantImgInScene < QuantImg)
        {
            LifeFunc();
        }
    }

    public void LifeFunc()
    {
        if (life <= Division)
        {
            GetComponent<Movimentacao3D>().CanMove = false;
            GetComponent<Movimentacao3D>().Stunned = true;
        }
        QuantImg = (int)life / Division;
        if (QuantImgInScene < QuantImg)
        {
            GameObject gb = GameObject.Instantiate(LifeSpritePrefab, Camera.main.ViewportToWorldPoint(new Vector3(X, 1.5f, 1)), ScreenGlass.transform.rotation);
            gb.transform.SetParent(ScreenGlass.transform);
            QuantImgInScene++;
            gb.GetComponent<Rigidbody>().useGravity = false;
            gb.GetComponent<Rigidbody>().isKinematic = true;
            gb.gameObject.GetComponent<LifePos>().PlayerNumber = PlayerNumber;
            gb.gameObject.GetComponent<LifePos>().X = X;
            gb.gameObject.GetComponent<LifePos>().Player = gameObject;
            gb.gameObject.GetComponent<ScaleLife>().TotalTatuLife = Division;
            gb.gameObject.GetComponent<ScaleLife>().TatuLife = Division;
            X += InitialX;
            ListOfImg.Add(gb);
        }
        else if (QuantImgInScene > QuantImg)
        {
            Destroy(ListOfImg[QuantImgInScene - 1], 15);
            ListOfImg[QuantImgInScene - 1].transform.SetParent(null);
            ListOfImg[QuantImgInScene - 1].GetComponent<LifePos>().enabled = false;
            ListOfImg[QuantImgInScene - 1].GetComponent<ScaleLife>().dead = true;
            ListOfImg[QuantImgInScene - 1].GetComponent<Animator>().enabled = false;
            ListOfImg[QuantImgInScene - 1].GetComponent<Rigidbody>().freezeRotation = false;
            ListOfImg[QuantImgInScene - 1].GetComponent<Rigidbody>().isKinematic = false;
            ListOfImg[QuantImgInScene - 1].GetComponent<Rigidbody>().useGravity = true;
            ListOfImg.RemoveAt(QuantImgInScene - 1);
            QuantImgInScene--;
            X -= InitialX;
        }
    }

    public void ApplyDamage(float hit)
    {

        life -= (int)hit;
        LifeFunc();
        ListOfImg[QuantImg - 1].GetComponent<ScaleLife>().TatuLife -= (int)hit;
        ListOfImg[QuantImgInScene - 1].GetComponent<ScaleLife>().UpdateScaleLife();
        gameObject.GetComponent<Movimentacao3D>().SetTakeDamageAnim();

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<FightCollider>() != null)
        {
            ApplyDamage(other.gameObject.GetComponent<FightCollider>().Damage);
        }
    }

    [ContextMenu("Tira 100")]
     void TestTiraVida()
    {
        ApplyDamage(100f);
    }

}
