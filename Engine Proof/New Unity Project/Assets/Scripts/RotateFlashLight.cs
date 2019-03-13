using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFlashLight : MonoBehaviour
{
    float speed = 10.0f;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.IsMove())
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);

            //change character sprite to match rotation
            if(direction.x > 0 && direction.y > 0)
            {
                player.SetDirection(0);
            }
            else if(direction.x > 0 && direction.y < 0)
            {
                player.SetDirection(1);
            }
            else if (direction.x < 0 && direction.y > 0)
            {
                player.SetDirection(2);
            }
            else if (direction.x < 0 && direction.y < 0)
            {
                player.SetDirection(3);
            }
        }
    }
}
