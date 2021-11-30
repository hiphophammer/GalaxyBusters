using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Lifetime(5f));
    }

    IEnumerator Lifetime(float time)
    {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
        Destroy(this);
    }
}
