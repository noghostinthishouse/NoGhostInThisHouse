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

    // Start is called before the first frame update
    void Start()
    {
        playerCol = GameObject.FindGameObjectWithTag("PlayerCol").GetComponent<PlayerMovement>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.IsMove())
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerCol.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
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
