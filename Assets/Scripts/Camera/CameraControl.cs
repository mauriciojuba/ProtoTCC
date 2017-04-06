using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField] Transform[] players;
    Transform DollyCam;
    int numPlayers = 0;
    [SerializeField] float distancia = 8;
    [SerializeField] float maxDistancia;
    [SerializeField] float minDistancia;
    [SerializeField] float velocidadeZoom;

    bool pForaDaTela;
    bool zoomIn;

    private void Start()
    {
        DollyCam = transform;
        players = new Transform[4];
        ChecarQuantidadePlayers();
    }

    private void Update()
    {
        DollyCam.position = posicionaCamera(CalculaCamTarget(numPlayers));
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
    Vector3 CalculaCamTarget(int numPlayers)
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
        return new Vector3(_target.x, _target.y + distancia, _target.z - distancia-2);
    }

    void ControlaBordaTela()
    {
        
        for (int i = 0; i < players.Length; i++)
        {
            Vector3 posOnScreen = Camera.main.WorldToViewportPoint(players[i].position);
            if (posOnScreen.x <= 0 || posOnScreen.x >= 1 || posOnScreen.y <= 0 || posOnScreen.y >= 1)
            {
                if (distancia <= maxDistancia)
                {
                    distancia += velocidadeZoom * Time.deltaTime;
                    return;
                }
            }
        }

    }


















}
