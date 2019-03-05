using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject[] nearbyTiles;
    public GameObject item;
    public SimpleGhost simpleGhost;
    public ChasingGhost chasingGhost;

    [SerializeField] private bool empty;
//  [SerializeField] private SimpleGhost simpleGhost;
//  [SerializeField] private ChasingGhost chasingGhost;
    
    //private Transform my_transform;
    private Player player;

    public bool playerOn;
    public bool flashlightPlaced;           //player place flashlight on the tile
    public bool flashlightOn;               //flashlight is shining on the tile

    int NoOfAdjacentTiles;

    void Start()
    {
        //my_transform = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        NoOfAdjacentTiles = nearbyTiles.Length;
        empty = true;
        playerOn = false;
        if (simpleGhost || chasingGhost)
        {
            empty = false;
        }
    }

    public GameObject GetAdjacentTile(int tileNo)
    {
        if(tileNo < NoOfAdjacentTiles)
        {
            return nearbyTiles[tileNo];
        }
        else
        {
            return null;
        }
    }

    public Tile GetAdjacentTileT(int tileNo)
    {
        if (tileNo < NoOfAdjacentTiles)
        {
            return nearbyTiles[tileNo].GetComponent<Tile>();
        }
        else
        {
            return null;
        }
    }

    public void SetEmpty()
    {
        empty = true;
    }

    public void SetNotEmpty()
    {
        empty = false;
    }

    public bool IsEmpty()
    {
        return empty;
    }

    public void FlashlightOn()
    {
        Debug.Log("on");
        flashlightOn = true;
        if (simpleGhost)
        {
            Debug.Log("sg back to furniture");
            simpleGhost.SetFurniture();
        }
        else if (chasingGhost)
        {
            Debug.Log("cg back to furniture");
            chasingGhost.SetFurniture();
        }
    }

    public void FlashlightOff()
    {
        Debug.Log("off");
        flashlightOn = false;
        if (simpleGhost)
        {
            Debug.Log("sg back to ghost");
            simpleGhost.SetGhost();
        }
        else if (chasingGhost)
        {
            Debug.Log("cg back to ghost");
            chasingGhost.SetGhost();
        }
    }

    void OnMouseDown()
    {
        if (!PlayerTurn.GameOver && PlayerTurn.playerTurn)
        {
            //check if player can move to this tile
            player.SelectTile(gameObject);
        }
    }
}
