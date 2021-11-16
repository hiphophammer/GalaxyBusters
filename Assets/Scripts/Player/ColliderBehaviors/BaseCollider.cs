using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollider : MonoBehaviour
{
    // Private member variables.
    private PlayerBehavior parent;
    private HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player Collision Detection Starting!");
        healthBar = parent.GetHealthBar();
    }

    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        Debug.Log("Detected Player Collision");

        if (other.CompareTag("EnemyProjectile") && parent.IsAlive())
        {
            // Update our health bar.
            healthBar.RemoveHealth(10.0f);
            Debug.Log("Losing Health");
            if(healthBar.Health() <= 0.0f)
            {
                parent.alive = false;
            }
        }
        else if (other.CompareTag("Enemy") && parent.IsAlive())
        {
            // Update our health bar.
            healthBar.RemoveHealth(10.0f);
            Debug.Log("Losing Health");
            if(healthBar.Health() <= 0.0f)
            {
                parent.alive = false;
            }
        }
        else if (other.CompareTag("PowerUp"))
        {
            PowerUpBehavior powerUpBehavior = other.GetComponent<PowerUpBehavior>();

            // Check our reference is valid and the item hasn't been picked up.
            if ((powerUpBehavior != null) && (!powerUpBehavior.HasBeenPickedUp()))
            {
                Item item = powerUpBehavior.item;
                powerUpBehavior.SetPickedUp();

                // DEBUG TODO: Add this to inventory.
                //Debug.Log(item.type);
                parent.GetInventory().AddItem(item, true);
            }
        }
    }
}
