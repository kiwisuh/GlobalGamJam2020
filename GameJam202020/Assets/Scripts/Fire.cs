using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fire : MonoBehaviour
{
	public ParticleSystem fire;
    // Start is called before the first frame update
    void Start()
    {
		fire = GetComponent<ParticleSystem>();
		fire.Stop();
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetMouseButtonDown(0)){
			fire.Play();
		}
		if(Input.GetMouseButtonUp(0)){
			fire.Stop();
		}

    }
}
