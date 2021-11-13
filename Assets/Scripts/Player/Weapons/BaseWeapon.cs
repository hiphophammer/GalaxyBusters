using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    // Constants.
    private const float PROJECTILE_Y_OFFSET = 0.4f;
    private const float PROJECTILE_X_OFFSET = 0.1f;

    private const string BULLET_TYPE_PREFIX = "Prefabs/";
    private const string DEFAULT_BULLET = "Projectile";

    // Private member variables.
    private PlayerBehavior parent;
    private CooldownBarBehavior cooldownBar;
    private string bulletType;
    private float damage;

    private bool dualStream;

    private bool shouldFire;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve a reference to the cooldown bar.
        cooldownBar = parent.GetWeaponCooldownBar();

        // Set our defaults.
        UpdateDamage();
        SetBullet(DEFAULT_BULLET);
        FireDualStream(false);
        StartFiring();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDamage();

        // Check if enough time has passed to fire another round.
        if (cooldownBar.ReadyToFire() && shouldFire)
        {
            // Spawn the bullets.
            if (dualStream)
            {
                // We need to shoot two streams of bullets.

                // Spawn the left bullet.
                SpawnBullet(-1.0f * PROJECTILE_X_OFFSET, PROJECTILE_Y_OFFSET);

                // And the right bullet.
                SpawnBullet(PROJECTILE_X_OFFSET, PROJECTILE_Y_OFFSET);
            }
            else
            {
                // We don't shoot the bullets in a cone.
                SpawnBullet(0.0f, PROJECTILE_Y_OFFSET);
            }            

            // Trigger the reload.
            cooldownBar.TriggerCooldown();
        }
    }

    // Modifiers.
    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }

    public void SetBullet(string bullet)
    {
        bulletType = BULLET_TYPE_PREFIX + bullet;
    }

    public string GetBullet()
    {
        return bulletType;
    }

    public void StartFiring()
    {
        shouldFire = true;
    }

    public void StopFiring()
    {
        shouldFire = false;
    }

    public void FireDualStream(bool dualStream)
    {
        this.dualStream = dualStream;
    }

    // Private helpers.
    private void UpdateDamage()
    {
        damage = parent.GetWeaponDamage();
    }

    private void SpawnBullet(float xOffset, float yOffset)
    {
        // Define the start position.
        Vector3 startPos = transform.position;
        startPos.x += xOffset;
        startPos.y += yOffset;

        // Instantiate the bullet.
        GameObject bullet = Instantiate(Resources.Load(bulletType) as GameObject,
                                        startPos,
                                        transform.rotation);

        // Configure the component of the bullet.
        ProjectileBehavior bulletBehavior = bullet.GetComponent<ProjectileBehavior>();
        bulletBehavior.SetParent(parent);
        bulletBehavior.SetDamage(damage);
    }
}
