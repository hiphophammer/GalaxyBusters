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
    private Color baseColor, alpha;

    private float OGradius;

    private float maxXPos = (20.0f / 3.0f) / 2.0f;
    private float maxYPos = 5.0f;

    private bool retrievedAxes;

    private GameObject hitbox;
    private bool active;

    private int count;
    private float elapsedTime;
    private const float TIME_BETWEEN_SPAWNS = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        maxXPos -= (parent.transform.localScale.x / 2.0f);
        maxYPos -= (parent.transform.localScale.y / 2.0f);

        cameraSupport = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(cameraSupport != null);

        movementAxes = parent.GetMovementAxes();
        speed = parent.GetSpeed();

        cooldownBar = parent.GetBasicAbilityCooldownBar();
        // Get parent's SpriteRenderer component.
        baseColor = parent.GetComponent<SpriteRenderer>().color;
        alpha = parent.GetComponent<SpriteRenderer>().color;
        alpha.a = .6f;


        // Get collider to expand and contract as necessary.
        CircleCollider2D col = GetComponent<CircleCollider2D>();

        // Set radius of collider to .1f
        col.radius = .1f;
        OGradius = col.radius;

        hitbox = Instantiate(Resources.Load("Prefabs/Hitbox") as GameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z - .1f), transform.rotation);
        float hitboxSize = GetComponent<CircleCollider2D>().radius * 2;
        hitbox.transform.parent = parent.transform;
        hitbox.transform.localScale = new Vector3(hitboxSize, hitboxSize, 1);
        hitbox.GetComponent<SpriteRenderer>().sortingOrder = 2;
        active = true;

        retrievedAxes = false;

        count = 1;
        elapsedTime = 0.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        RetrieveAxes();

        elapsedTime += Time.deltaTime;
        
        if (retrievedAxes)
        {
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


            // Get the bounds of our renderer.
            Bounds myBound = GetComponent<Renderer>().bounds;


            if (Input.GetAxis(axis) == 1.0f)
            {
                if (cooldownBar.ReadyToFire())
                {
                    // Blink
                    startPos = transform.position;
                    endPos = new Vector3(transform.position.x + (100 * leftRight), transform.position.y + (100 * upDown), transform.position.z);
                    endPos.x = Clip(-1.0f * maxXPos, maxXPos, endPos.x);
                    endPos.y = Clip(-1.0f * maxYPos, maxYPos, endPos.y);
                    Debug.Log(upDown);
                    if (startPos != endPos)
                    {
                        Blink = true;
                        cooldownBar.TriggerCooldown();
                        GetComponent<CircleCollider2D>().radius = OGradius * 6f;
                        parent.GetComponent<SpriteRenderer>().color = alpha;
                    }
                }
            }

            if (Blink)
            {
                transform.position = Vector3.Lerp(transform.position, endPos, 12.5f * Time.deltaTime);
                if (elapsedTime > TIME_BETWEEN_SPAWNS)
                {
                    elapsedTime = 0;
                    GameObject afterimage = Instantiate(Resources.Load("Prefabs/PlayerGhost") as GameObject, transform.position, transform.rotation);
                    SpriteRenderer renderer = afterimage.GetComponent<SpriteRenderer>();
                    renderer.sortingOrder = count++;
                    //afterimage.GetComponent<Renderer>().material.color = new Color32(255,114,114,125);
                    afterimage.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 0.5f);
                    renderer.sprite = GetComponent<SpriteRenderer>().sprite;
                }

                if (transform.position == endPos)
                {
                    GetComponent<CircleCollider2D>().radius = OGradius;
                    Blink = false;
                    parent.GetComponent<SpriteRenderer>().color = baseColor;
                }
            }
            else
            {
                // Translate the player by the computed amount.
                transform.Translate(leftRight, upDown, 0.0f);
            }
            
            if (Input.GetKeyDown("h"))
            {
                active = !active;
                hitbox.SetActive(active);
            }
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

            axis = parent.GetBasicAbilityAxis();
            bool axisValid = axis != null && axis != "";

            if (verticalAxisValid && horizontalAxisValid && axisValid)
            {
                retrievedAxes = true;
            }
        }
    }

    private void UpdateSpeed()
    {
        speed = parent.GetSpeed();
    }

    private float Clip(float min, float max, float val)
    {
        if (val < min)
        {
            return min;
        }
        else if (val > max)
        {
            return max;
        }
        else
        {
            return val;
        }
    }

    public bool getBlinkStatus()
    {
        return Blink;
    }
}
