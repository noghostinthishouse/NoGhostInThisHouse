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
    [SerializeField] private bool stunt;
    private bool start;

    private Animator anim;
    private int phase;

    private Vector3 distance;

    // Start is called before the first frame update
    void Start()
    {
        currentTileIndex = 0;
        eat = false;
        stunt = false;
        start = false;

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
        currentT.SetNotEmpty();
        // to fix when the ghost turn before its turn started after hitting retry button
        if (!start)
        {
            currentTileIndex = 0;
            start = true;
        }

        else if (!currentT.flashlightOn)
        {
            if (!eat && PlayerTurn.ghostFinished[ghostIndex])
            {
                if (!CheckPlayer())
                {
                    if (!stunt)
                    {
                        Turn();
                    }
                    else
                    {
                        stunt = false;
                    }
                    CheckPlayer();
                }
            }
        }
        else if(PlayerTurn.ghostFinished[ghostIndex])
        {
            stunt = true;
            //CheckPlayer();
        }

        if (eat && PlayerTurn.ghostFinished[ghostIndex])
        {
            if (!IsTurning())
            {
                Move();
            }
        }
        else
        {
            PlayerTurn.SetGhostTurn(ghostIndex);
        }
    }

    bool CheckPlayer()
    {
        if (pointedTile.playerOn)
        {
            eat = true;
        }
        return eat;
    }

    void Turn()
    {
        currentTileIndex = (currentTileIndex + 1) % tiles.Length;
        pointedTile = tiles[currentTileIndex].GetComponent<Tile>();
        CalculateDis();
        SetAnimation();
        //Debug.Log(currentTileIndex);
    }

    void Move()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, distance, step);
        if (Vector3.Distance(transform.position, distance) < 0.001f)
        {
            PlayerTurn.SetGameOver();
            PlayerTurn.SetGhostTurn(ghostIndex);
        }
    }

    public bool IsTurning()
    {
        string[] nameOfWalkAnims = { "DownR_To_DownL", "DownL_To_UpL", "UpL_To_UpR", "UpR_To_DownR"};

        foreach (string name in nameOfWalkAnims)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(name))
            {
                return true;
            }
        }
        return false;
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
