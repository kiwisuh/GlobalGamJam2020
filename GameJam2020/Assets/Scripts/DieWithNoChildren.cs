using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieWithNoChildren : MonoBehaviour
{
    public GameObject scrapObj;
    private float upperBounds = 20;
    private float lowerBounds = 10;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Random.Range(upperBounds, lowerBounds); i++)
        {
            GameObject newObject = Instantiate(scrapObj, transform.position, Quaternion.identity);

            newObject.transform.position = new Vector3(transform.position.z, 1.0f, transform.position.x);
            newObject.transform.parent = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
