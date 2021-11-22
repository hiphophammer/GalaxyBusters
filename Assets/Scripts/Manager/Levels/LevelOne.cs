using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOne : MonoBehaviour
{
    // Constants.
    private const float LEVEL_TIME = 10.0f;
    private const float TIME_BETWEEN_SPAWNS = 2.0f;

    // Private member variables.
    private float timeSinceStart;
    private float timeAtStart;
    private float timeUntilEnd;
    private float elapsedTime;
    
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
                    Spawn3Fighters();
                }
                else if (rand == 3)
                {
                    SpawnChaser();
                }
                else
                {
                    SpawnFighter();
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

    // Private helpers.
    private void LevelEnd()
    {
        Debug.Log("End of first level");
        Destroy(this);
    }

    private void SpawnUFO()
    {
        Instantiate(Resources.Load("Prefabs/UFO"), new Vector3(-40, 150, 0), Quaternion.Euler(new Vector3(0,0,180))); 
        Instantiate(Resources.Load("Prefabs/UFO"), new Vector3(40, 150, 0), Quaternion.Euler(new Vector3(0,0,180))); 
    }

    private void Spawn3Fighters()
    {
        Instantiate(Resources.Load("Prefabs/Fighter"), new Vector3(-1.667981f, 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
        Instantiate(Resources.Load("Prefabs/Fighter"), new Vector3(0, 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
        Instantiate(Resources.Load("Prefabs/Fighter"), new Vector3(1.667981f, 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
    }

    private void SpawnFighter()
    {
        Instantiate(Resources.Load("Prefabs/Fighter"), new Vector3(Random.Range(-3f, 3f), 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
    }

    private void SpawnChaser()
    {
        Instantiate(Resources.Load("Prefabs/Chaser"), new Vector3(Random.Range(-3f, 3f), 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
    }
}
