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
        healthBar = parent.GetHealthBar();
    }

    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("EnemyProjectile") && parent.IsAlive())
        {
            // Update our health bar.
            healthBar.RemoveHealth(0.25f / 2.0f);

            if (healthBar.Health() == 0.0f)
            {
                healthBar.AddHealth(1.0f);
            }
        }
    }
}
