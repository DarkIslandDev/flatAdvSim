using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Apple : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        rb.useGravity = false;
        anim.enabled = false;

        float randomDropTime = Random.Range(50, 100);
        StartCoroutine(AwaitForDrop(randomDropTime));
    }

    public IEnumerator AwaitForDrop(float DropTimer)
    {
        yield return new WaitForSeconds(DropTimer);

        anim.enabled = true;
        
        anim.SetTrigger("grewup");

        float randTime = Random.Range(15, 150);
        yield return new WaitForSeconds(randTime);

        anim.enabled = false;

        rb.useGravity = true;
    }
}
