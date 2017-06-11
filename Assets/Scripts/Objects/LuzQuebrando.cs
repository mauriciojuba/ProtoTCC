﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuzQuebrando : MonoBehaviour {

    public Light Luz;

    IEnumerator Pisca()
    {
        for (int i = 0; i < 3; i++)
        {
            Luz.enabled = false;

            yield return new WaitForSeconds(0.1f);

            Luz.enabled = true;

            yield return new WaitForSeconds(0.1f);

            Luz.enabled = false;
        }

        for (int i = 0; i < 6; i++)
        {
            Luz.enabled = false;

            yield return new WaitForSeconds(0.05f);

            Luz.enabled = true;

            yield return new WaitForSeconds(0.05f);

            Luz.enabled = false;
        }
    }
}
