using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {

	[Header("Define a cor da linha")]
	public Color linkColor = Color.green;
	[Space(10)]
	[Header("Define a cor dos pontos")]
    public Color waypointColor = Color.blue;
	[Header("Tamanho dos pontos")]
    public float radius = 0.1F;
	[Header("Proximo Waypoint")]
    public Transform[] Links;
	[Space(10)]
	[Tooltip ("Mesh para indicar a direção que o npc vai ir")]
    public Mesh Seta;

    public void OnDrawGizmos()
    {


        Gizmos.color = waypointColor;
        Gizmos.DrawSphere(transform.position, radius);
        for (int i = 0; i < Links.Length; i++)
        {
            if (Links[i] != null)
            {
                Gizmos.color = linkColor;
                Gizmos.DrawLine(transform.position, Links[i].position);

                

                Gizmos.DrawMesh(Seta, 0.5f * (transform.position + Links[i].position), Quaternion.LookRotation(Links[i].position - transform.position));
            }
        }
        
    }

}
