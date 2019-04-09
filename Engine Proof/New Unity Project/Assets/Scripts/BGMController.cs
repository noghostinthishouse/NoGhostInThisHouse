using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public int BGMIndex;

    void Start()
    {
        SoundManager.instance.PlayBGM(BGMIndex);
    }
}
