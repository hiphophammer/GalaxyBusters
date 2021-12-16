using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelENDLESS : MonoBehaviour
{
    // Constants
    private float LEVEL_TIME = 45.0f;
    
    // Private member variables.
    private float timeSinceStart;
    private float timeAtStart;
    private float timeUntilEnd;
    private float elapsedTime;

    private float TIME_BETWEEN_SPAWNS = 3f;


    public EnemySpawnControl spawner;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("LevelOne: Waking up!");
        timeSinceStart = 0.0f;
        timeAtStart = Time.time;
    }
    
    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (timeSinceStart >= LEVEL_TIME)
        {
            LevelEnd();
        }
        else
        {
            if (elapsedTime > TIME_BETWEEN_SPAWNS)
            {
                elapsedTime = 0;
                int rand = Random.Range(1, 6);
                if (rand == 1)
                {
                    spawner.SpawnChaser();
                    spawner.SpawnChaser();
                }
                else if (rand == 2)
                {
                    spawner.SpawnChaser();
                    spawner.SpawnChaser();
                    spawner.SpawnChaser();
                    spawner.SpawnChaser();
                }
                else if (rand == 3)
                {
                    spawner.SpawnFighter2();
                    spawner.SpawnFighter2();
                    spawner.SpawnFighter2();
                }
                else if (rand == 4)
                {
                    spawner.SpawnUFO2();
                    spawner.SpawnUFO2();
                    spawner.SpawnUFO2();
                    spawner.SpawnUFO2();
                }
                else
                {
                    spawner.SpawnSquare();
                    spawner.SpawnSquareFlank();
                }
            }
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
    
    private void LevelEnd()
    {
        Debug.Log("End of endless level. Repeating.");
        Destroy(this);
    }

    public void SetTimeBetweenSpawns(float time)
    {
        TIME_BETWEEN_SPAWNS = time;
    }
}
