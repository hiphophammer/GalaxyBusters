using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainAspectRatio : MonoBehaviour
{
    public float TARGET_ASPECT_RATIO = 2.0f / 3.0f;

    private float oldWindowAspectRatio = -1.0f;

    // Use this for initialization
    void Start()
    {
        DoThing();
    }

    void Update()
    {
        DoThing();
    }

    private void DoThing()
    {
        // Determine the game window's current aspect ratio.
        float windowAspectRatio = (float)Screen.width / (float)Screen.height;
        if (oldWindowAspectRatio != windowAspectRatio)
        {
            oldWindowAspectRatio = windowAspectRatio;

            // Current viewport height should be scaled by this amount.
            float scaleHeight = windowAspectRatio / TARGET_ASPECT_RATIO;

            // Obtain camera component so we can modify its viewport.
            Camera camera = GetComponent<Camera>();

            // If scaled height is less than current height, add letterbox.
            if (scaleHeight < 1.0f)
            {
                Rect rect = camera.rect;

                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f;

                camera.rect = rect;
            }
            else
            {
                // Add pillarbox.
                float scaleWidth = 1.0f / scaleHeight;

                Rect rect = camera.rect;

                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;

                camera.rect = rect;
            }
        }
    }
}
