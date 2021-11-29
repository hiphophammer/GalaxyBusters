using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquare : MonoBehaviour
{
    public EnemyHealth health;

    private Vector3 speed;
    
    CameraSupport cameraSupport;
    
    private float nextFire;
    public float fireRate;
    
    public float turnSpd;

    private float maxXPos = (20.0f / 3.0f) / 2.0f;
    private float maxYPos = 5.0f;

    private float timeSinceSpawn, timeAtSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        maxXPos -= (transform.localScale.x / 2.0f);
        maxYPos -= (transform.localScale.y / 2.0f);

        health = GetComponent<EnemyHealth>();
        health.setHealth(4);
        
        speed = new Vector3(0, -3f, 0);
        speed = speed * Time.fixedDeltaTime;

        cameraSupport = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(cameraSupport != null);


        fireRate = 1.2f;
        nextFire = 0f;

        timeSinceSpawn = 0;
        timeAtSpawn = Time.time;

        turnSpd = 15f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timeSinceSpawn > 1f && timeSinceSpawn < 10f)
        {
            shoot();
        }
        else
        {
            transform.Translate(speed, Space.World);
            
            if (transform.position.y <= (-1.0f * (maxYPos + .75f)))
            {
                Destroy(gameObject);
            }
            else if (transform.position.x <= (-1.0f * maxXPos) - 0.5f || transform.position.x >= (maxXPos + 0.5f))
            {
                Destroy(gameObject);
            }
        }

        timeSinceSpawn = Time.time - timeAtSpawn;
    }
    
    //Tells UFO to shoot projectiles.
    void shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            int random = Random.Range(-5, 6);
            for(int i = 150 + random; i <= 210 + random; i+=10)
            {
                GameObject b = Instantiate(Resources.Load("Prefabs/Bullet")) as GameObject;
                b.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
                b.transform.rotation = Quaternion.Euler(new Vector3(0,0,i));
            } 
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

