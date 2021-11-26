using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOne : MonoBehaviour
{
    // Constants.
    private const float LEVEL_TIME = 3.0f;
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
        Debug.Log("LevelOne: Waking up!");
        timeSinceStart = 0.0f;
        timeAtStart = Time.time;
        timeUntilEnd = LEVEL_TIME;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        timeUntilEnd = timeUntilEnd - elapsedTime;
        if (timeSinceStart >= LEVEL_TIME)
        {
            LevelEnd();
        }
        else
        {
            if (elapsedTime > TIME_BETWEEN_SPAWNS)
            {
                elapsedTime = 0;
                int rand = Random.Range(1, 5);
                if (rand == 1 || rand == 2)
                {
                    spawner.SpawnChaser();
                    spawner.SpawnSquare();
                }
                else if (rand == 3)
                {
                    spawner.SpawnChaser();
                    spawner.SpawnChaser();
                }
                else
                {
                    spawner.SpawnFighter();
                    spawner.SpawnFighter();
                    spawner.SpawnFighter();
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

    // Private helpers.
    private void LevelEnd()
    {
        Debug.Log("End of first level");
        Destroy(this);
    }
}
