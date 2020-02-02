

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	//private int SpawnID;

	private bool waveSpawn = false;
	public bool Spawn = true;
	public SpawnTypes spawnType = SpawnTypes.Normal;
	public GameObject[] spawnPoints;

	public float waveTimer = 30.0f;
	private float timeTillWave = 0.0f;
	//Wave controls
	public int totalWaves = 5;
	private int numWaves = 0;

	void Start()
	{

		//SpawnID = Random.Range(1, 500);
		//EasyEnemy = Instantiate(Resources.Load("Prefabs/Enemy", typeof(GameObject))) as GameObject;
		Instantiate(EasyEnemy);
	}

	void OnDrawGizmos()
	{

		Gizmos.color = gizmoColor;

		Gizmos.DrawCube(transform.position, new Vector3 (0.5f,0.5f,0.5f));
	}
	void Update ()
	{
		if(Spawn)
		{
			if (spawnType == SpawnTypes.Normal)
			{
				// if(numEnemy < totalEnemy)
				// {
				// 	spawnEnemy();
				// }
				// if(numEnemy == 0){
				// 	spawnEnemy();
				// }
			}
			// Spawns enemies only once
			// else if (spawnType == SpawnTypes.Once)
			// {
			// 	// checks to see if the overall spawned num of enemies is more or equal to the total to be spawned
			// 	if(spawnedEnemy >= totalEnemy)
			// 	{
			// 		//sets the spawner to false
			// 		Spawn = false;
			// 	}
			// 	else
			// 	{
			// 		// spawns an enemy
			// 		spawnEnemy();
			// 	}
			// }
			else if (spawnType == SpawnTypes.Wave)
			{
				if(numWaves < totalWaves + 1)
				{
					if (waveSpawn)
					{

						spawnEnemy();
					}
					if (numEnemy == 0)
					{

						waveSpawn = true;
						rounds++;
						totalEnemy+=10;
					}
					if(numEnemy == totalEnemy)
					{

						waveSpawn = false;
					}
				}
			}
			// else if(spawnType == SpawnTypes.TimedWave)
			// {
			//
			// 	if(numWaves <= totalWaves)
			// 	{
			// 		// Increases the timer to allow the timed waves to work
			// 		timeTillWave += Time.deltaTime;
			// 		if (waveSpawn)
			// 		{
			// 			//spawns an enemy
			// 			spawnEnemy();
			// 		}
			// 		// checks if the time is equal to the time required for a new wave
			// 		if (timeTillWave >= waveTimer)
			// 		{
			// 			// enables the wave spawner
			// 			waveSpawn = true;
			// 			// sets the time back to zero
			// 			timeTillWave = 0.0f;
			// 			// increases the number of waves
			// 			numWaves++;
			// 			// A hack to get it to spawn the same number of enemies regardless of how many have been killed
			// 			numEnemy = 0;
			// 		}
			// 		if(numEnemy >= totalEnemy)
			// 		{
			// 			// diables the wave spawner
			// 			waveSpawn = false;
			// 		}
			// 	}
			// 	else
			// 	{
			// 		Spawn = false;
			// 	}
			// }
		}

		if(Input.GetKeyDown(KeyCode.W)){
			killEnemy();
		}
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
