using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterimage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Lifetime(0.2f));
    }

    IEnumerator Lifetime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
