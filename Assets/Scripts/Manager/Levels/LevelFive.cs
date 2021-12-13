using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFive : MonoBehaviour
{
    // Constants.
    private const float LEVEL_TIME = 45.0f;
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
        Debug.Log("LevelFive: Waking up!");
        timeSinceStart = 0.0f;
        timeAtStart = Time.time;
        timeUntilEnd = LEVEL_TIME;
        spawner.SpawnBoss();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        timeUntilEnd = timeUntilEnd - elapsedTime;
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
        Debug.Log("End of fifth level");
        Destroy(this);
    }
}
