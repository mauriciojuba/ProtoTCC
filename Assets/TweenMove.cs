using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMove : MonoBehaviour
{

    public bool move;
    public string Path;
    public float time;

    public GameObject target;
    public GameObject MenuTarget;

    public bool volta;

    public void GoToMenu(string Tela)
    {
        Path = Tela;
        move = true;
    }

    public void olhala()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y" , 13 , "time" , 5));
        iTween.LookTo(gameObject, iTween.Hash("looktarget", target.transform, "time", 3 , "easetype", "easeInOutQuad"));
    }

    public void olhaMenu()
    {

        iTween.LookTo(gameObject, iTween.Hash("looktarget", MenuTarget.transform, "time", 2 , "easetype", "easeInOutQuad"));
    }

    public void Mover() {

        // Se a variavel Move se  torna verdadeira ele excuda o iTween expecificado pela string Path.
        
        if (move)
        {
            if (Path == "Options")
            {
                time = 9;

                iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(Path), "time", time, "orienttopath", true, "oncomplete", "olhala", "easetype", "easeInOutQuad"));

                time = 13;

                Path = "OptionsMenu";

                move = false;
            }

            else if (Path == "Game")
            {
                time = 5;

                iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(Path), "time", time, "orienttopath", true, "easetype", "easeInOutQuad"));

                move = false;
            }

            else if (Path == "Creditos")
            {

                time = 5;
                iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(Path), "time", time, "orienttopath", true, "easetype", "easeInOutQuad"));

                Path = "CreditosMenu";

                move = false;
            }


            else if (Path == "Exit")
            {

                time = 5;

                iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(Path), "time", time, "orienttopath", true, "easetype", "easeInOutQuad"));

                move = false;
            }

        }
    }

    public void Voltar() {

        // Esta funcao esta em teste 
        //Faz a Camera voltar para o local inicial  do menu principal

        if (volta)
        {
            iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(Path), "time", time / 2, "oncomplete", "olhaMenu", "orienttopath", true, "easetype", "easeInOutQuad"));

            volta = false;
        }
    }

    void FixedUpdate()
    {
        Mover();

        Voltar();
    }

}
