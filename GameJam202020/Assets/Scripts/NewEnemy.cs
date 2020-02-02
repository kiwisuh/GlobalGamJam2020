  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.UI;
  using UnityEngine.AI;

  public class NewEnemy : MonoBehaviour
  {
    public float startHealth = 100;
    public float distanceAway;
    public float deathDistance = 0.5f;
    public Transform thisObject;
    public Transform target;
    public NavMeshAgent navComponent;
    private float health;
    public int worth = 50;
    public Image healthBar;
    public float sightDistance = 30;
    public bool isClosestBeacon = false;
    public bool isClosestPlayer = false;
    public bool isClosestObject = false;
    //public List<Transform> transform_targets = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
      health = startHealth;
      target.position = Vector3.zero;
      navComponent = this.gameObject.GetComponent<NavMeshAgent>();
	  target = GameObject.FindGameObjectWithTag("Tower").transform;
    }

    // Update is called once per frame
    void Update()
    {
      // List<GameObject> collisionList = new List<GameObject>();
      // Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightDistance);
      // for (int i = 0; i < hitColliders.Length; i++){
      //     if (hitColliders[i].gameObject.tag == "Beacon"){
      //       collisionList.Add(hitColliders[i].gameObject);
      //     }else if(hitColliders[i].gameObject.tag == "Player"){
      //
      //     }else if(hitColliders[i].gameObjet.tag == "Object"){
      //
      //     }else{
      //
      //     }
      // }
      // // target.position = Vector3.zero;
      // target.position = Vector3.zero;
  		// DetectClosest("Beacon");
      // print("BEACON");
  		// if(target.position == Vector3.zero){
      //   print("PLAYER");
  		// 	DetectClosest("Player");
  		// }else if(target.position == Vector3.zero){
      //   print("OBJECT");
  		// 	DetectClosest("Object");
  		// }else if(target.position == Vector3.zero){
  		// 	target.position = Vector3.zero;
  		// }
  		// //float dist = Vector3.Distance(DetectClosest(sightDistance).transform.position, transform.position);
  		// print("target position: " + target.position);
      //float dist = Vector3.Distance(target.Position, transform.position);
  		navComponent.SetDestination(target.position);


  }

  public void Damage(float amount){
    healthBar.fillAmount = health / startHealth;
    health -= amount;
    if(health <= 0){
      DestroyObject();
    }
  }

  public void DestroyObject(){
    Destroy(gameObject);
  }

  private void setTargetPosition(string name){
    target = GameObject.FindGameObjectWithTag(name).transform;
  }

  private void DetectClosest(string name)
	    {
          print(name);
          Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightDistance);

	        if (hitColliders.Length > 0)
	        {
				print("Hit colliders length: " + hitColliders.Length);
	            float dist = 0;
	            int closest = 0;
	            for (int i = 0; i < hitColliders.Length; i++)
	            {
	                if (hitColliders[i].gameObject.tag == name)
	                {
	                    if (dist == 0)
	                    {
	                        dist = Vector3.Distance(hitColliders[i].gameObject.transform.position, transform.position);
	                        closest = i;
	                    }
	                    else
	                    {
	                        if (Vector3.Distance(hitColliders[i].gameObject.transform.position, transform.position) < dist)
	                        {
	                            closest = i;
	                            dist = Vector3.Distance(hitColliders[i].gameObject.transform.position, transform.position);
	                        }
	                    }
	                }
	            }
							Vector3 closestTargetPosition = new Vector3 (hitColliders[closest].transform.position.x, hitColliders[closest].transform.position.y, hitColliders[closest].transform.position.z);
							target.position = closestTargetPosition;
              //target = hitColliders[closest].transform;
              //target = hitColliders[closest].transform;
							//print("target detect position: " + target.transform);
              print("target detect object: " + target.gameObject.name);
	            // if (dist != 0)
	            // {
	            //     //closestEnemyPos = hitColliders[closest].gameObject.transform.position;
	            //     //active = true;
	            // }
	            // else
	            // {
	            //     target = hitColliders[closest].transform;
							//
	            // }
	    }
	}
  }
