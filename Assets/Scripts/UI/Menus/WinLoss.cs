using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLoss : MonoBehaviour
{
    // Constants.
    private const string CONDITION_WIN = "YOU WIN!";
    private const string CONDITION_WIN_MSG = "Yay. Good job.";

    private const string CONDITION_LOSS = "YOU LOSE!";
    private const string CONDITION_LOSS_MSG = "Were you even trying?";

    private const int MENU_SCENE_IDX = 0;

    // Public member variables.
    public TMPro.TextMeshProUGUI condition;
    public TMPro.TextMeshProUGUI message;
    public TMPro.TextMeshProUGUI score;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WinLoss: I'm up");
        if (GameManager.winLoss)
        {
            condition.text = CONDITION_WIN;
            message.text = CONDITION_WIN_MSG;
        }
        else
        {
            condition.text = CONDITION_LOSS;
            message.text = CONDITION_LOSS_MSG;
        }

        if (GameManager.showBothScores)
        {
            score.text = "SCORE: " + GameManager.player1Score;
        }
        else
        {
            score.text = "PLAYER 1 SCORE: " + GameManager.player1Score + "\n"
                            + "PLAYER 2 SCORE: " + GameManager.player2Score;
        }
    }

    public void BackButton()
    {
        SceneManager.LoadScene(MENU_SCENE_IDX);
    }
}
