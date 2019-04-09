using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    public GameObject target;
    public Vector3 offset;
    public Collider2D room;
    public Camera cam;
    public int BGMIndex;

    Vector3 Min;
    Vector3 Max;

    float height;
    float width;

    Vector3 targetPos;

    // Use this for initialization
    void Start()
    {
        height = 2.0f * cam.orthographicSize;
        width = height * cam.aspect;

        Min = room.bounds.min;
        Max = room.bounds.max;

        targetPos = transform.position;

        SoundManager.instance.PlayBGM(BGMIndex);
        SoundManager.instance.PlaySFX(12);          //plays enter sound
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * 5f;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);

            // check left bound
            if(transform.position.x < Min.x + width / 2)
            {
                Vector3 tmp = transform.position;
                tmp.x = Min.x + width / 2;
                transform.position = tmp;
            }
            // check lower bound
            if (transform.position.y < Min.y + height / 2)
            {
                Vector3 tmp = transform.position;
                tmp.y = Min.y + height / 2;
                transform.position = tmp;
            }
            // check right bound
            if (transform.position.x > Max.x - width / 2)
            {
                Vector3 tmp = transform.position;
                tmp.x = Max.x - width / 2;
                transform.position = tmp;
            }
            // check upper bound
            if (transform.position.y > Max.y - height / 2)
            {
                Vector3 tmp = transform.position;
                tmp.y = Max.y - height / 2;
                transform.position = tmp;
            }
        }
    }
}
