using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : MonoBehaviour
{
    public EnemyHealth health;

    private Vector3 speed;
    
    CameraSupport cameraSupport;
    
    private float nextFire;
    public float fireRate;

    private float maxXPos = (20.0f / 3.0f) / 2.0f;
    private float maxYPos = 5.0f;

    private float timeSinceSpawn, timeAtSpawn;

    void Start()
    {
        maxXPos -= (transform.localScale.x / 2.0f);
        maxYPos -= (transform.localScale.y / 2.0f);

        health = GetComponent<EnemyHealth>();
        health.setHealth(1);

        speed = new Vector3(0, 3f, 0);
        speed = speed * Time.fixedDeltaTime;

        cameraSupport = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(cameraSupport != null);

        fireRate = 150.0f;
        nextFire = 0f;

        timeSinceSpawn = 0;
        timeAtSpawn = Time.time;

    }

    void FixedUpdate()
    {
        transform.Translate(speed);
        shoot();
        
        if (transform.position.y <= (-1.0f * (maxYPos + .75f)))
        {
            Destroy(gameObject);
        }
        else if (transform.position.x <= (-1.0f * maxXPos) - 0.5f || transform.position.x >= (maxXPos + 0.5f))
        {
            Destroy(gameObject);
        }
        timeSinceSpawn = Time.time - timeAtSpawn;
    }
    
    //Tells Fighter to shoot projectiles.
    void shoot()
    {
        if (Time.time > nextFire)
        {
            //Debug.Log("Firing at " + Time.time);
            nextFire = Time.time + fireRate;
            Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("HeroProjectile"))
        {
            // As a HeroProjectile, other must have a ProjectileBehavior script attached.
            ProjectileBehavior damageDealer = other.GetComponent<ProjectileBehavior>();
            Debug.Log(damageDealer.GetParent());
            health.decreaseHealth(damageDealer.GetParent());
        }
    }

}
