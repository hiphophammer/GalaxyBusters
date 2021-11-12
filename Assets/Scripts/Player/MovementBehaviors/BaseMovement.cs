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

    // Start is called before the first frame update
    void Start()
    {
        cameraSupport = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(cameraSupport != null);

        movementAxes = parent.GetMovementAxes();
        speed = parent.GetSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();

        // Retrieve the values from our axes.
        float upDown = Input.GetAxis(movementAxes[0]);
        float leftRight = Input.GetAxis(movementAxes[1]);

        // Scale the values appropriately.
        upDown *= (speed * Time.deltaTime);
        leftRight *= (speed * Time.deltaTime);

        // Get the bounds of our renderer.
        Bounds myBound = GetComponent<Renderer>().bounds;

        // Check if our bounds exceed that of the camera.
        CameraSupport.WorldBoundStatus status = cameraSupport.CollideWorldBound(myBound);

        // Translate the player by the computed amount.
        transform.Translate(leftRight, upDown, 0.0f);
    }

    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }

    private void UpdateSpeed()
    {
        speed = parent.GetSpeed();
    }
}
