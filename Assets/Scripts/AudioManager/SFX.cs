using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour {

    public void PlaySoundSFX(string Name)
    {
        SoundManager.PlaySFX(Name);
    }
}
