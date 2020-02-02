using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class NewNewEnemy : MonoBehaviour
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
      target = DetectClosest();
      navComponent.SetDestination(target.position);
    }

    private Transform DetectClosest(){
      List<GameObject> beaconGameObjects = new List<GameObject>();
      List<GameObject> playerGameObjects = new List<GameObject>();
      List<GameObject> objectGameObjects = new List<GameObject>();
      Transform finalClosest;
      Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightDistance);
      if (hitColliders.Length > 0){
        foreach(Collider hit in hitColliders){

          if(hit.gameObject.tag == "Beacon"){
            beaconGameObjects.Add(hit.gameObject);
          }else if(hit.gameObject.tag == "Player"){
            playerGameObjects.Add(hit.gameObject);
          }else if(hit.gameObject.tag == "Object"){
            objectGameObjects.Add(hit.gameObject);
          }
        }
      }

      if(beaconGameObjects.Count > 0){
        finalClosest = getClosestDistance(beaconGameObjects).transform;
      }else if(playerGameObjects.Count > 0){
        finalClosest = getClosestDistance(playerGameObjects).transform;
      }else if(objectGameObjects.Count > 0){
        finalClosest = getClosestDistance(objectGameObjects).transform;
      }else{
        finalClosest = GameObject.FindGameObjectWithTag("Tower").transform;
      }

      return finalClosest;
    }

    private GameObject getClosestDistance(List<GameObject> myList){
      float dist = 0;
      int closest = 0;
      for (int i = 0; i < myList.Count; i++){
          if (myList[i].gameObject.tag == name){
              if (dist == 0){
                  dist = Vector3.Distance(myList[i].transform.position, transform.position);
                  closest = i;
              }
              else{
                  if (Vector3.Distance(myList[i].transform.position, transform.position) < dist){
                      closest = i;
                      dist = Vector3.Distance(myList[i].transform.position, transform.position);
                  }
              }
          }
      }//end of for loop
      return myList[closest];
    }

}
