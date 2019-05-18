using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn_Time : MonoBehaviour
{

    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Expire", timer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Expire (float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
