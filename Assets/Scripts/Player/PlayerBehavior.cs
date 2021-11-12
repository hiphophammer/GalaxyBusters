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

    // Public member variables.
    public CooldownBarBehavior weaponCooldown;
    public CooldownBarBehavior basicAbilityCooldownBar;
    public ChargeBarBehavior ultimateAbilityChargeBar;
    public HealthBar healthBar;
    public float speed = 3.0f;
    public bool playerOne = true;

    // Private member variables.
    private CameraSupport cameraSupport;

    private float weaponDamage;

    private string verticalAxis;
    private string horizontalAxis;
    private string basicAbilityAxis;
    private string ultimateAbilityAxis;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(weaponCooldown != null);
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
        
    }

    public void SetWeaponDamage(float damage)
    {
        this.weaponDamage = damage;
    }

    public float GetWeaponDamage()
    {
        return weaponDamage;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public float GetSpeed()
    {
        return speed;
    }

    // The only other thing is hitpoints...not sure how to do that with the current implementation of
    // the health bar.

    public CooldownBarBehavior GetWeaponCooldownBar()
    {
        return weaponCooldown;
    }

    public CooldownBarBehavior GetBasicAbilityCooldownBar()
    {
        return basicAbilityCooldownBar;
    }

    public string GetBasicAbilityAxis()
    {
        return basicAbilityAxis;
    }

    public ChargeBarBehavior GetUltimateAbilityChargeBar()
    {
        return ultimateAbilityChargeBar;
    }

    public string GetUltimateAbilityAxis()
    {
        return ultimateAbilityAxis;
    }
    
    public string[] GetMovementAxes()
    {
        return new string[] { verticalAxis, horizontalAxis };
    }

    public HealthBar GetHealthBar()
    {
        return healthBar;
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
}
