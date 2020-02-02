using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float startHealth = 100;
    private float health;
    public Image healthBar;

    public Text[] playerScrap;

    public GameObject renderObj;
    private Vector3 startPos;

    public GameObject[] gunObj;
    bool isDead = false;

    public int scrap;
    public int levelUpPrice;
    public int modPrice;
    private int levelChecked;
    private float lastTimeLevelUp;
    public float levelCooldown;

    public int playerNumber = 1;
    public float speed;
    private bool isDisabled = false;
    private Vector3 movementInput;
    private Vector3 turnInput;
    private Rigidbody rb;
    private bool interactInput;

    private string interactName;
    private string moveXAxisName;
    private string moveYAxisName;
    private string turnXAxisName;
    private string turnYAxisName;
    private string fireAxisName;
    private float fireInput;
    private GameObject cam;
    private float playerAngle;

    public GameObject projectile;
    public Transform gun;
    private bool gunDisable;
    private float lastTimeGunFired;
    public float gunCooldown;
    public float damage;
    public GameObject AttackObj;
    public float rangeDamage;
    public float rangeTick;

    private float lastHealTick;
    public float healCooldown;
    public int healAmount;

    enum PlayerMode{ Creator, Healer, Engineer, Modder};
    PlayerMode mode;

    //buff variables
    private bool buffed;
    public GameObject buffObj;
    public float damageBoost;
    private float boostCooldown;
    private float lastBoostTime;

    //Creator

    public int objectCost;
    public GameObject placeObject;
    public GameObject creationUI;
    private bool placementMode;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        startPos = transform.position;
        lastBoostTime = 0;
        boostCooldown = 0;
        buffed = false;
        health = startHealth;
        creationUI.SetActive(false);
        buffObj.SetActive(false);
        interactName = "Interact" + playerNumber;
        moveXAxisName = "LVertical" + playerNumber;
        moveYAxisName = "LHorizontal" + playerNumber;
        turnXAxisName = "RVertical" + playerNumber;
        turnYAxisName = "RHorizontal" + playerNumber;
        fireAxisName = "Fire" + playerNumber;
        cam = GameObject.FindGameObjectWithTag("CamRig");
        lastTimeGunFired = Time.time - gunCooldown;
        gunDisable = false;
        damageBoost = 1;
        
        if (playerNumber == 1)
        {
            mode = PlayerMode.Creator;
        }
        else if (playerNumber == 2)
        {
            mode = PlayerMode.Engineer;
        }
        else if (playerNumber == 3)
        {
            mode = PlayerMode.Healer;
        }
        else if (playerNumber == 4)
        {
            mode = PlayerMode.Modder;
        }
        gunObj[playerNumber - 1].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        playerScrap[playerNumber-1].text = "Player " + playerNumber + ": " + scrap;
        //Get input names
        movementInput.z = -Input.GetAxis(moveXAxisName);
        movementInput.x = Input.GetAxis(moveYAxisName);
        turnInput.x = Input.GetAxis(turnXAxisName);
        turnInput.z = -Input.GetAxis(turnYAxisName);
        fireInput = Input.GetAxis(fireAxisName);


        if (Input.GetButtonDown(interactName))
        {
            interactInput = true;
        }
        else
        {
            interactInput = false;
        }

        if(mode == PlayerMode.Healer)
        {
            if (Input.GetButton(interactName))
            {
                interactInput = true;
            }
            else
            {
                interactInput = false;
            }
        }

        //print(interactInput);
        //make sure they aint dead
        if (isDisabled)
        {
            rb.isKinematic = true;
            
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Respawn();
        }
        else
        {
            rb.isKinematic = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mode = PlayerMode.Creator;
            gunObj[0].SetActive(true);
            gunObj[1].SetActive(false);
            gunObj[2].SetActive(false);
            gunObj[3].SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gunObj[0].SetActive(false);
            gunObj[1].SetActive(true);
            gunObj[2].SetActive(false);
            gunObj[3].SetActive(false);
            mode = PlayerMode.Healer;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gunObj[0].SetActive(false);
            gunObj[1].SetActive(false);
            gunObj[2].SetActive(true);
            gunObj[3].SetActive(false);
            mode = PlayerMode.Engineer;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gunObj[0].SetActive(false);
            gunObj[1].SetActive(false);
            gunObj[2].SetActive(false);
            gunObj[3].SetActive(true);
            mode = PlayerMode.Modder;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Damage(20);
        }



        if(Time.time - lastBoostTime > boostCooldown)
        {
            buffObj.SetActive(false);
            boostCooldown = 0;
            damageBoost = 1.0f;
        }

        if (placementMode)
        {
            bool placed = false;
            string message = "";
            if (Input.GetButtonDown("X" + playerNumber))
            {
                placed = true;
                message = "turret";
            }
            else if (Input.GetButtonDown("Y" + playerNumber))
            {
                placed = true;
                message = "health";
            }
            else if (Input.GetButtonDown("B" + playerNumber))
            {
                placed = true;
                message = "beacon";
            }
            if (placed)
            {
                GameObject newObject = Instantiate(placeObject, transform.position+transform.forward, Quaternion.identity);
                newObject.SendMessage("SetType", message);
                creationUI.SetActive(false);
                placementMode = false;
            }
        }
        else
        {
            Turn();
            Move();
        }
        


        if (mode == PlayerMode.Creator)
        {
            if (fireInput > 0 && Time.time - lastTimeGunFired > gunCooldown)
            {
                Shoot();
                lastTimeGunFired = Time.time;
            }
            if (interactInput && scrap >= objectCost)
            {
                scrap -= objectCost;

                creationUI.SetActive(true);
                placementMode = true;
            }
        }
        else if(mode == PlayerMode.Healer)
        {

        }
        else if (mode == PlayerMode.Engineer)
        {
            if (fireInput > 0 && Time.time - lastTimeGunFired > gunCooldown)
            {
                Shoot();
                lastTimeGunFired = Time.time;
            }
        }


        if (mode == PlayerMode.Modder || mode == PlayerMode.Healer)
        {
            print(fireInput);
            if(fireInput > 0)
            {
                AttackObj.SetActive(true);
                AttackObj.SendMessage("SetDamage", rangeDamage*damageBoost);
                AttackObj.SendMessage("SetTick", rangeTick);
            }
            else
            {
                AttackObj.SetActive(false);
            }
        }
        else
        {
            AttackObj.SetActive(false);

        }
    }

    public void BuffLength(float length)
    {
        lastBoostTime = Time.time;
        boostCooldown = length;
        buffObj.SetActive(true);
    }

    public void BuffDamage(float dmg)
    {
        damageBoost = dmg;
    }

    public void Heal(float amount)
    {
        health += amount;
        if(health > startHealth)
        {
            health = startHealth;
        }
        healthBar.fillAmount = health / startHealth;

    }

    public void AddScrap(int amount)
    {
        scrap += amount;
    }

    private void Move()
    {

        Vector3 movement = new Vector3(movementInput.x * speed * Time.deltaTime, 0, movementInput.z * speed * Time.deltaTime);
        movement = cam.transform.TransformDirection(movement);
        rb.MovePosition(rb.position + movement);
    }

    private void Turn()
    {
        if (turnInput.x != 0 || turnInput.z != 0)
        {
            playerAngle = (Mathf.Atan2((0 - turnInput.x), (0 - turnInput.z)) * Mathf.Rad2Deg) - 60 - cam.transform.eulerAngles.y;
            transform.rotation = Quaternion.AngleAxis(playerAngle, Vector3.up);
        }
    }

    private void Shoot()
    {
        
        GameObject laserFired = Instantiate(projectile, gun.position, Quaternion.AngleAxis(playerAngle, Vector3.up));
        
        if (mode == PlayerMode.Engineer)
        {
            laserFired.SendMessage("SetType", "sawblade");
            laserFired.SendMessage("DamageInput", (damage*2)* damageBoost);
        }
        else if (mode == PlayerMode.Creator)
        {
            laserFired.SendMessage("SetType", "nail");
            laserFired.SendMessage("DamageInput", damage* damageBoost);
            //nail gun
        }
    }

    public void LevelReturn(int level)
    {
        levelChecked = level;
    }

    public void LevelMeUp(GameObject obj)
    {
        if(levelChecked != 3)
        {
            scrap -= levelUpPrice;
            obj.SendMessage("LevelUp");
        }
    }

    public void Damage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;
        //Debug.Log(health);
        if (health <= 0)
        {
            print("dead");
            isDead = true;
            isDisabled = true;
            renderObj.SetActive(false);
            transform.position = Vector3.zero;
            transform.tag = "DeadPlayer";
            //DestroyObject();
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void Respawn()
    {
        if (isDead)
        {
            transform.position = startPos;
            health = startHealth;
            isDead = false;
            isDisabled = false;
            renderObj.SetActive(true);
            healthBar.fillAmount = health / startHealth;
            gameObject.tag = "Player";
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (mode == PlayerMode.Engineer && (col.tag == "Object" || col.tag == "Beacon"))
        {
            col.gameObject.SendMessage("ToggleTriangle", true);

            if (Input.GetButtonDown(interactName) && scrap > levelUpPrice && Time.time - lastTimeLevelUp > levelCooldown)
            {
                if (col.tag == "Object" || col.tag == "Beacon")
                {
                    lastTimeLevelUp = Time.time;
                    col.gameObject.SendMessage("ReturnLevel", gameObject);
                }
            }
        }
        else if (mode == PlayerMode.Modder && (col.tag == "Object" || col.tag == "Beacon"))
        {
            col.gameObject.SendMessage("ToggleTriangle", true);

            if (Input.GetButtonDown(interactName) && scrap > modPrice)
            {
                if (col.tag == "Object" || col.tag == "Beacon")
                {
                    scrap -= modPrice;
                    col.gameObject.SendMessage("Mod");
                }
            }
        }
        else if (mode == PlayerMode.Healer && (col.tag == "Object" || col.tag == "Beacon"))
        {
            col.gameObject.SendMessage("ToggleTriangle", true);
            if (interactInput && scrap > 0 && Time.time - lastHealTick > healCooldown)
            {
                lastHealTick = Time.time;
                col.gameObject.SendMessage("Heal", healAmount);
                scrap--;
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Object" || col.tag == "Beacon")
        {
            col.gameObject.SendMessage("ToggleTriangle", false);
            
        }
    }

}
