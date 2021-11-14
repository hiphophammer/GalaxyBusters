using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{
    // Public member variables.
    public Item item;                       // The actual item itself containing the 
                                            // information.

    // Private member variables.
    private bool pickedUp;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(item != null);

        // Set the image and scale.
        GetComponent<SpriteRenderer>().sprite = item.icon;

        pickedUp = false;
    }

    void Update()
    {
        if (pickedUp)
        {
            Destroy(transform.gameObject);
        }
    }

    public bool HasBeenPickedUp()
    {
        return pickedUp;
    }

    public void SetPickedUp()
    {
        pickedUp = true;
    }
}
