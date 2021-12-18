using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBody : MonoBehaviour
{
    private GameObject bossLL;
    private GameObject bossLeft;
    private GameObject bossRight;
    private GameObject bossRR;

    private GameObject[] allWeapons;

    private Vector3 speed;

    private EnemyHealth health;

    int totalHealth;
    float[] damageDealt;

    GameObject explosion;
    GameObject powerUp;

    PlayerBehavior destroyerBehavior;

    Camera cam;
    CameraShake csx;

    private float timeSinceSpawn, timeAtSpawn, timeSinceAttack;

    private bool isVulnerable;
    
    // Start is called before the first frame update
    void Start()
    {
        float height = transform.position.y - 0.313f;
        bossLL = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/BossLeft")) as GameObject;
        bossLL.transform.position = new Vector3(-2.0015772f, height, 0f);

        bossLeft = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/BossLeft")) as GameObject;
        bossLeft.transform.position = new Vector3(-0.6671924f, height, 0f);

        bossRight = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/BossRight")) as GameObject;
        bossRight.transform.position = new Vector3(0.6671924f, height, 0f);

        bossRR = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/BossRight")) as GameObject;
        bossRR.transform.position = new Vector3(2.0015772f, height, 0f);

        allWeapons = new GameObject[]{bossLL, bossLeft, bossRight, bossRR};

        speed = new Vector3(0, -1f, 0);
        speed = speed * Time.fixedDeltaTime;

        health = GetComponent<EnemyHealth>();
        health.setHealth(1600);

        cam = Camera.main;
        csx = Camera.main.GetComponent<CameraShake>();

        timeSinceSpawn = 0;
        timeAtSpawn = Time.time;
        timeSinceAttack = 0;

        isVulnerable = false;

        StartCoroutine(StartSequence());
    }

    // Update is called once per frame
    void Update()
    {
        CheckVulnerable();

        if(transform.position.y > 3.6f)
        {
            transform.Translate(speed, Space.World);
            for(int i = 0; i < allWeapons.Length; i++)
            {
                allWeapons[i].transform.Translate(speed, Space.World);
            }
        }

        if(health.GetHealth() <= 800 && isVulnerable)
        {
            StartCoroutine(EndSequence());
        }

        if(Input.GetKeyUp("1")){
            StartCoroutine(PatternOneDelay(0.25f));
        }
        if (Input.GetKeyUp("2"))
        {
            StartCoroutine(PatternTwoDelay(0.5f));
        }
        if (Input.GetKeyUp("3"))
        {
            StartCoroutine(PatternThreeDelay(0.15f));
        }
        if (Input.GetKeyUp("4"))
        {
            StartCoroutine(EndSequence());
        }
        
        timeSinceSpawn = Time.time - timeAtSpawn;
    }

    private void CheckVulnerable()
    {
        if (bossLL.GetComponent<BossWeapon>().destroyed && bossLeft.GetComponent<BossWeapon>().destroyed &&
            bossRight.GetComponent<BossWeapon>().destroyed && bossRR.GetComponent<BossWeapon>().destroyed)
        {
            isVulnerable = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("HeroProjectile"))
        {
            if(isVulnerable)
            {
                // As a HeroProjectile, other must have a ProjectileBehavior script attached.
                ProjectileBehavior damageDealer = other.GetComponent<ProjectileBehavior>();
                Debug.Log(damageDealer.GetParent());
                health.decreaseHealth(damageDealer.GetParent());
            }
        }
    }

    IEnumerator StartSequence()
    {
        while(health.GetHealth() > 0)
        {
            int random = Random.Range(1, 4);
            if(random == 1){
                int count = 0;
                while(count < 20)
                {
                    for(int i = 0; i < allWeapons.Length; i++)
                    {
                        if(!allWeapons[i].GetComponent<BossWeapon>().destroyed || isVulnerable)
                        {
                            allWeapons[i].GetComponent<BossWeapon>().PatternOne();
                        }
                    }
                             yield return new WaitForSeconds(0.25f);
                    count++;
                }
                yield return new WaitForSeconds(1.5f);
            }
            else if (random == 2)
            {
                int count = 0;
                while(count < 10)
                {
                    random = Random.Range(0, 4);
                    GameObject x = null;
                 if(!allWeapons[random].GetComponent<BossWeapon>().destroyed || isVulnerable)
                    {
                        allWeapons[random].GetComponent<BossWeapon>().PatternTwo();
                    }
                 yield return new WaitForSeconds(0.5f);
                    count++;
                }
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                random = Random.Range(0, 4);
                int random1 = Random.Range(1, 4);
                while(random == random1)
                {
                    random1 = Random.Range(1, 4);
                }
                if(!allWeapons[random].GetComponent<BossWeapon>().destroyed || isVulnerable)
                {
                    allWeapons[random].GetComponent<BossWeapon>().PatternThree(0.15f);
                }
                if(!allWeapons[random1].GetComponent<BossWeapon>().destroyed || isVulnerable)
                {
                    allWeapons[random1].GetComponent<BossWeapon>().PatternThree(0.15f);
                }
                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    IEnumerator PatternOneDelay(float time)
    {
        int count = 0;
        while(count < 20)
        {
            for(int i = 0; i < allWeapons.Length; i++)
            {
                if(!allWeapons[i].GetComponent<BossWeapon>().destroyed || isVulnerable)
                {
                    allWeapons[i].GetComponent<BossWeapon>().PatternOne();
                }
            }

            yield return new WaitForSeconds(time);
            count++;
        }
        yield return new WaitForSeconds(1.5f);
    }
    
    IEnumerator PatternTwoDelay(float time)
    {
        int count = 0;
        while(count < 10)
        {
            int random = Random.Range(1, 5);
            GameObject x = null;

            if(!allWeapons[random].GetComponent<BossWeapon>().destroyed || isVulnerable)
            {
                allWeapons[random].GetComponent<BossWeapon>().PatternTwo();
            }
            
            yield return new WaitForSeconds(time);
            count++;
        }
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator PatternThreeDelay(float time)
    {
        int random = Random.Range(0, 4);
        int random1 = Random.Range(1, 4);
        while(random == random1)
        {
            random1 = Random.Range(1, 4);
        }
        if(!allWeapons[random].GetComponent<BossWeapon>().destroyed || isVulnerable)
        {
            allWeapons[random].GetComponent<BossWeapon>().PatternThree(time);
        }
        if(!allWeapons[random1].GetComponent<BossWeapon>().destroyed || isVulnerable)
        {
            allWeapons[random1].GetComponent<BossWeapon>().PatternThree(time);
        }
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator PatternFourDelay(float time)
    {
        //charging!
        for(int i = 0; i < allWeapons.Length; i++){
            allWeapons[i].GetComponent<BossWeapon>().PatternFour(time);
        }

        yield return new WaitForSeconds(time);

        // LAZERS OF DOOOOOOM
        StartCoroutine(csx.Shake(1.5f, 0.2f));
        yield return new WaitForSeconds(5f);
    }

    IEnumerator EndSequence(){
        //charging!
        for(int i = 0; i < allWeapons.Length; i++){
            allWeapons[i].GetComponent<BossWeapon>().PatternFour(10.0f);
        }
        
        if(health.GetHealth() <= 0)
        {
            for(int i = 0; i < allWeapons.Length; i++)
            {
                Destroy(allWeapons[i]);
                explosion = Instantiate(Resources.Load("Prefabs/Explosion"), allWeapons[i].transform.position, allWeapons[i].transform.rotation) as GameObject;
                Destroy(explosion.gameObject, 1);
            }
            Destroy(gameObject);
            explosion = Instantiate(Resources.Load("Prefabs/Explosion"), transform.position, transform.rotation) as GameObject;
            Destroy(explosion.gameObject, 1);
        }
        // LAZERS OF DOOOOOOM
        StartCoroutine(csx.Shake(1f, 0.5f));
        yield return new WaitForSeconds(5f);

    }
}
