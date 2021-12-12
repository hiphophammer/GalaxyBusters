/*
 * @file ProjectileBehavior.cs
 * 
 * 
 * 
 * @date 11/3/2021
 * @author Shakeel Khan
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    // Constants.
    private const float SPEED = 10.0f;          // This is the speed of the projectile.

    // Private member variables.
    private PlayerBehavior parent;              // A reference to the behavior of our
                                                // parent.
    private float damage;                       // How much damage this projectile does
                                                // to the enemy.
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

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }

    public PlayerBehavior GetParent()
    {
        return parent;
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

        if (other.CompareTag("Enemy"))
        {
            // Checks for if Vampire Bullet is active and acts accordingly.
            if (parent.GetWeapon().GetVampire())
            {
                HealthBar hp = parent.GetHealthBar();
                hp.AddHealth(5);
            }

            // Checks for if Penetration Bullet is active and acts accordingly. 
            // Set true for testing purposes, set to false to destroy bullet on impact with enemy.
            //
            // TODO: Find out why bullets destroy on collision with Chaser regardless of status.
            if (!parent.GetWeapon().GetPenetrate())
            {
                Destroy(gameObject);
            }
        }
    }
}