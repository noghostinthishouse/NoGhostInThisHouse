using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyActionGhost : MonoBehaviour
{
    public CopyActionGhostMovement ghost_movement;
    public GameObject currentTile;
    public float speed = 10.0f;

    private Tile currentT;

    private Player player;

    private GameObject nextTile;
    private Tile nextT;

    private int ghostIndex;
    private Vector3 distance; 

    private Animator anim;
    private SpriteRenderer sp;
    private int phase;              // 1 = top left, 2 = bottom left, 3 = bottom right, 4 = top right
    [SerializeField] private bool isFacingRight;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        currentT = currentTile.GetComponent<Tile>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();

        ghostIndex = PlayerTurn.AddGhost();
    }
    
    void Update()
    {
        if (player.my_flashight.GetComponent<Flashlight>().IsOn())
        {
            if (player.IsMove() && ghost_movement.GetTileToMove())
            {
                nextTile = ghost_movement.GetTileToMove();
                nextT = nextTile.GetComponent<Tile>();
                if (nextT.IsEmpty())
                {
                    CalculateDis();
                }
                else
                {
                    nextTile = null;
                    nextT = null;
                }
            }
            if (PlayerTurn.ghostFinished[ghostIndex] && nextTile)
            {
                SetAnimation();
                if (nextT.playerOn)
                {
                    PlayerTurn.GameOver = true;
                }
                Move();
            }
            else
            {
                PlayerTurn.SetGhostTurn(ghostIndex);
            }
        }
        else
        {
            PlayerTurn.SetGhostTurn(ghostIndex);
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

    void ChangeTile()
    {
        currentT.SetEmpty();
        nextT.SetNotEmpty();

        currentTile = nextTile;
        currentT = nextT;

        nextTile = null;
        nextT = null;
    }

    void CalculateDis()
    {
        //calculate which way to move to
        Vector3 tmp = nextTile.transform.position - currentTile.transform.position;
        distance = transform.position + tmp;

        //calculate which animation to use;
        if (nextTile.transform.position.x > currentTile.transform.position.x
            && nextTile.transform.position.y > currentTile.transform.position.y)
        {
            phase = 4;
        }
        else if (nextTile.transform.position.x > currentTile.transform.position.x
          && nextTile.transform.position.y < currentTile.transform.position.y)
        {
            phase = 3;
        }
        else if (nextTile.transform.position.x < currentTile.transform.position.x
            && nextTile.transform.position.y < currentTile.transform.position.y)
        {
            phase = 2;
        }
        else if (nextTile.transform.position.x < currentTile.transform.position.x
            && nextTile.transform.position.y > currentTile.transform.position.y)
        {
            phase = 1;
        }
    }

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
}
