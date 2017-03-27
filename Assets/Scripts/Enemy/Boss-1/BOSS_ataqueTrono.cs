using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_ataqueTrono : MonoBehaviour
{
    CapsuleCollider col;
    void Start()
    {
        col = GetComponent<CapsuleCollider>();
        col.enabled = false;
    }
    public void EnableCollider()
    {
        col.enabled = true;
        Invoke("DisableCollider", 1f);
    }
    void DisableCollider()
    {
        col.enabled = false;
    }
}