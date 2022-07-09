using System;
using UnityEngine;

public class TriggerCheker : MonoBehaviour
{
    public bool hasDropped;
    public string TagName;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == TagName)
        {
            hasDropped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        hasDropped = false;
    }
}