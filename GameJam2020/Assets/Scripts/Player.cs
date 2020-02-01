using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int playerNumber = 1;
    public float speed;
    private bool isDead = false;
    private Vector3 movementInput;
    private Vector3 turnInput;
    private Rigidbody rb;
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

    enum PlayerMode{ Creator, Healer, Engineer, Modder};
    PlayerMode mode;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        moveXAxisName = "LVertical" + playerNumber;
        moveYAxisName = "LHorizontal" + playerNumber;
        turnXAxisName = "RVertical" + playerNumber;
        turnYAxisName = "RHorizontal" + playerNumber;
        fireAxisName = "Fire" + playerNumber;
        cam = GameObject.FindGameObjectWithTag("CamRig");
        lastTimeGunFired = Time.time - gunCooldown;
        gunDisable = false;
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
    }

    // Update is called once per frame
    void Update()
    {

        if (isDead)
        {
            rb.isKinematic = false;

        }
        else
        {
            rb.isKinematic = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mode = PlayerMode.Creator;
            Debug.Log("1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("2");

            mode = PlayerMode.Healer;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("3");

            mode = PlayerMode.Engineer;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("4");

            mode = PlayerMode.Modder;
        }

        movementInput.z = -Input.GetAxis(moveXAxisName);
        movementInput.x = Input.GetAxis(moveYAxisName);
        turnInput.x = Input.GetAxis(turnXAxisName);
        turnInput.z = -Input.GetAxis(turnYAxisName);
        fireInput = Input.GetAxis(fireAxisName);
        Turn();
        Move();

        if(mode == PlayerMode.Creator || mode == PlayerMode.Engineer)
        {
            if (fireInput > 0 && Time.time - lastTimeGunFired > gunCooldown)
            {
                Shoot();
                lastTimeGunFired = Time.time;
            }
        }
        else
        {

        }
        

        

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
        }else if (mode == PlayerMode.Creator)
        {
            laserFired.SendMessage("SetType", "nail");

            //nail gun
        }
    }

}
