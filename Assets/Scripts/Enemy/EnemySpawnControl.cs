using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnControl : MonoBehaviour
{
    /*public void Spawn3Fighters()
    {
        Instantiate(Resources.Load("Prefabs/Fighter"), new Vector3(-1.667981f, 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
        Instantiate(Resources.Load("Prefabs/Fighter"), new Vector3(0, 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
        Instantiate(Resources.Load("Prefabs/Fighter"), new Vector3(1.667981f, 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
    }*/

    public void SpawnFighter()
    {
        Instantiate(Resources.Load("Prefabs/Fighter"), new Vector3(Random.Range(-3f, 3f), 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180)));
    }

    public void SpawnChaser()
    {
        Instantiate(Resources.Load("Prefabs/Chaser"), new Vector3(Random.Range(-3f, 3f), 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
    }

    public void SpawnUFO()
    {
        Instantiate(Resources.Load("Prefabs/UFO"), new Vector3(Random.Range(-3f, 3f), 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180)));
    }

    public void SpawnFighter2()
    {
        Instantiate(Resources.Load("Prefabs/Fighter2"), new Vector3(Random.Range(-3f, 3f), 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180)));
    }

    public void SpawnUFO2()
    {
        Instantiate(Resources.Load("Prefabs/UFO2"), new Vector3(Random.Range(-3f, 3f), 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180)));
    }

    public void SpawnSquare()
    {
        Instantiate(Resources.Load("Prefabs/Square"), new Vector3(Random.Range(-3f, 3f), 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180)));
    }

    public void SpawnUFOFlank()
    {
        Instantiate(Resources.Load("Prefabs/UFO"), new Vector3(-2f, 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
        Instantiate(Resources.Load("Prefabs/UFO"), new Vector3(2f, 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
    }

    public void SpawnSquareFlank()
    {
        Instantiate(Resources.Load("Prefabs/Square"), new Vector3(-2f, 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
        Instantiate(Resources.Load("Prefabs/Square"), new Vector3(2f, 5.5f, 0), Quaternion.Euler(new Vector3(0,0,180))); 
    }
}
