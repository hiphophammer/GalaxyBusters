using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    // Constants.
    private const float PROJECTILE_Y_OFFSET = 0.4f;
    private const string BULLET_TYPE_PREFIX = "Prefabs/";
    private const string DEFAULT_BULLET = "Projectile";
    
    // Public member variables.
    public float fireRate;

    // Private member variables.
    private PlayerBehavior parent;
    private string bulletType;
    private float damage;

    private bool vampire;
    private bool penetrate;

    private bool dualStream;

    private float nextFire;
    private bool shouldFire;

    private float PROJECTILE_X_OFFSET = 0;


    // Start is called before the first frame update
    void Start()
    {
        // Set our defaults.
        UpdateDamage();
        SetBullet(DEFAULT_BULLET);
        FireDualStream(true);
        nextFire = 0;
        shouldFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDamage();

        if (shouldFire == true)
        {
            shoot(); 
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
        if (dualStream)
        {
            PROJECTILE_X_OFFSET = PROJECTILE_X_OFFSET + .1f;
        }
        else
        {
            PROJECTILE_X_OFFSET = 0;
        }
        this.dualStream = dualStream;
    }

    public void SetPenetrate(bool penetrate)
    {
        this.penetrate = penetrate;
    }

    public bool GetPenetrate()
    {
        return this.penetrate;
    }

    public void SetVampire(bool vampire)
    {
        this.vampire = vampire;
    }

    public bool GetVampire()
    {
        return this.vampire;
    }
    
    public void SetFireRate(float fireRate)
    {
        this.fireRate = fireRate;
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

    // Tells BaseWeapon to shoot projectiles.
    // Does what SpawnBullet did, but localized entirely within this script, without reliance on Cooldown Bar.
    private void shoot()
    {
        if (Time.time > nextFire && shouldFire)
        {
            //Debug.Log("Firing at " + Time.time);
            nextFire = Time.time + fireRate;
            // Define the start position.

            // Instantiate the bullet.
            if(!dualStream)
            {
                GameObject bullet = Instantiate(Resources.Load(bulletType) as GameObject,
                                        transform.position,
                                        transform.rotation);

                // Configure the component of the bullet.
                ProjectileBehavior bulletBehavior = bullet.GetComponent<ProjectileBehavior>();
                bulletBehavior.SetParent(parent);
                bulletBehavior.SetDamage(damage);
            }
            else
            {
                GameObject leftBullet = Instantiate(Resources.Load(bulletType),
                                        new Vector3(transform.position.x + PROJECTILE_X_OFFSET, transform.position.y + PROJECTILE_Y_OFFSET, transform.position.z),
                                        transform.rotation) as GameObject;
                GameObject rightBullet = Instantiate(Resources.Load(bulletType),
                                        new Vector3(transform.position.x - PROJECTILE_X_OFFSET, transform.position.y + PROJECTILE_Y_OFFSET, transform.position.z),
                                        transform.rotation) as GameObject;

                // Configure the component of the bullet.
                ProjectileBehavior leftBulletBehavior = leftBullet.GetComponent<ProjectileBehavior>();
                leftBulletBehavior.SetParent(parent);
                leftBulletBehavior.SetDamage(damage);
                ProjectileBehavior rightBulletBehavior = rightBullet.GetComponent<ProjectileBehavior>();
                rightBulletBehavior.SetParent(parent);
                rightBulletBehavior.SetDamage(damage);
            }
            
        }
    }
}

