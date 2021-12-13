using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    // Public member variables.
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI comboMult;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(scoreText != null);
        scoreText.text = 0.ToString();
        comboMult.text = 1.ToString() + "x";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetCombo(float combo)
    {
        combo = Mathf.Round(combo * 100f) / 100f;
        comboMult.text = combo.ToString() + "x";
    }
}
