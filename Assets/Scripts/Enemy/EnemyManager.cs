using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Private member variables.
    private bool attack;
    private int numOfEnemies;           // The current number of enemies.
    private int numOfDestroyedEnemies;  // The number of destroyed enemies.

    /// <summary>
    /// This is called before the first frame update. We set the default sequencing mode
    /// for the enemies and create the enemies themselves.
    /// </summary>
    void Start()
    {
        // Create our enemies.
        ManageEnemies();
        attack = false;
    }

    /// <summary>
    /// This is called once per frame. We check whether the J key is pressed (which
    /// toggles between sequential and random sequencing of the waypoints) and 
    /// make sure we always have 10 enemies.
    /// </summary>
    void Update()
    {
        // Check if any new enemies need to be created.
        ManageEnemies();

        if (Input.GetKeyDown("j"))
        {
            attack = !attack;
        }
    }

    public bool ShouldAttack()
    {
        return attack;
    }

    /// <summary>
    /// Retrieves the number of enemies currently on the screen.
    /// </summary>
    /// <returns>The number of enemies currently on the screen.</returns>
    public int GetNumOfEnemies()
    {
        return numOfEnemies;
    }

    /// <summary>
    /// Retrieves the number of enemies that have been destroyed.
    /// </summary>
    /// <returns>The number of enemies that have been destroyed.</returns>
    public int GetNumOfDestroyedEnemies()
    {
        return numOfDestroyedEnemies;
    }

    /// <summary>
    /// This increments the count of enemies by one.
    /// </summary>
    public void IncrementNumOfEnemies()
    {
        numOfEnemies++;
    }

    /// <summary>
    /// This decrement the count of enemies by one.
    /// </summary>
    public void DecrementNumOfEnemies()
    {
        numOfEnemies--;
        numOfDestroyedEnemies++;
    }

    /// <summary>
    /// This manages the enemies by always making sure there are 10 of them and
    /// checking whether the E key is pressed, toggling whether the enemies should be
    /// moving or not.
    /// </summary>
    private void ManageEnemies()
    {
        // Always keep the number of enemies at 3.
        while (numOfEnemies < 3)
        {
            // Create a new enemy.
            GameObject enemy =
                Instantiate(Resources.Load("Prefabs/Enemy") as GameObject);

            // Update our count.
            numOfEnemies++;
        }
    }
}