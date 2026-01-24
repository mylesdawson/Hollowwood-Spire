

using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class WaveManager: MonoBehaviour
{
    public static WaveManager Instance;

    public int currentWave = 0;

    // Prefab location for each enemy wave
    [HideInInspector] public List<string> enemyWaves = new()
    {
        "Enemy",
        "Enemy",
    };

    Transform mainGround;

    [SerializeField] GameObject playerPrefab;

    [SerializeField] EnemyCanvas enemyCanvas;

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
        if(currentWave > enemyWaves.Count)
        {
            Debug.LogWarning("All waves completed!");
            return;
        }
        Debug.Log("Starting wave " + currentWave);
        var enemyPrefab = Resources.Load<GameObject>($"{enemyWaves[currentWave - 1]}");
        Instantiate(enemyPrefab, mainGround);

        enemyCanvas.Init(enemyPrefab.name);
    }

    public bool IsLastWave()
    {
        return currentWave >= enemyWaves.Count;
    }

}