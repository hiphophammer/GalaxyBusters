using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Constants.
    private const float PROJECTILE_Y_OFFSET = -0.4f;

    // Public member variables.
    public CooldownBarBehavior cooldownBar;

    // Private member variables.
    private EnemyManager enemyManager;          // A reference to the GameManager.
    private ScoreManager scoreManager;
    private CameraSupport cs;                   // A reference to the CameraSupport
                                                // component of the main camera.
    public HealthBar healthBar;

    private bool alive;                         // This tells us whether the projectile
                                                // is set to be terminated.
    private float[] damageDealt;                // This keeps track of how much damage
                                                // was done by each player to awards
                                                // assists, where one player dealt damage
                                                // but didn't actually fire the
                                                // round/missile whatever that destroyed
                                                // the enemy.

    void Start()
    {
        Debug.Assert(healthBar != null);
        healthBar.SetHitPoints(50.0f);

        // Set our reference to the EnemyManager.
        enemyManager = Camera.main.GetComponent<EnemyManager>();
        Debug.Assert(enemyManager != null);

        // Set our reference to the ScoreManager.
        scoreManager = Camera.main.GetComponent<ScoreManager>();
        Debug.Assert(scoreManager != null);

        // Set our position. First, try to get the CameraSupport component of the main
        // camera.
        cs = Camera.main.GetComponent<CameraSupport>();
        Debug.Assert(cs != null);

        SetStartPosition();

        damageDealt = new float[]{ 0.0f, 0.0f };
        alive = true;
    }

    /// <summary>
    /// This is called once per frame. It updates the orientation and position of the
    /// enemy, updating the target waypoint if necessary.
    /// </summary>
    void Update()
    {
        if (alive)
        {
            // Fire a projectile.
            FireRound();
        }
        else
        {
            enemyManager.DecrementNumOfEnemies();
            Destroy(transform.gameObject);
        }
    }

    /// <summary>
    /// Determines whether the enemy is alive.
    /// </summary>
    /// <returns>A boolean indicating whether the enemy is alive.</returns>
    public bool IsAlive()
    {
        return alive;
    }

    public void TakeDamage(float totalDamage, PlayerBehavior playerBehavior)
    {
        // Update our health bar.
        float damageTaken = healthBar.RemoveHealth(totalDamage);

        // Store the amount of damage done by the player that dealt this damage.
        int player = playerBehavior.IsPlayerOne() ? 0 : 1;
        damageDealt[player] += damageTaken;

        if (healthBar.Health() == 0.0f)
        {
            // Tell the player that dealt this damage that they destroyed the enemy.
            // This is solely for them to charge their ultimate ability further.
            playerBehavior.DestroyedEnemy();

            // Report to the score manager the amount of damage dealt, and who
            // destroyed us.
            scoreManager.DestroyedEnemy(damageDealt, player);

            alive = false;
        }
    }

    /// <summary>
    /// This sets the starting position of the enemy such that it's within 90% of the
    /// camera bounds.
    /// </summary>
    private void SetStartPosition()
    {
        // Get the bounds of the camera.
        Bounds camBounds = cs.GetWorldBound();

        // Compute the bounds of the enemy.
        Bounds enemyBounds = new Bounds(camBounds.center, 0.9f * camBounds.size);

        // Generate a random position for the enemy.
        float x = Random.Range(enemyBounds.min.x, enemyBounds.max.x);
        float y = Random.Range(0.0f, enemyBounds.max.y);
        Vector3 newPosition = new Vector3(x, y, 0.0f);

        // Update our position.
        transform.position = newPosition;
    }

    private void FireRound()
    {
        // Check if enough time has passed to fire another round.
        if (cooldownBar.ReadyToFire() && enemyManager.ShouldAttack())
        {
            // Instantiate the projectile.
            Vector3 startPos = transform.position;
            startPos.y += PROJECTILE_Y_OFFSET;
            GameObject projectile =
                Instantiate(Resources.Load("Prefabs/EnemyProjectile") as GameObject,
                            startPos,
                            transform.rotation);

            // Trigger the cooldown.
            cooldownBar.TriggerCooldown();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("HeroProjectile") && alive)
        {
            ProjectileBehavior projectileBehavior = other.GetComponent<ProjectileBehavior>();
            PlayerBehavior playerBehavior = projectileBehavior.GetParent();

            // Update our health bar.
            float damage = healthBar.RemoveHealth(projectileBehavior.GetDamage());

            // Store the amount of damage done by the player that dealt this damage.
            int player = playerBehavior.IsPlayerOne() ? 0 : 1;
            damageDealt[player] += damage;

            if (healthBar.Health() == 0.0f)
            {
                // Tell the player that dealt this damage that they destroyed the enemy.
                // This is solely for them to charge their ultimate ability further.
                playerBehavior.DestroyedEnemy();

                // Report to the score manager the amount of damage dealt, and who
                // destroyed us.
                scoreManager.DestroyedEnemy(damageDealt, player);

                alive = false;
            }
        }
    }
}