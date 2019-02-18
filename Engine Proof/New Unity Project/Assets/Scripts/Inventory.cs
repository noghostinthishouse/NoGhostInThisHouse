using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] inventory;
    [SerializeField] private int noOfItem;

    private int count;

    void Start()
    {
        count = 0;
        noOfItem = 1;           //can change the number later
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
    }

    public void DisplayAllItem()
    {
        Debug.Log("No. of item : ");
        Debug.Log(noOfItem);
        foreach(GameObject item in inventory)
        {
            Debug.Log(item);
        }
    }
}
