using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanguardUltimate : MonoBehaviour
{
    
    // Private member variables.
    private PlayerBehavior parent;
    private float parentSpeed;
    private BaseWeapon weapon;
    private float OGFireRate;
    private ChargeBarBehavior chargeBar;
    private CooldownBarBehavior cooldownBar;
    private string axis;
    private Color baseColor, alpha;
    private SpriteRenderer renderer;
    private bool Shield;

    private float SHIELD_TIME = 5.0f; 
    
    private float OGradius;

    // FSM variables.
    private enum UltimateAbilityState
    {
        shielded,
        charge
    };

    private UltimateAbilityState state;
    private float stateEntryTime;

    private bool retrievedAxis;
    // Start is called before the first frame update
    void Start()
    {
        // Retrieve a reference to our charge & cooldown bars and the axis.
        chargeBar = parent.GetUltimateAbilityChargeBar();
        axis = parent.GetUltimateAbilityAxis();

        // Retrieve a reference to parent's weapon.
        weapon = GetComponent<BaseWeapon>();
        Debug.Assert(weapon != null);

        chargeBar = parent.GetUltimateAbilityChargeBar();
        cooldownBar = parent.GetWeaponCooldownBar();

        // Get parent's SpriteRenderer component.
        renderer = parent.GetComponent<SpriteRenderer>();
        baseColor = renderer.color;
        // Start the FSM off in the charge state.
        state = UltimateAbilityState.charge;

        // Get collider to expand and contract as necessary.
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        OGradius = col.radius;

        retrievedAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        RetrieveAxis();
        if (retrievedAxis)
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
        if (!retrievedAxis)
        {
            axis = parent.GetUltimateAbilityAxis();
            if (axis != null && axis != "")
            {
                retrievedAxis = true;
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
            case UltimateAbilityState.shielded:
                ServiceShieldState();
                break;
        }
    }

    private void ServiceChargeState()
    {
        if ((Input.GetAxis(axis) == 1.0f) && chargeBar.Charged())
        {
            state = UltimateAbilityState.shielded;
            stateEntryTime = Time.time;
            Shield = true;
            cooldownBar.TriggerCooldown();
            GetComponent<CircleCollider2D>().radius = OGradius * 2f;
            Debug.Log(GetComponent<CircleCollider2D>().radius);
            parent.GetComponent<SpriteRenderer>().color = Color.Lerp(baseColor, alpha, .25f);
            // Trigger the reload.
            chargeBar.ResetCharge();
        }
    }

    private void ServiceShieldState()
    {
        float dTime = Time.time - stateEntryTime;
        if (dTime >= SHIELD_TIME)
        {
            GetComponent<CircleCollider2D>().radius = OGradius;
            Debug.Log(GetComponent<CircleCollider2D>().radius);
            Shield = false;
            parent.GetComponent<SpriteRenderer>().color = baseColor;
            // The ultimate ability has ended.
            state = UltimateAbilityState.charge;
        }
    }

    public bool getShieldStatus()
    {
        return Shield;
    }

    public void extendShieldTime()
    {
        SHIELD_TIME += 2;
    }
}
