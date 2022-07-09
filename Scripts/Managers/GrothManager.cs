using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GrothManager : MonoBehaviour
{
    public FoodStatsItem foodItem;
    public Transform[] foodHolders;
    
    private Transform localContainer;
    private Transform parentObject;
    private Transform moreParent;
    private Transform lbmoreParent;
    private GameObject food;

    private TriggerCheker cheker;
    
    private void Start()
    {
        localContainer = this.transform;

        parentObject = transform.parent;
        moreParent = parentObject.transform.parent;
        lbmoreParent = moreParent.transform.parent;

        cheker = lbmoreParent.gameObject.GetComponent<TriggerCheker>();
        
        
        int i = localContainer.GetComponentInChildren<Transform>().childCount;
        foodHolders = localContainer.GetComponentsInChildren<Transform>();
        foodHolders = new Transform[i];
        for (int j = 0; j < i; j++)
        {
            foodHolders[j] = localContainer.GetComponentInChildren<Transform>().GetChild(j);
        }

        StartCoroutine(WaitForGrowth(3.0f));
    }

    public IEnumerator WaitForGrowth(float GrowthTimer)
    {
        if (cheker.hasDropped == false)
        {
            yield return new WaitForSeconds(GrowthTimer);

            int rand = Random.Range(2, 5);
            for (int i = 0; i < foodHolders.Length - rand; i++)
            {
                if (i == 0)
                {
                    //
                }
                else
                {
                    foodItem.foodPrefab.transform.position = foodHolders[i].transform.position;
                    food = Instantiate(foodItem.foodPrefab.gameObject, foodHolders[i].transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
                    food.transform.localScale = new Vector3(0,0,0);
                    food.transform.SetParent(foodHolders[0]);
                }
            }
        }
    }
}
