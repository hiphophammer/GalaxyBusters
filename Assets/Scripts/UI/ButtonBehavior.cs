using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    public Dropdown menu;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        if(menu.value == 0)
        {
            Debug.Log("Fighter");
        }   
        if(menu.value == 1)
        {
            Debug.Log("Chaser");
        }   
        if(menu.value == 2)
        {
            Debug.Log("UFO");
        }   
    }


    
    
}
