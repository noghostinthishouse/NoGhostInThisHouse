using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Tile currentTile;
    public Tile lightFromTile;
    public Tile lightToTile;
    public SpriteRenderer[] light;

    void Start()
    {
        for (int i = 0; i < light.Length; i++)
        {
            light[i].enabled = false;
        }
    }
    
    void Update()
    {
        if(currentTile.flashlightOn && lightFromTile.playerOn)
        {
            lightToTile.flashlightOn = true;
        }
        else
        {
            lightToTile.flashlightOn = false;
        }
        ReflectLight(lightToTile.flashlightOn);
    }

    void ReflectLight(bool reflect)
    {
        for (int i = 0; i < light.Length; i++)
        {
            light[i].enabled = reflect;
        }
    }
}
