using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLoss : MonoBehaviour
{
    // Constants.
    private const string CONDITION_WIN = "YOU WIN!";
    private const string CONDITION_LOSS = "YOU LOSE!";
    
    private const int MENU_SCENE_IDX = 0;

    // Public member variables.
    public TMPro.TextMeshProUGUI condition;
    public TMPro.TextMeshProUGUI message;
    public TMPro.TextMeshProUGUI score;

    // Private member variables.
    private string[] conditionMsgs;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WinLoss: I'm up");
        if (GameManager.winLoss)
        {
            conditionMsgs = new string[] {"Yay. Good job.", "You must be SOOO smart.", "Hooray! You did... something?",
                                          "Achievement get: don't die.", "You win! Oh wait, is that up there?",
                                          "Congrats on beating this eas- HARD game.", "Nice work, I suppose?",
                                          "Now be a winner IRL.", "*slaps gold star on player*", "Want a cookie?",
                                          "omegamonkachampepegalul"};
            int rand = Random.Range(0, conditionMsgs.Length);
            condition.text = CONDITION_WIN;
            message.text = conditionMsgs[rand];
        }
        else
        {
            conditionMsgs = new string[] {"You had one job.", "Think this is an l ratio moment.", "Waddup, LOSER.",
                                          "¯\\_(ツ)_/¯", "Seriously?", "In case it hasn't kicked in, YOU LOSE!",
                                          "Aww, are you crying?", "mE no goOD aT GaMe", "Maybe try easy mode. Oh wait.",
                                          "Let me guess, lag?", "Try again...maybe.", "If you blame devs I swear to god-",
                                          "Maybe try with the screen on.", "Hint: don't lose."};
            int rand = Random.Range(0, conditionMsgs.Length);
            condition.text = CONDITION_LOSS;
            message.text = conditionMsgs[rand];
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
