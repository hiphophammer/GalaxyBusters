using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CooperativePlayMenu : MonoBehaviour
{
    // public TMPro.TMP_Dropdown player1Dropdown;
    // public TMPro.TMP_Dropdown player2Dropdown;

    // variable names One represents Player 1, Two represents variables for P2

    public Image shipImageOne;
    public Image hpBarOne, speedBarOne, damageBarOne;
    public TMPro.TMP_Text basicDescriptionOne, ultDescriptionOne;
    public TMPro.TMP_Text shipNameOne;

    public Image shipImageTwo;
    public Image hpBarTwo, speedBarTwo, damageBarTwo;
    public TMPro.TMP_Text basicDescriptionTwo, ultDescriptionTwo;
    public TMPro.TMP_Text shipNameTwo;

    private int shipIndexOne;
    private int shipIndexTwo;
    private GameObject shipObjectOne;
    private GameObject shipObjectTwo;
    private Sprite lancerSpriteOne, lancerSpriteTwo;
    private Sprite vanguardSpriteOne, vanguardSpriteTwo;
    private Sprite trailblazerSpriteOne, trailblazerSpriteTwo;

    private bool sceneLoaded;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Assert(player1Dropdown != null);
        // Debug.Assert(player2Dropdown != null);
        LoadComps();
    }

    private void LoadComps()
    {
        shipIndexOne = 0;
        shipIndexTwo = 0;
        shipObjectOne = shipImageOne.gameObject;
        shipObjectTwo = shipImageTwo.gameObject;
        lancerSpriteOne = Resources.Load<Sprite>("Textures/Ships/Player 1/Lancer");
        vanguardSpriteOne = Resources.Load<Sprite>("Textures/Ships/Player 1/Vanguard");
        trailblazerSpriteOne = Resources.Load<Sprite>("Textures/Ships/Player 1/Trailblazer");
        lancerSpriteTwo = Resources.Load<Sprite>("Textures/Ships/Player 2/Lancer");
        vanguardSpriteTwo = Resources.Load<Sprite>("Textures/Ships/Player 2/Vanguard");
        trailblazerSpriteTwo = Resources.Load<Sprite>("Textures/Ships/Player 2/Trailblazer");
    }

    // Update is called once per frame
    void Update()
    {
        // get key inputs
        // for p1
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (shipIndexOne == 0)
            {
                shipIndexOne++;
            }
            else if (shipIndexOne == 1)
            {
                shipIndexOne++;
            }
            else
            {
                shipIndexOne = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (shipIndexOne == 2)
            {
                shipIndexOne--;
            }
            else if (shipIndexOne == 1)
            {
                shipIndexOne--;
            }
            else
            {
                shipIndexOne = 2;
            }
        }
        // for p2
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (shipIndexTwo == 0)
            {
                shipIndexTwo++;
            }
            else if (shipIndexTwo == 1)
            {
                shipIndexTwo++;
            }
            else
            {
                shipIndexTwo = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (shipIndexTwo == 2)
            {
                shipIndexTwo--;
            }
            else if (shipIndexTwo == 1)
            {
                shipIndexTwo--;
            }
            else
            {
                shipIndexTwo = 2;
            }
        }
        // update each section accordingly
        // for p1
        if (shipIndexOne == 0) // lancer
        {
            shipObjectOne.GetComponent<Image>().sprite = lancerSpriteOne;
            shipNameOne.SetText("< Lancer >");
            hpBarOne.fillAmount = 0.33f;
            speedBarOne.fillAmount = 0.66f;
            damageBarOne.fillAmount = 1.0f;
            basicDescriptionOne.SetText("Launches a missile, dealing a small amount of AOE damage.");
            ultDescriptionOne.SetText("Sprays bullets in a conical pattern.");
        }
        else if (shipIndexOne == 1) // vanguard
        {
            shipObjectOne.GetComponent<Image>().sprite = vanguardSpriteOne;
            shipNameOne.SetText("< Vanguard >");
            hpBarOne.fillAmount = 1.0f;
            speedBarOne.fillAmount = 0.33f;
            damageBarOne.fillAmount = 0.66f;
            basicDescriptionOne.SetText("For a short time, take less damage and destroy any enemy on contact.");
            ultDescriptionOne.SetText("Shield. Gains health for each bullet absorbed.");
        }
        else // trailblazer
        {
            shipObjectOne.GetComponent<Image>().sprite = trailblazerSpriteOne;
            shipNameOne.SetText("< Trailblazer >");
            hpBarOne.fillAmount = 0.66f;
            speedBarOne.fillAmount = 1.0f;
            damageBarOne.fillAmount = 0.33f;
            basicDescriptionOne.SetText("Blinks in the direction of movement.");
            ultDescriptionOne.SetText("Becomes a ghost for a short period of time.");
        }
        // for p2
        if (shipIndexTwo == 0) // lancer
        {
            shipObjectTwo.GetComponent<Image>().sprite = lancerSpriteTwo;
            shipNameTwo.SetText("< Lancer >");
            hpBarTwo.fillAmount = 0.33f;
            speedBarTwo.fillAmount = 0.66f;
            damageBarTwo.fillAmount = 1.0f;
            basicDescriptionTwo.SetText("Launches a missile, dealing a small amount of AOE damage.");
            ultDescriptionTwo.SetText("Sprays bullets in a conical pattern.");
        }
        else if (shipIndexTwo == 1) // vanguard
        {
            shipObjectTwo.GetComponent<Image>().sprite = vanguardSpriteTwo;
            shipNameTwo.SetText("< Vanguard >");
            hpBarTwo.fillAmount = 1.0f;
            speedBarTwo.fillAmount = 0.33f;
            damageBarTwo.fillAmount = 0.66f;
            basicDescriptionTwo.SetText("For a short time, take less damage and destroy any enemy on contact.");
            ultDescriptionTwo.SetText("Shield. Gains health for each bullet absorbed.");
        }
        else // trailblazer
        {
            shipObjectTwo.GetComponent<Image>().sprite = trailblazerSpriteTwo;
            shipNameTwo.SetText("< Trailblazer >");
            hpBarTwo.fillAmount = 0.66f;
            speedBarTwo.fillAmount = 1.0f;
            damageBarTwo.fillAmount = 0.33f;
            basicDescriptionTwo.SetText("Blinks in the direction of movement.");
            ultDescriptionTwo.SetText("Becomes a ghost for a short period of time.");
        }

        if (Input.GetKeyDown(KeyCode.Return))
            StartGame();
    }
/*    private void StoreShipTypes()
    {
        MainMenu.player1Ship = player1Dropdown.options[player1Dropdown.value].text;
        MainMenu.player2Ship = player2Dropdown.options[player2Dropdown.value].text;
    }*/

    public void StartGame()
    {
        if (!sceneLoaded)
        {
            // StoreShipTypes();
            // set player ship strings accordingly
            if (shipIndexOne == 0)
                MainMenu.player1Ship = "Lancer";
            else if (shipIndexOne == 1)
                MainMenu.player1Ship = "Vanguard";
            else
                MainMenu.player1Ship = "Trailblazer";

            if (shipIndexTwo == 0)
                MainMenu.player2Ship = "Lancer";
            else if (shipIndexTwo == 1)
                MainMenu.player2Ship = "Vanguard";
            else
                MainMenu.player2Ship = "Trailblazer";

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            sceneLoaded = true;
        }
    }
}
