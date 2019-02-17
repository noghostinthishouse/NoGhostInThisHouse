﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject[] nearbyTiles;

    [SerializeField] private bool empty;
    [SerializeField] private SimpleGhost simpleGhost;
    [SerializeField] private ChasingGhost chasingGhost;
    [SerializeField] private GameObject item;
    //private Transform my_transform;
    private Player player;
    private ChasingGhost track;

    public bool playerOn;

    int NoOfAdjacentTiles;

    void Start()
    {
        //my_transform = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        NoOfAdjacentTiles = nearbyTiles.Length;
        track = GameObject.FindGameObjectWithTag("ChasingGhost").GetComponent<ChasingGhost>();
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

    public void PlaceItem(GameObject newItem)
    {
        item = newItem;
    }

    public void TakeItem()
    {
        item = null;
    }

    void OnMouseDown()
    {
        if (!PlayerTurn.GameOver && PlayerTurn.playerTurn)
        {
            player.SelectTile(gameObject);
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
