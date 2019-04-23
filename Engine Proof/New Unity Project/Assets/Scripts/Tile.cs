using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject[] nearbyTiles;
    public GameObject item;
    public GameObject GhostInitTile;

    [SerializeField] private bool empty;

    private Player player;

    public bool playerOn = false;
    public bool flashlightPlaced;           //player place flashlight on the tile
    public bool flashlightOn;               //flashlight is shining on the tile

    public HighlightTile ht;

    [SerializeField] private GameObject flashlight = null;

    int NoOfAdjacentTiles;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        NoOfAdjacentTiles = nearbyTiles.Length;
        empty = true;
        if (GhostInitTile)
        {
            empty = false;
        }
        if (flashlight)
        {
            flashlightPlaced = true;
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

    public void GetItem()
    {
        item.SetActive(false);
        item = null;
    }

    public void SetNotEmpty()
    {
        empty = false;
    }

    public bool IsEmpty()
    {
        return empty;
    }

    public void MoveToThisTile()
    {
        if (!PlayerTurn.GameOver && PlayerTurn.playerTurn)
        {
            player.SelectTile(gameObject);
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void SetHighlight(bool b)
    {
        ht.SetHighlight(b);
        ht.SetPosition(transform.position);
        if (b)
        {
            SoundManager.instance.PlaySFX(10);
        }
    }

    public void PlaceFlashlight(GameObject f)
    {
        flashlight = f;
        flashlightPlaced = true;
    }

    public GameObject PickUpFlashlight()
    {
        GameObject tmp = flashlight;
        flashlight = null;
        flashlightPlaced = false;

        return tmp;
    }

    /*void OnMouseDown()
    {
        //Debug.Log("click");
        if (!PlayerTurn.GameOver && PlayerTurn.playerTurn)
        {
            //check if player can move to this tile
            player.SelectTile(gameObject);
        }
    }*/
}
