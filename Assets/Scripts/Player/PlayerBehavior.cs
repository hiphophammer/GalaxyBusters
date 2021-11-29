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
    public bool alive = true;

    // Private member variables.
    private CameraSupport cameraSupport;

    private BaseWeapon weapon;

    private InventoryBehavior inventory;

    private string shipName;
    private bool specialItemEnabled;

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

        weapon = GetComponent<BaseWeapon>();
        Debug.Assert(weapon != null);

        Debug.Assert(inventory != null);

        SetupPlayer();
    }

    // Ship name accessors/modifiers.
    public void SetShipName(string shipName)
    {
        this.shipName = shipName;
    }

    public string GetShipName()
    {
        return shipName;
    }

    // Inevtory accessor/modifier.
    public void SetInventory(InventoryBehavior inventory)
    {
        this.inventory = inventory;
    }

    public InventoryBehavior GetInventory()
    {
        return inventory;
    }

    // Special item accessors/modifiers.
    public void EnableSpecialItem()
    {
        specialItemEnabled = true;
    }

    public void DisableSpecialItem()
    {
        specialItemEnabled = false;
    }

    // Weapon damage accessors/modifiers.
    public void SetWeaponDamage(float damage)
    {
        this.weaponDamage = damage;
    }

    public float GetWeaponDamage()
    {
        return weaponDamage;
    }

    public BaseWeapon GetWeapon()
    {
        return weapon;
    }

    // Speed accessors/modifiers.
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public float GetSpeed()
    {
        return speed;
    }

    // Accessors for cooldown/charge/health bars.
    public CooldownBarBehavior GetWeaponCooldownBar()
    {
        return weaponCooldown;
    }

    public CooldownBarBehavior GetBasicAbilityCooldownBar()
    {
        return basicAbilityCooldownBar;
    }

    public ChargeBarBehavior GetUltimateAbilityChargeBar()
    {
        return ultimateAbilityChargeBar;
    }

    public HealthBar GetHealthBar()
    {
        return healthBar;
    }

    // Axes accessors/modifiers.
    public string GetBasicAbilityAxis()
    {
        return basicAbilityAxis;
    }

    public string GetUltimateAbilityAxis()
    {
        return ultimateAbilityAxis;
    }
    
    public string[] GetMovementAxes()
    {
        return new string[] { verticalAxis, horizontalAxis };
    }

    // Misc. accessors/functions.
    public bool IsPlayerOne()
    {
        return playerOne;
    }

    public bool IsAlive()
    {
        return alive;
    }

    public void DestroyedEnemy(int enemyHealth)
    {
        // Add some charge to the ultimate ability charge bar.
        ultimateAbilityChargeBar.AddCharge((enemyHealth + 10) / 2.0f);
    }

    public string GetStatus()
    {
        float curHealth = healthBar.Health();
        float hp = healthBar.GetHitPoints();

        string healthMsg = "Health: " + curHealth + "/" + hp + "\n";
        string speedMsg = "Speed: " + speed + "\n";
        string damageMsg = "Damage: " + weaponDamage;

        return healthMsg + speedMsg + damageMsg;
    }

    // Private helper methods.
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

        specialItemEnabled = false;
    }
}
