using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSMMosquito : MonoBehaviour
{

    #region FSM States
    public enum FSMStates { Idle, Walk, ATK1, ATK2, Damage, StepBack, Grappled, Thrown, DrainLife, Die, Fall, Transition, Patrol };
    public FSMStates state = FSMStates.Idle;
    #endregion

    #region Variaveis

    public GameObject Target;
    public List<GameObject> Players;

    public Transform[] waypoints;

    public float MoveSpeed;             //Velocidade De Movimentção
    public float RotationSpeed;         //Velocidade De Rotação

    public float Vision = 5f;           //Area Para o Npc Identificar o Player
    public float SafeDist = 10f;        //Area Para o Npc desistir de perceguir o Player
    public float EnemyDist = 2f;        //Area para Iniciar o Ataque
    public float Life = 100;              //Vida Do NPC


    public bool TakeDamage = false;      //Verifica se o player levou dano
    [SerializeField] private float Distace;               //Distancia entre o NPC e o player
    [SerializeField] private float TimeToChangeTarget = 5f;
    private float BasicDamage;          //Valor base de Dano

    private float[] PlayersDist = new float[2];
    public Animator MosquitoAni;        //Aramazena as animações do mosquito
    private Transform myTransform;      //
    private int currentWayPoint;        //
    private Rigidbody rb;               //
    public float TimeToNextPoint = 5f;  //Tempo para o proximo way point
    private float TimeTo;               //
    private bool cor = false;
    public GameObject hitbox;

    #endregion

    #region Unity Functions

    void Start()
    {

        if (GameObject.FindWithTag("Player1_3D") != null)
            Players.Add(GameObject.FindWithTag("Player1_3D"));

        if (GameObject.FindWithTag("Player2_3D") != null)
            Players.Add(GameObject.FindWithTag("Player2_3D"));

        CalculaDistancia();
        StartCoroutine(CalcDist());

        MosquitoAni = gameObject.GetComponent<Animator>();
        TimeTo = TimeToNextPoint;
        myTransform = transform;
        rb = GetComponent<Rigidbody>();

    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Vision);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnemyDist);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, SafeDist);
    }

    public void FixedUpdate()
    {




        Distace = Vector3.Distance(Target.transform.position, gameObject.transform.position);

        //switch funciona como um "if" mas só para variaveis inteiras

        switch (state)
        {

            case FSMStates.Idle:
                Idle();
                break;

            case FSMStates.Patrol:
                Patrol();
                break;

            case FSMStates.Walk:
                Walk();
                break;

            case FSMStates.ATK1:
                ATK1();
                break;

            case FSMStates.ATK2:
                ATK2();
                break;

            case FSMStates.Damage:
                Damage();
                break;

            case FSMStates.StepBack:
                StepBack();
                break;

            case FSMStates.Grappled:
                Grappled();
                break;

            case FSMStates.Thrown:
                Thrown();
                break;

            case FSMStates.DrainLife:
                DrainLife();
                break;

            case FSMStates.Die:
                Die();
                break;

            case FSMStates.Fall:
                Fall();
                break;

            case FSMStates.Transition:
                Transition();
                break;


        }

    }

    IEnumerator EsperaAnim(float tempo, string NomeEstado)
    {

        yield return new WaitForSeconds(tempo);
        if (NomeEstado == "StepBack")
            state = FSMStates.StepBack;


        cor = false;

    }

    IEnumerator CalcDist()
    {
        yield return new WaitForSeconds(TimeToChangeTarget);
        CalculaDistancia();
        StartCoroutine(CalcDist());
    }

    #endregion

    #region Minhas funcoes

    public void CalculaDistancia()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            PlayersDist[i] = Vector3.Distance(Players[i].transform.position, gameObject.transform.position);
        }

        if (PlayersDist[0] < PlayersDist[1])
            Target = Players[0];
        else
            Target = Players[1];

    }

    public void HitBoxOn()
    {
        hitbox.SetActive(true);
    }

    public void HitBoxOff()
    {
        hitbox.SetActive(false);
    }

    #endregion

    #region Idle
    private void Idle()
    {

        MosquitoAni.SetBool("IsIdle", true);

        //Verifica se o player entrou no alcance da visao do mosquito
        if (Vector3.Distance(Target.transform.position, gameObject.transform.position) < Vision)
        {
            MosquitoAni.SetBool("IsIdle", false);
            state = FSMStates.Walk;
        }
        TimeToNextPoint -= Time.deltaTime;
        if (TimeToNextPoint < 0)
        {
            currentWayPoint++;
            if (currentWayPoint >= waypoints.Length)
                currentWayPoint = 0;
            TimeToNextPoint = TimeTo;

            MosquitoAni.SetBool("IsIdle", false);
            state = FSMStates.Patrol;

        }

        if (TakeDamage)
            state = FSMStates.Damage;

    }
    #endregion

    #region Patrol
    private void Patrol()
    {
        MosquitoAni.SetBool("IsWalk", true);
        Vector3 dir = waypoints[currentWayPoint].position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * RotationSpeed);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (dir.sqrMagnitude <= 1)
        {
            MosquitoAni.SetBool("IsWalk", false);
            state = FSMStates.Idle;
        }
        else
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * MoveSpeed);

        if (Distace < Vision)
            state = FSMStates.Walk;

        if (TakeDamage)
            state = FSMStates.Damage;

    }
    #endregion

    #region Walk
    private void Walk()
    {

        MosquitoAni.SetBool("IsWalk", true);
        Vector3 dir = Target.transform.position;

        //rotaciona o Npc apontando para o alvo
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Target.transform.position - myTransform.position), Time.deltaTime * RotationSpeed);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (Distace > EnemyDist && Vector3.Distance(Target.transform.position, gameObject.transform.position) < SafeDist)
        {


            //Move o Npc para o alvo;
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * MoveSpeed);
        }

        if (Distace <= EnemyDist)
            state = FSMStates.ATK1;

        if (Distace > SafeDist + 1)
            state = FSMStates.Patrol;

        if (TakeDamage)
            state = FSMStates.Damage;

    }
    #endregion

    #region ATK1
    private void ATK1()
    {
        if (!cor)
        {
            MosquitoAni.SetTrigger("ATK1");
        }

        state = FSMStates.Walk;

        if (TakeDamage)
            state = FSMStates.Damage;

    }
    #endregion

    #region ATK2
    private void ATK2()
    {

    }
    #endregion

    #region Damage
    private void Damage()
    {



        MosquitoAni.SetBool("IsWalk", false);
        if (!cor)
        {
            MosquitoAni.SetTrigger("TakeDamage");
            Life -= 20;
            if (Life > 0)
            {
                StartCoroutine(EsperaAnim(1f, "StepBack"));
                cor = true;
            }

            else
                state = FSMStates.Die;

        }


        TakeDamage = false;

    }
    #endregion

    #region Step back
    private void StepBack()
    {


        TimeToNextPoint = 0;
        //rotaciona o Npc apontando para lodo oposto do alvo
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(myTransform.position - Target.transform.position), RotationSpeed * Time.deltaTime);
        //Move o Npc para o alvo;
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;

        if (Distace > SafeDist + 2)
            state = FSMStates.Idle;

    }
    #endregion

    #region Grappled
    private void Grappled()
    {

    }
    #endregion

    #region Thrown
    private void Thrown()
    {

    }
    #endregion

    #region Drain Life
    private void DrainLife()
    {

    }
    #endregion

    #region Die
    private void Die()
    {
        MosquitoAni.SetBool("UsingWings", false);
        MosquitoAni.SetTrigger("Death");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;



    }
    #endregion

    #region Fall
    private void Fall()
    {

    }
    #endregion

    #region Transition
    private void Transition()
    {

    }
    #endregion
}
