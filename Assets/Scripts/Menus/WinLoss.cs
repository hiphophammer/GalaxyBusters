using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLoss : MonoBehaviour
{
    public TMPro.TextMeshProUGUI condition;
    public TMPro.TextMeshProUGUI message;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WinLoss: I'm up");
        if(GameManager.winLoss)
        {
            condition.text = "YOU WIN!";
            message.text = "Yay. Good job.";
        }
    }

    public void BackButton()
    {
        Debug.Log("Going home");
        SceneManager.LoadScene(0);
    }
}
