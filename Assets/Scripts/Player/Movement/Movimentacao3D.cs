using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentacao3D : MonoBehaviour {

    private Rigidbody rb;
	//Array de direções que o player pode olhar
	public Transform[] Directions;
	//Direção atual que o player esta olhando
	public int ActualDirection;

	//define a velocidade de movimentação do player
	public float Speed;

	//define se o player esta em movimento ou não.
	public bool InMovement;

	//Define o numero do Player.
	public int PlayerNumber;

	//GO do personagem que sera movimentado.
	public GameObject Player;

	[SerializeField] private float JumpForce;
	[SerializeField] private bool InGround;

	public Camera CameraMain;
    public CameraControl DollyCam;
    public Transform playerRoot;
    public Transform direcoes;
    public Transform camScreen;
    public float velTransicao;
    public bool onScreen;
    Quaternion direct2D;
    float _2dY,_2dX;

    public LayerMask NoIgnoredLayers = -1;
	[SerializeField] private float MaxJump;
	private bool Jumping;

	public bool CanMove;


	public Animator Anim;
	void Start () {
		CanMove = true;
        rb = Player.GetComponent<Rigidbody>();
        CameraMain = Camera.main;
		ActualDirection = 3;
        direct2D = Quaternion.Euler(90f, 0f, 0f);
		if (gameObject.GetComponent<Animator>() != null) {
			Anim = GetComponent<Animator> ();
		}
    }

	void Update(){
		//TestPosition ();
		SetAnimations();
		if (CanMove) {
			Jump ();
			goToScreen ();
			exitScreen ();
		}
    }

	void FixedUpdate () {
		if (CanMove) {
			DirectionDefinition ();
		}
		

	}


	//Define a direção que o player olha de acordo com os botoes que ele aperta.
	void DirectionDefinition(){
			if (Input.GetAxisRaw ("Vertical P" + PlayerNumber) > 0) {
				InMovement = true;
				//Define a direção para Cima-Esquerda.
				if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) < 0) {
					ActualDirection = 4;
                direct2D = Quaternion.Euler(225f, 90f, 90f);
            } 
			//Define a direção para Cima-Direita.
			else if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) > 0) {
					ActualDirection = 5;
                    direct2D = Quaternion.Euler(315f, 90f, 90f);
            } 
			//Define a direção para Cima.
			else {
					ActualDirection = 0;
                direct2D = Quaternion.Euler(270f, 90f, 90f);

            }
			} else if (Input.GetAxisRaw ("Vertical P" + PlayerNumber) < 0) {
				InMovement = true;
				//Define a direção para Baixo-Esquerda
				if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) < 0) {
					ActualDirection = 6;
                direct2D = Quaternion.Euler(135f, 90f, 90f);
            } 
			//Define a direção para Baixo-Direita
			else if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) > 0) {
					ActualDirection = 7;
                direct2D = Quaternion.Euler(45f, 90f, 90f);
            } 
			//Define a direção para Baixo.
			else {
					ActualDirection = 1;
                    direct2D = Quaternion.Euler(90f, 90f, 90f);
                }
			} 
		//Define a direção para Esquerda
		else if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) < 0) {
				InMovement = true;
				ActualDirection = 2;
            direct2D = Quaternion.Euler(180f, 90f, 90f);
            
        } 
		//Define a direção para Direita
		else if (Input.GetAxisRaw ("Horizontal P" + PlayerNumber) > 0) {
				InMovement = true;
				ActualDirection = 3;
            direct2D = Quaternion.Euler(0f, 90f, 90f);
        }
        else {
            InMovement = false;
        }
        if (!onScreen) transform.LookAt(new Vector3(Directions[ActualDirection].position.x, transform.position.y, Directions[ActualDirection].position.z));
        else if (reachScreen) transform.localRotation = direct2D;

        //Define se o Player esta em movimento ou não, alterando apenas a velocidade em x e em z, para nao alterar a gravidade.
        if (!onScreen)
        {
            if (InMovement)
            {
                Vector3 V3 = rb.velocity;
				if (Input.GetAxis ("Horizontal P" + PlayerNumber) > 0) {
					V3.x = transform.forward.x * Input.GetAxis ("Horizontal P" + PlayerNumber) * Speed;
				} else if (Input.GetAxis ("Horizontal P" + PlayerNumber) < 0) {
					V3.x = transform.forward.x * -Input.GetAxis ("Horizontal P" + PlayerNumber) * Speed;
				}
				if (Input.GetAxis ("Vertical P" + PlayerNumber) > 0) {
					V3.z = transform.forward.z * Input.GetAxis ("Vertical P" + PlayerNumber) * Speed;
				} else if (Input.GetAxis ("Vertical P" + PlayerNumber) < 0) {
					V3.z = transform.forward.z * -Input.GetAxis ("Vertical P" + PlayerNumber) * Speed;
				}
                rb.velocity = V3;
            }
            else if (!InMovement)
            {
                Vector3 V3 = rb.velocity;
                V3.x = transform.forward.x * 0;
                V3.z = transform.forward.z * 0;
                rb.velocity = V3;
            }
        }
        else
        {
            if (InMovement)
            {
                Vector3 V3 = rb.velocity;
                V3.x = transform.forward.x * Speed / 10;
                V3.z = transform.forward.z * Speed / 10;
                V3.y = transform.forward.z * Speed / 10;
                rb.velocity = V3;
            }
            else if (!InMovement)
            {
                Vector3 V3 = rb.velocity;
                V3.x = transform.forward.x * 0;
                V3.z = transform.forward.z * 0;
                V3.y = transform.forward.z * 0;
                rb.velocity = V3;
            }
        }
    }

    
	//função para pulo
	void Jump(){
		//verifica se o player esta encostando no chão
		InGround = Physics.Linecast (Player.transform.position, Player.transform.position - Vector3.up * 1.1f, NoIgnoredLayers);
		Debug.DrawLine (Player.transform.position,Player.transform.position - Vector3.up * 1.1f);

		//se estiver no chao, pula, apertando A no controle.
		if (Input.GetButtonDown ("A P" + PlayerNumber) && InGround &&!onScreen) {
			
			Jumping = true;
			SetJumpAnim ();
			Vector3 V3 = rb.velocity;
			V3.y = JumpForce;
            rb.velocity = V3;
		}
		if(Input.GetButtonUp("A P" + PlayerNumber)){
			Jumping = false;
		}


		if(Jumping && !onScreen){
			Vector3 V3 = rb.velocity;
			V3.y += JumpForce;
            rb.velocity = V3;
            

        }
		if(rb.velocity.y > MaxJump && !onScreen)
        {
			Jumping = false;
		}
        if (Input.GetButtonDown("LB P" + PlayerNumber) && !onScreen)
        {
            //desabilita gravidade coloca o player como child da tela e faz o caminho do player pra tela(MoveTowards) e diminui o tamanho do player, pra não ficar gigante ao se aproximar
			SetAnimOnScreen();
			rb.useGravity = false;
            transform.SetParent(camScreen);
            _2dX = Random.Range(-1.5f, +1.5f);
            _2dY = Random.Range(-0.8f, +0.8f);
            onScreen = true;
        }

    }

    // variavel que fala quando o personagem vai voltar para o 3D
    [SerializeField] private bool toWorld,reachScreen;

    void goToScreen()
    {
        //testa se o jogador pediu pra ir pra tela
        if (onScreen)
        {
            //variavel para parar de controlar o jogador após ele chegar na tela
            if (!reachScreen)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(camScreen.localPosition.x + _2dX,
                    camScreen.localPosition.y + _2dY, camScreen.localPosition.z), velTransicao);
                //rotação pra deixar o modelo pronto pra movimentação na tela e colocar os pés do modelo no "vidro"
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.AngleAxis(90, Vector3.right), velTransicao * 10);
                transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.25f, 0.25f, 0.25f), velTransicao / 10);
				if (transform.localPosition == new Vector3(camScreen.localPosition.x + _2dX, camScreen.localPosition.y + _2dY,camScreen.localPosition.z) && transform.localRotation == Quaternion.AngleAxis(90, Vector3.right) && transform.localScale == new Vector3(0.25f, 0.25f, 0.25f))
                {
                    reachScreen = true;
                }
            }
            //coloca as esferas de direções na tela também;
            direcoes.transform.SetParent(camScreen);
            direcoes.transform.localRotation = Quaternion.AngleAxis(270, Vector3.right);


            //se o jogador pedir pra descer
            if (Input.GetButtonDown("RB P" + PlayerNumber))
            {
                //tira ele do parent, ativa a variavel que fala que é pra ir pro 3D, liga a gravidade, e desativa a variavel que fala q ele ta na tela
				SetAnimOffScreen();
                transform.SetParent(playerRoot);
                direcoes.transform.SetParent(playerRoot);
                toWorld = true;
				rb.velocity = Vector3.zero;
                rb.useGravity = true;
                reachScreen = false;
                onScreen = false;
            }
        }
    }
    
    //funçao que tira ele da tela
    void exitScreen()
    {
        //se a variavel q fala pra ele sair da tela tiver ligada ele deve consertar o tamanho e a rotação do personagem
        if (toWorld)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(DollyCam.CalculaCamTarget(DollyCam.numPlayers).x + _2dX,
                    2f, DollyCam.CalculaCamTarget(DollyCam.numPlayers).z + _2dY), velTransicao);
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(1, 1, 1), velTransicao / 10);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.AngleAxis(0, Vector3.right), velTransicao * 5);
            direcoes.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        }
        //uma vez que o tamanho esta ok a variavel pode ficar falsa.
        if(transform.localScale == new Vector3(1, 1, 1) && transform.localRotation.x == 0f && transform.localPosition.z <= 2f)
        {
            toWorld = false;
        }
    }


    //Ver o metodo no CameraControl
	void TestPosition(){
		//Calcula a distancia entre o personagem e a camera.
		var DistanceZ = (transform.position - CameraMain.transform.position).z;

		//Calcula o ponto maximo de movimentação pra esquerda.
		var Leftborder = CameraMain.ViewportToWorldPoint (new Vector3 (0.01f, 0, DistanceZ)).x;

		//Calcula o ponto maximo de movimentação pra direita.
		var Rightborder = CameraMain.ViewportToWorldPoint (new Vector3 (0.99f, 0, DistanceZ)).x;

		//Calcula o ponto maximo de movimentação pra Baixo.
		var Bottomborder = CameraMain.ViewportToWorldPoint (new Vector3 (0, 0, 3)).z;

		//Calcula o ponto maximo de movimentação pra Cima.
		var Topborder = CameraMain.ViewportToWorldPoint (new Vector3 (0, 0, 48)).z;

		//Mantem o personagem sempre dentro do espaço da camera.
		transform.position = new Vector3 (
			Mathf.Clamp (transform.position.x, Leftborder, Rightborder),
			/*Mathf.Clamp (*/transform.position.y/*, Bottomborder, Topborder)*/,
			Mathf.Clamp (transform.position.z,Bottomborder,Topborder ));
	}

	void SetAnimations(){
		if (Anim == null) {
			return;
		}
		if (rb.velocity.y > 0)
			Anim.SetBool ("Jumping", true);
		else
			Anim.SetBool ("Jumping", false);
		Anim.SetBool ("InAir", !InGround);
		Anim.SetBool ("InMovement", InMovement);
		Anim.SetBool ("ReachScreen", reachScreen);
	}

	void SetJumpAnim(){
		if (Anim == null) {
			return;
		}
		Anim.SetTrigger ("Jump");
	}

	public void SetAttackAnim(int AttackNumber){
		if (Anim == null) {
			return;
		}
		Anim.SetTrigger ("Attack");
		Anim.SetInteger ("AttackNumber", AttackNumber);
	}

	void SetAnimOnScreen(){
		if (Anim == null) {
			return;
		}
		Anim.SetTrigger ("JumpScreen");
	}

	void SetAnimOffScreen(){
		if (Anim == null) {
			return;
		}
		Anim.SetTrigger ("JumpOffScreen");
	}
}

