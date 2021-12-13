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
    private string axis;

    private float maxXPos = (20.0f / 3.0f) / 2.0f;
    private float maxYPos = 5.0f;

    private bool retrievedAxes;

    private int count;

    private float elapsedTime;
    private const float TIME_BETWEEN_SPAWNS = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        cameraSupport = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(cameraSupport != null);

        movementAxes = parent.GetMovementAxes();
        speed = parent.GetSpeed();

        maxXPos -= (parent.transform.localScale.x / 2.0f);
        maxYPos -= (parent.transform.localScale.y / 2.0f);

        retrievedAxes = false;

        count = 1;
        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        RetrieveAxes();
        
        if (retrievedAxes)
        {
            if (elapsedTime > TIME_BETWEEN_SPAWNS)
            {
                elapsedTime = 0;
                GameObject afterimage = Instantiate(Resources.Load("Prefabs/PlayerGhost") as GameObject, transform.position, transform.rotation);
                SpriteRenderer renderer = afterimage.GetComponent<SpriteRenderer>();
                renderer.sortingOrder = count++;
                afterimage.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                renderer.sprite = GetComponent<SpriteRenderer>().sprite;
            }
            
            UpdateSpeed();

            // Retrieve the values from our axes.
            float upDown = Input.GetAxisRaw(movementAxes[0]);
            float leftRight = Input.GetAxisRaw(movementAxes[1]);

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

    private void UpdateSpeed()
    {
        speed = parent.GetSpeed();
    }

    // Private helpers.
    private void RetrieveAxes()
    {
        if (!retrievedAxes)
        {
            movementAxes = parent.GetMovementAxes();
            bool verticalAxisValid = movementAxes[0] != null && movementAxes[0] != "";
            bool horizontalAxisValid = movementAxes[1] != null && movementAxes[1] != "";

            axis = parent.GetBasicAbilityAxis();
            bool axisValid = axis != null && axis != "";

            if (verticalAxisValid && horizontalAxisValid && axisValid)
            {
                retrievedAxes = true;
            }
        }
    }
}
