﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSMMosquito : MonoBehaviour
{

    #region FSM States
    public enum FSMStates
    {
        Idle, Walk, ATK1, ATK2, Damage, StepBack, Grappled, Thrown, DrainLife, Die, Fall, Transition, Patrol, GoToScreen,
        OnScreen, GoToWorld
    };
    public FSMStates state = FSMStates.Idle;
    #endregion

    #region Variaveis

    public GameObject Target;
    public List<GameObject> Players;

    public GameObject VidaPlayer;
    public GameObject VidaTatu;


    public float MoveSpeed;                                    //Velocidade De Movimentção
    public float RotationSpeed;                                //Velocidade De Rotação


    public Transform[] waypoints;                              //Lista de Waypoints

    public float Vision = 5f;                                  //Area Para o Npc Identificar o Player
    public float SafeDist = 10f;                               //Area Para o Npc desistir de perceguir o Player
    public float EnemyDist = 2f;                               //Area para Iniciar o Ataque
    public float LifeDrainDist = 1f;

    public GameObject ModelMosquito;
    private float LifeDist;


    public float Life = 100;                                   //Vida Do NPC
    [SerializeField] private float MaxLife;                    //Vida Maxima

    public bool TakeDamage = false;                            //Verifica se o player levou dano
    private float[] PlayersDist = new float[2];
    public Animator MosquitoAni;                               //Aramazena as animações do mosquito
    private Transform myTransform;                             //
    private int currentWayPoint;                               //
    public float TimeToNextPoint = 5f;                         //Tempo para o proximo way point
    private float TimeTo;                                      //
    private bool cor = false;
    public GameObject[] hitbox;                                  //Hitbox do ataque do mosquito
    public bool grappled = false;                              //Verifica se o mosquito esta sendo agarrado
    [SerializeField] private Rigidbody rb;                     //
    [SerializeField] private float CooldownAtk = 3f;           //Tempo de recarga do ataque
    [SerializeField] private float TimerAtk;                   //Tempo de recarga do ataque
    [SerializeField] private float Distace;                    //Distancia entre o NPC e o player
    [SerializeField] private float TimeToChangeTarget = 5f;    //
    [SerializeField] private Collider DeathCollider;
    [SerializeField] private bool reachScreen;
    public float velTransicao;                                 //Velocidade da tranzição 
    public Transform model;
    public Transform direcoes;
    public bool toWorld;
    public CameraControl DollyCam;
    private bool descer = false;
    private bool returned;
    public Transform camScreen;
    float _2dY, _2dX;
    public bool onScreen;

    private GameObject part;

    public GameObject ParticulaLifeDrain;

	public Transform rootJoint;




    #endregion

    #region Unity Functions

    void Start()
    {
        //teste
        //if(GameObject.Find("SoundManager") != null)
        //SoundManager.PlayCappedSFX("Mosquito_Flying_Loop_01", "Mosquito_Flying_Loop_01_cap");

        //Encontra os player

        if (GameObject.FindWithTag("Player1_3D") != null)
        {
            Players.Add(GameObject.FindWithTag("Player1_3D"));
        }

        if (GameObject.FindWithTag("Player2_3D") != null)
            Players.Add(GameObject.FindWithTag("Player2_3D"));


        CalculaDistancia();
        StartCoroutine(CalcDist());

        MosquitoAni = gameObject.GetComponent<Animator>();
        TimeTo = TimeToNextPoint;
        myTransform = transform;
        rb = gameObject.GetComponent<Rigidbody>();
        MaxLife = Life;
		if (GameObject.FindWithTag("ScreenGlass") != null)
			camScreen = GameObject.FindWithTag ("ScreenGlass").transform;
		if (GameObject.FindWithTag ("DollyCam") != null)
			DollyCam = GameObject.FindWithTag ("DollyCam").GetComponent<CameraControl> ();
    }

    //mostra as distancias de interacoes do mosquitpo
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

            case FSMStates.GoToScreen:
                GoToScreen();
                break;

            case FSMStates.GoToWorld:
                GoToWorld();
                break;

            case FSMStates.OnScreen:
                OnScreen();
                break;



        }

    }

    IEnumerator ResetStates()
    {
        yield return new WaitForSeconds(1);
        state = FSMStates.Patrol;
        returned = false;
    }

    IEnumerator EsperaAnim(float tempo, string NomeEstado)
    {

        yield return new WaitForSeconds(tempo);
        if (NomeEstado == "StepBack")
            state = FSMStates.StepBack;

        if(NomeEstado == "OnScreen")
            state = FSMStates.OnScreen;

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

    //pega a posicao da ultima vida de um player e seta como alvo


    public void PegaVidaPlayer()
    {
        VidaPlayer = Players[(int)Random.Range(0, Players.Count - 1)];

        VidaTatu = (GameObject)VidaPlayer.GetComponent<Life>().ListOfImg[VidaPlayer.GetComponent<Life>().ListOfImg.Count - 1];
    }
    public Transform direction;

    //move o mosquito para a vida  do player

    public void MovePraVida()
    {
        if(VidaTatu.GetComponent<ScaleLife> ().TatuLife <= 0) DrainLifeEnd();
        LifeDist = Vector3.Distance(VidaTatu.transform.position, gameObject.transform.position);

        //colocar o modelo no centro do objeto pernilongo
        ModelMosquito.transform.localPosition = new Vector3(ModelMosquito.transform.localPosition.x, ModelMosquito.transform.localPosition.y, -1f);

        //para não olhar direto pro tatu
        Vector3 correctLook = Vector3.Lerp(direction.position, VidaTatu.transform.position, RotationSpeed * Time.deltaTime);

        //olha para o tatu e coloca arruma a posição, ponta-cabeça
        transform.LookAt(correctLook, new Vector3(-1, -1, 1));

        // transform.position = Vector3.MoveTowards(transform.position, VidaTatu.transform.position, MoveSpeed);

        rb.MovePosition(transform.position + transform.forward * Time.deltaTime * MoveSpeed);
    }

    //executa quando acaba a animaçao sugar a vida do player

    public void DrainLifeEnd()
    {

        ParticleSystem particleemitter = part.GetComponent<ParticleSystem>();
        if (particleemitter != null)
        {
            ParticleSystem.EmissionModule emit = particleemitter.emission;
            emit.enabled = false;
        }
        Destroy(part, 5f);

        //VidaTatu.GetComponent<LifePos>().Player.GetComponent<Life>().LifeQuant -= 100;

      //  VidaTatu.GetComponent<LifePos>().Player.GetComponent<Life>().UpdateL1 = true;


        Descer();
        MoveSpeed = 4f;
        Life += 100;
        
            MosquitoAni.SetBool("GoingToWorld", true);
        MosquitoAni.SetBool("LifeDrain", false);
        state = FSMStates.GoToWorld;

    }

    public void DrainLifeStart()
    {
        part = Instantiate(ParticulaLifeDrain, VidaTatu.transform.position, Quaternion.identity) as GameObject;
		part.transform.parent = VidaTatu.transform;

		part.GetComponent<ParticleHoming>().target = rootJoint;

        part.SetActive(true);

		StartCoroutine (DrainLifeCor ());

    }

    //Calcula a Distancia do Player mais Proximo 

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
        else if (Players.Count == 1)
        {

            Target = Players[0];

        }
    }

    //Liga as hitbox de ataque do mosquito

    public void HitBoxOn()
    {
        if (!onScreen)
            hitbox[0].GetComponent<Collider>().enabled = true;

        else
            hitbox[1].GetComponent<Collider>().enabled = true;
    }

    //Desliga as hitbox de ataque do mosquito

    public void HitBoxOff()
    {
        if (!onScreen)
            hitbox[0].GetComponent<Collider>().enabled = false;

        else
            hitbox[1].GetComponent<Collider>().enabled = false;
    }

    void Descer()
    {

        transform.SetParent(null);
        toWorld = true;
        reachScreen = false;
        onScreen = false;

    }

    //faz o mosquito ir para a tela

    void goToScreen()
    {
        if (!reachScreen)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(camScreen.localPosition.x + _2dX,
                camScreen.localPosition.y + _2dY, camScreen.localPosition.z), velTransicao);
            //rotação pra deixar o modelo pronto pra movimentação na tela e colocar os pés do modelo no "vidro"
            ModelMosquito.transform.localRotation = Quaternion.RotateTowards(ModelMosquito.transform.localRotation, Quaternion.AngleAxis(90, Vector3.right), velTransicao * 10);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.AngleAxis(90, Vector3.right), velTransicao * 10);
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), velTransicao / 10);
            if (transform.localPosition == new Vector3(camScreen.localPosition.x + _2dX, camScreen.localPosition.y + _2dY, camScreen.localPosition.z) && transform.localRotation == Quaternion.AngleAxis(90, Vector3.right) && transform.localScale == new Vector3(0.5f, 0.5f, 0.5f))
            {
				transform.localPosition = new Vector3 (camScreen.localPosition.x + _2dX, camScreen.localPosition.y + _2dY, camScreen.localPosition.z);
				transform.localRotation = Quaternion.AngleAxis (90, Vector3.right);
				rb.velocity = Vector3.zero;
                reachScreen = true;
            }
        }

        //se o jogador pedir pra descer
        if (descer)
        {
            //tira ele do parent, ativa a variavel que fala que é pra ir pro 3D, liga a gravidade, e desativa a variavel que fala q ele ta na tela
            //SetAnimOffScreen();
            transform.SetParent(null);
            toWorld = true;
            reachScreen = false;
            onScreen = false;
            state = FSMStates.Fall;
        }
    }

    //faz o mosquito sair da tela

    void exitScreen()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(DollyCam.target.position.x,
                1f, DollyCam.target.position.z), velTransicao);

        ModelMosquito.transform.localRotation = Quaternion.RotateTowards(ModelMosquito.transform.localRotation, Quaternion.AngleAxis(0, Vector3.right), velTransicao * 10);
        transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(1, 1, 1), velTransicao / 10);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.AngleAxis(0, Vector3.right), velTransicao * 5);
        model.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, 0, 0), velTransicao * 5);


        if (transform.localScale == new Vector3(1, 1, 1) && transform.localRotation.x == 0f)
        {
            rb.useGravity = true;
            model.localRotation = Quaternion.Euler(0, 0, 0);
            toWorld = false;
        }

    }

    #endregion

    //executado quado esta patrulhando 
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

    //estado de patrulha do mosquito
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

        if (Distace < Vision)
        {
            MosquitoAni.SetBool("IsParolling", false);
            state = FSMStates.Walk;
        }
        if (TakeDamage)
            state = FSMStates.Damage;

    }
    #endregion

    //estado do mosquito indo atraz do player
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
        if (Distace <= EnemyDist && TimerAtk >= CooldownAtk)
        {
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

    //estado de ataque fraco do mosquito
    #region ATK1
    private void ATK1()
    {

        MosquitoAni.SetTrigger("ATK1");

        state = FSMStates.Walk;

        if (TakeDamage)
            state = FSMStates.Damage;

    }
    #endregion

    //estado de ataque forte do mosquito
    #region ATK2
    private void ATK2()
    {

    }
    #endregion

    //estado de dano do mosquito
    #region Damage
    private void Damage()
    {
        if (!onScreen)
        {
            MosquitoAni.SetBool("FightingWalk", false);
            if (!cor)
            {
                if (Life > 0 && Life <= MaxLife * 0.2f && !grappled)
                {
                    StartCoroutine(EsperaAnim(1f, "StepBack"));
                    cor = true;
                }
                else if (Life <= 0)
                    state = FSMStates.Die;
            }
            TakeDamage = false;
            if (MosquitoAni.GetCurrentAnimatorStateInfo(0).IsName("Take Damage") && MosquitoAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f && !grappled && Life > 0)
            {
                state = FSMStates.Walk;
            }
            else if (MosquitoAni.GetCurrentAnimatorStateInfo(0).IsName("Take Damage") && MosquitoAni.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f && grappled && Life > 0)
            {
                state = FSMStates.Grappled;
            }
        }
        if (onScreen)
        {
            MosquitoAni.SetTrigger("TakeDamageScreen");
            TakeDamage = false;
            state = FSMStates.OnScreen;


        }

    }
    #endregion

    //estado que faz o mosquito recuar
    #region Step back
    private void StepBack()
    {

        MosquitoAni.SetBool("FightingWalk", false);
        TimeToNextPoint = 0;
        //rotaciona o Npc apontando para lodo oposto do alvo
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(myTransform.position - Target.transform.position), RotationSpeed * Time.deltaTime);
        //Move o Npc para o alvo;
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;

        if (Distace > SafeDist + 2 && Life <= MaxLife * 0.2f)
        {
            MosquitoAni.SetTrigger("GoToScreen");
            MosquitoAni.SetBool("UsingWings", false);
            MosquitoAni.SetBool("GoingToScreen", true);

            state = FSMStates.GoToScreen;
        }

        else if (Distace > SafeDist + 2)
            state = FSMStates.Idle;

    }
    #endregion

    //estado do mosquito agarrado
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

    //estado do mosquito sendo arremessado
    #region Thrown
    private void Thrown()
    {
        grappled = false;
        MosquitoAni.SetBool("IsIdle", false);
        MosquitoAni.SetBool("IsParolling", false);
        MosquitoAni.SetBool("FightingWalk", false);
        MosquitoAni.SetBool("UsingWings", true);
        if (!returned)
        {
            StartCoroutine(ResetStates());
            returned = true;
        }
    }
    #endregion

    //estado do mosquito sugando a vida do player
    #region Drain Life
    private void DrainLife()
    {
        if(TakeDamage)
        state = FSMStates.Damage;

        

        MosquitoAni.SetBool("LifeDrain", true);
		//para não olhar direto pro tatu
		Vector3 correctLook = Vector3.Lerp(direction.position, VidaTatu.transform.position, RotationSpeed * Time.deltaTime);

		//olha para o tatu e coloca arruma a posição, ponta-cabeça
		transform.LookAt(correctLook, new Vector3(-1, -1, 1));


    }

    #endregion

    //estado de morte do mosquito
    #region Die
    private void Die()
    {

        Descer();
        gameObject.GetComponent<Rigidbody>().useGravity = true;

        if (!onScreen)
        {
            MosquitoAni.SetBool("UsingWings", false);
            MosquitoAni.SetTrigger("Death");
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            DeathCollider.enabled = true;
        }

        else
        {
            exitScreen();
            MosquitoAni.SetBool("UsingWings", false);
            MosquitoAni.SetTrigger("Death");
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            DeathCollider.enabled = true;
        }

    }
    #endregion

    //estado do mosquito caindo da tela
    #region Fall
    private void Fall()
    {
        if (toWorld) exitScreen();
        else state = FSMStates.Idle;
    }
    #endregion

    //estado do mosquito indo pra tela 
    #region GoToScreen
    private void GoToScreen()
    {
        if (!onScreen)
        {
            rb.useGravity = false;
            transform.SetParent(camScreen);
            _2dX = Random.Range(-1.5f, +1.5f);
            _2dY = Random.Range(-0.8f, +0.8f);
            onScreen = true;
        }
        else goToScreen();

        if (reachScreen)
        {
            PegaVidaPlayer();
            MosquitoAni.SetBool("GoingToScreen", false);
            state = FSMStates.OnScreen;
        }
    }
    #endregion

    //estado de movimentacao do mosquito na tela 
    #region OnScreen
    private void OnScreen()
    {

        if (TakeDamage)
        {
            state = FSMStates.Damage;
        }

        MovePraVida();

        MoveSpeed = 0.5f;

        if (LifeDist <= LifeDrainDist)
            state = FSMStates.DrainLife;

        if (Life >= 0.5f * MaxLife)
        {
            Descer();
            state = FSMStates.GoToWorld;
        }
    }
    #endregion

    //estado do mosquito indo pro mundo
    #region GoToWorld
    private void GoToWorld()
    {
        exitScreen();
        MosquitoAni.SetBool("UsingWings", true);
        MosquitoAni.SetBool("GoingToWorld", true);
        if (!toWorld)
        {
            MosquitoAni.SetBool("GoingToWorld", false);
            MosquitoAni.SetBool("FightingWalk", true);
            state = FSMStates.StepBack;
        }
    }
    #endregion

    //deletar
    #region Transition
    private void Transition()
    {
    }
    #endregion

    public void SetTakeDamageAnim()
    {
        MosquitoAni.SetTrigger("TakeDamage");
    }

	IEnumerator DrainLifeCor(){
		while (VidaTatu.GetComponent<ScaleLife> ().TatuLife > 0) {
			yield return new WaitForSeconds (0.3f);
			VidaTatu.GetComponent<ScaleLife> ().TatuLife -= 10;
			VidaTatu.GetComponent<LifePos> ().Player.GetComponent<Life> ().LifeQuant -= 10;
			VidaTatu.GetComponent<LifePos> ().Player.GetComponent<Life> ().UpdateL1 = true;
			VidaTatu.GetComponent<ScaleLife> ().UpdateScaleLife ();
		}
	}
}
