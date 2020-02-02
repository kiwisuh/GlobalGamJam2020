using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    private float lifeTime = 10;
    private float lifeTime2 = 13;
    private float lifeStart;
    Rigidbody rb;
    private float vel = 100;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomDir = new Vector3(Random.Range(-vel, vel), Random.Range(-vel, vel), Random.Range(-vel, vel));

        rb = GetComponent<Rigidbody>();
        lifeStart = Time.time;
        rb.AddForce(randomDir);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lifeStart > lifeTime)
        {
            transform.localScale *=(1 - Time.deltaTime * 2f);
            if (Time.time - lifeStart > lifeTime2)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            col.gameObject.SendMessage("AddScrap", 1);
            Destroy(gameObject);
        }
    }
}
