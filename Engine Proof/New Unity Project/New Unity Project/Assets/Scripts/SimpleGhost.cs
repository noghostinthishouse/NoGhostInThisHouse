using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGhost : MonoBehaviour
{
//  [SerializeField] private bool triggered;
    public bool triggered;
    public bool stunt;
    public float speed;

    public GameObject tile;
    private bool eat;
    private Tile t;
    private Vector3 distance;
    private int ghostIndex;

    void Start()
    {
        stunt = false;
        ghostIndex = PlayerTurn.AddGhost();
        triggered = false;
        t = tile.GetComponent<Tile>();
        eat = false;
    }

    void Update()
    {
        if (!t.flashlightOn)
        {
            //1st trigger statement
            if (!PlayerTurn.playerTurn && PlayerTurn.ghostFinished[ghostIndex])
            {
                CheckPlayer();
                PlayerTurn.SetGhostTurn(ghostIndex);
            }
            //2nd trigger statement (game over)
            else if (eat)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, distance, step);
                if (Vector3.Distance(transform.position, distance) < 0.001f)
                {
                    eat = false;
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
        for(int i = 0; i < 4; i++)
        {
            if (t.GetAdjacentTileT(i).playerOn)
            {
                if (triggered)
                {
                    //Debug.Log("Game over");
                    eat = true;
                    PlayerTurn.GameOver = true;
                    CalculateDis(t.GetAdjacentTile(i).GetComponent<Transform>());
                }
                else
                {
                    triggered = true;
                    //Debug.Log("trigger");
                }
            }
        }
    }
    
    void CalculateDis(Transform n_tile)
    {
        //calculate which way to move to
        Vector3 tmp = n_tile.transform.position - tile.transform.position;
        distance = transform.position + tmp;
    }
    
}
