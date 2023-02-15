using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WaveHandler : Etienne.Singleton<WaveHandler>
{
    [SerializeField] private CanvasGroup endGameCanvas;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Waves waves;
    private List<Waves.Spawn> leftToSpawn;
    private float waveTimer;
    public  string path;

    [Serializable]
    public class Waves
    {
        public Spawn[] Spawns;

        [Serializable]
        public class Spawn
        {
            public float spawnTime;
            public string EnemyStatsName;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/My Games/{Application.productName}";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        LoadWaveHandler(path + "/WaveHandler.json");
    }

    private void LoadWaveHandler(string path)
    {
        if (!File.Exists(path)) File.WriteAllText(path, JsonUtility.ToJson(waves, true));
        else waves = JsonUtility.FromJson<Waves>(File.ReadAllText(path));
    }

    private void Start()
    {
        leftToSpawn = waves.Spawns.OrderBy(x => x.spawnTime).ToList();
        endGameCanvas.alpha = 0f;
        endGameCanvas.blocksRaycasts = false;
        endGameCanvas.interactable = false;
    }

    private void Update()
    {
        if (leftToSpawn.Count <= 0) return;
        waveTimer += Time.deltaTime;

        if (leftToSpawn[0].spawnTime <= waveTimer)
        {
            SpawnEnemy(leftToSpawn[0], leftToSpawn.Count == 1);
            leftToSpawn.RemoveAt(0);
        }

    }

    private void SpawnEnemy(Waves.Spawn spawn, bool isLast)
    {
        Enemy enemy = GameObject.Instantiate(enemyPrefab);
        string enemyPath = $"{path}/{spawn.EnemyStatsName}.json";
        if (!File.Exists(enemyPath))
        {
            string json = JsonUtility.ToJson(enemyPrefab.Stats, true);
            File.WriteAllText(enemyPath, json);
        }
        enemy.LoadEnemyStats(enemyPath);
        if (isLast) enemy.OnDie += WinGame;
    }

    private void WinGame()
    {
        if (!enabled) return;
        GameObject.FindObjectOfType<Player>().enabled = false;
        EndGame("- Win -", Color.black);
    }

    public void LooseGame()
    {
        if (!enabled) return;
        EndGame("- Lost -", Color.red);
    }

    private void EndGame(string text, Color color)
    {
        endGameCanvas.alpha = 1f;
        endGameCanvas.blocksRaycasts = true;
        endGameCanvas.interactable = true;
        endGameCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
        color.a = .8f;
        endGameCanvas.GetComponent<Image>().color = color;
        enabled = false;
    }
}
