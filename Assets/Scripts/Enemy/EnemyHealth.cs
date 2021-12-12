using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    int totalHealth;
    float[] damageDealt;
    
    GameObject Explosion;
    GameObject powerUp;

    PlayerBehavior destroyerBehavior;

    Camera cam;
    ScoreManager score;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        score = cam.GetComponent<ScoreManager>();
        damageDealt = new float[2];
        damageDealt[0] = 0;
        damageDealt[1] = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            
            destroyerBehavior.DestroyedEnemy(totalHealth);
            Destroy(gameObject);
            Explosion = Instantiate(Resources.Load("Prefabs/Explosion"), transform.position, transform.rotation) as GameObject;
            Destroy(Explosion.gameObject, 1);
            
            //10% to spawn powerup
            int chance = Random.Range(1, 11);
            int powerUpChoice = Random.Range(1, 11);
            //Debug.Log("Power Up Spawn = " + chance + "Power Up Choice = " + powerUpChoice);
            if(chance == 1)
            {
                powerUp = Instantiate(Resources.Load<GameObject>("Prefabs/PowerUp")) as GameObject;
                powerUp.transform.position = transform.position;
                PowerUpBehavior powerBehavior = powerUp.GetComponent<PowerUpBehavior>();
                if (powerUpChoice <= 7 )
                {
                    powerBehavior.item = Resources.Load<Item>("PowerUps/Health Pack") as Item;
                }
                else 
                {
                    powerBehavior.item = Resources.Load<Item>("PowerUps/Ultimate Charge") as Item;
                }
            }

            if(destroyerBehavior.IsPlayerOne())
            {
                score.DestroyedEnemy(damageDealt, 0, totalHealth * destroyerBehavior.comboMult);
                if(destroyerBehavior.comboMult < 4f)
                {
                    destroyerBehavior.comboMult += totalHealth * .01f;
                }
                else if(destroyerBehavior.comboMult >= 4f)
                {
                    destroyerBehavior.comboMult = 4f;
                }
            }
            else
            {
                score.DestroyedEnemy(damageDealt, 1, totalHealth * destroyerBehavior.comboMult);
                if((destroyerBehavior.comboMult + totalHealth * .01f) < 4f)
                {
                    destroyerBehavior.comboMult += totalHealth * .01f;
                }
                else if((destroyerBehavior.comboMult + totalHealth * .01f) >= 4f)
                {
                    destroyerBehavior.comboMult = 4f;
                }
            }
            
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void setHealth(int h)
    {
        health = h;
        totalHealth = health;
    }

    public void decreaseHealth(PlayerBehavior damageDealer)
    {
        if(damageDealer.IsPlayerOne())
        {
            Debug.Log("Player 1 did damage.");
            damageDealt[0] = damageDealt[0] + (int)damageDealer.GetWeaponDamage();
            destroyerBehavior = damageDealer;
            health = health - (int)damageDealer.GetWeaponDamage();
        }
        else
        {
            damageDealt[1] = damageDealt[1] + (int)damageDealer.GetWeaponDamage();
            destroyerBehavior = damageDealer;
            health = health - (int)damageDealer.GetWeaponDamage();
        }
    }

    // instantDeath is used by Vanguard's ram to instantly kill any enemy the player touches. It could also be used for babby mode.
    public void instantDeath(PlayerBehavior damageDealer)
    {
        if(damageDealer.IsPlayerOne())
        {
            Debug.Log("Player 1 did damage.");
            damageDealt[0] = damageDealt[0] + totalHealth;
            destroyerBehavior = damageDealer;
            health = health - totalHealth;
        }
        else
        {
            damageDealt[1] = damageDealt[1] + totalHealth;
            destroyerBehavior = damageDealer;
            health = health - totalHealth;
        }
    }

    // missileImpact calculates any damage that occurs as a result of Lancer's missile.
    public void missileImpact(float damage, PlayerBehavior damageDealer)
    {
        if(damageDealer.IsPlayerOne())
        {
            Debug.Log("Player 1 did damage.");
            damageDealt[0] = damageDealt[0] + (int)damage;
            destroyerBehavior = damageDealer;
            health = health - (int)damage;
        }
        else
        {
            damageDealt[1] = damageDealt[1] + (int)damage;
            destroyerBehavior = damageDealer;
            health = health - (int)damage;
        }
    }
}
