using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailblazerMovement : MonoBehaviour
{
     // Private member variables.
    PlayerBehavior parent;
    CameraSupport cameraSupport;
    private CooldownBarBehavior cooldownBar;

    private string[] movementAxes;
    private float speed;
    private string axis;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool Blink;

    // Start is called before the first frame update
    void Start()
    {
        cameraSupport = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(cameraSupport != null);

        movementAxes = parent.GetMovementAxes();
        speed = parent.GetSpeed();

        cooldownBar = parent.GetBasicAbilityCooldownBar();
        axis = parent.GetBasicAbilityAxis();

        
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

        if (Input.GetAxis(axis) == 1.0f)
        {
            if (cooldownBar.ReadyToFire())
            {
                //Blink
                startPos = transform.position;
                endPos = new Vector3(transform.position.x + (500 * leftRight) , transform.position.y + (500 * upDown), transform.position.z);
                Debug.Log(upDown);
                if (startPos != endPos)
                {
                    Blink = true;
                    cooldownBar.TriggerCooldown();
                    parent.GetComponent<Collider2D>().enabled = false;
                }
            }
        }

        if (Blink)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, 20f * Time.deltaTime);
            if (transform.position == endPos)
            {
                Instantiate(Resources.Load("Prefabs/BulletDestructionZone"), transform.position, transform.rotation);
                Blink = false;
                parent.GetComponent<Collider2D>().enabled = true;
            }
        }
        else
        {
             // Translate the player by the computed amount.
            transform.Translate(leftRight, upDown, 0.0f);
        }
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
