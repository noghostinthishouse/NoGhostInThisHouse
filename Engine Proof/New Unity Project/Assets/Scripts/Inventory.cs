using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] inventory;
    public GameObject endTile;

    public bool allItem;

    [SerializeField] private int noOfItem;
    private int count;

    void Start()
    {
        allItem = false;
        count = 0;                             //can change the number later
        inventory = new GameObject[noOfItem];

        //init array with null
        for (int i = 0; i < noOfItem; i++)
        {
            inventory[i] = null;
        }
    }

    public void TakeItem(GameObject item)
    {
        Debug.Log("Item taken");
        inventory[count] = item;
        if (count < noOfItem - 1)
        {
            count++;
        }
        if(count == noOfItem-1)
        {
            allItem = true;
        }
        else
        {
            allItem = false;
        }
    }
}
