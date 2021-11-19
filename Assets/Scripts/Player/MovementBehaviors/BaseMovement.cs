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

        // Scale the values appropriately and check for bounds
        Vector3 pos = transform.position;
        upDown *= (speed * Time.deltaTime);
        if(upDown < 0 && pos.y <= -5){
            upDown = 0;
        } else if(upDown > 0 && pos.y >= 5){
            upDown = 0;
        }

        leftRight *= (speed * Time.deltaTime);
        if(leftRight < 0 && pos.x <= -3.335962){
            leftRight = 0;
        } else if(leftRight > 0 && pos.x >= 3.335962){
            leftRight = 0;
        }

        /* This broke, not sure why, added bandage fix above
        ///////////////////////////////////////////////////
        // Get the bounds of our renderer.
        Bounds myBound = GetComponent<Renderer>().bounds;

        // Check if our bounds exceed that of the camera.
        CameraSupport.WorldBoundStatus status = cameraSupport.CollideWorldBound(myBound);*/

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
