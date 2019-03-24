using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradedChasingGhost : MonoBehaviour
{
    public bool triggered;
    public bool stunt;
    public float speed;

    public GameObject tile;             //the tile that the ghost is standing on
    private Tile t;

    public GameObject nextTile;         //the tile in front of the ghost
    private Tile nextTile_t;

    public GameObject nextTile2;        //the further tile in front of the ghost
    private Tile nextTile_t2;

    private bool eat;
    private Vector3 distance;

    private int ghostIndex;             //use to end this ghost's turn
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
        nextTile_t2 = nextTile2.GetComponent<Tile>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        eat = false;
    }

    void Update()
    {
        if (!t.flashlightOn)
        {
            //Debug.Log(PlayerTurn.ghostFinished[ghostIndex]);
            anim.SetBool("Stunt", false);
            //this statement is use to detect player in front of their tile
            if (!triggered && PlayerTurn.ghostFinished[ghostIndex])
            {
                //check for trigger
                CheckPlayer();
                //calculate which tile to move to
                CalculateDis();
                if (nextTile_t.playerOn)
                {
                    PlayerTurn.GameOver = true;
                    Move();
                }
                else
                {
                    PlayerTurn.SetGhostTurn(ghostIndex);
                }
            }
            //this statement is use to move the ghost after it is triggered
            else if (triggered && PlayerTurn.ghostFinished[ghostIndex])
            {
                SetAnimation();
                Move();
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
        Debug.Log(nextTile_t2.playerOn);
        if (nextTile_t.playerOn || nextTile_t2.playerOn)
        {
            triggered = true;
            Debug.Log("triggered");
        }
    }

    void Move()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, distance, step);
        if (Vector3.Distance(transform.position, distance) < 0.001f)
        {
            anim.SetBool("Behind", false);
            anim.SetBool("Front", false);
            ChangeTile();
            CalculateDis();
            if (PlayerTurn.GameOver)
            {
                PlayerTurn.SetGameOver();
            }
            else
            {
                PlayerTurn.SetGhostTurn(ghostIndex);
            }
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
        }
        else if (nextTile.transform.position.x > tile.transform.position.x
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

        nextTile = nextTile2;
        nextTile_t = nextTile_t2;

        nextTile2 = player.GetPlayerCurrentTile();
        nextTile_t2 = nextTile2.GetComponent<Tile>();
    }
}
