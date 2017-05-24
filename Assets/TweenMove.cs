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

    // Update is called once per frame
    void Update()
    {

        // transform.LookAt(Target.transform.position);

        if (move)
        {
            iTween.MoveTo(gameObject,iTween.Hash("path", iTweenPath.GetPath(Path), "time", time, "orienttopath", true));

            move = false;
        }

        if (volta)
        {
            iTween.MoveFrom(gameObject, iTween.Hash("path", iTweenPath.GetPath(Path), "time", time));

            volta = false;
        }
    }

}
