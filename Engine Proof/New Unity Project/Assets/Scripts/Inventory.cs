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
    private GameObject doorHighlight;

    void Start()
    {
        doorHighlight = GameObject.FindGameObjectWithTag("Door Highlight");

        allItem = false;

        if (noOfItem > 0)
        {
            doorHighlight.SetActive(false);
        }

        count = 0;                             //can change the number later
        inventory = new GameObject[noOfItem];

        //init array with null

        for (int i = 0; i < noOfItem; i++)
        {
            inventory[i] = null;
        }
        if(noOfItem == 0)
        {
            allItem = true;
        }
    }

    public void TakeItem(GameObject item)
    {
        //Debug.Log(inventory.Length);
        inventory[count] = item;
        SoundManager.instance.PlaySFX(13);
        if (count < noOfItem - 1)
        {
            count++;
        }
        if(count == noOfItem-1)
        {
            allItem = true;
            doorHighlight.SetActive(true);
        }
        else
        {
            allItem = false;
        }
    }
}
