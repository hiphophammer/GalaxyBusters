using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBullet : MonoBehaviour
{
    private Vector3 speed;
    Camera cam;
    CameraBounds camBounds;
    Bounds bound;
    // Start is called before the first frame update
    void Start()
    {
        speed = new Vector3(0, 60f, 0);
        speed = speed * Time.fixedDeltaTime;

        cam = Camera.main;
        camBounds = cam.GetComponent<CameraBounds>();
        bound = camBounds.bounds;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if(pos.x > bound.max.x || pos.x < bound.min.x)
        {
            Destroy(gameObject);
        }
        if(pos.y > bound.max.y|| pos.y < bound.min.y)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        transform.Translate(speed);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            EnemyHealth e = col.gameObject.GetComponent<EnemyHealth>();
            e.decreaseHealth();
            Destroy(gameObject);
        }
    }
    
}
