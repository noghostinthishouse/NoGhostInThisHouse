using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLightGhost : MonoBehaviour
{
    public GameObject currentTile;
    public AllTiles allTiles;
    public float speed;
    public Flashlight[] flashlight;

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

        anim.SetBool("Awake", true);
        anim.SetBool("Front", true);
    }

    void Update()
    {
        currentT.SetNotEmpty();
        if (PlayerTurn.ghostFinished[ghostIndex])
        {
            if (!currentT.flashlightPlaced && !currentT.playerOn && !move && CheckAnyFlashlightOn())
            {
                move = true;
                anim.SetBool("Move", true);
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
                CalculateDis();
                SetAnimation();
            }
            else if (move)
            {
                Move();
            }
            else
            {
                PlayerTurn.SetGhostTurn(ghostIndex);
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
                CalculateDis();
            }
        }
        if (CheckAnyFlashlightOn())
        {
            anim.SetBool("Awake", true);
        }
        else
        {
            anim.SetBool("Awake", false);
        }
    }
    
    bool CheckAnyFlashlightOn()
    {
        foreach(Flashlight f in flashlight)
        {
            if (f.IsOn())
            {
                return true;
            }
        }
        return false;
    }

    GameObject FindPath(GameObject target)
    {
        List<GameObject> checkedTile = new List<GameObject>();                      // keep tile game object that has been checked, clears after each route check
        List<int> path = new List<int>();                                           // keep number of tile use to go to the target
        //Dictionary<GameObject, int> pathCount = new Dictionary<GameObject, int>();

        GameObject tmpTile;
        Tile tmpTileT;

        GameObject[] sortedTile1 = SortTile(currentT.nearbyTiles, target);

        for (int i = 0; i < sortedTile1.Length; i++)
        {
            // add the current tile so it wont' search backwards
            checkedTile.Add(currentTile);
            path.Add(1);
            //pathCount.Add(currentT.GetAdjacentTile(i), 1);

            tmpTile = sortedTile1[i];
            tmpTileT = tmpTile.GetComponent<Tile>();

            bool deadEnd = false;

            while(tmpTile != target && tmpTileT.IsEmpty() && !deadEnd)
            {
                //pathCount[currentT.GetAdjacentTile(i)]++;
                path[i]++;
                checkedTile.Add(tmpTile);
                deadEnd = true;
                //sort before loop
                GameObject[] sortedTile2 = SortTile(tmpTileT.nearbyTiles, target);
                for (int j = 0; j < sortedTile2.Length; j++)
                {
                    // check if the tile hasn't already been check and is empty
                    if (!Search(checkedTile, sortedTile2[j]) && sortedTile2[j].GetComponent<Tile>().IsEmpty())
                    {
                        tmpTile = tmpTileT.nearbyTiles[j];
                        deadEnd = false;
                        break;
                    }

                }
                tmpTileT = tmpTile.GetComponent<Tile>();
                //if (deadEnd)
                //{
                //    Debug.Log("DEAD END");
                //}
            }
            // Debug.Log("END ROUTE " + (i + 1));
            // clear after finished one route to start finding the next one
            checkedTile.Clear();
            if (deadEnd || (!tmpTileT.IsEmpty() && !deadEnd))
            {
                path[i] = -1;
            }
        }

        // compare which path is closer to the target
        int num = 100;
        int index = 0;
        for(int i = 0; i < path.Count; i++)
        {
            // Debug.Log(currentT.nearbyTiles[i] + " : " + path[i]);
            if(num > path[i] && path[i] != -1)
            {
                num = path[i];
                index = i;
            }
        }
        // Debug.Log("END");
        return currentT.nearbyTiles[index];
    }
    
    GameObject[] SortTile(GameObject[] tileToSort, GameObject target)
    {
        GameObject[] tiles = tileToSort;
        for (int i = 0; i < tileToSort.Length - 1; i++)
        {
            GameObject tmp;
            if (Vector3.Distance(target.transform.position, tileToSort[i].transform.position) > Vector3.Distance(target.transform.position, tileToSort[i + 1].transform.position))
            {
                tmp = tileToSort[i];
                tileToSort[i] = tileToSort[i + 1];
                tileToSort[i + 1] = tmp;
            }
        }
        return tileToSort;
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

    bool Search(List<GameObject> checkedTile, GameObject searchItem)
    {
        foreach(GameObject t in checkedTile)
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
            anim.SetBool("Move", false);

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
                anim.SetBool("Front", false);
                break;
            case 2:
                if (isFacingRight)
                {
                    sp.flipX = false;
                    isFacingRight = !isFacingRight;
                }
                anim.SetBool("Front", true);
                anim.SetBool("Behind", false);
                break;
            case 3:
                if (!isFacingRight)
                {
                    sp.flipX = true;
                    isFacingRight = !isFacingRight;
                }
                anim.SetBool("Front", true);
                anim.SetBool("Behind", false);
                break;
            case 4:
                if (!isFacingRight)
                {
                    sp.flipX = true;
                    isFacingRight = !isFacingRight;
                }
                anim.SetBool("Behind", true);
                anim.SetBool("Front", false);
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