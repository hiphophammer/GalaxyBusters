using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltBar : MonoBehaviour
{
    public Image iconFiller;
    public Camera mainCam;
    public bool forPlayerOne;
    public string barType;                      // sets which ult bar it is for: "Lancer", "Vanguard", "Trailblazer"

    private GameManager managerInstance;
    private PlayerBehavior player;
    private string shipName;

    // Start is called before the first frame update
    void Start()
    {
        managerInstance = mainCam.GetComponent<GameManager>();
        Debug.Assert(managerInstance != null);
        if (forPlayerOne)
        {
            player = managerInstance.GetPlayer1();
            Debug.Assert(player != null);
            shipName = player.GetShipName();
        }
        else
        {
            if (!managerInstance.SinglePlayer())
                player = managerInstance.GetPlayer2();
            else
                player = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null && player.GetShipName() == barType)
        {
            float percentage = player.ultimateAbilityChargeBar.percentage;
            if (percentage > 1.0f)
            {
                percentage = 1.0f;
            }
            iconFiller.fillAmount = percentage;
        }
    }
}
