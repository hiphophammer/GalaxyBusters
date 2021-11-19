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

    private void Start()
    {
        Debug.Assert(singlePlayerMenu != null);
        Debug.Assert(cooperativePlayMenu != null);

        player1Ship = null;
        player2Ship = null;

        singlePlayerMenu.SetActive(false);
        cooperativePlayMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
