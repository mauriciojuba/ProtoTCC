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
    [SerializeField] private Rigidbody rb;               //
    public float TimeToNextPoint = 5f;  //Tempo para o proximo way point
    private float TimeTo;               //
    private bool cor = false;
    public GameObject hitbox;
    public bool grappled = false;
	[SerializeField] private float CooldownAtk = 3f;
	[SerializeField] private float TimerAtk;
	[SerializeField] private float MaxLife;
	private bool returned;
	[SerializeField] private Collider DeathCollider;



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
        rb = gameObject.GetComponent<Rigidbody>();
		MaxLife = Life;

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

	IEnumerator ResetStates(){
		yield return new WaitForSeconds (1);
		state = FSMStates.Patrol;
		returned = false;
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
        if (Players.Count > 1)
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
        else if(Players.Count == 1) {

            Target = Players[0];

        }
    }

    public void HitBoxOn()
    {
		hitbox.GetComponent<Collider> ().enabled = true;
    }

    public void HitBoxOff()
    {
		hitbox.GetComponent<Collider> ().enabled = false;
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
        MosquitoAni.SetBool("IsParolling", true);
        Vector3 dir = waypoints[currentWayPoint].position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * RotationSpeed);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (dir.sqrMagnitude <= 1)
        {
            MosquitoAni.SetBool("IsParolling", false);
            state = FSMStates.Idle;
        }
        else
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * MoveSpeed);

		if (Distace < Vision) {
			MosquitoAni.SetBool("IsParolling", false);
			state = FSMStates.Walk;
		}
        if (TakeDamage)
            state = FSMStates.Damage;

    }
    #endregion

    #region Walk
    private void Walk()
    {

        MosquitoAni.SetBool("FightingWalk", true);
        Vector3 dir = Target.transform.position;

        //rotaciona o Npc apontando para o alvo
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Target.transform.position - myTransform.position), Time.deltaTime * RotationSpeed);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (Distace > EnemyDist && Vector3.Distance(Target.transform.position, gameObject.transform.position) < SafeDist)
        {


            //Move o Npc para o alvo;
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * MoveSpeed);
        }

		TimerAtk += Time.deltaTime;
		if (Distace <= EnemyDist && TimerAtk >= CooldownAtk) {
			state = FSMStates.ATK1;
			TimerAtk = 0;
		}

        if (Distace > SafeDist + 1)
        {
            MosquitoAni.SetBool("FightingWalk", false);
            state = FSMStates.Patrol;
        }

        if (TakeDamage)
            state = FSMStates.Damage;

    }
    #endregion

    #region ATK1
    private void ATK1()
    {
		
	 	MosquitoAni.SetTrigger ("ATK1");
		

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
        MosquitoAni.SetBool("FightingWalk", false);
        if (!cor)
        {
			if (Life > 0 && Life <= MaxLife * 0.2f && !grappled)
            {
                StartCoroutine(EsperaAnim(1f, "StepBack"));
                cor = true;
            }
			else if(Life <= 0)
                state = FSMStates.Die;
        }
        TakeDamage = false;
		if (MosquitoAni.GetCurrentAnimatorStateInfo (0).IsName ("Take Damage") && MosquitoAni.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.7f && !grappled && Life > 0) {
			state = FSMStates.Walk;
		} else if (MosquitoAni.GetCurrentAnimatorStateInfo (0).IsName ("Take Damage") && MosquitoAni.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.7f && grappled && Life > 0) {
			state = FSMStates.Grappled;
		}

    }
    #endregion

    #region Step back
    private void StepBack()
    {

		MosquitoAni.SetBool("FightingWalk", false);
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
		grappled = true;
        MosquitoAni.SetBool("IsIdle", true);
        MosquitoAni.SetBool("IsParolling", false);
        MosquitoAni.SetBool("FightingWalk", false);
        MosquitoAni.SetBool("UsingWings", false);
    }
    #endregion

    #region Thrown
    private void Thrown()
    {
		grappled = false;
		MosquitoAni.SetBool("IsIdle", false);
		MosquitoAni.SetBool("IsParolling", false);
		MosquitoAni.SetBool("FightingWalk", false);
		MosquitoAni.SetBool("UsingWings", false);
		if (!returned) {
			StartCoroutine (ResetStates ());
			returned = true;
		}
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
		DeathCollider.enabled = true;
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

	public void SetTakeDamageAnim(){
		MosquitoAni.SetTrigger("TakeDamage");
	}
}
