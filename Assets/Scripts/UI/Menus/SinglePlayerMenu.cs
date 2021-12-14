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
    public GameObject PlayerSelectionArrowLeft;
    public GameObject PlayerSelectionArrowRight;

    private bool sceneLoaded = false;
    private int shipIndex = 0;
    private GameObject shipObject; // game object that shipImage is attached to
    private Sprite lancerSprite;
    private Sprite vanguardSprite;
    private Sprite trailblazerSprite;

    private float initialPos_bL;
    private float initialPos_bR;
    private const float vanguardOffset = 18.0f;
    private const float trailblazerOffset = 40.0f;


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
        initialPos_bL = PlayerSelectionArrowLeft.GetComponent<RectTransform>().localPosition.x;
        initialPos_bR = PlayerSelectionArrowRight.GetComponent<RectTransform>().localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        // check for any input
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            RightArrow();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            LeftArrow();
        }

        // update screen
        if (shipIndex == 0) // lancer
        {
            shipObject.GetComponent<Image>().sprite = lancerSprite;
            shipName.SetText("Lancer");
            hpBar.fillAmount = 0.33f;
            speedBar.fillAmount = 0.66f;
            damageBar.fillAmount = 1.0f;
            basicDescription.SetText("Launches a missile, dealing a small AOE damage.");
            ultDescription.SetText("Sprays bullets in cone");
            PlayerSelectionArrowLeft.GetComponent<RectTransform>().localPosition = new Vector2(initialPos_bL, PlayerSelectionArrowLeft.GetComponent<RectTransform>().localPosition.y);
            PlayerSelectionArrowRight.GetComponent<RectTransform>().localPosition = new Vector2(initialPos_bR, PlayerSelectionArrowRight.GetComponent<RectTransform>().localPosition.y);
        }
        else if (shipIndex == 1) // vanguard
        {
            shipObject.GetComponent<Image>().sprite = vanguardSprite;
            shipName.SetText("Vanguard");
            hpBar.fillAmount = 1.0f;
            speedBar.fillAmount = 0.33f;
            damageBar.fillAmount = 0.66f;
            basicDescription.SetText("Ram. Reduces damage taken.");
            ultDescription.SetText("Shield. Absorbs damage taken.");
            PlayerSelectionArrowLeft.GetComponent<RectTransform>().localPosition = new Vector2(initialPos_bL - vanguardOffset, PlayerSelectionArrowLeft.GetComponent<RectTransform>().localPosition.y);
            PlayerSelectionArrowRight.GetComponent<RectTransform>().localPosition = new Vector2(initialPos_bR + vanguardOffset, PlayerSelectionArrowRight.GetComponent<RectTransform>().localPosition.y);
        }
        else // trailblazer
        {
            shipObject.GetComponent<Image>().sprite = trailblazerSprite;
            shipName.SetText("Trailblazer");
            hpBar.fillAmount = 0.66f;
            speedBar.fillAmount = 1.0f;
            damageBar.fillAmount = 0.33f;
            basicDescription.SetText("Blinks in the direction of movement.");
            ultDescription.SetText("Becomes a ghost for a short period of time.");
            PlayerSelectionArrowLeft.GetComponent<RectTransform>().localPosition = new Vector2(initialPos_bL - trailblazerOffset, PlayerSelectionArrowLeft.GetComponent<RectTransform>().localPosition.y);
            PlayerSelectionArrowRight.GetComponent<RectTransform>().localPosition = new Vector2(initialPos_bR + trailblazerOffset, PlayerSelectionArrowRight.GetComponent<RectTransform>().localPosition.y);
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
    public void RightArrow()
    {
        if (shipIndex == 0)
        {
            shipIndex++;
        }
        else if (shipIndex == 1)
        {
            shipIndex++;
        }
        else
        {
            shipIndex = 0;
        }
    }
    public void LeftArrow()
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
