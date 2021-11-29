using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserBehavior : MonoBehaviour, EnemyBehavior
{
    public EnemyHealth health;
    
    private Vector3 speed;
   
    CameraSupport cameraSupport;

    private float maxXPos = (20.0f / 3.0f) / 2.0f;
    private float maxYPos = 5.0f;
   
    private float nextFire;
    public float fireRate;

    GameObject hero;
    GameObject[] players;

    private float timeSinceSpawn, timeAtSpawn;

    
    // Start is called before the first frame update
    void Start()
    {
        maxXPos -= (transform.localScale.x / 2.0f);
        maxYPos -= (transform.localScale.y / 2.0f);

        health = GetComponent<EnemyHealth>();
        health.setHealth(1);
        
        speed = new Vector3(0, 2.5f, 0);
        speed = speed * Time.fixedDeltaTime;

        cameraSupport = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(cameraSupport != null);

        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 2)
        {
            hero = players[Random.Range(0,2)];
        }
        else
        {
            hero = players[0];
        }
        

        fireRate = 0.75f;
        nextFire = 0f;

        timeSinceSpawn = 0;
        timeAtSpawn = Time.time;

    }

    void FixedUpdate()
    {
        
        transform.Translate(speed);
        if(timeSinceSpawn < 5f)
        {
            PointAtPosition(hero.transform.position, .025f);
            shoot();
        }
        else
        {
            Instantiate(Resources.Load("Prefabs/SelfDestruct"), transform.position, transform.rotation);
            Destroy(gameObject);
        }
        
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


    /// <summary>
    /// This points the top of the enemy at the specified position, at the specified rate.
    /// </summary>
    /// <param name="p">The position to point to.</param>
    /// <param name="r">The rate at which we should turn towards it.</param>
    private void PointAtPosition(Vector3 p, float r)
    {
        Vector3 v = p - transform.position;
        transform.up = Vector3.LerpUnclamped(transform.up, v, r);
    }

    //Tells Chaser to shoot projectiles.
    void shoot()
    {
        if (Time.time > nextFire)
        {
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

    public void TakeDamage(float totalDamage, PlayerBehavior playerBehavior)
    {
        health.decreaseHealth(playerBehavior);
    }

    public bool IsAlive()
    {
        return health.GetHealth() > 0;
    }
}
