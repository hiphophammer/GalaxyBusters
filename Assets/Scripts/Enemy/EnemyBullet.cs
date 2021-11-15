using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 speed;
    Camera cam;
    CameraBounds camBounds;
    Bounds bound;
    // Start is called before the first frame update
    void Start()
    {
        speed = new Vector3(0, 50f, 0);
        speed = speed * Time.fixedDeltaTime;

        cam = Camera.main;
        camBounds = cam.GetComponent<CameraBounds>();
        bound = camBounds.bounds;
        
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        if(pos.x > bound.max.x|| pos.x < bound.min.x)
        {
            Destroy(gameObject);
        }
        if(pos.y > bound.max.y || pos.y < bound.min.y)
        {
            Destroy(gameObject);
        }

        transform.Translate(speed);
    }
}
