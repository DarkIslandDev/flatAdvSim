using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceStatsItem resourceItem;
    public WeightedRandomList<ResourceStatsItem> resourceTable;
    private Transform resourceTransform;

    public void CreateResourceItem()
    {
        resourceItem = resourceTable.GetRandom();
        resourceTransform = transform.GetChild(0).gameObject.transform;
        resourceTransform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * resourceTransform.rotation;
        Instantiate(resourceItem.resourcePrefab, resourceTransform);
    }

    public void GetResourceStat()
    {
        int rand = Random.Range(resourceItem.minCount, resourceItem.maxCount);

        switch (resourceItem.resourceType)
        {
            case ResourceType.Wood:
                StatisticManager.Wood += rand;
                Debug.Log("Добыто " + rand + " " + resourceItem.resourceType.ToString());
                break;
            
            case ResourceType.Rock:
                StatisticManager.Rock += rand;
                Debug.Log("Добыто " + rand + " " + resourceItem.resourceType.ToString());
                break;
        }
        
        Destroy(resourceTransform.gameObject);
        // Destroy(transform.GetChild(0).gameObject);
    }
}