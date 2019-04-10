using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTiles : MonoBehaviour
{
    public GameObject[] Tiles;
    
    public GameObject FindPlayerTile()
    {
        for(int i = 0; i < Tiles.Length; i++)
        {
            if (Tiles[i].GetComponent<Tile>().playerOn)
            {
                return Tiles[i];
            }
        }
        return null;
    }
}
