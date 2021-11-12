using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static string player1Ship;
    public static string player2Ship;

    public TMPro.TMP_Dropdown player1Dropdown;
    public TMPro.TMP_Dropdown player2Dropdown;

    private bool sceneLoaded = false;

    private void Start()
    {
        Debug.Assert(player1Dropdown != null);
        Debug.Assert(player2Dropdown != null);

        player1Ship = null;
        player2Ship = null;
    }

    public void StartGame()
    {
        if (!sceneLoaded)
        {
            GetShipTypes();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            sceneLoaded = true;
        }
    }

    private void GetShipTypes()
    {
        player1Ship = player1Dropdown.options[player1Dropdown.value].text;
        player2Ship = player2Dropdown.options[player2Dropdown.value].text;
    }
}
