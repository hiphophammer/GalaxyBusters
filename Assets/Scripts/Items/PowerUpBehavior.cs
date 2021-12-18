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
    private float maxXPos = (20.0f / 3.0f) / 2.0f;
    private float maxYPos = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(item != null);
        maxXPos -= (transform.localScale.x / 2.0f);
        maxYPos -= (transform.localScale.y / 2.0f);
        // Set the image and scale.
        GetComponent<SpriteRenderer>().sprite = item.icon;
        pickedUp = false;
    }

    void Update()
    {
        transform.Translate(Vector3.down * .008f, Space.World);
        if (pickedUp)
        {
            Destroy(transform.gameObject);
        }
        if (transform.position.y <= (-1.0f * (maxYPos + .75f)))
        {
            Destroy(gameObject);
        }
        else if (transform.position.x <= (-1.0f * maxXPos) - 0.5f || transform.position.x >= (maxXPos + 0.5f))
        {
            Destroy(gameObject);
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
