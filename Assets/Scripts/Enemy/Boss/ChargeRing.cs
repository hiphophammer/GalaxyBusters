using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeRing : MonoBehaviour
{
    public float targetSize = 0.1f;
    public float speed = 4.0f;
     
    private Vector3 targetScale;
    
    // Start is called before the first frame update
    void Start()
    {
        targetScale = transform.localScale * targetSize;
        StartCoroutine(Lifetime(4f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp (transform.localScale, targetScale, speed * Time.deltaTime);
    }

    IEnumerator Lifetime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this);
    }
}
