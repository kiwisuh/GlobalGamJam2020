using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Creator : MonoBehaviour
{

	private int creatorType;
	public Image inventory;
	public Image mheal;
	public Image mbecaon;
	public Image mturret;
	public GameObject player;
	public Material newMat;
	public bool showInventory;
    // Start is called before the first frame update
    void Start()
    {
		newMat = Resources.Load("Materials/HealthMaterial", typeof(Material)) as Material;
    }

    // Update is called once per frame
    void Update()
    {
			if(Input.GetKeyDown(KeyCode.I)){
			showInventory = true;
				selectOption();
			}
		if(showInventory){
			if(Input.GetKeyDown(KeyCode.W)){
				print("HHHHH");
				createHealthPool();
			}if(Input.GetKeyDown(KeyCode.A)){
				createBeacon();
			}if(Input.GetKeyDown(KeyCode.S)){
				createTurret();
			}
		}

    }

		public void selectOption(){
		print("test");
			inventory.color = new Color(1,1,1,1);
			mheal.color = new Color(0,1,0,1);
			mbecaon.color = new Color(0,0,1,1);
			mturret.color = new Color(1,0,0,1);



		}

		public void createBeacon(){

		}

		public void createTurret(){

		}

		public void createHealthPool(){
			print("HealthPOOL BITCH");
			creatorType = 1;
			player.GetComponent<MeshRenderer>().material = newMat;
		}
}
