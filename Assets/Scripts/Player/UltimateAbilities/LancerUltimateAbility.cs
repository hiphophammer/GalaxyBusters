using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerUltimateAbility : MonoBehaviour
{
    // Private member variables.
    private PlayerBehavior parent;
    private ChargeBarBehavior chargeBar;
    private string axis;

    // Start is called before the first frame update
    void Start()
    {
        chargeBar = parent.GetUltimateAbilityChargeBar();
        axis = parent.GetUltimateAbilityAxis();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the spacebar is being pressed.
        if (Input.GetAxis(axis) == 1.0f)
        {
            if (chargeBar.Charged())
            {
                // Trigger the reload.
                chargeBar.ResetCharge();
            }
        }
    }

    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }
}
