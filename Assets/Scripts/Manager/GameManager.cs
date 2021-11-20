using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Vector3 player1StartPos = new Vector3(-1.5f, -2.5f, 0.0f);
    private Vector3 player2StartPos = new Vector3(1.5f, -2.5f, 0.0f);

    private ScoreManager scoreManager;
    private PlayerBehavior player1;
    private PlayerBehavior player2;

    public GameObject levelAttach;
    
    public TMPro.TextMeshProUGUI playerScore;
    public TMPro.TextMeshProUGUI playerStatus;
    public TMPro.TextMeshProUGUI levelNum;
    public TMPro.TextMeshProUGUI levelName;

    public Sprite player1LancerSprite;
    public Sprite player1VanguardSprite;
    public Sprite player1TrailblazerSprite;

    public Sprite player2LancerSprite;
    public Sprite player2VanguardSprite;
    public Sprite player2TrailblazerSprite;

    public string ship1;
    public string ship2;

    public static bool winLoss;

    // DEBUG TODO: Remove
    private InventoryBehavior inventory;
    bool testsDone;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager: Waking up!");
        scoreManager = Camera.main.GetComponent<ScoreManager>();
        Debug.Assert(scoreManager != null);

        Debug.Assert(playerScore != null);
        Debug.Assert(playerStatus != null);

        ship1 = MainMenu.player1Ship;

        // DEBUG TODO: Remove
        ship1 = "Lancer";

        ship2 = MainMenu.player2Ship;

        Debug.Log("Player 1 ship: " + ship1);
        Debug.Log("Player 2 ship: " + ship2);
        
        Debug.Assert(ship2 == null);
        BuildPlayers();
        StartCoroutine(StartGame());

        // DEBUG TODO: Remove
        inventory = GameObject.FindGameObjectWithTag("Player1Inventory").GetComponent<InventoryBehavior>();
        inventory.SetPlayer(player1);

        testsDone = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatus();
        DetectCondition();

        //if (testsDone)
        //{
        //    // Create some common items.
        //    Item common1 = new Item();
        //    common1.type = Item.ItemType.common;
        //    common1.dHP = 5.0f;

        //    Item common2 = new Item();
        //    common2.type = Item.ItemType.common;
        //    common2.isPowerUp = true;
        //    common2.dSpeed = 5.0f;

        //    bool retVal = inventory.AddItem(common1);  // success
        //    retVal = inventory.AddItem(common2);  // success, and should be removed with ClearPowerUps()

        //    // Create some rare items.
        //    Item rare1 = new Item();
        //    rare1.type = Item.ItemType.rare;
        //    rare1.dSpeed = 7.0f;
        //    rare1.dDamage = -7.0f;

        //    Item rare2 = new Item();
        //    rare2.type = Item.ItemType.rare;
        //    rare2.isPowerUp = true;
        //    rare2.dHP = 7.0f;
        //    rare2.dSpeed = -7.0f;

        //    retVal = inventory.AddItem(rare1);    // success
        //    retVal = inventory.AddItem(rare2);    // success, and should be removed with ClearPowerUps()
        //                                          // we should also see the number of hit points increase by 7.

        //    // Create some epic items.
        //    Item epic1 = new Item();
        //    epic1.type = Item.ItemType.epic;
        //    epic1.ID = 1;   // swap bullet
        //    epic1.bulletName = "piercing";

        //    Item epic2 = new Item();
        //    epic2.type = Item.ItemType.epic;
        //    epic2.ID = 2;   // dual stream

        //    Item epic3 = new Item();
        //    epic3.type = Item.ItemType.epic;
        //    epic3.ID = 3;   // shield

        //    //retVal = inventory.AddItem(epic1);    // success
        //    retVal = inventory.AddItem(epic2);    // success
        //    retVal = inventory.AddItem(epic3);    // this should fail as we already have 2.

        //    // Create our special items.
        //    // first, try adding the one of a different name.
        //    // then, try adding the correct one.
        //    // then try adding the other correct one (should fail).
        //    // finally, try adding the incorrect one again (should fail).
        //    Item special1 = new Item();
        //    special1.type = Item.ItemType.special;
        //    special1.ship = "Lancer";

        //    Item special2 = new Item();
        //    special2.type = Item.ItemType.special;
        //    special2.ship = "Lancer";

        //    Item special3 = new Item();
        //    special3.type = Item.ItemType.special;
        //    special3.ship = "Vanguard";

        //    retVal = inventory.AddItem(special3);     // fail
        //    retVal = inventory.AddItem(special1);     // success
        //    retVal = inventory.AddItem(special2);     // fail
        //    retVal = inventory.AddItem(special3);     // fail

        //    // Clear the powerups.
        //    inventory.ClearPowerUps();

        //    testsDone = false;
        //}
    }

    public PlayerBehavior GetPlayer1()
    {
        return player1;
    }

    private void BuildPlayers()
    {
        Quaternion rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        if((ship1 != null && ship1 != "None") && (ship2 == null || ship2 == "None")){
            Debug.Log("GameManager: Singleplayer detected");
            player1StartPos = new Vector3(0.0f, -2.5f, 0.0f);
        }

        for (int i = 0; i < 2; i++)
        {
            bool playerOne = i == 0;
            string ship = playerOne ? ship1 : ship2;

            if (ship != null && ship != "None")
            {
                // Instantiate the game object.
                Vector3 startPos = playerOne ? player1StartPos : player2StartPos;
                GameObject player = Instantiate(Resources.Load("Prefabs/Player") as GameObject,
                                                    startPos,
                                                    rotation);
                player.tag = "Player";

                // Set up the behavior component.
                PlayerBehavior playerBehavior = player.GetComponent<PlayerBehavior>();
                playerBehavior.playerOne = playerOne;
                playerBehavior.SetShipName(ship);

                if (playerOne)
                {
                    player1 = playerBehavior;
                }
                else
                {
                    player2 = playerBehavior;
                }

                // Set the sprite and add appropriate components.
                SpriteRenderer renderer = player.GetComponent<SpriteRenderer>();
                if (ship == "Lancer")
                {
                    renderer.sprite = playerOne ? player1LancerSprite : player2LancerSprite;

                    // Set stats.
                    playerBehavior.GetHealthBar().SetHitPoints(50.0f);
                    playerBehavior.SetWeaponDamage(25.0f);
                    playerBehavior.SetSpeed(10.0f);

                    // Add appropriate components.
                    player.AddComponent<BaseMovement>();
                    player.GetComponent<BaseMovement>().SetParent(playerBehavior);

                    player.AddComponent<BaseCollider>();
                    player.GetComponent<BaseCollider>().SetParent(playerBehavior);

                    player.AddComponent<BaseWeapon>();
                    player.GetComponent<BaseWeapon>().SetParent(playerBehavior);

                    player.AddComponent<LancerBasicAbility>();
                    player.GetComponent<LancerBasicAbility>().SetParent(playerBehavior);

                    player.AddComponent<LancerUltimateAbility>();
                    player.GetComponent<LancerUltimateAbility>().SetParent(playerBehavior);
                }
                else if (ship == "Vanguard")
                {
                    renderer.sprite = playerOne ? player1VanguardSprite : player2VanguardSprite;

                    playerBehavior.GetHealthBar().SetHitPoints(100.0f);
                    playerBehavior.SetWeaponDamage(15.0f);
                    playerBehavior.SetSpeed(3.0f);
                }
                else if (ship == "Trailblazer")
                {
                    // Otherwise, given the string is not null, it must be the trailblazer.
                    renderer.sprite = playerOne ? player1TrailblazerSprite : player2TrailblazerSprite;

                    playerBehavior.GetHealthBar().SetHitPoints(75.0f);
                    playerBehavior.SetWeaponDamage(5.0f);
                    playerBehavior.SetSpeed(5.0f);
                }
            }
        }
    }

    private void UpdateStatus()
    {
        playerScore.text = scoreManager.GetStatus();
        playerStatus.text = player1.GetStatus();
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("GameManager: Disabling text now");
        levelNum.enabled = false;
        levelName.enabled = false;

        // Level 1
        Debug.Log("GameManager: Running Level 1");
        levelAttach.AddComponent<LevelOne>();
        float time = levelAttach.GetComponent<LevelOne>().levelTime;
        Debug.Log("Waiting for " + time + " seconds");
        yield return new WaitForSeconds(180f);

        // Victory!
        if(player1.IsAlive() || player2.IsAlive())
        {
            winLoss = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void DetectCondition()
    {
        if(!player1.IsAlive())
        {
            Debug.Log("PLAYER DIED");
            winLoss = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
