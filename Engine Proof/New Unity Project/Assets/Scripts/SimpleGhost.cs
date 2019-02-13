using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGhost : MonoBehaviour
{
    [SerializeField] private bool triggered;
    public GameObject tile;
    private Tile t;
    private Player player;
    
    void Start()
    {
        triggered = false;
        t = tile.GetComponent<Tile>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        
    }

    public void CheckPlayer()
    {
        for(int i = 0; i < 4; i++)
        {
            if (t.GetAdjacentTileT(i).playerOn)
            {
                if (triggered)
                {
                    Debug.Log("Game over");
                }
                else
                {
                    triggered = true;
                    Debug.Log("trigger");
                }
            }
        }
    }
}
