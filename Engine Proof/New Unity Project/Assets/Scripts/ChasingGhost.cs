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

    private Animator anim;
    private SpriteRenderer sp;
    private int phase;              // 1 = top left, 2 = bottom left, 3 = bottom right, 4 = top right
    [SerializeField] private bool isFacingRight;

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
            anim.SetBool("Stunt", false);
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
                SetAnimation();
                //Debug.Log("Chasing");
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, distance, step);
                if (Vector3.Distance(transform.position, distance) < 0.001f)
                {
                    anim.SetBool("Behind", false);
                    anim.SetBool("Front", false);
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
            anim.SetBool("Stunt", true);
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

        //calculate which animation to use;
        if (nextTile.transform.position.x > tile.transform.position.x 
            && nextTile.transform.position.y > tile.transform.position.y)
        {
            phase = 4;
        } else if (nextTile.transform.position.x > tile.transform.position.x
            && nextTile.transform.position.y < tile.transform.position.y)
        {
            phase = 3;
        }
        else if (nextTile.transform.position.x < tile.transform.position.x
            && nextTile.transform.position.y < tile.transform.position.y)
        {
            phase = 2;
        }
        else if (nextTile.transform.position.x < tile.transform.position.x 
            && nextTile.transform.position.y > tile.transform.position.y)
        {
            phase = 1;
        }
        
    }

    // 1 = top left, 2 = bottom left, 3 = bottom right, 4 = top right
    void SetAnimation()
    {
        switch (phase)
        {
            case 1:
                if (isFacingRight)
                {
                    sp.flipX = false;
                    isFacingRight = !isFacingRight;
                }
                anim.SetBool("Behind", true);
                break;
            case 2:
                if (isFacingRight)
                {
                    sp.flipX = false;
                    isFacingRight = !isFacingRight;
                }
                anim.SetBool("Front", true);
                break;
            case 3:
                if (!isFacingRight)
                {
                    sp.flipX = true;
                    isFacingRight = !isFacingRight;
                }
                anim.SetBool("Front", true);
                break;
            case 4:
                if (!isFacingRight)
                {
                    sp.flipX = true;
                    isFacingRight = !isFacingRight;
                }
                anim.SetBool("Behind", true);
                break;
        }
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
