using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObj : MonoBehaviour
{
    private float damage;
    private float lastDamageTick;
    private float cooldown;
    private bool doDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time-lastDamageTick > cooldown)
        {
            lastDamageTick = Time.time;
            doDamage = true;
        }
        else
        {
            doDamage = false;
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if(doDamage && col.tag == "Enemy")
        {
            col.gameObject.SendMessage("Damage", damage);
        }
    }

    public void SetDamage(float input)
    {
        damage = input;
    }

    public void SetTick(float input)
    {
        cooldown = input;
    }
}
