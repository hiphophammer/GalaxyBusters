using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOne : MonoBehaviour
{
    public float levelTime, timeSinceStart, timeAtStart;
    float timeBetweenSpawns, elapsedTime;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("LevelOne: Waking up!");
        levelTime = 180f;
        timeSinceStart = 0f;
        timeAtStart = Time.time;

        timeBetweenSpawns = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("LevelOne: Wave incoming!");
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
                    Spawn3Fighters();
                }
                else
                {
                    SpawnChaser();
                }
            }
        }
        
        Debug.Log(timeSinceStart);
        timeSinceStart = Time.time - timeAtStart; 
    }

    void levelEnd()
    {
        Debug.Log("End of first level");
        Destroy(this);
    }

    public void SpawnUFO()
    {
        Instantiate(Resources.Load("Prefabs/UFO"), new Vector3(-40, 150, 0), Quaternion.Euler(new Vector3(0,0,0))); 
        Instantiate(Resources.Load("Prefabs/UFO"), new Vector3(40, 150, 0), Quaternion.Euler(new Vector3(0,0,0))); 
    }

    public void Spawn3Fighters()
    {
        Instantiate(Resources.Load("Prefabs/Fighter"), new Vector3(-1.667981f, 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
        Instantiate(Resources.Load("Prefabs/Fighter"), new Vector3(0, 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
        Instantiate(Resources.Load("Prefabs/Fighter"), new Vector3(1.667981f, 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
    }

    public void SpawnFighter()
    {
        Instantiate(Resources.Load("Prefabs/Fighter"), new Vector3(0, 150, 0), Quaternion.Euler(new Vector3(0,0,0))); 
    }

    public void SpawnChaser()
    {
        Instantiate(Resources.Load("Prefabs/Chaser"), new Vector3(Random.Range(-3f, 3f), 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
    }
}
