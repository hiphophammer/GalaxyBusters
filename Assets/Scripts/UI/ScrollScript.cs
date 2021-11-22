using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    public float speed;
    public bool scroll;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            scroll = !scroll;
        }

        if (scroll)
        {
            GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.0f, Time.time * speed);
        }
    }
}
