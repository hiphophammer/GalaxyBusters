using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOLVL2 : MonoBehaviour
{
   public EnemyHealth health;

    private Vector3 speed;
    
    CameraSupport cameraSupport;

    private float maxXPos = (20.0f / 3.0f) / 2.0f;
    private float maxYPos = 5.0f;
    
    private float nextFire;
    public float fireRate;

    public float turnSpd;

    private float timeSinceSpawn, timeAtSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        maxXPos -= (transform.localScale.x / 2.0f);
        maxYPos -= (transform.localScale.y / 2.0f);
        
        health = GetComponent<EnemyHealth>();
        health.setHealth(160);
        
        speed = new Vector3(0, -3f, 0);
        speed = speed * Time.fixedDeltaTime;

        cameraSupport = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(cameraSupport != null);

        fireRate = 0.35f;
        nextFire = 0f;

        timeSinceSpawn = 0;
        timeAtSpawn = Time.time;

        turnSpd = 20f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timeSinceSpawn > 1f && timeSinceSpawn < 10f)
        {
            transform.Rotate(Vector3.forward * spin(turnSpd) * Time.fixedDeltaTime);
            shoot();
        }
        else
        {
            transform.Translate(speed, Space.World);
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

    float spin(float spd)
    {
        if(this.transform.position.x < 0)
        {
            return spd;
        }
        else
        {
            return -spd;
        }
    }
    
    //Tells UFO to shoot projectiles.
    void shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, transform.rotation);
            Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, Quaternion.Inverse(transform.rotation));
            GameObject behind = Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, transform.rotation) as GameObject;
            GameObject right = Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, transform.rotation) as GameObject;
            GameObject left = Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, transform.rotation) as GameObject;
            GameObject behind2 = Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, Quaternion.Inverse(transform.rotation)) as GameObject;
            GameObject right2 = Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, Quaternion.Inverse(transform.rotation)) as GameObject;
            GameObject left2 = Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, Quaternion.Inverse(transform.rotation)) as GameObject;

            if(transform.position.x < 0)
            {
                behind.transform.Rotate(Vector3.forward * 180f);
                right.transform.Rotate(Vector3.forward * 90f);
                left.transform.Rotate(Vector3.forward * -90f);
                behind2.transform.Rotate(Vector3.forward * 180f);
                right2.transform.Rotate(Vector3.forward * -90f);
                left2.transform.Rotate(Vector3.forward * 90f);
            }
            else if(transform.position.x >= 0)
            {
                behind.transform.Rotate(Vector3.forward * 180f);
                right.transform.Rotate(Vector3.forward * -90f);
                left.transform.Rotate(Vector3.forward * 90f);
                behind2.transform.Rotate(Vector3.forward * 180f);
                right2.transform.Rotate(Vector3.forward * 90f);
                left2.transform.Rotate(Vector3.forward * -90f);
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
