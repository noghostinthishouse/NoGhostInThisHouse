using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Player player;
    private PlayerMovement playerm;

    private Transform playerTrans;
    private Vector3 offset;

    private Tile pointedTile;
    private Tile prevPointedTile;

    [SerializeField] private bool placeFlashlight;

    public SpriteRenderer[] sp;
    public float angle;
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
        playerm = GameObject.FindGameObjectWithTag("PlayerCol").GetComponent<PlayerMovement>();
        playerTrans = player.GetComponent<Transform>();
        offset = new Vector3(0.0f, -1.0f, 0.0f);
        if (!placeFlashlight)
        {
            transform.position = playerTrans.position + offset;
        }
    }
    
    void Update()
    {
        if (!placeFlashlight)
        {
            if (player.enableRotate)
            {
                Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
            }
            else if (player.IsMove())
            {
                angle = playerm.angle;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
            }
            else
            {
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
            }
        }
    }

    public void SetAngle(float a)
    {
        angle = a;
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
                pointedTile = collider.GetComponent<Tile>();
                if (turnOn)
                {
                    pointedTile.flashlightOn = true;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "Tile")
        {
            if (pointedTile)
            {
                pointedTile.flashlightOn = false;
                pointedTile = null;
            }
        }
    }

    public Tile GetPointedTile()
    {
        return pointedTile;
    }

    public void TurnOff()
    {
        if (!placeFlashlight)
        {
            turnOn = false;
            if (pointedTile)
            {
                pointedTile.flashlightOn = false;
            }
            for (int i = 0; i < sp.Length; i++)
            {
                sp[i].enabled = false;
            }
        }
    }

    public void TurnOn()
    {
        if (!placeFlashlight)
        {
            turnOn = true;
            if (pointedTile)
            {
                pointedTile.flashlightOn = true;
            }
            for (int i = 0; i < sp.Length; i++)
            {
                sp[i].enabled = true;
            }
        }
    }

    public bool IsOn()
    {
        return turnOn;
    }
}
