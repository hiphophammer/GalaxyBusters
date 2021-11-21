using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestructionZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, .5f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("EnemyProjectile"))
        {
            Destroy(col.gameObject);
        }
    }
}
