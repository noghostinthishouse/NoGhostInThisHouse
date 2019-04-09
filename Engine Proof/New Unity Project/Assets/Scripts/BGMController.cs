using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public int BGMIndex;

    void Start()
    {
        SoundManager.instance.PlaySFX(11);
        SoundManager.instance.PlayBGM(1);
    }
}
