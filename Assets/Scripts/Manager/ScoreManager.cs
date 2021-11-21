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

    private bool determinedMode;
    private bool singlePlayer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(player1ScoreDisplay != null);
        Debug.Assert(player2ScoreDisplay != null);

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

    public void DestroyedEnemy(float[] damageDealt, int destroyer)
    {
        if (destroyer == 0)
        {
            // Player 1 destroyed the enemy.
            player1Score += 1.0f;
            player2Score += damageDealt[1];
        }
        else
        {
            // Player 2 destroyed the enemy.
            player1Score += damageDealt[0];
            player2Score += 1.0f;
        }
    }

    private void UpdateDisplays()
    {
        player1ScoreDisplay.SetScore((int) player1Score);
        if (singlePlayer)
        {
            player2ScoreDisplay.SetScore((int)player2Score);
        }
    }
}
