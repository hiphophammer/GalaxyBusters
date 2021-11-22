using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CooperativePlayMenu : MonoBehaviour
{
    public TMPro.TMP_Dropdown player1Dropdown;
    public TMPro.TMP_Dropdown player2Dropdown;

    private bool sceneLoaded;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(player1Dropdown != null);
        Debug.Assert(player2Dropdown != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void StoreShipTypes()
    {
        MainMenu.player1Ship = player1Dropdown.options[player1Dropdown.value].text;
        MainMenu.player2Ship = player2Dropdown.options[player2Dropdown.value].text;
    }

    public void StartGame()
    {
        if (!sceneLoaded)
        {
            StoreShipTypes();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            sceneLoaded = true;
        }
    }
}
