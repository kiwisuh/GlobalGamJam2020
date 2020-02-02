using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{

    private GameObject[] players;
    public bool gameOver = false;
    public GameObject enemy;
    public float spawnTime;
    private float lastSpawn;
    public static int wave;
    public int numEnemies;
    private int currentEnemies;
    public Text waveText;
    public Text enemiesText;
    public Text bigText;
    private bool waveComplete;
    private float timeUntilNextWave;
    public int timeBetweenWaves;
    public float spawningSpeedIncreaseByWave;
    public int startingEnemies;
    public float SpawnHeight;
    public float spawnDistance;
    public static int playersInGame;



    // Start is called before the first frame update
    void Start()
    {
        playersInGame = 4;
        waveComplete = false;
        currentEnemies = 0;
        wave = 1;
        lastSpawn = Time.time;
        waveText.text = "Wave " + wave;
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (waveComplete)
            {
                int countdownTime = timeBetweenWaves - (Mathf.RoundToInt(Time.time) - Mathf.RoundToInt(timeUntilNextWave));

                if (countdownTime <= 0)
                {
                    nextWave();
                    bigText.text = "";
                }
                else
                {
                    bigText.text = "Wave complete! Next wave in: " + countdownTime;
                }
            }
            if (currentEnemies < startingEnemies + (wave * numEnemies))
            {

                if (Time.time - lastSpawn > spawnTime)
                {
                    currentEnemies++;
                    lastSpawn = Time.time;
                    int angle = Random.Range(1, 360);
                    Vector3 spawnPoint = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnDistance;
                    spawnPoint.y = SpawnHeight;
                    GameObject enemySpawned = Instantiate(enemy, spawnPoint, Quaternion.identity);
                }
                enemiesText.text = "Enemies Remaining: " + (GameObject.FindGameObjectsWithTag("Enemy").Length + (startingEnemies + (wave * numEnemies) - currentEnemies));
            }
            else if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !waveComplete)
            {
                waveComplete = true;

                foreach (GameObject player in players)
                {
                    player.SendMessage("Respawn");
                }
                timeUntilNextWave = Time.time;
            }
            else
            {
                enemiesText.text = "Enemies Remaining: " + GameObject.FindGameObjectsWithTag("Enemy").Length;
            }
        }
    }
    void nextWave()
    {
        spawnTime -= spawningSpeedIncreaseByWave;
        wave += 1;
        waveComplete = false;
        currentEnemies = 0;
        waveText.text = "Wave " + wave;
    }
}
