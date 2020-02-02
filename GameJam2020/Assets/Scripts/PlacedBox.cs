using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacedBox : MonoBehaviour
{

    enum Mode {Turret, PulseTurret, Bomb, Beacon, Health, Buff};
    Mode mode;
    public int level = 1;
    
    private Renderer rend;

    public GameObject[] turretObj;
    public GameObject[] healthObj;
    public GameObject[] beaconObj;
    public GameObject[] pulseTurretObj;
    public GameObject[] bombObj;
    public GameObject[] buffObj;
    public GameObject stillObj1;
    public GameObject stillObj2;


    private CapsuleCollider capCol;

    public GameObject currentObj;
    public GameObject objHolder;

    public GameObject TriImg;

    public float sightDistance;
    private Vector3 closestEnemyPos;
    private Vector3 closestPlayerPos;
    private bool active;

    public bool bensActivateButton;

    //health
    public float startHealth = 100;
    private float health;
    public Image healthBar;
    public float healthIncrease;

    //Turret
    public GameObject projectile;
    public Transform gun;
    public Transform gun1;
    public Transform gun2;
    public float gunDamage;
    public float gunDamageIncrease;
    public float turretCooldown;
    public float turretCooldownDecrease;
    private float timeLastShot;

    //pulseTurret
    public float pulseTurretCooldown;
    public float rotateSpeed;

    //beaconbomb
    public float explosionRadius;
    public float explosionDamage;
    public float explisionDamageIncrease;

    //buff
    public float buffLength;
    public float buffDamage;
    public float buffDamageIncrease;
    public float buffLengthIncrease;

    //heal
    public float healAmount;
    public float healAmountIncrease;
    private float lastTimeHealed;
    public float healCooldown;
    public float healCooldownDecrease;

    //beacon damage
    public float beaconDamage = 0;
    public float beaconDamageIncrease;

    // Start is called before the first frame update
    void Start()
    {
        capCol = GetComponent<CapsuleCollider>();
        health = startHealth;
        timeLastShot = Time.time - turretCooldown;
        lastTimeHealed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rend = GetComponent<Renderer>();

        if (Input.GetKeyDown(KeyCode.S))
        {
            Damage(20);
        }
        if (mode == Mode.Turret)
        {
            
            DetectClosest("Enemy");
            if (active)
            {
                Vector3 saveValues = transform.eulerAngles;
                this.transform.LookAt(closestEnemyPos);
                transform.eulerAngles = new Vector3(saveValues.x, transform.eulerAngles.y, saveValues.z);
                stillObj1.transform.eulerAngles = Vector3.zero;
                stillObj2.transform.eulerAngles = Vector3.zero;
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

            stillObj1.transform.eulerAngles = Vector3.zero;
            stillObj2.transform.eulerAngles = Vector3.zero;
            if (Time.time - timeLastShot > pulseTurretCooldown)
            {
                timeLastShot = Time.time;
                Shoot();
            }
        }
        else if (mode == Mode.Buff)
        {
            GetNearbyPlayers();
        }
        else if (mode == Mode.Health)
        {
            if(Time.time - lastTimeHealed > healCooldownDecrease)
            {
                GetNearbyPlayers();
            }
        }
        
    }

    public void Heal(float amount)
    {
        health += amount*level;
        if (health > startHealth)
        {
            health = startHealth;
        }
        healthBar.fillAmount = health / startHealth;

    }

    private void GetNearbyPlayers()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightDistance);
        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.tag == "Player")
                {
                    if (mode == Mode.Buff)
                    {
                        hitColliders[i].SendMessage("BuffLength", buffLength);
                        hitColliders[i].SendMessage("BuffDamage", buffDamage);
                    }
                    else if(mode == Mode.Health)
                    {
                        hitColliders[i].SendMessage("Heal", healAmount);
                        lastTimeHealed = Time.time;
                    }
                }
            }
        }
    }

    private void DetectClosest(string name)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightDistance);

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

    public void SetType(string type)
    {
        capCol = GetComponent<CapsuleCollider>();
        if (type == "turret")
        {
            mode = Mode.Turret;
            ChooseSkin();
            stillObj1.SetActive(true);
            stillObj2.SetActive(false);
            capCol.radius = 2f;
            //rend.material = turretMat;
        }
        else if (type == "pulseTurret")
        {
            mode = Mode.PulseTurret;
            ChooseSkin();
            stillObj2.SetActive(true);
            stillObj1.SetActive(false);
            capCol.radius = 2f;
        }
        else if (type == "beacon")
        {
            mode = Mode.Beacon;
            ChooseSkin();
            transform.tag = "Beacon";
            //rend.material = beaconMat;
            capCol.radius = 0.7f;
        }
        else if (type == "bomb")
        {
            transform.tag = "Beacon";
            mode = Mode.Bomb;
            ChooseSkin();
            capCol.radius = 0.5f;

        }
        else if (type == "buff")
        {
            mode = Mode.Buff;
            ChooseSkin();

        }
        else if (type == "health")
        {
            //rend.material = healthMat;
            mode = Mode.Health;
            ChooseSkin();
            GetNearbyPlayers();

        }
    }

    public void Shoot()
    {
        if (mode == Mode.Turret)
        {
            if(level < 3)
            {

                GameObject laserFired = Instantiate(projectile, gun.position, Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up));
                laserFired.SendMessage("SetType", "bullet");
                laserFired.SendMessage("DamageInput", gunDamage);
            }
            else
            {
                GameObject laserFired1 = Instantiate(projectile, gun1.position, Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up));
                laserFired1.SendMessage("SetType", "bullet");
                laserFired1.SendMessage("DamageInput", gunDamage);

                GameObject laserFired2 = Instantiate(projectile, gun2.position, Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up));
                laserFired2.SendMessage("SetType", "bullet");
                laserFired2.SendMessage("DamageInput", gunDamage);
            }
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
                laserFired.SendMessage("DamageInput", gunDamage);

            }
        }
    }

    public void ReturnLevel(GameObject obj)
    {
        obj.SendMessage("LevelReturn", level);
        obj.SendMessage("LevelMeUp", gameObject);
    }

    public void GetHit(GameObject obj)
    {
        if (mode == Mode.Beacon)
        {
            obj.SendMessage("Damage", beaconDamage);
        }
    }

    public void LevelUp()
    {
        
        if (level != 3)
        {
            
            if(level == 1)
            {
                level = 2;
            }
            else if (level == 2)
            {
                level = 3;
            }
            //health
            //heal amount, heal time, buff amount, buff time, damage, shoot speed, 
            healAmount *= healAmountIncrease;
            healCooldown -= healCooldownDecrease;
            buffLength += buffLengthIncrease;
            buffDamage *= buffDamageIncrease;
            gunDamage *= gunDamageIncrease;
            beaconDamage += beaconDamageIncrease;
            explosionDamage += explisionDamageIncrease;
            health += (healthIncrease *(level-1));
            startHealth += (healthIncrease * (level - 1));
            ChooseSkin();
            healthBar.fillAmount = health / startHealth;


        }

    }

    public void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightDistance);

        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.tag == "Enemy")
                {
                    hitColliders[i].gameObject.SendMessage("Damage", explosionDamage);
                }
            }
        }
        DestroyObject();
        
    }

    public void Damage(float dmg)
    {
        health -= dmg;
        healthBar.fillAmount = health / startHealth;

        if (health < 0)
        {
            DestroyObject();
        }
        if(mode == Mode.Bomb)
        {
            Explode();
        }
    }

    public void Mod()
    {
        if(mode == Mode.Beacon)
        {
            mode = Mode.Bomb;
            SetType("bomb");
        }
        else if (mode == Mode.Bomb)
        {
            mode = Mode.Beacon;
            SetType("beacon");
        }
        else if (mode == Mode.Buff)
        {
            mode = Mode.Health;
            SetType("health");
        }
        else if(mode == Mode.Health)
        {
            mode = Mode.Buff;
            SetType("buff");
        }
        else if (mode == Mode.PulseTurret)
        {
            mode = Mode.Turret;
            SetType("turret");
        }
        else if (mode == Mode.Turret)
        {
            mode = Mode.PulseTurret;
            SetType("pulseTurret");
        }
    }
    
    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void ChooseSkin()
    {
        foreach (Transform child in objHolder.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (mode == Mode.Beacon)
        {
            beaconObj[level - 1].SetActive(true);
        }
        else if (mode == Mode.Bomb)
        {
            bombObj[level - 1].SetActive(true);
        }
        else if (mode == Mode.Buff)
        {
            buffObj[level - 1].SetActive(true);
        }
        else if (mode == Mode.Health)
        {
            healthObj[level - 1].SetActive(true);
        }
        else if (mode == Mode.PulseTurret)
        {
            pulseTurretObj[level - 1].SetActive(true);
        }
        else if (mode == Mode.Turret)
        {
            turretObj[level - 1].SetActive(true);
        }
    }

    public void ToggleTriangle(bool on)
    {
        TriImg.SetActive(on);
    }
}
