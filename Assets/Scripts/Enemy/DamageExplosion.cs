using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageExplosion : MonoBehaviour
{
    ParticleSystem p;
    List<ParticleCollisionEvent> colEvents = new List<ParticleCollisionEvent>();
    bool hit;
    // Start is called before the first frame update
    void Start()
    {
        p = GetComponent<ParticleSystem>();
        Destroy(gameObject, 1);
        hit = true;
    }

    void OnParticleCollision(GameObject other)
    {
        int events = p.GetCollisionEvents(other, colEvents);
        for (int i = 0; i < events; i++)
        {   
            if (hit)
            {
                Debug.Log("Health Decreased");
                hit = false;
            } 
        }
    }
}
