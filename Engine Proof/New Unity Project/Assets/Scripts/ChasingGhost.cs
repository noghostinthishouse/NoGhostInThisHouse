using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingGhost : MonoBehaviour
{
//  [SerializeField] private bool triggered;
    public bool triggered;
    public bool stunt;
    public float speed;

    public GameObject tile;
    private Tile t;

    public GameObject nextTile;
    private Tile nextTile_t;
    
    private bool eat;
    private Vector3 distance;

    private int ghostIndex;
    private Player player;

    private SpriteRenderer sp;
    public Sprite[] sprites;                // has 2 elements: ghost and furniture sprites respectively

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        stunt = false;
        ghostIndex = PlayerTurn.AddGhost();
        triggered = false;
        t = tile.GetComponent<Tile>();
        nextTile_t = nextTile.GetComponent<Tile>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        eat = false;
    }

    void Update()
    {
        if (!t.flashlightOn)
        {
            //this statement is use to detect player in front of their tile
            if (!PlayerTurn.playerTurn && !triggered && PlayerTurn.ghostFinished[ghostIndex])
            {
                //check for trigger
                CheckPlayer();
                CalculateDis();
                //calculate which tile to move to
                PlayerTurn.SetGhostTurn(ghostIndex);
            }
            //this statement is use to move the ghost after it is triggered
            else if (!PlayerTurn.playerTurn && triggered && PlayerTurn.ghostFinished[ghostIndex])
            {
                //Debug.Log("Chasing");
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, distance, step);
                if (Vector3.Distance(transform.position, distance) < 0.001f)
                {
                    PlayerTurn.SetGhostTurn(ghostIndex);
                    ChangeTile();
                    CalculateDis();
                }
            }
        }
        else
        {
            Debug.Log("stunt");
            PlayerTurn.SetGhostTurn(ghostIndex);
        }
    }

    public void CheckPlayer()
    {
        if (nextTile_t.playerOn)
        {
            triggered = true;
            //Debug.Log("trigger");
        }
    }
    
    void CalculateDis()
    {
        //calculate which way to move to
        Vector3 tmp = nextTile.transform.position - tile.transform.position;
        distance = transform.position + tmp;
    }
    
    public void SetFurniture()
    {
        sp.sprite = sprites[1];
    }

    public void SetGhost()
    {
        sp.sprite = sprites[0];
    }

    public void ChangeTile()
    {
        t.SetEmpty();
        nextTile_t.SetNotEmpty();
        tile = nextTile;
        t = nextTile_t;
        nextTile = player.GetPlayerCurrentTile();
        nextTile_t = nextTile.GetComponent<Tile>();
    }
}
