using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlightPlace : MonoBehaviour
{
    public SpriteRenderer[] flashlightSP;
    public Transform flashlight;

    Vector3 offset;

    void Start()
    {
        offset = flashlight.position - transform.position;
        for (int i = 0; i < flashlightSP.Length; i++)
        {
            flashlightSP[i].enabled = false;
        }
    }

    void Update()
    {
        Vector3 tmp = transform.position;
        tmp = flashlight.position + offset;
        transform.position = tmp;
    }

    public void PutFlashlightDown(int index)
    {
        flashlightSP[index].enabled = true;
        flashlightSP[4].enabled = true;
    }

    public void PickFlashlightUp()
    {
        for (int i = 0; i < flashlightSP.Length; i++)
        {
            flashlightSP[i].enabled = false;
        }
    }

}
