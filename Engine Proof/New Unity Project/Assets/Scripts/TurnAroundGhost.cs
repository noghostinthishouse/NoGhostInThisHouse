using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAroundGhost : MonoBehaviour
{
    public GameObject currentTile;
    public GameObject[] tiles;

    public float speed;

    private Tile currentT;
    private Tile pointedTile;

    private int currentTileIndex;
    private int ghostIndex;

    private bool eat;

    private Animator anim;
    private SpriteRenderer sp;
    private int phase;
    [SerializeField] private bool isFacingRight;

    private Vector3 distance;

    // Start is called before the first frame update
    void Start()
    {
        currentTileIndex = 0;
        eat = false;

        currentT = currentTile.GetComponent<Tile>();
        pointedTile = tiles[currentTileIndex].GetComponent<Tile>();

        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        ghostIndex = PlayerTurn.AddGhost();
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentT.flashlightOn)
        {
            //anim.SetBool("Stunt", false);
            if (!eat && PlayerTurn.ghostFinished[ghostIndex])
            {
                if (!CheckPlayer())
                {
                    Turn();
                    CheckPlayer();
                }
            }
            if (eat)
            {
                Move();
            }
            else
            {
                PlayerTurn.SetGhostTurn(ghostIndex);
            }
        }
        else
        {
            //anim.SetBool("Stunt", true);
            PlayerTurn.SetGhostTurn(ghostIndex);
        }
    }

    bool CheckPlayer()
    {
        if (pointedTile.playerOn)
        {
            eat = true;
            return true;
        }
        return false;
    }

    void Turn()
    {
        currentTileIndex = (currentTileIndex + 1) % tiles.Length;
        pointedTile = tiles[currentTileIndex].GetComponent<Tile>();
        CalculateDis();
        //SetAnimation();
    }

    void Move()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, distance, step);
        if (Vector3.Distance(transform.position, distance) < 0.001f)
        {
            //anim.SetBool("Behind", false);
            //anim.SetBool("Front", false);
            PlayerTurn.SetGameOver();
        }
    }

    void CalculateDis()
    {
        //calculate which way to move to
        Vector3 tmp = pointedTile.transform.position - currentTile.transform.position;
        distance = transform.position + tmp;

        //calculate which animation to use;
        if (pointedTile.transform.position.x > currentTile.transform.position.x
            && pointedTile.transform.position.y > currentTile.transform.position.y)
        {
            phase = 4;
        }
        else if (pointedTile.transform.position.x > currentTile.transform.position.x
          && pointedTile.transform.position.y < currentTile.transform.position.y)
        {
            phase = 3;
        }
        else if (pointedTile.transform.position.x < currentTile.transform.position.x
            && pointedTile.transform.position.y < currentTile.transform.position.y)
        {
            phase = 2;
        }
        else if (pointedTile.transform.position.x < currentTile.transform.position.x
            && pointedTile.transform.position.y > currentTile.transform.position.y)
        {
            phase = 1;
        }
    }

    //1 = top left, 2 = bottom left, 3 = bottom right, 4 = top right
    //void SetAnimation()
    //{
    //    switch (phase)
    //    {
    //        case 1:
    //            if (isFacingRight)
    //            {
    //                sp.flipX = false;
    //                isFacingRight = !isFacingRight;
    //            }
    //            anim.SetBool("Behind", true);
    //            break;
    //        case 2:
    //            if (isFacingRight)
    //            {
    //                sp.flipX = false;
    //                isFacingRight = !isFacingRight;
    //            }
    //            anim.SetBool("Front", true);
    //            break;
    //        case 3:
    //            if (!isFacingRight)
    //            {
    //                sp.flipX = true;
    //                isFacingRight = !isFacingRight;
    //            }
    //            anim.SetBool("Front", true);
    //            break;
    //        case 4:
    //            if (!isFacingRight)
    //            {
    //                sp.flipX = true;
    //                isFacingRight = !isFacingRight;
    //            }
    //            anim.SetBool("Behind", true);
    //            break;
    //    }
    //}
}
