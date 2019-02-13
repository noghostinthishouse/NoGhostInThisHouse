using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject[] nearbyTiles;

    [SerializeField] private bool empty;
    [SerializeField] private GameObject ghost;
    [SerializeField] private GameObject item;
    private Transform my_transform;
    private Player player;

    int NoOfAdjacentTiles;

    void Start()
    {
        my_transform = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        NoOfAdjacentTiles = nearbyTiles.Length;
        empty = true;
        if(ghost != null)
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
        player.SelectTile(gameObject);
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
