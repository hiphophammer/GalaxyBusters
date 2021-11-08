using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Public member variables.
    public GameObject parent;                   // The object we must stick to.
    public float xOffset;                       // Our X-offset from the parent.
    public float yOffset;                       // Our Y-offset from the parent.

    // Private member variables.
    private float maxWidth;                     // The maximum width of the reload bar.
    private float curHealth;                    // The current health.
    private bool charged;                       // Whether we're fully charged.

    /// <summary>
    /// This is called before the first frame update. We setup the reload bar and make it
    /// invisible.
    /// </summary>
    void Start()
    {
        Debug.Assert(parent != null);

        // We start off with full health.
        curHealth = 1.0f;

        // Set the width of the reload bar.
        Vector3 curScale = transform.localScale;
        maxWidth = curScale.x;
    }

    /// <summary>
    /// This is called once per frame. If we are still reloading then we must update the
    /// reload bar.
    /// </summary>
    void Update()
    {
        UpdateBar();

        UpdatePosition();
    }

    public float Health()
    {
        return curHealth;
    }

    public void AddHealth(float delta)
    {
        curHealth += delta;

        if (curHealth > 1.0f)
        {
            curHealth = 1.0f;
        }
    }

    public void RemoveHealth(float delta)
    {
        curHealth -= delta;
        
        if (curHealth < 0.0f)
        {
            curHealth = 0.0f;
        }
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
        // Update the bar width.
        float percentage = curHealth / 1.0f;
        if (percentage > 1.0f)
        {
            percentage = 1.0f;
        }
        float newWidth = percentage * maxWidth;    // The new width of the bar.

        Vector3 curScale = transform.localScale;
        Vector3 newScale = new Vector3(newWidth, curScale.y, curScale.z);
        transform.localScale = newScale;

        // Update the color of the bar.
        Color newColor = Color.Lerp(Color.red, Color.green, percentage);
        GetComponent<SpriteRenderer>().color = newColor;
    }
}