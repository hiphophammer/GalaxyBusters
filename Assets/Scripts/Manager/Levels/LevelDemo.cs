using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDemo : MonoBehaviour
{
    // Constants.
    private const float LEVEL_TIME = 100000.0f;
    private const float TIME_BETWEEN_SPAWNS = 2.0f;

    // Private member variables.
    private float timeSinceStart;
    private float timeAtStart;
    private float timeUntilEnd;
    private float elapsedTime;

    public EnemySpawnControl spawner;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("LevelDemo: Waking up!");
        timeSinceStart = 0.0f;
        timeAtStart = Time.time;
        timeUntilEnd = LEVEL_TIME;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        timeUntilEnd = timeUntilEnd - elapsedTime;
        if (timeSinceStart >= LEVEL_TIME || Input.GetKeyUp("H"))
        {
            
            LevelEnd();
        }
        
        // Calls to manually spawn in swarms of enemies
        if (Input.GetKeyUp("1"))
        {
            spawner.SpawnFighter();
            spawner.SpawnFighter();
            spawner.SpawnFighter();
        }
        if (Input.GetKeyUp("2"))
        {
            spawner.SpawnChaser();
            spawner.SpawnChaser();
        }
        if (Input.GetKeyUp("3"))
        {
            spawner.SpawnUFO();
            spawner.SpawnChaser();
            spawner.SpawnChaser();
        }
        if (Input.GetKeyUp("4"))
        {
            spawner.SpawnUFOFlank();
        }
        if (Input.GetKeyUp("5"))
        {
            spawner.SpawnSquare();
        }
        if (Input.GetKeyUp("6"))
        {
            spawner.SpawnSquareFlank();
        }
        if (Input.GetKeyUp("7"))
        {
            spawner.SpawnChaser();
            spawner.SpawnChaser();
            spawner.SpawnChaser();
            spawner.SpawnChaser();
            spawner.SpawnChaser();
            spawner.SpawnChaser();
        }
        if (Input.GetKeyUp("8"))
        {
            spawner.SpawnFighter2();
            spawner.SpawnFighter2();
            spawner.SpawnFighter2();
        }
        if (Input.GetKeyUp("9"))
        {
            spawner.SpawnUFO2();
        }

        timeSinceStart = Time.time - timeAtStart; 
    }

    // Public methods.
    public float GetLevelTime()
    {
        return LEVEL_TIME;
    }

    public void SetSpawner(EnemySpawnControl s)
    {
        spawner = s;
    }

    // Private helpers.
    private void LevelEnd()
    {
        Debug.Log("End of demo level");
        Destroy(this);
    }
}
