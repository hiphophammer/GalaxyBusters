using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectionButton : MonoBehaviour
{
    // Constants.
    private const float IMAGE_SCALE = 1.0f;

    private Color commonColor = Color.white;
    private Color rareColor = new Color(0, 0, 100);
    private Color epicColor = new Color(153, 0, 255);
    private Color specialColor = Color.red;

    // Public member variables.
    public Image image;                         // To display the icon.
    public TMPro.TextMeshProUGUI type;          // For the type.
    public TMPro.TextMeshProUGUI description;   // For the description.

    // Private member variables.
    private Item item;                          // The actual item itself containing the 
                                                // information.

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the references to the UI elements are valid.
        Debug.Assert(image != null);
        Debug.Assert(type != null);
        Debug.Assert(description != null);
    }

    // Public modifiers.
    public void SetItem(Item newItem)
    {
        item = newItem;
        UpdateButton();
    }

    // Private helper methods.
    private void UpdateButton()
    {
        // Set the icon and scale.
        image.transform.localScale = new Vector3(IMAGE_SCALE, IMAGE_SCALE, 1.0f);
        image.sprite = item.icon;

        // Set up the type.
        type.text = GetType(item.type);
        type.color = GetTextColor(item.type);

        // Set up the description.
        description.text = item.description;
    }

    private string GetType(Item.ItemType type)
    {
        if (item.type == Item.ItemType.common)
        {
            return "COMMON";
        }
        else if (item.type == Item.ItemType.rare)
        {
            return "RARE";
        }
        else if (item.type == Item.ItemType.epic)
        {
            return "EPIC";
        }
        else
        {
            return "SPECIAL";
        }
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
