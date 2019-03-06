using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Player player;
    private Vector3 upperRightTransform = new Vector3(0, 0, 0);
    private Vector3 lowerRightTransform = new Vector3(0, 0, 270);
    private Vector3 upperLeftTransform = new Vector3(0, 0, 90);
    private Vector3 lowerLeftTransform = new Vector3(0, 0, 180);
    private Transform playerTrans;
    private Vector3 offset;
    private Tile pointedTile;
    private Tile prevPointedTile;
    private float playerDirectionX = 1.0f;
    private float playerDirectionY = 1.0f;
    private bool placeFlashlight;

    public float topRightOffsetX;
    public float topRightOffsetY;
    public float bottomRightOffsetX;
    public float bottomRightOffsetY;
    public float topLeftOffsetX;
    public float topLeftOffsetY;
    public float bottomLeftOffsetX;
    public float bottomLeftOffsetY;

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
                offset.Set(topRightOffsetX, topRightOffsetY, 1.0f);
                transform.eulerAngles = upperRightTransform;
                //Debug.Log("Facing top right");
            }
            else if (playerDirectionX > 0 && playerDirectionY < 0)
            {
                offset.Set(bottomRightOffsetX, bottomRightOffsetY, -1.0f);
                transform.eulerAngles = lowerRightTransform;
                //Debug.Log("Facing bottom right");
            }
            else if (playerDirectionX < 0 && playerDirectionY > 0)
            {
                offset.Set(topLeftOffsetX, topLeftOffsetY, 1.0f);
                transform.eulerAngles = upperLeftTransform;
                //Debug.Log("Facing top left");
            }
            else if (playerDirectionX < 0 && playerDirectionY < 0)
            {
                offset.Set(bottomLeftOffsetX, bottomLeftOffsetY, -1.0f);
                transform.eulerAngles = lowerLeftTransform;
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
