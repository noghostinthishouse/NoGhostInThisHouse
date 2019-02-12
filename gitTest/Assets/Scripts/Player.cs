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

        speed = 5.0f;
	}
	
	void Update () {
        //only move when play select a valid tile
        if (move)
        {
            /*float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, distance, step);
            if (transform.position.magnitude >= distance.magnitude)
            {
                move = false;
            }*/
            transform.position = distance;
        }
	}

    public void SelectTile(GameObject selectedTile)
    {
        for(int i = 0; i < 4; i++)
        {
            if(tile.GetAdjacentTile(i) == selectedTile)
            {
                //assign nextTile and get Tile component
                nextTile = selectedTile;
                tile_nextTile = nextTile.GetComponent<Tile>();

                //adjacent tile and is empty
                if (tile_nextTile.IsEmpty())
                {
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
            Debug.Log("Not adjacent tile");
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
