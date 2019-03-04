using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Player player;
    private Transform playerTrans;
    private Vector3 offset;
    private Tile pointedTile;
    private float playerDirectionX;
    private float playerDirectionY;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerTrans = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        FlashlightFollowPlayer();
    }

    public void FlashlightFollowPlayer()
    {
    //    Debug.Log("Flashlight follow player was called");

        playerDirectionX = player.directionX;
        playerDirectionY = player.directionY;

        if (playerDirectionX > 0 && playerDirectionY > 0)
        {
            offset.Set(1.5f, -0.5f, 0.0f);
            Debug.Log("Facing top right");
        } else if (playerDirectionX > 0 && playerDirectionY < 0)
        {
            offset.Set(1.5f, -2.5f, 0.0f);
            Debug.Log("Facing bottom right");
        }
        else if (playerDirectionX < 0 && playerDirectionY > 0)
        {
            offset.Set(-1.5f, -0.5f, 0.0f);
            Debug.Log("Facing top left");
        }
        else if (playerDirectionX < 0 && playerDirectionY < 0)
        {
            offset.Set(-1.5f, -2.5f, 0.0f);
            Debug.Log("Facing bottom left");
        }
        transform.position = playerTrans.position + offset;
    }
    
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Tile")
        {
        //    Debug.Log("flashlight touching tile");
            pointedTile = collider.GetComponent<Tile>();
            if (pointedTile.simpleGhost != null)
            {
                Debug.Log("Found simple ghost");
                pointedTile.simpleGhost.triggered = false;
            }
            if (pointedTile.chasingGhost != null)
            {
                Debug.Log("Found chasing ghost");
                pointedTile.chasingGhost.triggered = false;
            }
        }
    }
}
