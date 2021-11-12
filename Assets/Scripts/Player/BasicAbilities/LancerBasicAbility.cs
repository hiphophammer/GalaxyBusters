using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerBasicAbility : MonoBehaviour
{
    // Constants.
    private const float PROJECTILE_Y_OFFSET = 0.4f;
    private const float DAMAGE = 0.75f;

    // Private member variables.
    private PlayerBehavior parent;
    private CooldownBarBehavior cooldownBar;
    private string axis;

    // Start is called before the first frame update
    void Start()
    {
        cooldownBar = parent.GetBasicAbilityCooldownBar();
        axis = parent.GetBasicAbilityAxis();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the ability is being activated.
        if (Input.GetAxis(axis) == 1.0f)
        {
            if (cooldownBar.ReadyToFire())
            {
                // Launch missile.
                Vector3 startPos = transform.position;
                startPos.y += PROJECTILE_Y_OFFSET;
                GameObject projectile = Instantiate(Resources.Load("Prefabs/Projectile") as GameObject,
                                                    startPos,
                                                    transform.rotation);
                projectile.GetComponent<SpriteRenderer>().color = Color.red;

                ProjectileBehavior projectileBehavior = projectile.GetComponent<ProjectileBehavior>();
                projectileBehavior.SetParent(parent);
                projectileBehavior.SetDamage(DAMAGE);

                // Trigger the reload.
                cooldownBar.TriggerCooldown();
            }
        }
    }

    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }
}
