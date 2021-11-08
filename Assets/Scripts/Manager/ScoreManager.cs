using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    private float player1Score;
    private float player2Score;

    // Start is called before the first frame update
    void Start()
    {
        player1Score = 0.0f;
        player2Score = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public string GetStatus()
    {
        string player1ScoreMsg = "Player 1: " + player1Score;
        string player2ScoreMsg = "Player 2: " + player2Score;
        return player1ScoreMsg + "\n" + player2ScoreMsg;
    }
}
