using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubNode : MonoBehaviour
{
    public GameObject plane;
    public GameObject circle;

    public GameObject[] resources;
    public bool canResource;

    void Start()
    {
        plane = transform.GetChild(0).gameObject;
        circle = transform.GetChild(1).gameObject;

        if(canResource)
        {
            int resourceNum = Random.Range(0, resources.Length);
            Instantiate(resources[resourceNum], this.transform);
            canResource = false;
        }
    }

    // public void GetInformationAboutResource()
    // {
    //     resource = transform.GetChild(2).gameObject;
    //     if(resource == null)
    //     {
    //         return;
    //     }
    //     else
    //     {
    //         resourceInformation = resource.GetComponent<Resource>();
    //     }
    // }
}
