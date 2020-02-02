

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{

	public Color gizmoColor = Color.red;

	public enum SpawnTypes
    {
		Normal,
		Once,
		Wave,
		TimedWave
    }

	public GameObject EasyEnemy;

	public int rounds = 0;

	public int totalEnemy = 10;
	public int numEnemy = 0;
	private int spawnedEnemy = 0;
	public int score = 0;
	public float timeLeft = 5.0f;
	public Text displayText;
	//private int SpawnID;

	private bool waveSpawn = false;
	public bool Spawn = false;
	public SpawnTypes spawnType = SpawnTypes.Normal;
	public GameObject[] spawnPoints;

	public float waveTimer = 30.0f;
	private float timeTillWave = 0.0f;
	//Wave controls
	//public int totalWaves = 5;
	private int numWaves = 0;

	void Start()
	{

		Instantiate(EasyEnemy);
		spawnType = SpawnTypes.Wave;
	}

	void Update ()
	{
		timeLeft -= Time.deltaTime;
		print("time left: " + timeLeft);
		displayText.text = "CountDown: " + timeLeft.ToString("F0");
		if(timeLeft <= 0){
			print("start the game!");
			displayText.text = "Start!";
			Spawn = true;
		}else{
			Spawn = false;
		}

		if(Spawn)
		{
			displayText.text = "Round " + rounds.ToString();
			if (waveSpawn){
				spawnEnemy();
			}
			if (numEnemy == 0){
				waveSpawn = true;
				rounds++;
				totalEnemy+=10;
			}
			if(numEnemy == totalEnemy){
				waveSpawn = false;
			}

		}

		if(Input.GetKeyDown(KeyCode.W)){
			killEnemy();
		}
	}
	private void countSpawn(){
		//waveSpawn = true;
		rounds++;
		totalEnemy+=10;
		Spawn = false;
		timeLeft = 5.0f;
	}
	// spawns an enemy based on the enemy level that you selected
	private void spawnEnemy()
	{
		float spawnPointX = Random.Range(-10, 10);
		float spawnPointZ = Random.Range(-10, 10);
		Vector3 spawnPosition = new Vector3(spawnPointX, 0.5f, spawnPointZ);

		GameObject Enemy = (GameObject) Instantiate(EasyEnemy, spawnPosition, Quaternion.identity);
		// Increase the total number of enemies spawned and the number of spawned enemies
		numEnemy++;
		spawnedEnemy++;
	}
	// Call this function from the enemy when it "dies" to remove an enemy count
	public void killEnemy()
	{
		// if the enemy's spawnId is equal to this spawnersID then remove an enemy count
		//if (SpawnID == sID)
		//{
		//	numEnemy--;
		//}
		print("killing enemy");
		score++;
		numEnemy--;
		Destroy(GameObject.FindGameObjectWithTag ("Enemy"));
	}
	//enable the spawner based on spawnerID
	// public void enableSpawner(int sID)
	// {
	// 	if (SpawnID == sID)
	// 	{
	// 		Spawn = true;
	// 	}
	// }
	// //disable the spawner based on spawnerID
	// public void disableSpawner(int sID)
	// {
	// 	if(SpawnID == sID)
	// 	{
	// 		Spawn = false;
	// 	}
	// }

	// Enable the spawner, useful for trigger events because you don't know the spawner's ID.
	public void enableTrigger()
	{
		Spawn = true;
	}
}
