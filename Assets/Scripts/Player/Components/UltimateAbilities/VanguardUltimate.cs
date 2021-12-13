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

    private bool specialItemActive;

    private float SHIELD_TIME = 5.0f; 
    
    private float OGradius;

    private GameObject shield;
    
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
        if (parent.GetSpecialItemStatus() && !specialItemActive)
        {
            extendShieldTime();
            specialItemActive = true;
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
            GetComponent<CircleCollider2D>().radius = .4f;
            Debug.Log(GetComponent<CircleCollider2D>().radius);
            
            shield = Instantiate(Resources.Load("Prefabs/SHIELD") as GameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z - .1f), transform.rotation);
            shield.transform.parent = parent.transform;
            float shieldsize = GetComponent<CircleCollider2D>().radius * 2;
            shield.transform.localScale = new Vector3(shieldsize, shieldsize, 1);
            shield.GetComponent<SpriteRenderer>().sortingOrder = 2;

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
            Debug.Log(dTime);
            Destroy(shield);
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
        SHIELD_TIME = 10f;
    }
}
