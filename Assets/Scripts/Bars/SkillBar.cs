using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour
{
    public Image barFiller;
    public Camera mainCam;

    private GameManager managerInstance;
    private PlayerBehavior player;

    // Start is called before the first frame update
    void Start()
    {
        managerInstance = mainCam.GetComponent<GameManager>();
        player = managerInstance.GetPlayer1();
        barFiller.fillAmount = 1.0f;
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
