using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    private GameObject powerUp;

    private EnemyHealth health;

    PlayerBehavior destroyerBehavior;

    public bool destroyed;

    ScoreManager score;
    
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<EnemyHealth>();
        health.setHealth(1000);

        score = Camera.main.GetComponent<ScoreManager>();

        destroyed = false;
    }

    void Update()
    {
        if(health.GetHealth() <= 0 && !destroyed)
        {
            GetComponent<Renderer>().material.color = new Color(0.5f, 0f, 0f);
            GameObject Explosion = Instantiate(Resources.Load("Prefabs/Explosion"), transform.position, transform.rotation) as GameObject;
            Destroy(Explosion.gameObject, 1);
            destroyed = true;
        }
    }

    public void PatternOne()
    {
        Debug.Log("Firing 1");
        GameObject b = Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, transform.rotation) as GameObject;
        b.transform.position = new Vector3(Random.Range(-3.335962f, 3.335962f), 2.44681f, this.transform.position.z);
        b.transform.rotation = Quaternion.Euler(new Vector3(0,0,180));
    }
    
    public void PatternTwo()
    {
        Debug.Log("Firing 2");
        int random = Random.Range(-5, 6);
        for(int i = 110 + random; i <= 250 + random; i+=10){
            GameObject b = Instantiate(Resources.Load("Prefabs/Bullet"), transform.position, transform.rotation) as GameObject;
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
                GameObject b = Instantiate(Resources.Load("Prefabs/BossBullet"), transform.position, transform.rotation) as GameObject;
                b.transform.position = new Vector3(this.transform.position.x, 2.44681f, this.transform.position.z);
                b.transform.rotation = Quaternion.Euler(new Vector3(0,0,i));
                yield return new WaitForSeconds(time);
            }
            for(int i = 250; i >= 110; i-=10)
            {
                GameObject b = Instantiate(Resources.Load("Prefabs/BossBullet"), transform.position, transform.rotation) as GameObject;
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
        GameObject b = Instantiate(Resources.Load("Prefabs/ChargeRing"), transform.position, transform.rotation) as GameObject;
        b.transform.position = new Vector3(this.transform.position.x, 2.44681f, this.transform.position.z);
        yield return new WaitForSeconds(time);
        GameObject c =  Instantiate(Resources.Load("Prefabs/BossBeam"), transform.position, transform.rotation) as GameObject;
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
            health.decreaseHealth(damageDealer.GetParent());
        }
    }
}
