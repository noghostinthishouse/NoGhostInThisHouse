using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject currentTile;

    private Transform current_t;
    private GameObject nextTile;
    private Transform next_t;
    private Tile tile;
    private Tile tile_nextTile;
    private bool move;
    private Vector3 distance;

	void Start () {
        current_t = currentTile.GetComponent<Transform>();
        tile = currentTile.GetComponent<Tile>();
        nextTile = null;
        move = false;
        //tile.DebugGetAllTile();
	}
	
	void Update () {
        //only move when play select a valid tile
        if (move)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, distance, step);
			if(Vector3.Distance(transform.position,distance) < 0.001f)
			{
				move = false;
            }
            //transform.position = distance;
        }
	}

    public void SelectTile(GameObject selectedTile)
    {
        bool found = false;
        if (!move)
        {
            for (int i = 0; i < 4; i++)
            {
                found = false;
                if (tile.GetAdjacentTile(i) == selectedTile)
                {
                    //assign nextTile and get Tile component
                    nextTile = selectedTile;
                    tile_nextTile = selectedTile.GetComponent<Tile>();
                    found = true;

                    //adjacent tile and is empty
                    if (tile_nextTile.IsEmpty())
                    {
                        found = true;
                        //enable move
                        move = true;

                        //get Transform
                        next_t = nextTile.GetComponent<Transform>();
                        CalculateDis();
                        NextTurn();
                    }
                }
            }
            //if move is not enable, tell the player that they can't move to the tile
            if (!move)
            {
                if (found)
                {
                    if (!tile_nextTile.IsEmpty())
                    {
                        Debug.Log("Tile is not empty");
                    }
                }
                else
                {
                    Debug.Log("Not adjacent tile");
                }

            }
        }
        
    }

    void CalculateDis()
    {
        //calculate which way to move to
        Vector3 tmp = next_t.position - current_t.position;
        distance = transform.position + tmp;
    }

    void NextTurn()
    {
        //change currentTile to nextTile
        //change nextTile to null
        currentTile = nextTile;
        current_t = nextTile.GetComponent<Transform>();
        tile = nextTile.GetComponent<Tile>();

        nextTile = null;
        next_t = null;
        tile_nextTile = null;
    }
}
