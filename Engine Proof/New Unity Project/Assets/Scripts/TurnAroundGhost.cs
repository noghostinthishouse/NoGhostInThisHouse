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
    private bool stunt;

    private Animator anim;
    private int phase;

    private Vector3 distance;

    // Start is called before the first frame update
    void Start()
    {
        currentTileIndex = 0;
        eat = false;
        stunt = false;

        currentT = currentTile.GetComponent<Tile>();
        pointedTile = tiles[currentTileIndex].GetComponent<Tile>();
        anim = GetComponent<Animator>();

        CalculateDis();
        SetAnimation();

        ghostIndex = PlayerTurn.AddGhost();
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentT.flashlightOn)
        {
            if (!stunt)
            {
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
                stunt = false;
                PlayerTurn.SetGhostTurn(ghostIndex);
            }
        }
        else
        {
            stunt = true;
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
        SetAnimation();
        Debug.Log(currentTileIndex);
    }

    void Move()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, distance, step);
        if (Vector3.Distance(transform.position, distance) < 0.001f)
        {
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
            phase = 0;
        }
        else if (pointedTile.transform.position.x > currentTile.transform.position.x
          && pointedTile.transform.position.y < currentTile.transform.position.y)
        {
            phase = 1;
        }
        else if (pointedTile.transform.position.x < currentTile.transform.position.x
            && pointedTile.transform.position.y < currentTile.transform.position.y)
        {
            phase = 2;
        }
        else if (pointedTile.transform.position.x < currentTile.transform.position.x
            && pointedTile.transform.position.y > currentTile.transform.position.y)
        {
            phase = 3;
        }
    }

    void SetAnimation()
    {
        anim.SetInteger("Phase", phase);
    }
}
