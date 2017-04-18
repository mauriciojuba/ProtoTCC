using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField] Transform[] players;
    Transform DollyCam;
    public int numPlayers = 0;
    [SerializeField] float velocidadeMovimento;
    [SerializeField] float distancia = 8;
    [SerializeField] float maxDistancia;
    [SerializeField] float minDistancia;
    [SerializeField] float velocidadeZoom;


    bool alguemFora;
    Vector3 vel = Vector3.zero;

    private void Start()
    {
        DollyCam = transform;
        players = new Transform[4];
        ChecarQuantidadePlayers();
        DollyCam.position = posicionaCamera(CalculaCamTarget(numPlayers));
    }

    private void FixedUpdate()
    {
        DollyCam.position = Vector3.SmoothDamp(DollyCam.position, posicionaCamera(CalculaCamTarget(numPlayers)), ref vel, (velocidadeMovimento / 10) * Time.deltaTime);
        ControlaBordaTela();
    }


    void ChecarQuantidadePlayers()
    {
        //alterar a tag quando não for mais necessário o "3D" no final
        for (int i = 0; i < players.Length; i++)
        {
            if (GameObject.FindWithTag("Player"+(i+1)+"_3D") != null)
            {
                players[i] = GameObject.FindWithTag("Player"+(i+1)+"_3D").transform;
                numPlayers++;
            }
        }
    }

    //marca o ponto entre os jogadores que a camera ficará olhando
    public Vector3 CalculaCamTarget(int numPlayers)
    {
        switch (numPlayers)
        {
            case 1:
                return players[0].position;
            case 2:
                return Vector3.Lerp(players[0].position, players[1].position, 0.5f);
            case 3:
                return Vector3.Lerp(Vector3.Lerp(players[0].position, players[1].position, 0.5f), players[2].position, 0.33f);
            case 4:
                return Vector3.Lerp(Vector3.Lerp(Vector3.Lerp(players[0].position, players[1].position, 0.5f), players[2].position, 0.33f), players[3].position, 0.25f);
        }
        return Vector3.one;
    }

    Vector3 posicionaCamera(Vector3 _target)
    {
        return new Vector3(_target.x, _target.y + distancia, _target.z - distancia - 2);
    }

    
    void ControlaBordaTela()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if(players[i].GetComponent<Movimentacao3D>().onScreen == false) { 
                Vector3 testOffScreen = Camera.main.WorldToViewportPoint(players[i].position);
                if ((testOffScreen.x <= 0.1 || testOffScreen.x >= 0.9 || testOffScreen.y <= 0.1 || testOffScreen.y >= 0.9))
                {
                    alguemFora = true;
                    if (distancia >= maxDistancia)
                    {
                        lockPlayerMovement(players[i].transform);
                    }
                }
            }
        }
        for (int j = 0; j < players.Length; j++)
        {
            Vector3 testOnScreen = Camera.main.WorldToViewportPoint(players[j].position);
            if ((testOnScreen.x > 0.1 && testOnScreen.x < 0.9 && testOnScreen.y > 0.1 && testOnScreen.y < 0.9) && !alguemFora )
            {
                if (players[j].GetComponent<Movimentacao3D>() != null && players[j].GetComponent<Movimentacao3D>().InMovement)
                {
                    distancia = Mathf.MoveTowards(distancia, minDistancia, velocidadeZoom * Time.deltaTime);
                    return;
                }
            }
            else
            {
                distancia = Mathf.MoveTowards(distancia, maxDistancia, velocidadeZoom * Time.deltaTime);
                alguemFora = false;
                return;
            }
        }

    }
    
    void lockPlayerMovement(Transform t)
    {
        //trava o movimento do jogador
    }
}

