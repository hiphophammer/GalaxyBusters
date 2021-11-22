using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltBar : MonoBehaviour
{
    public Image iconFiller;
    public Camera mainCam;

    private GameManager managerInstance;
    private PlayerBehavior player;

    // Start is called before the first frame update
    void Start()
    {
        managerInstance = mainCam.GetComponent<GameManager>();
        player = managerInstance.GetPlayer1();
        iconFiller.fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float percentage = player.ultimateAbilityChargeBar.percentage;
        if (percentage > 1.0f)
        {
            percentage = 1.0f;
        }
        iconFiller.fillAmount = percentage;
    }
}
