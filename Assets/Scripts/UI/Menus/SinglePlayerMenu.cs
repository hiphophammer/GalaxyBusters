using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SinglePlayerMenu : MonoBehaviour
{
    public TMPro.TMP_Dropdown player1Dropdown;

    private bool sceneLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(player1Dropdown != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StoreShipType()
    {
        Debug.Log("Menu: Storing ship type");
        MainMenu.player1Ship = player1Dropdown.options[player1Dropdown.value].text;
        Debug.Log("Menu: Ship is: " + MainMenu.player1Ship);
    }

    public void StartGame()
    {
        if (!sceneLoaded)
        {
            StoreShipType();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            sceneLoaded = true;
        }
    }
}
