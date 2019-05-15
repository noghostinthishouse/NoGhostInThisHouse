using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyActionGhostMovement : MonoBehaviour
{
    public float speed = 10.0f;

    private PlayerMovement playerCol;
    private Player player;
    private Tile pointedTile;
    private Tile prevTile;

    float angle;

    // Start is called before the first frame update
    void Start()
    {
        playerCol = GameObject.FindGameObjectWithTag("PlayerCol").GetComponent<PlayerMovement>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        angle = playerCol.angle;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.IsMove())
        {
            angle = playerCol.angle;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Tile")
        {
            pointedTile = collider.GetComponent<Tile>();
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Tile")
        {
            pointedTile = collider.GetComponent<Tile>();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "Tile")
        {
            pointedTile = null;
        }
    }

    public GameObject GetTileToMove()
    {
        if (pointedTile)
        {
            return pointedTile.GetGameObject();
        }
        return null;
    }
}
