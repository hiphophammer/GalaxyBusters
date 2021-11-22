using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailblazerUltimateAbility : MonoBehaviour
{
    // Constants
    private const float INTANGIBILITY_TIME = 5.0f; 

    // Private member variables.
    private PlayerBehavior parent;
    private BaseWeapon weapon;
    private ChargeBarBehavior chargeBar;
    private string axis;
    private Color baseColor, alpha;
    private SpriteRenderer renderer;

    // FSM variables.
    private enum UltimateAbilityState
    {
        intangible,
        charge
    };

    private UltimateAbilityState state;
    private float stateEntryTime;

    private bool retrivedAxis;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve a reference to our charge & cooldown bars and the axis.
        chargeBar = parent.GetUltimateAbilityChargeBar();
        axis = parent.GetUltimateAbilityAxis();
        
        // Get parent's SpriteRenderer component.
        renderer = parent.GetComponent<SpriteRenderer>();
        baseColor = renderer.color;
        alpha = renderer.color;
        // Start the FSM off in the charge state.
        state = UltimateAbilityState.charge;

        retrivedAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        RetrieveAxis();
        if (retrivedAxis)
        {
            UpdateFSM();
        }
    }

    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }

    // Private helper methods.
    private void RetrieveAxis()
    {
        if (!retrivedAxis)
        {
            axis = parent.GetUltimateAbilityAxis();
            if (axis != null && axis != "")
            {
                retrivedAxis = true;
            }
        }
    }

    // FSM definition.
    private void UpdateFSM()
    {
        switch (state)
        {
            case UltimateAbilityState.charge:
                ServiceChargeState();
                break;
            case UltimateAbilityState.intangible:
                ServiceIntangibilityState();
                break;
        }
    }

    private void ServiceChargeState()
    {
        if ((Input.GetAxis(axis) == 1.0f) && chargeBar.Charged())
        {
            state = UltimateAbilityState.intangible;
            stateEntryTime = Time.time;

            // Trigger the reload.
            chargeBar.ResetCharge();
        }
    }

    private void ServiceIntangibilityState()
    {
        alpha.a = 0.6f;
        float dTime = Time.time - stateEntryTime;
        if (dTime >= INTANGIBILITY_TIME)
        {
            parent.GetComponent<Collider2D>().enabled = true;
            parent.GetComponent<SpriteRenderer>().material.color = baseColor;
            // The ultimate ability has ended.
            state = UltimateAbilityState.charge;
        }
        else
        {
            parent.GetComponent<Collider2D>().enabled = false;
            parent.GetComponent<SpriteRenderer>().material.color = alpha;
        }
    }


}

