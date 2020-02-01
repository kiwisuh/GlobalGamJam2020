using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float lifetime;
    public float nailLifetime;
    public float sawbladeLifetime;
    public float bulletLifetime;
    public float speed;
    public float damage;
    enum Mode {Nail, Sawblade, Bullet};
    Mode mode;
    public GameObject sawBlade;
    public GameObject nail;
    public GameObject bullet;
    private BoxCollider bc;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        //isNail = true;
        bc = GetComponent<BoxCollider>();
        if (mode == Mode.Sawblade)
        {
            speed /= 2;
            sawBlade.SetActive(true);
            bullet.SetActive(false);
            nail.SetActive(false);
            bc.size = new Vector3(1.0f, 0.1f, 1.0f);
        }
        else if(mode == Mode.Nail)
        {
            sawBlade.SetActive(false);
            bullet.SetActive(false);
            nail.SetActive(true);
        }
        else if (mode == Mode.Bullet)
        {
            sawBlade.SetActive(false);
            bullet.SetActive(true);
            nail.SetActive(false);
            speed *= 1.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (transform.forward * speed * Time.deltaTime);
        if(Time.time - startTime > lifetime)
        {
            DestroyObject();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Enemy")
        {
            //DestroyObject();
            col.SendMessage("Damage", damage);
        }
        else
        {
            //DestroyObject();
        }
       
    }


    void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void SetType(string type)
    {
        if(type == "sawblade")
        {
            lifetime = sawbladeLifetime;
            mode = Mode.Sawblade;
        }
        else if(type == "nail")
        {
            lifetime = nailLifetime;
            mode = Mode.Sawblade;
        }
        else if(type == "bullet")
        {
            lifetime = bulletLifetime;
            mode = Mode.Bullet;
        }
    }

}
