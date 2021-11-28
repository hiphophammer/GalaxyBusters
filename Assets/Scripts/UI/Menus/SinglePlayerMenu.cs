using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SinglePlayerMenu : MonoBehaviour
{
    // public TMPro.TMP_Dropdown player1Dropdown;
    public Image shipImage;
    public Image hpBar, speedBar, damageBar;
    public TMPro.TMP_Text basicDescription, ultDescription;
    public TMPro.TMP_Text shipName;

    private bool sceneLoaded = false;
    private int shipIndex = 0;
    private GameObject shipObject; // game object that shipImage is attached to
    private Sprite lancerSprite;
    private Sprite vanguardSprite;
    private Sprite trailblazerSprite;


    // Start is called before the first frame update
    void Start()
    {
        // Debug.Assert(player1Dropdown != null);
        LoadComps();
    }

    // Loads components: sprites, game objects...
    private void LoadComps()
    {
        shipObject = shipImage.gameObject;
        lancerSprite = Resources.Load<Sprite>("Textures/Ships/Player 1/Lancer");
        vanguardSprite = Resources.Load<Sprite>("Textures/Ships/Player 1/Vanguard");
        trailblazerSprite = Resources.Load<Sprite>("Textures/Ships/Player 1/Trailblazer");
    }

    // Update is called once per frame
    void Update()
    {
        // check for any input
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if(shipIndex == 0)
            {
                shipIndex++;
            }
            else if(shipIndex == 1)
            {
                shipIndex++;
            }
            else
            {
                shipIndex = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (shipIndex == 2)
            {
                shipIndex--;
            }
            else if (shipIndex == 1)
            {
                shipIndex--;
            }
            else
            {
                shipIndex = 2;
            }
        }

        // update screen
        if (shipIndex == 0) // lancer
        {
            shipObject.GetComponent<Image>().sprite = lancerSprite;
            shipName.SetText("< Lancer >");
            hpBar.fillAmount = 0.33f;
            speedBar.fillAmount = 0.66f;
            damageBar.fillAmount = 1.0f;
            basicDescription.SetText("Launches a missile, dealing a small AOE damage.");
            ultDescription.SetText("Sprays bullets in cone");
        }
        else if (shipIndex == 1) // vanguard
        {
            shipObject.GetComponent<Image>().sprite = vanguardSprite;
            shipName.SetText("< Vanguard >");
            hpBar.fillAmount = 1.0f;
            speedBar.fillAmount = 0.33f;
            damageBar.fillAmount = 0.66f;
            basicDescription.SetText("Gains small shield.");
            ultDescription.SetText("Takes reduced damage for a duration");
        }
        else // trailblazer
        {
            shipObject.GetComponent<Image>().sprite = trailblazerSprite;
            shipName.SetText("< Trailblazer >");
            hpBar.fillAmount = 0.66f;
            speedBar.fillAmount = 1.0f;
            damageBar.fillAmount = 0.33f;
            basicDescription.SetText("Blinks in the dirction of the arrow key.");
            ultDescription.SetText("Becomes a ghost for a short period of time.");
        }

        if (Input.GetKeyDown(KeyCode.Return))
            StartGame();
    }

/*    private void StoreShipType()
    {
        Debug.Log("Menu: Storing ship type");
        MainMenu.player1Ship = player1Dropdown.options[player1Dropdown.value].text;
        Debug.Log("Menu: Ship is: " + MainMenu.player1Ship);
    }*/

    public void StartGame()
    {
        if (!sceneLoaded)
        {
            if (shipIndex == 0)
                MainMenu.player1Ship = "Lancer";
            else if (shipIndex == 1)
                MainMenu.player1Ship = "Vanguard";
            else
                MainMenu.player1Ship = "Trailblazer";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            sceneLoaded = true;
        }
    }
}
