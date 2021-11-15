using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject singlePlayerMenu;
    public GameObject cooperativePlayMenu;

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
        Debug.Assert(singlePlayerMenu != null);
        Debug.Assert(cooperativePlayMenu != null);

        singlePlayerMenu.SetActive(false);
        cooperativePlayMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
