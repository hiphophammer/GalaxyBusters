using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOne : MonoBehaviour
{
    public float levelTime, timeSinceStart, timeAtStart;
    float timeBetweenSpawns, elapsedTime;
    EnemySpawnControl e;
    
    // Start is called before the first frame update
    void Start()
    {
        levelTime = 180f;
        timeSinceStart = 0f;
        timeAtStart = Time.time;

        timeBetweenSpawns = 4f;

        e = GetComponent<EnemySpawnControl>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(timeSinceStart >= levelTime)
        {
            levelEnd();
        }
        else
        {
            if (elapsedTime > timeBetweenSpawns)
            {
                elapsedTime = 0;
                int rand = Random.Range(1,3);
                if (rand == 1)
                {
                    e.Spawn3Fighters();
                }
                else
                {
                    e.SpawnChaser();
                }
            }
        }
        
        Debug.Log(timeSinceStart);
        timeSinceStart = Time.time - timeAtStart; 
    }

    void levelEnd()
    {

    }
}
