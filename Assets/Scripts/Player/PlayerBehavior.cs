using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    // Constants.
    private const string PLAYER_1_VERTICAL_AXIS = "Player1Vertical";
    private const string PLAYER_1_HORIZONTAL_AXIS = "Player1Horizontal";

    private const string PLAYER_2_VERTICAL_AXIS = "Player2Vertical";
    private const string PLAYER_2_HORIZONTAL_AXIS = "Player2Horizontal";

    private const string PLAYER_1_BASIC_ABILITY_AXIS = "Player1BasicAbility";
    private const string PLAYER_1_SPECIAL_ABILITY_AXIS = "Player1UltimateAbility";

    private const string PLAYER_2_BASIC_ABILITY_AXIS = "Player2BasicAbility";
    private const string PLAYER_2_SPECIAL_ABILITY_AXIS = "Player2UltimateAbility";

    private const float PROJECTILE_Y_OFFSET = 0.4f;

    // Public member variables.
    public CooldownBarBehavior autoFireCooldownBar;
    public CooldownBarBehavior basicAbilityCooldownBar;
    public RechargeBarBehavior ultimateAbilityChargeBar;
    public HealthBar healthBar;
    public float speed = 3.0f;
    public bool playerOne = true;

    // Private member variables.
    private CameraSupport cameraSupport;

    private string verticalAxis;
    private string horizontalAxis;
    private string basicAbilityAxis;
    private string ultimateAbilityAxis;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(autoFireCooldownBar != null);
        Debug.Assert(basicAbilityCooldownBar != null);
        Debug.Assert(ultimateAbilityChargeBar != null);
        Debug.Assert(healthBar != null);

        cameraSupport = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(cameraSupport != null);

        SetupPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();

        FireRound();

        UseBasicAbility();

        UseUltimateAbility();
    }

    public bool IsPlayerOne()
    {
        return playerOne;
    }

    public bool IsAlive()
    {
        return healthBar.Health() != 0.0f;
    }

    public void DestroyedEnemy()
    {
        // Add some charge to the ultimate ability charge bar.
        ultimateAbilityChargeBar.AddCharge(25.0f / 2.0f);
    }

    private void SetupPlayer()
    {
        // Make sure the player is facing upright.
        transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

        // Determine which axes we'll be using for movement.
        verticalAxis = playerOne ? PLAYER_1_VERTICAL_AXIS : PLAYER_2_VERTICAL_AXIS;
        horizontalAxis = playerOne ? PLAYER_1_HORIZONTAL_AXIS: PLAYER_2_HORIZONTAL_AXIS;

        // Determine which axes to use for our abilities.
        basicAbilityAxis = playerOne ? PLAYER_1_BASIC_ABILITY_AXIS : PLAYER_2_BASIC_ABILITY_AXIS;
        ultimateAbilityAxis = playerOne ? PLAYER_1_SPECIAL_ABILITY_AXIS : PLAYER_2_SPECIAL_ABILITY_AXIS;
    }

    private float Clip(float val, float min, float max)
    {
        // Use absolute value for everything, allowing us to use ranges that are negative
        // or positive.
        if (Mathf.Abs(val) < Mathf.Abs(min))
        {
            // We are PAST the lower bound so return that.
            return min;
        }
        else if (Mathf.Abs(val) > Mathf.Abs(max))
        {
            // We are PAST the upper bound so return that.
            return max;
        }
        else
        {
            // We are WITHIN bounds so return the original value.
            return val;
        }
    }

    private void UpdatePosition()
    {
        // Retrieve the values from our axes.
        float upDown = Input.GetAxis(verticalAxis);
        float leftRight = Input.GetAxis(horizontalAxis);

        // Scale the values appropriately.
        upDown *= (speed * Time.deltaTime);
        leftRight *= (speed * Time.deltaTime);

        // Get the bounds of our renderer.
        Bounds myBound = GetComponent<Renderer>().bounds;

        // Check if our bounds exceed that of the camera.
        CameraSupport.WorldBoundStatus status = cameraSupport.CollideWorldBound(myBound);

        // Translate the player by the computed amount.
        transform.Translate(leftRight, upDown, 0.0f);
    }

    private void FireRound()
    {
        // Check if enough time has passed to fire another round.
        if (autoFireCooldownBar.ReadyToFire())
        {
            // Instantiate the projectile.
            Vector3 startPos = transform.position;
            startPos.y += PROJECTILE_Y_OFFSET;
            GameObject projectile =
                Instantiate(Resources.Load("Prefabs/Projectile") as GameObject,
                            startPos,
                            transform.rotation);
            ProjectileBehavior projectileBehavior = projectile.GetComponent<ProjectileBehavior>();
            projectileBehavior.SetParent(this);

            // Trigger the reload.
            autoFireCooldownBar.TriggerCooldown();
        }
    }

    private void UseBasicAbility()
    {
        // Check if the spacebar is being pressed.
        if (Input.GetAxis(basicAbilityAxis) == 1.0f)
        {
            if (basicAbilityCooldownBar.ReadyToFire())
            {
                // Trigger the reload.
                basicAbilityCooldownBar.TriggerCooldown();
            }
        }
    }

    private void UseUltimateAbility()
    {
        // Check if the spacebar is being pressed.
        if (Input.GetAxis(ultimateAbilityAxis) == 1.0f)
        {
            if (ultimateAbilityChargeBar.Charged())
            {
                // Trigger the reload.
                ultimateAbilityChargeBar.ResetCharge();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("EnemyProjectile") && IsAlive())
        {
            // Update our health bar.
            healthBar.RemoveHealth(0.25f / 2.0f);

            if (healthBar.Health() == 0.0f)
            {
                healthBar.AddHealth(1.0f);
            }
        }
    }
}
