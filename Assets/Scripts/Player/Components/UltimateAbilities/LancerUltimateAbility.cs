using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerUltimateAbility : MonoBehaviour
{
    // Constants.
    private const float BULLET_VERTICAL_OFFSET = 0.3f;

    private const float BULLET_SPRAY_TIME = 6.0f;
    private const float THETA = 15.0f * Mathf.Deg2Rad;
    private const int BULLETS_PER_WAVE = 5;

    // Private member variables.
    private PlayerBehavior parent;
    private BaseWeapon weapon;
    private ChargeBarBehavior chargeBar;
    private CooldownBarBehavior cooldownBar;
    private string axis;

    private bool retrievedAxis;

    private float nextFire;

    // Bullet info.
    private string bulletType;
    private float damage;

    // FSM variables.
    private enum UltimateAbilityState
    {
        sprayBullets,
        charge
    };

    private UltimateAbilityState state;
    private float stateEntryTime;


    // Start is called before the first frame update
    void Start()
    {
        // Get a valid reference to our weapon.
        weapon = GetComponent<BaseWeapon>();
        Debug.Assert(weapon != null);

        // Retrieve a reference to our charge & cooldown bars and the axis.
        chargeBar = parent.GetUltimateAbilityChargeBar();
        cooldownBar = parent.GetWeaponCooldownBar();

        retrievedAxis = false;

        nextFire = 0;

        // Start the FSM off in the charge state.
        state = UltimateAbilityState.charge;

        // Grab the bullet info for the first time.
        UpdateBulletInfo();
    }

    // Update is called once per frame
    void Update()
    {
        RetrieveAxis();

        if (retrievedAxis)
        {
            UpdateBulletInfo();
            UpdateFSM();
        }
    }

    // Public modifiers.
    public void SetParent(PlayerBehavior parent)
    {
        this.parent = parent;
    }

    // Private helpers.
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

    private void UpdateBulletInfo()
    {
        bulletType = weapon.GetBullet();
        damage = parent.GetWeaponDamage();
    }

    // FSM definition.
    private void UpdateFSM()
    {
        switch (state)
        {
            case UltimateAbilityState.charge:
                ServiceChargeState();
                break;
            case UltimateAbilityState.sprayBullets:
                ServiceSprayBulletsState();
                break;
        }
    }

    // State service method definitions.
    private void ServiceChargeState()
    {
        if ((Input.GetAxis(axis) == 1.0f) && chargeBar.Charged())
        {
            state = UltimateAbilityState.sprayBullets;
            stateEntryTime = Time.time;

            weapon.StopFiring();

            // Trigger the reload.
            chargeBar.ResetCharge();
        }
    }

    private Vector3 RotateVector(Vector3 vector, float theta)
    {
        float cosTheta = Mathf.Cos(theta);
        float sinTheta = Mathf.Sin(theta);
        float xPrime = (vector.x * cosTheta) - (vector.y * sinTheta);
        float yPrime = (vector.x * sinTheta) + (vector.y * cosTheta);

        return new Vector3(xPrime, yPrime, 0.0f);
    }

    private void ServiceSprayBulletsState()
    {
        float dTime = Time.time - stateEntryTime;
        if (dTime >= BULLET_SPRAY_TIME)
        {
            // The ultimate ability has ended.
            state = UltimateAbilityState.charge;
            weapon.StartFiring();
        }
        else
        {
            if (Time.time > nextFire)
            {
                // Spray bullets.
                float dTheta = THETA / (BULLETS_PER_WAVE - 1);
                float angle = -1.0f * (THETA / 2);
                float endAngle = THETA / 2;

                // This is the direction the leftmost bullet will travel in.
                Vector3 direction = Vector3.up;
                Vector3 startPos;

                // Work our way from left to right, computing the direction of each
                // bullet.
                while (angle <= endAngle)
                {
                    direction = RotateVector(Vector3.up, angle);

                    startPos = transform.position;
                    startPos.y += BULLET_VERTICAL_OFFSET;
                    startPos -= (Vector3.up - direction);   // This puts it in the right
                                                            // position, effectively creating
                                                            // an arc.

                    // Now we spawn the projectile.
                    Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    GameObject bullet = Instantiate(Resources.Load(bulletType) as GameObject,
                                                    startPos,
                                                    rotation);
                    bullet.transform.up = direction;

                    // Configure the component of the bullet.
                    ProjectileBehavior bulletBehavior = bullet.GetComponent<ProjectileBehavior>();
                    bulletBehavior.SetParent(parent);
                    bulletBehavior.SetDamage(damage);

                    angle += dTheta;
                }

                nextFire = Time.time + weapon.fireRate;
            }
        }
    }
}
