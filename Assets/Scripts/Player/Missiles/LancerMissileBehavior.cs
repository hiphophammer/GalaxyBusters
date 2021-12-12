using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerMissileBehavior : MonoBehaviour
{
    // Constants.
    private const float ACCELERATION = 1.01f; // This is the amount by which the speed
                                               // increases by each update.
    private const float MAX_SPEED = 15.0f;     // The max speed of the missile.

    private const float POST_MORTEM_PERIOD = 0.5f;

    private const float DAMAGE = 40f;        // The amount of damage dealt to the enemy
                                               // we make a head-on collision with.
    private const float AOE_RADIUS = 4.5f;

    // Private member variables.
    private PlayerBehavior parent;              // A reference to the behavior of our
                                                // parent.
    private float curSpeed;
    private bool alive;                         // This tells us whether the projectile
                                                // is set to be terminated.
    private float timeOfDeath;

    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        curSpeed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            // Check whether we are in the bounds of the camera. If not, terminate.
            CheckBounds();

            // Update our position.
            UpdatePosition();
        }
        else
        {
            float dTime = Time.time - timeOfDeath;
            if (dTime > POST_MORTEM_PERIOD)
            {
                // Destroy ourselves.
                Destroy(transform.gameObject);
            }
            else
            {
                //UpdatePosition();
            }
        }
    }

    // Public accessors.
    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }

    public PlayerBehavior GetParent()
    {
        return parent;
    }

    public float GetDamage()
    {
        return DAMAGE;
    }

    // Private helper methods.
    private void CheckBounds()
    {
        // First, try to get the CameraSupport component of the main camera.
        CameraSupport cs = Camera.main.GetComponent<CameraSupport>();
        if (cs != null)
        {
            // Get the bounds of the Sprite Renderer.
            Bounds myBound = GetComponent<Renderer>().bounds;

            // Check if our bounds exceeds that of the camera.
            CameraSupport.WorldBoundStatus status = cs.CollideWorldBound(myBound);

            if (status != CameraSupport.WorldBoundStatus.Inside)
            {
                // We aren't inside the bounds of the camera, so we terminate.
                if (alive)
                {
                    alive = false;
                    GetComponent<SpriteRenderer>().enabled = false;
                    timeOfDeath = Time.time;
                }
            }
        }
    }

    private void UpdatePosition()
    {
        // Keep heading in the same direction at the same speed.
        transform.position += transform.up * (curSpeed * Time.deltaTime);

        // Update our speed.
        if (curSpeed < MAX_SPEED)
        {
            curSpeed *= ACCELERATION;
        }
        else
        {
            curSpeed = MAX_SPEED;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("Enemy"))
        {
            curSpeed = 0f;
            // Check if the enemy is alive.
            EnemyHealth eHealth = collision.gameObject.GetComponent<EnemyHealth>();

            if (eHealth.health > 0)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                alive = false;
                timeOfDeath = Time.time;

                // Deal damage to the enemy we directly collided with.
                eHealth.missileImpact(DAMAGE, parent);

                // Deal damage to any enemy within the AOE radius.
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < enemies.Length; i++)
                {
                    GameObject enemy = enemies[i];

                    // Make sure this is not the enemy we collided with.
                    if (enemy.name != this.name)
                    {
                        // Compute the distance between us and it.
                        float dst = Vector3.Distance(enemy.transform.position,
                                                    transform.position);
                        
                        if (dst <= AOE_RADIUS)
                        {
                            float damage = (dst / AOE_RADIUS) * DAMAGE;
                            enemy.GetComponent<EnemyHealth>().missileImpact(damage, parent);
                        }
                    }
                }
            }
        }
    }
}
