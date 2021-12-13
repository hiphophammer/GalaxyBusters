using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailblazerCollider : MonoBehaviour
{
    // Private member variables.
    private PlayerBehavior parent;
    private HealthBar healthBar;
    private ScoreManager score;
    private bool specialItemActive;

    // Start is called before the first frame update
    void Start()
    {
        score = Camera.main.GetComponent<ScoreManager>();
        Debug.Log("Player Collision Detection Starting!");
        healthBar = parent.GetHealthBar();
        specialItemActive = false;
    }

    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }

    public void UpdateSpecialItemStatus(bool status)
    {
        specialItemActive = status;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        Debug.Log("Detected Player Collision");
        
        TrailblazerMovement m = parent.GetComponent<TrailblazerMovement>();
        TrailblazerUltimateAbility ult = parent.GetComponent<TrailblazerUltimateAbility>();
        BaseWeapon weapon = parent.GetComponent<BaseWeapon>();

        float baseDamage = parent.GetWeaponDamage();
        float damageModifier = 1.0f;

        if (other.CompareTag("EnemyProjectile") && parent.IsAlive())
        {
            if (!m.getBlinkStatus() && !ult.getGhostStatus())
            {
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
            else if(ult.getGhostStatus())
            {
                if(!specialItemActive)
                {
                    damageModifier += .2f;
                    parent.SetWeaponDamage(baseDamage * damageModifier);
                    weapon.fireRate -= .01f;
                }
                else
                {
                    damageModifier += .4f;
                    parent.SetWeaponDamage(baseDamage * damageModifier);
                    weapon.fireRate -= .02f;
                }
            }
            
        }
        else if (other.CompareTag("Enemy") && parent.IsAlive())
        {
            if (!m.getBlinkStatus() && !ult.getGhostStatus())
            {
                // Update our health bar.
                healthBar.RemoveHealth(10.0f);
                Debug.Log("Losing Health");
                parent.comboMult = 1f;
                score.UpdateCombo(parent, parent.comboMult);
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
