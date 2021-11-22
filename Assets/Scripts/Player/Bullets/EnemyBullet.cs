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
        speed = new Vector3(0, 5.0f, 0);
        speed = speed * Time.fixedDeltaTime;

        cam = Camera.main;
        camBounds = cam.GetComponent<CameraBounds>();
        bound = camBounds.bounds;
        
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        if(transform.position.y < bound.min.y || transform.position.x < bound.min.x || transform.position.x > bound.max.x)
        {
            Destroy(gameObject);
        }

        transform.Translate(speed);
    }
}
