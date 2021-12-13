using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollider : MonoBehaviour
{
    // Private member variables.
    private PlayerBehavior parent;
    private HealthBar healthBar;
    private Color baseColor;
    private ScoreManager score;

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

        if (other.CompareTag("EnemyProjectile") && parent.IsAlive())
        {
            StartCoroutine(parent.csx.Shake(0.1f, 0.1f));
            StartCoroutine(DamageFlash());
            // Update our health bar.
            healthBar.RemoveHealth(10.0f);
            Debug.Log("Losing Health");
            parent.comboMult = 1f;
            score.UpdateCombo(parent, parent.comboMult);
            if(healthBar.Health() <= 0.0f)
            {
                parent.alive = false;
            }
            Destroy(other);
        }
        else if ((other.CompareTag("Enemy") || other.CompareTag("Boss")) && parent.IsAlive())
        {
            StartCoroutine(parent.csx.Shake(0.1f, 0.1f));
            StartCoroutine(DamageFlash());
            // Update our health bar.
            healthBar.RemoveHealth(10.0f);
            Debug.Log("Losing Health");
            parent.comboMult = 1f;
            score.UpdateCombo(parent, parent.comboMult);
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
