using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager: Waking up!");
        scoreManager = Camera.main.GetComponent<ScoreManager>();
        Debug.Assert(scoreManager != null);

        Debug.Assert(playerScore != null);
        Debug.Assert(playerStatus != null);

        ship1 = MainMenu.player1Ship;
        ship2 = MainMenu.player2Ship;

        Debug.Log("Player 1 ship: " + ship1);
        Debug.Log("Player 2 ship: " + ship2);
        
        Debug.Assert(ship2 == null);
        BuildPlayers();
        StartCoroutine(StartLevels());

        // Put in some power-ups.
        //GameObject item = Instantiate(Resources.Load("Prefabs/ItemDisplay") as GameObject,
        //                        new Vector3(125.0f, 560.0f, 0.0f),
        //                        Quaternion.identity,
        //                        GameObject.FindGameObjectWithTag("GameCanvas").transform);
        //item.GetComponent<ItemDisplayBehavior>().powerUpMode = true;
        //item.GetComponent<ItemDisplayBehavior>().item = Resources.Load("items/CommonTest") as Item;

        //item = Instantiate(Resources.Load("Prefabs/ItemDisplay") as GameObject,
        //                        new Vector3(272.0f, 560.0f, 0.0f),
        //                        Quaternion.identity,
        //                        GameObject.FindGameObjectWithTag("GameCanvas").transform);
        //item.GetComponent<ItemDisplayBehavior>().powerUpMode = true;
        //item.GetComponent<ItemDisplayBehavior>().item = Resources.Load("items/RareTest") as Item;

        //item = Instantiate(Resources.Load("Prefabs/ItemDisplay") as GameObject,
        //                        new Vector3(458.0f, 560.0f, 0.0f),
        //                        Quaternion.identity,
        //                        GameObject.FindGameObjectWithTag("GameCanvas").transform);
        //item.GetComponent<ItemDisplayBehavior>().powerUpMode = true;
        //item.GetComponent<ItemDisplayBehavior>().item = Resources.Load("items/EpicTest") as Item;

        //item = Instantiate(Resources.Load("Prefabs/ItemDisplay") as GameObject,
        //                        new Vector3(610.0f, 560.0f, 0.0f),
        //                        Quaternion.identity,
        //                        GameObject.FindGameObjectWithTag("GameCanvas").transform);
        //item.GetComponent<ItemDisplayBehavior>().powerUpMode = true;
        //item.GetComponent<ItemDisplayBehavior>().item = Resources.Load("items/SpecialTest") as Item;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatus();
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

    private IEnumerator StartLevels()
    {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("GameManager: Disabling text now");
        levelNum.enabled = false;
        levelName.enabled = false;

        // Level 1
        Debug.Log("GameManager: Running Level 1");
        levelAttach.AddComponent<LevelOne>();
        float time = levelAttach.GetComponent<LevelOne>().levelTime;
        //yield return new WaitForSeconds(time);
    }
}
