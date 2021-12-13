using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelThree2P : MonoBehaviour
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
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnFighter2();
        spawner.SpawnFighter2();
        yield return new WaitForSeconds(4f);
        spawner.SpawnSquare();
        spawner.SpawnSquareFlank();
        yield return new WaitForSeconds(4f);
        spawner.SpawnUFO();
        spawner.SpawnUFO();
        spawner.SpawnUFOFlank();
        yield return new WaitForSeconds(4f);
        spawner.SpawnSquare();
        yield return new WaitForSeconds(4f);
        spawner.SpawnUFO();
        spawner.SpawnUFO();
        spawner.SpawnUFO();
        yield return new WaitForSeconds(4f);
        spawner.SpawnFighter2();
        spawner.SpawnFighter2();
        spawner.SpawnFighter2();
        spawner.SpawnFighter2();
        yield return new WaitForSeconds(6f);
        spawner.SpawnSquareFlank();
        spawner.SpawnSquareFlank();
        yield return new WaitForSeconds(4f);
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(3f);
        spawner.SpawnSquareFlank();
        spawner.SpawnSquareFlank();
        yield return new WaitForSeconds(5f);
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        yield return new WaitForSeconds(3f);
        spawner.SpawnFighter2();
        spawner.SpawnFighter2();
        spawner.SpawnFighter2();
        spawner.SpawnFighter2();
        yield return new WaitForSeconds(4f);
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        spawner.SpawnChaser();
        LevelEnd();
    }
    
    private void LevelEnd()
    {
        Debug.Log("End of third level");
        Destroy(this);
    }
}
