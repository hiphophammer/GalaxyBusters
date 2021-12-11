using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanguardCollider : MonoBehaviour
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
        VanguardMovement m = parent.GetComponent<VanguardMovement>();

        if (other.CompareTag("EnemyProjectile") && parent.IsAlive())
        {
            if (!m.getShieldStatus())
            {
                // Update our health bar.
                healthBar.RemoveHealth(10.0f);
                Debug.Log("Losing Health");
                parent.comboMult = 1f;
                if(healthBar.Health() <= 0.0f)
                {
                    parent.alive = false;
                }
            }
            healthBar.AddHealth(1.0f);
            Destroy(other);
        }
        else if (other.CompareTag("Enemy") && parent.IsAlive())
        {
            if (!m.getShieldStatus())
            {
                // Update our health bar.
                healthBar.RemoveHealth(10.0f);
                Debug.Log("Losing Health");
                parent.comboMult = 1f;
                if(healthBar.Health() <= 0.0f)
                {
                    parent.alive = false;
                    GameObject Explosion = Instantiate(Resources.Load("Prefabs/Explosion"), transform.position, transform.rotation) as GameObject;
                    Destroy(Explosion.gameObject, 1);
                }
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
                if (item.isPowerUp && item.ID == 0)
                {
                    parent.GetHealthBar().AddHealth(item.dHP);
                }
                if (item.isPowerUp && item.ID == 1)
                {
                    parent.ultimateAbilityChargeBar.AddCharge(25.0f / 2.0f);
                }
            }
        }
    }
}
