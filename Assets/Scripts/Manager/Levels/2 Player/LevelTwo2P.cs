using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwo2P : MonoBehaviour
{
    // Constants.
    private const float LEVEL_TIME = 45.0f;
    private const float TIME_BETWEEN_SPAWNS = 3f;

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
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 3
        spawner.SpawnChaser();
        spawner.SpawnUFO();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 6
        spawner.SpawnUFO();
        spawner.SpawnUFO();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 9
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 12
        spawner.SpawnUFO();
        spawner.SpawnUFO();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 15
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 18
        spawner.SpawnUFOFlank();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 21
        spawner.SpawnUFOFlank();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 24
        spawner.SpawnFighter2();
        spawner.SpawnFighter2();
        spawner.SpawnFighter2();
        spawner.SpawnFighter2();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 27
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 30
        spawner.SpawnUFOFlank();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 33
        spawner.SpawnUFO();
        spawner.SpawnUFO();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 36
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 39
        spawner.SpawnUFOFlank();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 42
        spawner.SpawnUFOFlank();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        spawner.SpawnFighter();
        yield return new WaitForSeconds(TIME_BETWEEN_SPAWNS); // 45
        LevelEnd();
    }
    
    private void LevelEnd()
    {
        Debug.Log("End of second level");
        Destroy(this);
    }
}
