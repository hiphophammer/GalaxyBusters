using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    // Constants.
    private const float PROJECTILE_Y_OFFSET = 0.4f;

    // Private member variables.
    private PlayerBehavior parent;
    private CooldownBarBehavior cooldownBar;
    private float damage;

    // Start is called before the first frame update
    void Start()
    {
        cooldownBar = parent.GetWeaponCooldownBar();
        damage = parent.GetWeaponDamage();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDamage();

        // Check if enough time has passed to fire another round.
        if (cooldownBar.ReadyToFire())
        {
            // Instantiate the projectile.
            Vector3 startPos = transform.position;
            startPos.y += PROJECTILE_Y_OFFSET;
            GameObject projectile =
                Instantiate(Resources.Load("Prefabs/Projectile") as GameObject,
                            startPos,
                            transform.rotation);
            ProjectileBehavior projectileBehavior = projectile.GetComponent<ProjectileBehavior>();
            projectileBehavior.SetParent(parent);
            projectileBehavior.SetDamage(damage);

            // Trigger the reload.
            cooldownBar.TriggerCooldown();
        }
    }

    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }

    private void UpdateDamage()
    {
        damage = parent.GetWeaponDamage();
    }
}
