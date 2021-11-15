using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{

    public Image circleBar;                     // primary bar for HP from 1.0f to 0.5f
    public Image horizontalBar;                 // secondary bar for HP from 0.5f to 0.0f
    public Camera mainCam;

    private const float CIRCLE_MAX = 0.75f;

    private GameManager managerInstance;
    private PlayerBehavior player;

    // Start is called before the first frame update
    void Start()
    {
        managerInstance = mainCam.GetComponent<GameManager>();
        player = managerInstance.GetPlayer1();
    }

    // Update is called once per frame
    void Update()
    {
        float percentage = player.healthBar.Health() / player.healthBar.GetHitPoints();
        if (percentage <= 0.5f) // less than 50% health: empty the radial bar
        {
            circleBar.fillAmount = 0.0f;
            horizontalBar.fillAmount = percentage * 2;

        }
        else // more than 50% health: fill the horizontal bar and then work on the radial one
        {
            horizontalBar.fillAmount = 1.0f;
            circleBar.fillAmount = (percentage - 0.5f) * 2 * CIRCLE_MAX;
        }
    }
}
