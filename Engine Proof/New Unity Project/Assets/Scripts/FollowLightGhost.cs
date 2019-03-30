using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLightGhost : MonoBehaviour
{
    public GameObject currentTile;
    public AllTiles allTiles;
    public float speed;

    private GameObject nextTile;        // holds the tile to the shortest route
    private GameObject targetTile;

    private Tile currentT;
    private Tile nextT;
    private Tile targetT;


    private Player player;

    private Vector3 distance;
    private int ghostIndex;
    private bool move;
    private bool eat;

    //find path
    private List<GameObject> checkedPath;
    private List<GameObject> path;
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
            if (!currentT.flashlightPlaced || !currentT.playerOn)
            {
                if (IsFlashlightPlaced())
                {
                    targetTile = FindTileWithFlashlight();
                }
                else
                {
                    targetTile = FindTileWithPlayer();
                }

                nextTile = FindPath(targetTile);
                nextT = nextTile.GetComponent<Tile>();
                if (nextT.playerOn)
                {
                    eat = true;
                }
                if (move)
                {
                    CalculateDis();
                    SetAnimation();
                    Move();
                }
            }
            else
            {
                PlayerTurn.SetGhostTurn(ghostIndex);
            }
        }
    }
    
    GameObject FindPath(GameObject targetT)
    {
        List<GameObject> checkTile = new List<GameObject>();        // keep tile game object that has been checked, clears after each route check
        List<int> path = new List<int>();                           // keep number of tile use to go to the target

        GameObject tmpTile;
        Tile tmpTileT;

        for (int i = 0; i < currentT.nearbyTiles.Length; i++)
        {
            // add the current tile so it wont' search backwards
            checkTile.Add(currentTile);
            path.Add(1);

            tmpTile = currentT.GetAdjacentTile(i);
            tmpTileT = currentT.GetComponent<Tile>();

            bool deadEnd = false;
            while(tmpTile != targetT && tmpTileT.IsEmpty() && !deadEnd)
            {
                path[i]++;
                checkTile.Add(tmpTile);
                deadEnd = true;
                for (int j = 0; j < tmpTileT.nearbyTiles.Length; j++)
                {
                    // check if the tile hasn't already been check and is empty
                    if (!Search(tmpTileT.nearbyTiles[j]) && tmpTileT.IsEmpty())
                    {
                        tmpTile = tmpTileT.nearbyTiles[j];
                        deadEnd = false;
                        break;
                    }
                }
                tmpTileT = tmpTile.GetComponent<Tile>();
            }
            // clear after finished one route to start finding the next one
            checkTile.Clear();
            if (deadEnd)
            {
                path[i] = -1;
            }
        }

        // compare which path is closer to the target
        int num = 100;
        int index = 0;
        for(int i = 0; i < path.Count; i++)
        {
            if(num > path[i] && path[i] != -1)
            {
                num = path[i];
                index = i;
                Debug.Log(path.Count);      //------------------------------------------------------------------
            }
        }
        return currentT.nearbyTiles[index];
    }

    GameObject FindTileWithFlashlight()
    {
        foreach (GameObject t in allTiles.Tiles)
        {
            if (t.GetComponent<Tile>().flashlightPlaced)
            {
                return t;
            }
        }
        return null;
    }

    GameObject FindTileWithPlayer()
    {
        foreach (GameObject t in allTiles.Tiles)
        {
            if (t.GetComponent<Tile>().playerOn)
            {
                return t;
            }
        }
        return null;
    }

    bool IsFlashlightPlaced()
    {
        foreach(GameObject t in allTiles.Tiles)
        {
            if (t.GetComponent<Tile>().flashlightPlaced)
            {
                return true;
            }
        }
        return false;
    }

    bool Search(GameObject searchItem)
    {
        foreach(GameObject t in allTiles.Tiles)
        {
            if(t == searchItem)
            {
                return true;
            }
        }
        return false;
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

        nextT = null;
        nextTile = null;
    }
}