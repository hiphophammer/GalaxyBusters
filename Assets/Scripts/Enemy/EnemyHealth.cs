using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    int health;
    int bulletDamage;
    GameObject Explosion;
    GameObject powerUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health == 0)
        {
            Destroy(gameObject);
            Explosion = Instantiate(Resources.Load("Prefabs/Explosion"), transform.position, transform.rotation) as GameObject;
            Destroy(Explosion.gameObject, 1);
            
            //20% to spawn powerup
            int chance = Random.Range(1, 6);
            if(chance == 1)
            {
                powerUp = Resources.Load<GameObject>("Prefabs/PowerUp") as GameObject;
                powerUp.transform.position = transform.position;
                powerUp.transform.rotation = transform.rotation;
            }
            
        }
    }

    public void setHealth(int h, int lvl)
    {
        health = h * lvl;
    }

    public void decreaseHealth()
    {
        health--;
    }
}
