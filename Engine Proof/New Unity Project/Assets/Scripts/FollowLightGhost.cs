using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLightGhost : MonoBehaviour
{
    public GameObject currentTile;
    public TileCheck tileCheck;
    public float speed;

    private GameObject nextTile;        // holds the tile to the shortest route

    private Tile currentT;
    private Tile nextT;

    private Player player;

    private Vector3 distance;
    private int ghostIndex;
    private bool move;
    private bool eat;
    private int count;

    private Animator anim;
    private SpriteRenderer sp;
    private int phase;
    [SerializeField] private bool isFacingRight;


    void Start()
    {
        ghostIndex = PlayerTurn.AddGhost();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        currentT = currentTile.GetComponent<Tile>();
        nextTile = null;
        nextT = null;

        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();

        move = false;
        eat = false;
        count = 0;
    }

    void Update()
    {
        if (PlayerTurn.ghostFinished[ghostIndex])
        {
            
        }
    }

    void FindPath()
    {
        // wiki dijkstra
        // https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
    }

    void Move()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, distance, step);
        if (Vector3.Distance(transform.position, distance) < 0.001f)
        {
            move = false;
            anim.SetBool("Behind", false);
            anim.SetBool("Front", false);

            ChangeTile();

            nextT = null;
            nextTile = null;

            if (eat)
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
    
    public void ChangeTile()
    {
        currentT.SetEmpty();
        nextT.SetNotEmpty();

        currentTile = nextTile;
        currentT = nextT;
    }
}