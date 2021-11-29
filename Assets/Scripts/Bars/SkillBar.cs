using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour
{
    public Image barFiller;
    public Camera mainCam;
    public bool forPlayerOne;

    private GameManager managerInstance;
    private PlayerBehavior player;

    // Start is called before the first frame update
    void Start()
    {
        managerInstance = mainCam.GetComponent<GameManager>();
        if (forPlayerOne)
            player = managerInstance.GetPlayer1();
        else
        {
            if (managerInstance.GetPlayer2() == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                player = managerInstance.GetPlayer2();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float percentage = player.basicAbilityCooldownBar.percentage;
        if (percentage > 1.0f)
        {
            percentage = 1.0f;
        }
        barFiller.fillAmount = percentage;

    }
}
