using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSMAranha : MonoBehaviour
{

    #region FSM States
    public enum FSMStates
    {
        Idle, Walk, ATK1, ATK2, Damage, StepBack, Grappled, Thrown, Die, Transition, Patrol
    };
    public FSMStates state = FSMStates.Idle;
    #endregion

    #region Variaveis

    public GameObject Target;
    public List<GameObject> Players;
    public GameObject Shot;

    public GameObject VidaPlayer;
    public GameObject VidaTatu;


    public float MoveSpeed;                                    //Velocidade De Movimentção
    public float RotationSpeed;                                //Velocidade De Rotação

    public Transform[] waypoints;                              //Lista de Waypoints

    public float Vision = 5f;                                  //Area Para o Npc Identificar o Player
    public float SafeDist = 10f;                               //Area Para o Npc desistir de perceguir o Player
    public float EnemyDist = 2f;                               //Area para Iniciar o Ataque

    public GameObject ModelMosquito;
    private float LifeDist;

    public float Life = 100;                                   //Vida Do NPC
    [SerializeField] private float MaxLife;                    //Vida Maxima

    public bool TakeDamage = false;                            //Verifica se o player levou dano
    private float[] PlayersDist = new float[2];
    public Animator AranhaAnimator;                               //Aramazena as animações da Aranha
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
    public float velTransicao;                                 //Velocidade da tranzição pra tela
    public Transform model;
    public Transform direcoes;
    public bool toWorld;
    public CameraControl DollyCam;
    private bool descer = false;
    private bool returned;
    public Transform camScreen;
    float _2dY, _2dX;
    public bool onScreen;




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

        AranhaAnimator = gameObject.GetComponent<Animator>();
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

            case FSMStates.Die:
                Die();
                break;

            case FSMStates.Transition:
                Transition();
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
            if(hitbox[0] !=  null)
            hitbox[0].GetComponent<Collider>().enabled = true;

        else
            if(hitbox[1] != null)
            hitbox[1].GetComponent<Collider>().enabled = true;
    }

    //Desliga as hitbox de ataque do mosquito

    public void HitBoxOff()
    {
        if (!onScreen)
            if (hitbox[0] != null)
                hitbox[0].GetComponent<Collider>().enabled = false;

        else
                if (hitbox[1] != null)
                hitbox[1].GetComponent<Collider>().enabled = false;
    }

    void Descer()
    {

        transform.SetParent(null);
        toWorld = true;
        reachScreen = false;
        onScreen = false;

    }

    public void SpiderShot()
    {
        
    }

    //faz o mosquito ir para a tela

    //------------------------------------------------------------------------------------------------------//
    //void goToScreen()
    // {
    //     if (!reachScreen)
    //     {
    //         transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(camScreen.localPosition.x + _2dX,
    //             camScreen.localPosition.y + _2dY, camScreen.localPosition.z), velTransicao);
    //         //rotação pra deixar o modelo pronto pra movimentação na tela e colocar os pés do modelo no "vidro"
    //         ModelMosquito.transform.localRotation = Quaternion.RotateTowards(ModelMosquito.transform.localRotation, Quaternion.AngleAxis(90, Vector3.right), velTransicao * 10);
    //         transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.AngleAxis(90, Vector3.right), velTransicao * 10);
    //         transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), velTransicao / 10);
    //         if (transform.localPosition == new Vector3(camScreen.localPosition.x + _2dX, camScreen.localPosition.y + _2dY, camScreen.localPosition.z) && transform.localRotation == Quaternion.AngleAxis(90, Vector3.right) && transform.localScale == new Vector3(0.5f, 0.5f, 0.5f))
    //         {
    //	transform.localPosition = new Vector3 (camScreen.localPosition.x + _2dX, camScreen.localPosition.y + _2dY, camScreen.localPosition.z);
    //	transform.localRotation = Quaternion.AngleAxis (90, Vector3.right);
    //	rb.velocity = Vector3.zero;
    //             reachScreen = true;
    //         }
    //     }

    //     //se o jogador pedir pra descer
    //     if (descer)
    //     {
    //         //tira ele do parent, ativa a variavel que fala que é pra ir pro 3D, liga a gravidade, e desativa a variavel que fala q ele ta na tela
    //         //SetAnimOffScreen();
    //         transform.SetParent(null);
    //         toWorld = true;
    //         reachScreen = false;
    //         onScreen = false;
    //         state = FSMStates.Fall;
    //     }
    // }

    //------------------------------------------------------------------------------------------------------//
    //faz o mosquito sair da tela


    //------------------------------------------------------------------------------------------------------//
    //void exitScreen()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, new Vector3(DollyCam.target.position.x,
    //            1f, DollyCam.target.position.z), velTransicao);

    //    ModelMosquito.transform.localRotation = Quaternion.RotateTowards(ModelMosquito.transform.localRotation, Quaternion.AngleAxis(0, Vector3.right), velTransicao * 10);
    //    transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(1, 1, 1), velTransicao / 10);
    //    transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.AngleAxis(0, Vector3.right), velTransicao * 5);
    //    model.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(0, 0, 0), velTransicao * 5);


    //    if (transform.localScale == new Vector3(1, 1, 1) && transform.localRotation.x == 0f)
    //    {
    //        rb.useGravity = true;
    //        model.localRotation = Quaternion.Euler(0, 0, 0);
    //        toWorld = false;
    //    }

    //}
    //------------------------------------------------------------------------------------------------------//


    #endregion

    //executado quado esta patrulhando 
    #region Idle
    private void Idle()
    {

        AranhaAnimator.SetBool("IsIdle", true);

        //Verifica se o player entrou no alcance da visao do mosquito
        if (Vector3.Distance(Target.transform.position, gameObject.transform.position) < Vision)
        {
            AranhaAnimator.SetBool("IsIdle", false);
            state = FSMStates.Walk;
        }
        TimeToNextPoint -= Time.deltaTime;
        if (TimeToNextPoint < 0)
        {
            currentWayPoint++;
            if (currentWayPoint >= waypoints.Length)
                currentWayPoint = 0;
            TimeToNextPoint = TimeTo;

            AranhaAnimator.SetBool("IsIdle", false);
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
        AranhaAnimator.SetBool("IsParolling", true);
        Vector3 dir = waypoints[currentWayPoint].position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * RotationSpeed);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (dir.sqrMagnitude <= 1)
        {
            AranhaAnimator.SetBool("IsParolling", false);
            state = FSMStates.Idle;
        }
        else
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * MoveSpeed);

        if (Distace < Vision)
        {
            AranhaAnimator.SetBool("IsParolling", false);
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

        AranhaAnimator.SetBool("FightingWalk", true);
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
            AranhaAnimator.SetBool("FightingWalk", false);
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
        //intanciar o gameobjec do tiro da aranha.
        iTween.LookTo(gameObject, iTween.Hash("looktarget", Target.transform, "time", 3, "oncomplete", "SpiderShot"));


        AranhaAnimator.SetTrigger("ATK1");

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
            AranhaAnimator.SetBool("FightingWalk", false);
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
            if (AranhaAnimator.GetCurrentAnimatorStateInfo(0).IsName("Take Damage") && AranhaAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f && !grappled && Life > 0)
            {
                state = FSMStates.Walk;
            }
            else if (AranhaAnimator.GetCurrentAnimatorStateInfo(0).IsName("Take Damage") && AranhaAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f && grappled && Life > 0)
            {
                state = FSMStates.Grappled;
            }
        }

    }
    #endregion

    //estado que faz o mosquito recuar
    #region Step back
    private void StepBack()
    {

        AranhaAnimator.SetBool("FightingWalk", false);
        TimeToNextPoint = 0;

        //rotaciona o Npc apontando para lodo oposto do alvo
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(myTransform.position - Target.transform.position), RotationSpeed * Time.deltaTime);

        //Move o Npc para o alvo;
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;

        if (Distace > SafeDist + 2 && Life <= MaxLife * 0.2f)
        {
            AranhaAnimator.SetTrigger("GoToScreen");
            AranhaAnimator.SetBool("UsingWings", false);
            AranhaAnimator.SetBool("GoingToScreen", true);
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
        AranhaAnimator.SetBool("IsIdle", true);
        AranhaAnimator.SetBool("IsParolling", false);
        AranhaAnimator.SetBool("FightingWalk", false);
        AranhaAnimator.SetBool("UsingWings", false);
    }
    #endregion

    //estado do mosquito sendo arremessado
    #region Thrown
    private void Thrown()
    {
        grappled = false;
        AranhaAnimator.SetBool("IsIdle", false);
        AranhaAnimator.SetBool("IsParolling", false);
        AranhaAnimator.SetBool("FightingWalk", false);
        AranhaAnimator.SetBool("UsingWings", true);

        if (!returned)
        {
            StartCoroutine(ResetStates());
            returned = true;
        }
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
            AranhaAnimator.SetTrigger("Death");
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            DeathCollider.enabled = true;
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
        AranhaAnimator.SetTrigger("TakeDamage");
    }
}
