using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour {

    [Range(0, 100)]
    public int LootChance;
    public GameObject[] Loot;

    public void Drop()
    {
        float random = Random.Range(0, 100);
        if (random <= LootChance)
        {
            if (Loot[0] != null)
            {
                int drop = Random.Range(0, Loot.Length);
                Debug.Log(drop);
                GameObject gb = GameObject.Instantiate(Loot[drop], transform.position, Quaternion.identity) as GameObject;
                gb.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gb.GetComponent<Rigidbody>().AddForce(gb.transform.up * 350);
            }
        }
    }
}
