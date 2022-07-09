// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class ProgressBar : MonoBehaviour
// {
//     private Animator animator;
//     private GameObject panel;
//     
//     // private Slider slider;
//
//     // private float targetLoad;
//     // public float FillSpeed = 0.03f;
//     
//     // public SimpleTileGenerator simpleTileGenerator;
//
//     private void Awake()
//     {
//         animator = GetComponent<Animator>();
//         panel = GetComponent<GameObject>();
//         panel.SetActive(true);
//         // slider = transform.GetChild(1).GetComponent<Slider>();
//     }
//
//     private void Update()
//     {
//         // if (simpleTileGenerator.hasGenerated == 1)
//         // {
//         //     StartCoroutine(AwaitForStartAnimation("Start"));
//         // }
//         
//         // if (slider.value < targetLoad)
//         // {
//         //     slider.value += FillSpeed * Time.deltaTime;
//         // }
//     }
//
//     public IEnumerator AwaitForStartAnimation(string trigger)
//     {
//         animator.SetTrigger("Start");
//         
//         yield return new WaitForSeconds(2.2f);
//
//         
//         animator.enabled = false;
//         Destroy(gameObject);
//
//     }
//
//     // public void LoadingProgress()
//     // {
//     //     float loadProg = (float)simpleTileGenerator.count;
//     //     // targetLoad = slider.value + loadProg;
//     //     targetLoad = slider.value + 1443.0f;
//     // }
// }
