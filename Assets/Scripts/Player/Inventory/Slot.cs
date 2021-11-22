using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    // Public member variables.
    public Image icon;
    public TMPro.TextMeshProUGUI countText;

    // Private member variables.
    private Sprite defaultIcon;

    // Start is called before the first frame update
    void Start()
    {
        defaultIcon = icon.sprite;
        Clear();
    }

    public void SetIcon(Sprite newIcon, int count)
    {
        icon.sprite = newIcon;
        countText.text = count.ToString();
        if (count > 1)
        {
            ShowText();
        }
        else
        {
            HideText();
        }
    }

    public void Clear()
    {
        icon.sprite = defaultIcon;
        HideText();
    }

    private void HideText()
    {
        countText.alpha = 0.0f;
    }

    private void ShowText()
    {
        countText.alpha = 255.0f;
    }
}

