using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMove : MonoBehaviour
{

    public bool move;
    public string Path;
    public float time;

    public GameObject target;
    
    // public GameObject Target;

    public bool volta;

    // Use this for initialization
    void Start()
    {



    }

    public void olhala()
    {
        iTween.LookTo(gameObject, iTween.Hash("looktarget", target.transform, "time", 3 , "easetype", "easeInOutQuad"));
    }

    // Update is called once per frame
    void Update()
    {

        // transform.LookAt(Target.transform.position);

        if (move)
        {
            iTween.MoveTo(gameObject,iTween.Hash("path", iTweenPath.GetPath(Path), "time", time, "orienttopath", true , "oncomplete", "olhala" , "easetype" , "easeInOutQuad"));

            

            move = false;
        }

        if (volta)
        {
            iTween.MoveFrom(gameObject, iTween.Hash("path", iTweenPath.GetPath(Path), "time", time));

            volta = false;
        }
    }

}
