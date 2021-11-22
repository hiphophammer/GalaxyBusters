using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // Constants.
    private const float SPEED = -5.0f;          // This is the speed of the projectile.

    // Private member variables.
    private bool alive;                         // This tells us whether the projectile
                                                // is set to be terminated.

    /// <summary>
    /// This is called before the first frame update. Here we set the speed of the projectile
    /// and play a noise if appropriate.
    /// </summary>
    void Start()
    {
        alive = true;
    }

    /// <summary>
    /// This method is called once per frame. It checks whether the projectile is within the
    /// bounds of the camera and terminates if not. Also updates the position of the projectile.
    /// </summary>
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
            // Destroy ourselves.
            Destroy(transform.gameObject);
        }
    }

    /// <summary>
    /// This private helper method determines whether the projectile is outside of the camera
    /// bounds and should self-terminate.
    /// </summary>
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
                }
            }
        }
    }

    /// <summary>
    /// This updates the position of the egg.
    /// </summary>
    private void UpdatePosition()
    {
        // Keep heading in the same direction at the same speed.
        transform.position += transform.up * (SPEED * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            // Check if the enemy is alive.
            PlayerBehavior playerBehavior =
                                collision.gameObject.GetComponent<PlayerBehavior>();

            if (playerBehavior != null && playerBehavior.IsAlive() && alive)
            {
                alive = false;
            }
        }
    }
}