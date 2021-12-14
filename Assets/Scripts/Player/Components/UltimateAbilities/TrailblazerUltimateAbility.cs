using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailblazerUltimateAbility : MonoBehaviour
{


    // Private member variables.
    private PlayerBehavior parent;
    private float parentSpeed;
    private BaseWeapon weapon;
    private float OGFireRate;
    private ChargeBarBehavior chargeBar;
    private CooldownBarBehavior cooldownBar;

    private float INTANGIBILITY_TIME = 5.0f; 

    private float weaponDamage;
    
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

    private bool specialItemActive;
    private bool ghost;
    private bool retrivedAxis;

    private int count;
    private float elapsedTime;
    private const float TIME_BETWEEN_SPAWNS = 0.05f;

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

        // Get parent's weapon damage.
        weaponDamage = parent.GetWeaponDamage();

        // Get parent's SpriteRenderer component.
        renderer = parent.GetComponent<SpriteRenderer>();
        baseColor = renderer.color;
        alpha = renderer.color;
        // Start the FSM off in the charge state.
        state = UltimateAbilityState.charge;

        retrivedAxis = false;

        count = 1;
        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        RetrieveAxis();
        elapsedTime += Time.deltaTime;

        if (retrivedAxis)
        {
            UpdateFSM();
        }
        if (parent.GetSpecialItemStatus() && !specialItemActive)
        {
            increaseGhostTime();
            specialItemActive = true;
            GetComponent<TrailblazerCollider>().UpdateSpecialItemStatus(specialItemActive);
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
            parentSpeed = parent.speed;
            OGFireRate = weapon.fireRate;
            parent.speed *= 1.1f;
            weapon.fireRate /= 1.5f;
            ghost = true;
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
            parent.GetComponent<SpriteRenderer>().material.color = baseColor;
            parent.speed = parentSpeed;
            weapon.fireRate = OGFireRate;
            parent.SetWeaponDamage(weaponDamage);
            ghost = false;
            // The ultimate ability has ended.
            state = UltimateAbilityState.charge;
        }
        else
        {
            parent.GetComponent<SpriteRenderer>().material.color = alpha;
            if (elapsedTime > TIME_BETWEEN_SPAWNS)
            {
                elapsedTime = 0;
                GameObject afterimage = Instantiate(Resources.Load("Prefabs/PlayerGhost") as GameObject, transform.position, transform.rotation);
                SpriteRenderer renderer = afterimage.GetComponent<SpriteRenderer>();
                renderer.sortingOrder = count++;
                afterimage.GetComponent<Renderer>().material.color = new Color32(255,114,114,125);
                renderer.sprite = GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    public bool getGhostStatus()
    {
        return ghost;
    }

    public void increaseGhostTime()
    {
        INTANGIBILITY_TIME = 10f;
    }

}

