using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Player player;

    //private Vector3 upperRightTransform = new Vector3(0, 0, 0);
    //private Vector3 lowerRightTransform = new Vector3(0, 0, 270);
    //private Vector3 upperLeftTransform = new Vector3(0, 0, 90);
    //private Vector3 lowerLeftTransform = new Vector3(0, 0, 180);

    private Transform playerTrans;
    private Vector3 offset;

    private Tile pointedTile;
    private Tile prevPointedTile;

    //private float playerDirectionX = 1.0f;
    //private float playerDirectionY = 1.0f;

    private bool placeFlashlight;

    //flashlight directions
    //public float topRightOffsetX;
    //public float topRightOffsetY;
    //public float bottomRightOffsetX;
    //public float bottomRightOffsetY;
    //public float topLeftOffsetX;
    //public float topLeftOffsetY;
    //public float bottomLeftOffsetX;
    //public float bottomLeftOffsetY;

    public SpriteRenderer[] sp;
    private int spNum;
    private bool turnOn;

    public float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        turnOn = true;
        pointedTile = null;
        prevPointedTile = null;
        placeFlashlight = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerTrans = player.GetComponent<Transform>();
        offset = playerTrans.position - transform.position;
        transform.position = playerTrans.position - offset;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (!player.IsMove() && !PlayerTurn.GameOver && PlayerTurn.playerTurn)
        {
            if (!placeFlashlight)
            {
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
            }
        }
    }

    public void FlashlightFollowPlayer()
    {
        //Debug.Log("Flashlight follow player was called");
        if (!placeFlashlight)
        {
            transform.position = playerTrans.position - offset;
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
            if (turnOn)
            {
                pointedTile.flashlightOn = true;
            }
        }
    }

    public Tile GetPointedTile()
    {
        return pointedTile;
    }

    public void TurnOff()
    {
        turnOn = false;
        pointedTile.flashlightOn = false;
        for (int i = 0; i < sp.Length; i++)
        {
            sp[i].enabled = false;
        }
    }

    public void TurnOn()
    {
        turnOn = true;
        pointedTile.flashlightOn = true;
        for (int i = 0; i < sp.Length; i++)
        {
            sp[i].enabled = true;
        }
    }
}
