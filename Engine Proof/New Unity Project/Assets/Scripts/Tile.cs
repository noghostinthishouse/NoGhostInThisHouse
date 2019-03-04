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

    void OnMouseDown()
    {
        if (!PlayerTurn.GameOver && PlayerTurn.playerTurn)
        {
            if (Input.GetMouseButtonDown(1))
            {
                //place/pick up flashlight
            }
            else
            {
                //check if player can move to this tile
                player.SelectTile(gameObject);
            }
        }
    }

    /*
    public void DebugGetAllTile()
    {
        foreach (GameObject tile in nearbyTiles) {
            Debug.Log(tile);
        }
    }
    */
}
