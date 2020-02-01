using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedBox : MonoBehaviour
{

    enum Mode {Turret, PulseTurret, Bomb, Beacon, Health, Buff};
    Mode mode;
    public float level = 1;


    public float sightDistance;
    private Vector3 closestEnemyPos;
    private Vector3 closestPlayerPos;
    private bool active;

    public bool bensActivateButton;

    //Turret
    public GameObject projectile;
    public Transform gun;
    public float turretCooldown;
    private float timeLastShot;

    //pulseTurret
    public float pulseTurretCooldown;
    public float rotateSpeed;

    //beaconbomb
    public float explosionRadius;
    public float explosionDamage;

    // Start is called before the first frame update
    void Start()
    {
        mode = Mode.PulseTurret;
        timeLastShot = Time.time - turretCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == Mode.Turret)
        {
            DetectClosest("Enemy");
            if (active)
            {
                this.transform.LookAt(closestEnemyPos);
                if (Time.time - timeLastShot > turretCooldown)
                {
                    timeLastShot = Time.time;
                    Shoot();
                }
            }
        }
        else if (mode == Mode.PulseTurret)
        {
            transform.eulerAngles += new Vector3(0.0f, rotateSpeed * Time.deltaTime, 0.0f);

            if (Time.time - timeLastShot > pulseTurretCooldown)
            {
                timeLastShot = Time.time;
                Shoot();
            }
        }
        
    }


    private void DetectClosest(string name)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightDistance);
        Debug.Log(hitColliders);
        if (hitColliders.Length > 0)
        {
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
            if (dist != 0)
            {
                closestEnemyPos = hitColliders[closest].gameObject.transform.position;
                active = true;
            }
            else
            {
                active = false;
                
            }
        }
    }

    public void Shoot()
    {
        if (mode == Mode.Turret)
        {
            GameObject laserFired = Instantiate(projectile, gun.position, Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up));
            laserFired.SendMessage("SetType", "bullet");
        }
        else if (mode == Mode.PulseTurret)
        {
            float angle = 0f;
            float numShots;
            if (level == 1)
            {
                numShots = 4;
            }
            else
            {
                numShots = 8;
            }
            for (int i = 0; i < numShots; i++)
            {
                angle += 360f / numShots;

                GameObject laserFired = Instantiate(projectile, transform.position, Quaternion.AngleAxis(transform.eulerAngles.y + angle, Vector3.up));
                laserFired.SendMessage("SetType", "bullet");

            }
        }
        else if (mode == Mode.Bomb)
        {

        }
    }

    public void LevelUp()
    {
        level += 1;
        if(level > 3)
        {
            level = 3;
        }
    }

    public void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightDistance);

        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.tag == name)
                {
                    hitColliders[i].gameObject.SendMessage("Damage", explosionDamage);
                }
            }
        }
        DestroyObject();
        
    }

    public void Damage()
    {
        if(mode == Mode.Bomb)
        {
            Explode();
        }
    }


    public void DestroyObject()
    {
        Destroy(gameObject);
    }

}
