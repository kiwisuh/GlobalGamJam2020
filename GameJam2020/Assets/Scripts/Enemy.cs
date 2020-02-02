using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Transform target;
    public NavMeshAgent navComponent;
    public float sightDistance = 30;
    public float damage;
    public float stopDistance;

    public GameObject attackObj;
    public float attackDist;
    public float attackCooldown;
    public float attackTime;
    private float lastAttackTime;
    public GameObject scrapExplode;

    public float startHealth = 100;
    private float health;
    public int worth = 50;
    public Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        lastAttackTime = 0;
        health = startHealth;
        //target.position = Vector3.zero;
        navComponent = this.gameObject.GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Tower").transform;
        transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
        attackObj.SendMessage("DamageInput", damage);

    }

    public void Attack()
    {
        attackObj.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        target = DetectClosest();
        target.position = new Vector3(target.position.x, 1.0f, target.position.z);

        if(Time.time - lastAttackTime > attackCooldown)
        {
            attackObj.SetActive(false);

            if (Vector3.Distance(transform.position, target.position) < attackDist)
            {
                attackObj.SetActive(true);
                lastAttackTime = Time.time;
            }
            else
            {
                if (Vector3.Distance(transform.position, target.position) > stopDistance)
                {
                    navComponent.SetDestination(target.position);
                }
                else
                {
                    navComponent.velocity = Vector3.zero;
                }
            }

        }
        else if(Time.time - lastAttackTime > attackTime)
        {
            attackObj.SetActive(false);
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
            if (Vector3.Distance(transform.position, target.position) > stopDistance)
            {
                navComponent.SetDestination(target.position);
            }
            else
            {
                navComponent.velocity = Vector3.zero;
            }
        }
        else
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;

            attackObj.SetActive(true);
        }
    }

    public void Damage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;

        navComponent.velocity = Vector3.zero;
        if (health <= 0)
        {
            DestroyObject();
        }
    }

    private Transform DetectClosest()
    {
        List<GameObject> beaconGameObjects = new List<GameObject>();
        List<GameObject> playerGameObjects = new List<GameObject>();
        List<GameObject> objectGameObjects = new List<GameObject>();
        Transform finalClosest;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightDistance);
        if (hitColliders.Length > 0)
        {
            foreach (Collider hit in hitColliders)
            {

                if (hit.gameObject.tag == "Beacon")
                {
                    beaconGameObjects.Add(hit.gameObject);
                }
                else if (hit.gameObject.tag == "Player")
                {
                    playerGameObjects.Add(hit.gameObject);
                }
                else if (hit.gameObject.tag == "Object")
                {
                    objectGameObjects.Add(hit.gameObject);
                }
            }
        }

        if (beaconGameObjects.Count > 0)
        {
            finalClosest = getClosestDistance(beaconGameObjects).transform;
        }
        else if (playerGameObjects.Count > 0)
        {
            finalClosest = getClosestDistance(playerGameObjects).transform;
        }
        else if (objectGameObjects.Count > 0)
        {
            finalClosest = getClosestDistance(objectGameObjects).transform;
        }
        else
        {
            finalClosest = GameObject.FindGameObjectWithTag("Tower").transform;
        }

        return finalClosest;
    }

    private GameObject getClosestDistance(List<GameObject> myList)
    {
        float dist = 0;
        int closest = 0;
        for (int i = 0; i < myList.Count; i++)
        {
            if (myList[i].gameObject.tag == name)
            {
                if (dist == 0)
                {
                    dist = Vector3.Distance(myList[i].transform.position, transform.position);
                    closest = i;
                }
                else
                {
                    if (Vector3.Distance(myList[i].transform.position, transform.position) < dist)
                    {
                        closest = i;
                        dist = Vector3.Distance(myList[i].transform.position, transform.position);
                    }
                }
            }
        }//end of for loop
        return myList[closest];
    }

    public void DestroyObject()
    {
        GameObject newObject = Instantiate(scrapExplode, transform.position, Quaternion.identity);

        newObject.transform.position = new Vector3(transform.position.z, 1.0f, transform.position.x);

        Destroy(gameObject);
    }
}
