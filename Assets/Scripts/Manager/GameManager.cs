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

        CreatePlayerOne();
        CreatePlayerTwo();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatus();
    }

    private void CreatePlayerOne()
    {
        if (MainMenu.player1Ship != "None")
        {
            Quaternion rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            GameObject player1 = Instantiate(Resources.Load("Prefabs/Player") as GameObject,
                                                player1StartPos,
                                                rotation);
            player1.tag = "Player";

            // Set the sprite.
            SpriteRenderer renderer = player1.GetComponent<SpriteRenderer>();

            if (MainMenu.player1Ship == "Lancer")
            {
                renderer.sprite = player1LancerSprite;
            }
            else if (MainMenu.player1Ship == "Vanguard")
            {
                renderer.sprite = player1VanguardSprite;
            }
            else
            {
                renderer.sprite = player1TrailblazerSprite;
            }

            PlayerBehavior playerBehavior = player1.GetComponent<PlayerBehavior>();
            playerBehavior.SetWeaponDamage(0.5f);
        }
    }

    private void CreatePlayerTwo()
    {
        if (MainMenu.player2Ship != "None")
        {
            Quaternion rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            GameObject player2 = Instantiate(Resources.Load("Prefabs/Player") as GameObject,
                                                player2StartPos,
                                                rotation);
            player2.tag = "Player";
            player2.GetComponent<PlayerBehavior>().playerOne = false;

            // Set the sprite.
            SpriteRenderer renderer = player2.GetComponent<SpriteRenderer>();

            if (MainMenu.player2Ship == "Lancer")
            {
                renderer.sprite = player2LancerSprite;
            }
            else if (MainMenu.player2Ship == "Vanguard")
            {
                renderer.sprite = player2VanguardSprite;
            }
            else
            {
                renderer.sprite = player2TrailblazerSprite;
            }

            PlayerBehavior playerBehavior = player2.GetComponent<PlayerBehavior>();
            playerBehavior.SetWeaponDamage(0.25f);
        }
    }

    private void UpdateStatus()
    {
        playerScore.text = scoreManager.GetStatus();
        Debug.Log(scoreManager.GetStatus());
    }
}
