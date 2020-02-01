using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
  public float startHealth = 100;
  private float health;
  public int worth = 50;
  public Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
      health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Damage(float amount){
      healthBar.fillAmount = health / startHealth;
      health -= amount;
      if(health <= 0){
        DestroyObject();
      }
    }

    public void DestroyObject(){
      Destroy(gameObject);
    }
}
