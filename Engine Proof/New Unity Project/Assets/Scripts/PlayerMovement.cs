﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Tile pointedTile;
    private float speed = 10.0f;
    private Player player;
    private int phase;

    public float angle;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (PlayerTurn.playerTurn && !player.IsMove() && !PlayerTurn.GameOver)
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
            //change character sprite to match rotation
            if (player.enableRotate)
            {
                if (direction.x > 0 && direction.y > 0)
                {
                    phase = 0;
                }
                else if (direction.x > 0 && direction.y < 0)
                {
                    phase = 1;
                }
                else if (direction.x < 0 && direction.y > 0)
                {
                    phase = 2;
                }
                else if (direction.x < 0 && direction.y < 0)
                {
                    phase = 3;
                }
                player.SetDirection(phase);
            }
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
        for(int i = 0; i < 4; i++)
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
