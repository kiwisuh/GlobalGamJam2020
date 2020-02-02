using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjEnemy : MonoBehaviour
{
    private bool damageDone = false;
    private float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider col)
    {
        if((col.tag == "Tower" || col.tag == "Player" || col.tag == "Object" || col.tag == "Beacon") && damageDone == false)
        {
            col.gameObject.SendMessage("Damage", damage);
            damageDone = true;
            if (col.tag == "Beacon")
            {
                col.gameObject.SendMessage("GetHit", gameObject);
            }
        }
    }

    public void Damage(float dmg)
    {
        print("Damage sent " + dmg);
        transform.parent.gameObject.SendMessage("Damage", dmg);
    }

    private void OnEnable()
    {
        damageDone = false;
    }

    public void DamageInput(float input)
    {
        damage = input;
    }

}
