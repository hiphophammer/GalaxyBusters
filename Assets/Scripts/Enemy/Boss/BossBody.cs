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

    public int health;
    int totalHealth;
    float[] damageDealt;

    GameObject explosion;
    GameObject powerUp;

    PlayerBehavior destroyerBehavior;

    Camera cam;
    CameraShake csx;
    ScoreManager score;

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

        health = 10;
        totalHealth = 10;

        cam = Camera.main;
        csx = Camera.main.GetComponent<CameraShake>();
        score = cam.GetComponent<ScoreManager>();
        damageDealt = new float[2];
        damageDealt[0] = 0;
        damageDealt[1] = 0;

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

        if(health <= 0)
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

    public int GetHealth()
    {
        return health;
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
                decreaseHealth(damageDealer.GetParent());
            }
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

    IEnumerator StartSequence()
    {
        while(true)
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
                        random = Random.Range(1, 5);
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
                    random = Random.Range(1, 5);
                    int random1 = Random.Range(1, 5);
                    while(random == random1)
                    {
                        random1 = Random.Range(1, 5);
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
        int random = Random.Range(1, 5);
        int random1 = Random.Range(1, 5);
        while(random == random1)
        {
            random1 = Random.Range(1, 5);
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
        StartCoroutine(csx.Shake(5f, 0.2f));
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator EndSequence(){
                //charging!
                for(int i = 0; i < allWeapons.Length; i++){
                    allWeapons[i].GetComponent<BossWeapon>().PatternFour(4.0f);
                }

                yield return new WaitForSeconds(4.0f);

                // LAZERS OF DOOOOOOM
                StartCoroutine(csx.Shake(5f, 0.2f));
                yield return new WaitForSeconds(1.5f);
                Destroy(gameObject);
                explosion = Instantiate(Resources.Load("Prefabs/explosion"), transform.position, transform.rotation) as GameObject;
                Destroy(explosion.gameObject, 1);

                for(int i = 0; i < allWeapons.Length; i++)
                {
                    Destroy(allWeapons[i]);
                    explosion = Instantiate(Resources.Load("Prefabs/explosion"), allWeapons[i].transform.position, allWeapons[i].transform.rotation) as GameObject;
                    Destroy(explosion.gameObject, 1);
                }

                if(destroyerBehavior.IsPlayerOne())
                {
                    score.DestroyedEnemy(damageDealt, 0, totalHealth);
                }
                else
                {
                    score.DestroyedEnemy(damageDealt, 1, totalHealth);
                }
    }
}
