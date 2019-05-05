using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Tile pointedTile;
    private float speed = 10.0f;
    private Player player;
    private int phase;
    private Vector3 direction;

    public float angle;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // 30 = top right
        // -40 = bottom right
        // 160 = top left
        // -130 = bottom left
    }

    void Update()
    {
        if (PlayerTurn.playerTurn && !PlayerTurn.Pause)
        {
            CalculateDirection();
            //angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Tile")
        {
            pointedTile = collider.GetComponent<Tile>();
            if (CheckMovable())
            {
                pointedTile.SetHighlight(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Tile")
        {
            if (pointedTile)
            {
                pointedTile.SetHighlight(false);
                pointedTile = null;
            }
        }
    }

    bool CheckMovable()
    {
        for(int i = 0; i < pointedTile.nearbyTiles.Length; i++)
        {
            if (pointedTile.GetAdjacentTileT(i))
            {
                if (pointedTile.GetAdjacentTileT(i).playerOn && pointedTile.IsEmpty())
                {
                    return true;
                }
            }
        }
        return false;
    }

    void CalculateDirection()
    {
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        //change character sprite to match rotation
        if (direction.x > 0 && direction.y > 0)
        {
            angle = 30;
            phase = 0;
        }
        else if (direction.x > 0 && direction.y < 0)
        {
            angle = -40;
            phase = 1;
        }
        else if (direction.x < 0 && direction.y > 0)
        {
            angle = 160;
            phase = 2;
        }
        else if (direction.x < 0 && direction.y < 0)
        {
            angle = -130;
            phase = 3;
        }
        if (player.enableRotate)
        {
            player.SetDirection(phase);
        }
    }

    public void SelectThisTile()
    {
        if (pointedTile)
        {
            pointedTile.MoveToThisTile();
        }
    }

    public int GetPhase()
    {
        return phase;
    }
}
