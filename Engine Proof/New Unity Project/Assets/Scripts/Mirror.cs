using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Tile currentTile;
    public Tile tile1;
    public Tile tile2;
    public SpriteRenderer[] light1;
    public SpriteRenderer[] light2;

    private bool checkReflect;

    void Start()
    {
        checkReflect = false;
        for (int i = 0; i < light1.Length; i++)
        {
            light1[i].enabled = false;
            light2[i].enabled = false;
        }
    }

    void Update()
    {
        if (currentTile.flashlightOn)
        {
            if (tile1.playerOn || tile1.flashlightPlaced)
            {
                TurnOnLight(2);
            }
            else if (tile2.playerOn || tile2.flashlightPlaced)
            {
                TurnOnLight(1);
            }
        }
        else
        {
            TurnOffLight();
        }
    }

    public void ReflectLightOn()
    {
        tile2.flashlightOn = true;
        tile1.flashlightOn = true;
    }

    public void ReflectLightOff()
    {
        tile2.flashlightOn = false;
        tile1.flashlightOn = false;
        TurnOffLight();
    }

    void TurnOffLight()
    {
        for (int i = 0; i < light1.Length; i++)
        {
            light1[i].enabled = false;
            light2[i].enabled = false;
        }
    }

    void TurnOnLight(int t)
    {
        if (t == 1)
        {
            for (int i = 0; i < light1.Length; i++)
            {
                light1[i].enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < light2.Length; i++)
            {
                light2[i].enabled = true;
            }
        }
    }

}
