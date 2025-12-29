

using System.Collections.Generic;
using UnityEngine;

public class WaveManager: MonoBehaviour
{
    public static WaveManager Instance;

    public int currentWave = 0;

    // Prefab location for each enemy wave
    public List<string> waves = new()
    {
        "Enemy",
        "Enemy",
        "Enemy",
    };

    Transform mainGround;


    void Awake()
    {
        mainGround = GameObject.Find("MainGround").transform;

        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void StartNextWave()
    {
        currentWave++;
        if(currentWave > waves.Count)
        {
            Debug.LogWarning("All waves completed!");
            return;
        }
        Debug.Log("Starting wave " + currentWave);
        var enemyPrefab = Resources.Load<GameObject>($"{waves[currentWave - 1]}");
        Instantiate(enemyPrefab, mainGround);
    }

}