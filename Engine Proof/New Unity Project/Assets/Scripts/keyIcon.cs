using UnityEngine;
using UnityEngine.UI;

public class keyIcon : MonoBehaviour
{
    private Image key_icon;
    private Inventory my_inventory;
    public Sprite[] sprites; // 0 - key not picked up ; 1 - key picked up

    bool hasKey;

    void Start()
    {
        if(GameObject.Find("Key Icon"))
        {
            key_icon = GameObject.Find("Key Icon").GetComponent<Image>();
            hasKey = true;
        }
        else
        {
            hasKey = false;
        }
        my_inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    void Update()
    {
        if (hasKey)
        {
            if (my_inventory.allItem)
            {
                key_icon.sprite = sprites[1];
            }
            else
            {
                key_icon.sprite = sprites[0];
            }
        }
    }
}
