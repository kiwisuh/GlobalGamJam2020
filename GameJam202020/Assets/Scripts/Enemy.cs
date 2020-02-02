using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
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
	//public List<Transform> transform_targets = new List<Transform>();
	// Start is called before the first frame update
	void Start()
	{
		health = startHealth;
		target.position = Vector3.zero;
		navComponent = this.gameObject.GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update()
	{
		target.position = Vector3.zero;
		DetectClosest("Beacon");
		if(target.position == Vector3.zero){
			DetectClosest("Player");
		}else if(target.position == Vector3.zero){
			DetectClosest("Object");
		}else if(target.position == Vector3.zero){
			target.position = Vector3.zero;
		}
		//float dist = Vector3.Distance(DetectClosest(sightDistance).transform.position, transform.position);
		print("target position: " + target.position);
			navComponent.SetDestination(target.position);

			//if(dist <= deathDistance){
				//attack player
			//}

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

	private void DetectClosest(string name)
	    {

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
							print("target detect position: " + target.transform);
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
