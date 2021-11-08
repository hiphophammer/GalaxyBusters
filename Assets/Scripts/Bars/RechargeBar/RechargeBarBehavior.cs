using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechargeBarBehavior : MonoBehaviour
{
    // Public member variables.
    public GameObject parent;                   // The object we must stick to.
    public float xOffset;                       // Our X-offset from the parent.
    public float yOffset;                       // Our Y-offset from the parent.
    public float maxCharge;                     // The maximum charge (when the bar
                                                // should be appear full).

    // Private member variables.
    private float maxWidth;                     // The maximum width of the reload bar.
    private float curCharge;                    // The current charge.
    private bool charged;                       // Whether we're fully charged.

    /// <summary>
    /// This is called before the first frame update. We setup the reload bar and make it
    /// invisible.
    /// </summary>
    void Start()
    {
        Debug.Assert(parent != null);

        // Ready to fire from the start.
        charged = true;

        // Set the width of the reload bar.
        Vector3 curScale = transform.localScale;
        maxWidth = curScale.x;

        // Set the color of the bar.
        Color color = new Color(173.0f / 255.0f, 44.0f / 255.0f, 255.0f / 255.0f);
        GetComponent<SpriteRenderer>().color = color;
    }

    /// <summary>
    /// This is called once per frame. If we are still reloading then we must update the
    /// reload bar.
    /// </summary>
    void Update()
    {
        if (!charged)
        {
            UpdateBar();
        }

        UpdatePosition();
    }

    /// <summary>
    /// This triggers a reload and should be called once a egg has been fired.
    /// </summary>
    public void ResetCharge()
    {
        charged = false;
        curCharge = 0.0f;
    }

    /// <summary>
    /// This determines whether the reload time has passed and another egg can be fired.
    /// </summary>
    /// <returns>Returns a boolean indicating whether the reload time has
    /// passed.</returns>
    public bool Charged()
    {
        return charged;
    }

    public void AddCharge(float delta)
    {
        curCharge += delta;
    }

    /// <summary>
    /// Updates the position of the reload bar to stick to the player it's associated with.
    /// </summary>
    private void UpdatePosition()
    {
        Vector3 parentPos = parent.transform.position;
        Vector3 newPos = new Vector3(parentPos.x + xOffset, parentPos.y + yOffset, 0.0f);
        transform.position = newPos;
    }

    /// <summary>
    /// This private helper method updates the reload bar and checks if the reload time has
    /// passed.
    /// </summary>
    private void UpdateBar()
    {
        // Check if we are fully charged.
        if (curCharge >= maxCharge)
        {
            charged = true;
        }

        
        // Update the bar width.
        float percentage = curCharge / maxCharge;
        if (percentage > 1.0f)
        {
            percentage = 1.0f;
        }
        float newWidth = percentage * maxWidth;    // The new width of the bar.

        Vector3 curScale = transform.localScale;
        Vector3 newScale = new Vector3(newWidth, curScale.y, curScale.z);
        transform.localScale = newScale;

        // Update the color of the bar.
        Color newColor = Color.Lerp(new Color(118.0f / 255.0f,
                                                198.0f / 255.0f,
                                                255.0f / 255.0f),
                                    new Color(173.0f / 255.0f,
                                                44.0f / 255.0f,
                                                255.0f / 255.0f),
                                    percentage);
        GetComponent<SpriteRenderer>().color = newColor;
    }
}