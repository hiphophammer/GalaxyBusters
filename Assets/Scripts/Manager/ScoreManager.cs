using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Public member variables.
    public ScoreDisplay player1ScoreDisplay;
    public ScoreDisplay player2ScoreDisplay;

    // Private member variables.
    private float player1Score;
    private float player2Score;

    private float player1Combo;
    private float player2Combo;

    private bool determinedMode;
    private bool singlePlayer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(player1ScoreDisplay != null);
        Debug.Assert(player2ScoreDisplay != null);

        player1ScoreDisplay.Show();
        player2ScoreDisplay.Show();

        player1Score = 0.0f;
        player2Score = 0.0f;

        determinedMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        DetermineMode();
        UpdateDisplays();
    }

    public float GetPlayer1Score()
    {
        return player1Score;
    }

    public float GetPlayer2Score()
    {
        return player2Score;
    }

    private void DetermineMode()
    {
        if (!determinedMode)
        {
            GameManager gameManager = Camera.main.GetComponent<GameManager>();
            if (gameManager.Ready())
            {
                singlePlayer = gameManager.SinglePlayer();
                if (singlePlayer)
                {
                    player2ScoreDisplay.Hide();
                }
                determinedMode = true;
            }
        }
    }

    public void DestroyedEnemy(float[] damageDealt, int destroyer, float totalEnemyHealth)
    {
        if (destroyer == 0)
        {
            // Player 1 destroyed the enemy.
            player1Score += totalEnemyHealth;
            player2Score += damageDealt[1];
        }
        else
        {
            // Player 2 destroyed the enemy.
            player1Score += damageDealt[0];
            player2Score += totalEnemyHealth;
        }
    }

    private void UpdateDisplays()
    {
        player1ScoreDisplay.SetScore((int) player1Score);
        if (!singlePlayer)
        {
            player2ScoreDisplay.SetScore((int)player2Score);
        }
        else
        {
            player2ScoreDisplay.Hide();
        }
    }

    public void UpdateCombo(PlayerBehavior player, float combo)
    {
        if (player.IsPlayerOne())
        {
            // Player 1 destroyed the enemy.
            player1ScoreDisplay.SetCombo(combo);
        }
        else
        {
            // Player 2 destroyed the enemy.
            player2ScoreDisplay.SetCombo(combo);
        }
    }
}
