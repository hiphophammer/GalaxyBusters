using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanguardCollider : MonoBehaviour
{
    // Private member variables.
    private PlayerBehavior parent;
    private HealthBar healthBar;
    private ChargeBarBehavior chargeBar;
    private ScoreManager score;
    private Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        score = Camera.main.GetComponent<ScoreManager>();
        Debug.Log("Player Collision Detection Starting!");
        healthBar = parent.GetHealthBar();
        baseColor = GetComponent<Renderer>().material.color;
    }

    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        Debug.Log("Detected Player Collision");
        VanguardUltimate ult = parent.GetComponent<VanguardUltimate>();
        VanguardMovement m = parent.GetComponent<VanguardMovement>();
        chargeBar = parent.GetUltimateAbilityChargeBar();

        if (other.CompareTag("EnemyProjectile") && parent.IsAlive())
        {
            if (!ult.getShieldStatus())
            {
                if (!m.getRamStatus())
                {
                    StartCoroutine(parent.csx.Shake(0.1f, 0.1f));
                    StartCoroutine(DamageFlash());
                    // Update our health bar.
                    healthBar.RemoveHealth(10.0f);
                    Debug.Log("Losing Health");
                    parent.comboMult = 1f;
                    score.UpdateCombo(parent, parent.comboMult);
                    chargeBar.AddCharge(5f / 2.0f);
                }
                if (m.getRamStatus())
                {
                    healthBar.RemoveHealth(2.0f);
                    Debug.Log("Losing Reduced Health");
                    chargeBar.AddCharge(5f / 2.0f);
                }
            }
            else
            {
                healthBar.AddHealth(2.5f);
            }
            Destroy(other);
        }
        else if (other.CompareTag("Enemy") && parent.IsAlive())
        {
            if (!ult.getShieldStatus())
            {
                if (!m.getRamStatus())
                {
                    StartCoroutine(parent.csx.Shake(0.1f, 0.1f));
                    StartCoroutine(DamageFlash());
                    // Update our health bar.
                    healthBar.RemoveHealth(10.0f);
                    Debug.Log("Losing Health");
                    parent.comboMult = 1f;
                    score.UpdateCombo(parent, parent.comboMult);
                }
                if (m.getRamStatus())
                {
                    healthBar.RemoveHealth(2.0f);
                    // Check if the enemy is alive.
                    EnemyBehavior enemyBehavior = collision.gameObject.GetComponent<EnemyBehavior>();
                    EnemyHealth ehealth = collision.gameObject.GetComponent<EnemyHealth>();
                    ehealth.instantDeath(parent);
                    Debug.Log("Enemy Destroyed");
                }
            }
            else
            {
                if (m.getRamStatus())
                {
                    // Check if the enemy is alive.
                    EnemyBehavior enemyBehavior = collision.gameObject.GetComponent<EnemyBehavior>();
                    EnemyHealth ehealth = collision.gameObject.GetComponent<EnemyHealth>();
                    ehealth.instantDeath(parent);
                    Debug.Log("Enemy Destroyed");
                }
            }
        }
        else if (other.CompareTag("Boss") || other.CompareTag("BossPart") && parent.IsAlive())
        {
            if (!ult.getShieldStatus())
            {
                if (!m.getRamStatus())
                {
                    StartCoroutine(parent.csx.Shake(0.1f, 0.1f));
                    StartCoroutine(DamageFlash());
                    // Update our health bar.
                    healthBar.RemoveHealth(10.0f);
                    Debug.Log("Losing Health");
                    parent.comboMult = 1f;
                    score.UpdateCombo(parent, parent.comboMult);
                }
                if (m.getRamStatus())
                {
                    healthBar.RemoveHealth(5.0f);
                    // Check if the enemy is alive.
                    EnemyHealth ehealth = collision.gameObject.GetComponent<EnemyHealth>();
                    ehealth.decreaseHealth(parent);
                    Debug.Log("Enemy Destroyed");
                }
            }
            else
            {
                if (m.getRamStatus())
                {
                    // Check if the enemy is alive.
                    EnemyHealth ehealth = collision.gameObject.GetComponent<EnemyHealth>();
                    ehealth.decreaseHealth(parent);
                    Debug.Log("Enemy Destroyed");
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
        // Check if health is 0 or less (Death)
            if(healthBar.Health() <= 0.0f)
            {
                parent.alive = false;
            }
    }

    IEnumerator DamageFlash()
    {
        GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f);
        yield return new WaitForSeconds(0.016f);
        GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f);
        yield return new WaitForSeconds(0.016f);
        GetComponent<Renderer>().material.color = new Color(0f, 0f, 1f);
        yield return new WaitForSeconds(0.016f);
        GetComponent<Renderer>().material.color = new Color(1f, 1f, 0f);
        yield return new WaitForSeconds(0.016f);
        GetComponent<Renderer>().material.color = new Color(1f, 0f, 1f);
        yield return new WaitForSeconds(0.016f);
        GetComponent<Renderer>().material.color = new Color(0f, 1f, 1f);
        yield return new WaitForSeconds(0.016f);
        GetComponent<Renderer>().material.color = baseColor;
    }
}
