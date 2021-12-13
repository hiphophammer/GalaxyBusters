using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwo : MonoBehaviour
{
    // Constants.
    private const float LEVEL_TIME = 45.0f;
    private const float TIME_BETWEEN_SPAWNS = 5.0f;

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
        StartCoroutine(StartLevel());
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
    private IEnumerator StartLevel()
    {
        spawner.SpawnUFOFlank();
        yield return new WaitForSeconds(5f);
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(5f);
        spawner.SpawnUFO();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(5f);
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(5f);
        spawner.SpawnUFO();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(5f);
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        yield return new WaitForSeconds(5f);
        spawner.SpawnUFOFlank();
        yield return new WaitForSeconds(5f);
        spawner.SpawnUFOFlank();
        yield return new WaitForSeconds(5f);
        spawner.SpawnFighter2();
        spawner.SpawnFighter2();
        yield return new WaitForSeconds(5f);
        LevelEnd();
    }
    
    private void LevelEnd()
    {
        Debug.Log("End of second level");
        Destroy(this);
    }
}
