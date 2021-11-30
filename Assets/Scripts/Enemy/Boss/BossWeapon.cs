using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    private GameObject bullet = null;
    private GameObject orb = null;
    private GameObject beam = null;
    private GameObject ring = null;

    PlayerBehavior destroyerBehavior;

    public int health;
    int totalHealth;
    float[] damageDealt;

    public bool destroyed;
    
    // Start is called before the first frame update
    void Start()
    {
        bullet = Resources.Load<GameObject>("Prefabs/Bullet") as GameObject;
        orb = Resources.Load<GameObject>("Prefabs/BossBullet") as GameObject;
        beam = Resources.Load<GameObject>("Prefabs/BossBeam") as GameObject;
        ring = Resources.Load<GameObject>("Prefabs/ChargeRing") as GameObject;

        health = 10;
        totalHealth = health;
        damageDealt = new float[2];
        damageDealt[0] = 0;
        damageDealt[1] = 0;

        destroyed = false;
    }

    void Update()
    {
        if(health <= 0)
        {
            destroyerBehavior.DestroyedEnemy(totalHealth);
            GetComponent<Renderer>().material.color = new Color(0.5f, 0f, 0f);
            destroyed = true;
        }
    }

    public void PatternOne()
    {
        Debug.Log("Firing 1");
        GameObject b = GameObject.Instantiate(bullet) as GameObject;
        b.transform.position = new Vector3(Random.Range(-3.335962f, 3.335962f), 2.44681f, this.transform.position.z);
        b.transform.rotation = Quaternion.Euler(new Vector3(0,0,180));
    }
    
    public void PatternTwo()
    {
        Debug.Log("Firing 2");
        int random = Random.Range(-5, 6);
        for(int i = 110 + random; i <= 250 + random; i+=10){
            GameObject b = GameObject.Instantiate(bullet) as GameObject;
            b.transform.position = new Vector3(this.transform.position.x, 2.44681f, this.transform.position.z);
            b.transform.rotation = Quaternion.Euler(new Vector3(0,0,i));
        }
    }

    public void PatternThree(float time)
    {
        Debug.Log("Firing 3");
        StartCoroutine(PatternThreeDelay(time));
    }

    IEnumerator PatternThreeDelay(float time)
    {
        for(int j = 0; j < 4; j++)
        {
            for(int i = 110; i <= 250; i+=10)
            {
                GameObject b = GameObject.Instantiate(orb) as GameObject;
                b.transform.position = new Vector3(this.transform.position.x, 2.44681f, this.transform.position.z);
                b.transform.rotation = Quaternion.Euler(new Vector3(0,0,i));
                yield return new WaitForSeconds(time);
            }
            for(int i = 250; i >= 110; i-=10)
            {
                GameObject b = GameObject.Instantiate(orb) as GameObject;
                b.transform.position = new Vector3(this.transform.position.x, 2.44681f, this.transform.position.z);
                b.transform.rotation = Quaternion.Euler(new Vector3(0,0,i));
                yield return new WaitForSeconds(time);
            }
        }
    }

    public void PatternFour(float time)
    {
        Debug.Log("Firing 4");
        StartCoroutine(PatternFourDelay(time));
    }

    IEnumerator PatternFourDelay(float time)
    {
        GameObject b = GameObject.Instantiate(ring) as GameObject;
        b.transform.position = new Vector3(this.transform.position.x, 2.44681f, this.transform.position.z);
        yield return new WaitForSeconds(time);
        GameObject c = GameObject.Instantiate(beam) as GameObject;
        c.transform.position = new Vector3(this.transform.position.x, -4.32f, this.transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("HeroProjectile"))
        {
            // As a HeroProjectile, other must have a ProjectileBehavior script attached.
            ProjectileBehavior damageDealer = other.GetComponent<ProjectileBehavior>();
            Debug.Log(damageDealer.GetParent());
            decreaseHealth(damageDealer.GetParent());
        }
    }

    public void decreaseHealth(PlayerBehavior damageDealer)
    {
        if(damageDealer.IsPlayerOne())
        {
            Debug.Log("Player 1 did damage.");
            damageDealt[0]++;
            destroyerBehavior = damageDealer;
            health--;
        }
        else
        {
            damageDealt[1]++;
            destroyerBehavior = damageDealer;
            health--;
        }
    }
}
