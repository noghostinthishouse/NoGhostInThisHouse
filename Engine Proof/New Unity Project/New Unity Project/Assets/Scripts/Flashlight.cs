using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Player player;
    private Transform playerTrans;
    private Vector3 offset;
    private Tile pointedTile;
    private Tile prevPointedTile;
    private float playerDirectionX;
    private float playerDirectionY;
    private bool placeFlashlight;

    // Start is called before the first frame update
    void Start()
    {
        pointedTile = null;
        prevPointedTile = null;
        placeFlashlight = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerTrans = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    /*void Update()
    {

    }*/

    public void FlashlightFollowPlayer()
    {
        //    Debug.Log("Flashlight follow player was called");
        //follow player when not placed
        if (!placeFlashlight)
        {
            playerDirectionX = player.directionX;
            playerDirectionY = player.directionY;

            if (playerDirectionX > 0 && playerDirectionY > 0)
            {
                offset.Set(1.5f, -0.5f, 1.0f);
                //Debug.Log("Facing top right");
            }
            else if (playerDirectionX > 0 && playerDirectionY < 0)
            {
                offset.Set(1.5f, -2.5f, -1.0f);
                //Debug.Log("Facing bottom right");
            }
            else if (playerDirectionX < 0 && playerDirectionY > 0)
            {
                offset.Set(-1.5f, -0.5f, 1.0f);
                //Debug.Log("Facing top left");
            }
            else if (playerDirectionX < 0 && playerDirectionY < 0)
            {
                offset.Set(-1.5f, -2.5f, -1.0f);
                //Debug.Log("Facing bottom left");
            }
            transform.position = playerTrans.position + offset;
        }
    }
    
    public void Place()
    {
        placeFlashlight = true;
    }

    public void PickUp()
    {
        placeFlashlight = false;
    }

    public bool IsPlaced()
    {
        return placeFlashlight;
    }

    //change to OnTriggerEnter2D, so it will only call once
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Tile")
        {
            //Debug.Log("flashlight touching tile");

            //----------------------------------------------------------
            // I've create a new bool 'flashlightOn' in Tile
            // when true --> ghost will not do anything
            //----------------------------------------------------------

            if (pointedTile)
            {
                //I had trouble with OnTriggerExit2D() so I did this
                prevPointedTile = pointedTile;
                prevPointedTile.flashlightOn = false;
            }
            pointedTile = collider.GetComponent<Tile>();
            pointedTile.flashlightOn = true;
            
            /*if (pointedTile.simpleGhost != null)
            {
                Debug.Log("Found simple ghost");
                pointedTile.simpleGhost.stunt = true;
            }
            if (pointedTile.chasingGhost != null)
            {
                Debug.Log("Found chasing ghost");
                pointedTile.chasingGhost.stunt = true;
            }*/
        }
    }
}
