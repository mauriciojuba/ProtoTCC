using UnityEngine;
using System.Collections;

public class FSMMosquito : MonoBehaviour {

    #region FSM States
    public enum FSMStates { Idle, Walk, ATK1, ATK2, Damage, StepBack, Grappled, Thrown, DrainLife, Die, Fall, Transition, Patrol };
    public FSMStates state = FSMStates.Idle;
    #endregion

    #region Variaveis

    public GameObject Target;

    public Transform[] waypoints;

    public float MoveSpeed;
    public float RotationSpeed;

    public float Vision = 5f;
    public float SafeDist = 10f;
    public float EnemyDist = 2f;
    public int Life = 100;

    public bool levoudano = false;

    public float Distace;

    private float BasicDamage;


    public Animator MosquitoAni;
    private Transform myTransform;
    private int currentWayPoint;
    private Rigidbody rb;
    public float TimeToNextPoint = 5f;
    private float TimeTo;
    #endregion

    #region Unity Functions

    void Start () {

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

    public void FixedUpdate(){

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
	#endregion

	#region Idle
	private void Idle(){

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

        // if (........)
        //state = FSMStates.Estado_2;
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

        if(Distace < Vision)
            state = FSMStates.Walk;

    }
    #endregion

    #region Walk
    private void Walk(){

        Vector3 dir = Target.transform.position;

        //rotaciona o Npc apontando para o alvo
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Target.transform.position- myTransform.position), Time.deltaTime * RotationSpeed);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (Distace > EnemyDist && Vector3.Distance(Target.transform.position, gameObject.transform.position) < SafeDist)
        {
            
            
            //Move o Npc para o alvo;
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * MoveSpeed);
        }

        if(Distace > SafeDist +1)
            state = FSMStates.Patrol;

        if (levoudano)
            state = FSMStates.Damage;

    }
	#endregion

	#region ATK1
	private void ATK1(){

	}
    #endregion

    #region ATK2
    private void ATK2(){

	}
    #endregion

    #region Damage
    private void Damage()
    {
        Life -= 20;
        levoudano = false;

        state = FSMStates.StepBack;
    }
    #endregion

    #region Step back
    private void StepBack()
    {
        
        //rotaciona o Npc apontando para lodo oposto do alvo
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,  Quaternion.LookRotation(myTransform.position - Target.transform.position), RotationSpeed * Time.deltaTime);
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
