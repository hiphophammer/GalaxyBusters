using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Public member variables.
    private PlayerBehavior player1;
    private PlayerBehavior player2;

    public GameObject levelAttach;

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

    public InventoryBehavior player1Inventory;
    public InventoryBehavior player2Inventory;

    public static bool winLoss;

    // Private member variables.
    private Vector3 player1StartPos = new Vector3(-1.5f, -2.5f, 0.0f);
    private Vector3 player2StartPos = new Vector3(1.5f, -2.5f, 0.0f);

    private bool ready;
    private bool singlePlayer;


    // Start is called before the first frame update
    void Start()
    {
        // Perform some checks.
        Debug.Log("GameManager: Waking up!");

        // Determine which players to build.
        ship1 = MainMenu.player1Ship;
        ship2 = MainMenu.player2Ship;

        Debug.Log("Player 1 ship: " + ship1);
        Debug.Log("Player 2 ship: " + ship2);

        // Make sure we have valid references to our inventories.
        Debug.Assert(player1Inventory != null);
        Debug.Assert(player2Inventory != null);

        // Build the players.
        BuildPlayers();
        StartCoroutine(StartGame());

        // Set up the inventories.
        player1Inventory.SetPlayer(player1);
        if (player2 != null)
        {
            player2Inventory.SetPlayer(player2);
        }

        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        DetectCondition();
    }

    public bool Ready()
    {
        return ready;
    }

    public bool SinglePlayer()
    {
        return singlePlayer;
    }

    public PlayerBehavior GetPlayer1()
    {
        return player1;
    }

    public PlayerBehavior GetPlayer2()
    {
        return player2;
    }

    private void BuildPlayers()
    {
        Quaternion rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        if ((ship1 != null && ship1 != "None") && (ship2 == null || ship2 == "None"))
        {
            Debug.Log("GameManager: Singleplayer detected");
            player1StartPos = new Vector3(0.0f, -2.5f, 0.0f);
            player2Inventory.Hide();
            singlePlayer = true;
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
                playerBehavior.SetInventory(playerOne ? player1Inventory : player2Inventory);

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

                    // Add appropriate components
                    player.AddComponent<TrailblazerMovement>();
                    player.GetComponent<TrailblazerMovement>().SetParent(playerBehavior);

                    player.AddComponent<BaseCollider>();
                    player.GetComponent<BaseCollider>().SetParent(playerBehavior);

                    player.AddComponent<BaseWeapon>();
                    player.GetComponent<BaseWeapon>().SetParent(playerBehavior);

                    player.AddComponent<TrailblazerUltimateAbility>();
                    player.GetComponent<TrailblazerUltimateAbility>().SetParent(playerBehavior);
                }
            }
        }
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
        yield return new WaitForSeconds(180.0f);

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
