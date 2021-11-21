using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    // Private member variables.
    PlayerBehavior parent;
    CameraSupport cameraSupport;

    private string[] movementAxes;
    private float speed;

    private float maxXPos = (20.0f / 3.0f) / 2.0f;
    private float maxYPos = 5.0f;

    private bool retrievedAxes;

    // Start is called before the first frame update
    void Start()
    {
        cameraSupport = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(cameraSupport != null);

        speed = parent.GetSpeed();

        maxXPos -= (parent.transform.localScale.x / 2.0f);
        maxYPos -= (parent.transform.localScale.y / 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        RetrieveAxes();

        if (retrievedAxes)
        {
            UpdateSpeed();

            // Retrieve the values from our axes.
            float upDown = Input.GetAxis(movementAxes[0]);
            float leftRight = Input.GetAxis(movementAxes[1]);

            // Scale the values appropriately and check for bounds
            Vector3 pos = transform.position;
            upDown *= (speed * Time.deltaTime);
            if ((upDown < 0.0f) && (pos.y <= (-1.0f * maxYPos + 0.4f)))
            {
                upDown = 0.0f;
            }
            else if ((upDown > 0.0f) && (pos.y >= maxYPos))
            {
                upDown = 0.0f;
            }

            leftRight *= (speed * Time.deltaTime);
            if ((leftRight < 0.0f) && (pos.x <= (-1.0f * maxXPos)))
            {
                leftRight = 0.0f;
            }
            else if ((leftRight > 0.0f) && (pos.x >= maxXPos))
            {
                leftRight = 0.0f;
            }

            // Translate the player by the computed amount.
            transform.Translate(leftRight, upDown, 0.0f);
        }
    }

    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }

    // Private helpers.
    private void RetrieveAxes()
    {
        if (!retrievedAxes)
        {
            movementAxes = parent.GetMovementAxes();
            bool verticalAxisValid = movementAxes[0] != null && movementAxes[0] != "";
            bool horizontalAxisValid = movementAxes[1] != null && movementAxes[1] != "";
            if (verticalAxisValid && horizontalAxisValid)
            {
                retrievedAxes = true;
            }
        }
    }

    private void UpdateSpeed()
    {
        speed = parent.GetSpeed();
    }
}
