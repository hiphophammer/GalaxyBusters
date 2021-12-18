using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 speed;

    CameraSupport cameraSupport;

    private float maxXPos = (20.0f / 3.0f) / 2.0f;
    private float maxYPos = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        maxXPos -= (transform.localScale.x / 2.0f);
        maxYPos -= (transform.localScale.y / 2.0f);

        speed = new Vector3(0, 5.0f, 0);
        speed = speed * Time.fixedDeltaTime;

        cameraSupport = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(cameraSupport != null);
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        
        if (transform.position.y <= (-1.0f * (maxYPos + .25f)))
        {
            Destroy(gameObject);
        }
        else if (transform.position.x <= (-1.0f * maxXPos) - .5f || transform.position.x >= (maxXPos + .5f))
        {
            Destroy(gameObject);
        }

        transform.Translate(speed);
    }
}
