using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Destruir))]
[RequireComponent(typeof(DropLoot))]
public class ObjLife : MonoBehaviour
{

    public float Life;
    public bool Damage;

    [Header("Destruir")]
    public bool PowerDestroy = true;
    [Tooltip("defalt: Apaga imediatamente apos acabar a vida")]
    public string TypeOfDestruction = "default";

    [HideInInspector] public FightCollider Fight;
    private Destruir Dest;


    // Use this for initialization
    void Start()
    {
        Dest = gameObject.GetComponent<Destruir>();
        if (Life == 0)
            Life = 50;
    }

    public void TakeMamage(float Hit)
    {
        Life -= Hit;
        Fight.InstantiateParticle();

        if (Life <= 0f)
            Dest.ApagarDaExistencia();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<FightCollider>() != null)
        {
            Fight = other.gameObject.GetComponent<FightCollider>();
            TakeMamage(other.GetComponent<FightCollider>().Damage);
        }
    }
}
