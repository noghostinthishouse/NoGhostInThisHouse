using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightTile : MonoBehaviour
{
    public SpriteRenderer[] highlights;

    void Start()
    {
        //set all highlight to false
        for (int i = 0; i < highlights.Length; i++)
        {
            highlights[i].enabled = false;
        }
    }

    public void SetHighlight(bool b)
    {
        for (int i = 0; i < highlights.Length; i++)
        {
            highlights[i].enabled = b;
        }
    }

    public void SetPosition(Vector3 newPos)
    {
        transform.position = newPos;
    }
    
}
