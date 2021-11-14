using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Vector3 player1StartPos = new Vector3(-1.5f, -2.5f, 0.0f);
    private Vector3 player2StartPos = new Vector3(1.5f, -2.5f, 0.0f);

    private ScoreManager scoreManager;

    public TMPro.TextMeshProUGUI playerScore;

    public Sprite player1LancerSprite;
    public Sprite player1VanguardSprite;
    public Sprite player1TrailblazerSprite;

    public Sprite player2LancerSprite;
    public Sprite player2VanguardSprite;
    public Sprite player2TrailblazerSprite;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = Camera.main.GetComponent<ScoreManager>();
        Debug.Assert(scoreManager != null);

        Debug.Assert(playerScore != null);

        Debug.Log("Player 1 ship: " + MainMenu.player1Ship);
        Debug.Log("Player 2 ship: " + MainMenu.player2Ship);

        BuildPlayers();

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

    private void BuildPlayers()
    {
        Quaternion rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

        for (int i = 0; i < 2; i++)
        {
            bool playerOne = i == 0;
            string ship = playerOne ? MainMenu.player1Ship : MainMenu.player2Ship;

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

                // Set the sprite and add appropriate components.
                SpriteRenderer renderer = player.GetComponent<SpriteRenderer>();
                if (ship == "Lancer")
                {
                    renderer.sprite = playerOne ? player1LancerSprite : player2LancerSprite;

                    // Set stats.
                    playerBehavior.SetWeaponDamage(0.5f);
                    playerBehavior.SetSpeed(5.0f);

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

                    playerBehavior.SetWeaponDamage(0.3f);
                    playerBehavior.SetSpeed(3.0f);
                }
                else
                {
                    // Otherwise, given the string is not null, it must be the trailblazer.
                    renderer.sprite = playerOne ? player1TrailblazerSprite : player2TrailblazerSprite;

                    playerBehavior.SetWeaponDamage(0.1f);
                    playerBehavior.SetSpeed(7.0f);
                }
            }
        }
    }

    private void UpdateStatus()
    {
        playerScore.text = scoreManager.GetStatus();
        Debug.Log(scoreManager.GetStatus());
    }
}
