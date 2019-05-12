using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSounds : MonoBehaviour
{
    public bool blanketGhost;
    public bool chasingGhost;
    public bool owlGhost;
    public bool followLightGhost;
    public bool copyActionGhost;
    public bool madGhost;

    private static List<int> ghostSoundIndexes = new List<int>();

    private bool playSound;

    // Start is called before the first frame update
    void Start()
    {
        playSound = false;
        if (blanketGhost)
        {
            ghostSoundIndexes.Add(19);
            ghostSoundIndexes.Add(20);
        }
        if (chasingGhost)
        {
            // add sound index
        }
        if (owlGhost)
        {
            ghostSoundIndexes.Add(17);
        }
        if (followLightGhost)
        {
            // add sound index
        }
        if (copyActionGhost)
        {
            // add sound index
        }
        if (madGhost)
        {
            // add sound index
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (!PlayerTurn.playerTurn && !playSound)
    //    {
    //        playSound = true;
            
    //    }else if (PlayerTurn.playerTurn)
    //    {
    //        playSound = false;
    //    }
    //}

    public void PlayGhostSound()
    {
        if (RandomPlaySound())
        {
            SoundManager.instance.PlaySFX(ghostSoundIndexes[Random.Range(0, ghostSoundIndexes.Count)]);
        }
    }

    bool RandomPlaySound()
    {
        float tmp = Random.Range(1, 100);
        Debug.Log(tmp);
        if (tmp >= 70)
        {
            return true;
        }
        return false;
    }
}
