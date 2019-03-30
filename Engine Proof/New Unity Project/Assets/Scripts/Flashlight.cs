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

    [SerializeField] private bool placeFlashlight;

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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerTrans = player.GetComponent<Transform>();
        offset = new Vector3(0.0f, -1.0f, 0.0f);
        if (!placeFlashlight)
        {
            transform.position = playerTrans.position + offset;
        }
    }
    
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
        if (!placeFlashlight)
        {
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
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Tile")
        {
            //----------------------------------------------------------
            // I've create a new bool 'flashlightOn' in Tile
            // when true --> ghost will not do anything
            //----------------------------------------------------------
            if (!collider.GetComponent<Tile>().flashlightOn)
            {
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
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Tile")
        {
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
