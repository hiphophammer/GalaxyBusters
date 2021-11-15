using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayBehavior : MonoBehaviour
{
    // Constants.
    private const float POWER_UP_SCALE = 1.0f;
    private const float ITEM_SELECTION_SCALE = 3.0f;

    private Color commonColor = Color.white;
    private Color rareColor = new Color(0, 0, 100);
    private Color epicColor = new Color(153, 0, 255);
    private Color specialColor = Color.red;

    // Public member variables.
    public Item item;                       // The actual item itself containing the 
                                            // information.
    public Image image;                     // To display the icon.
    public TMPro.TextMeshProUGUI text;      // For the description.
    public bool powerUpMode;                // Whether the icon should be displayed in
                                            // power-up mode (smaller and w/out a
                                            // description).

    // Private member variables.
    private bool pickedUp;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(item != null);
        Debug.Assert(image != null);
        Debug.Assert(text != null);

        // Set the image and scale.
        float scale = powerUpMode ? POWER_UP_SCALE : ITEM_SELECTION_SCALE;
        image.transform.localScale = new Vector3(scale, scale, 1.0f);
        image.sprite = item.icon;

        // Set up the text and visibility.
        text.text = item.description;
        text.color = GetTextColor(item.type);
        text.enabled = !powerUpMode;

        pickedUp = false;
    }

    private void Update()
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

    private Color GetTextColor(Item.ItemType type)
    {
        if (item.type == Item.ItemType.common)
        {
            return commonColor;
        }
        else if (item.type == Item.ItemType.rare)
        {
            return rareColor;
        }
        else if (item.type == Item.ItemType.epic)
        {
            return epicColor;
        }
        else
        {
            return specialColor;
        }
    }
}
