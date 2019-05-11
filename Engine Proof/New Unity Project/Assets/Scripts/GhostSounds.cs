using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSounds : MonoBehaviour
{
    public int[] allGhostSoundsInLevel;

    private bool playSound;

    // Start is called before the first frame update
    void Start()
    {
        playSound = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerTurn.playerTurn && !playSound)
        {
            playSound = true;
            SoundManager.instance.PlaySFX(allGhostSoundsInLevel[Random.Range(0, allGhostSoundsInLevel.Length)]);
        }else if (PlayerTurn.playerTurn)
        {
            playSound = false;
        }
    }
}
