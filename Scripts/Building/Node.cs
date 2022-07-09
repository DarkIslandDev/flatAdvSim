using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public enum NodeState
{
    freedom,
    captivity,
    resource
}

public class Node : MonoBehaviour
{
    #region Inspector

    BuildManager buildManager;
    
    [Header("Состояние Нода")] public NodeState nodeState;

    [Header("Смещение постройки")] public  Vector3 positionOffset;

    [Header("Постройка в Ноде")] public GameObject building;

    // [Header("Скрипт управления в Ноде")] public EntityBuild entityScript;
    
    [Header("Скрипт ресурсов")] public Resource resource;

    #endregion
    
    private void Start()
    {
        buildManager = BuildManager.instance;

        if (nodeState == NodeState.resource)
        {
            resource = GetComponent<Resource>();

            resource.CreateResourceItem();
        }
    }
    
    public void SwitchNodeState()
    {
        switch(nodeState)
        {
            case NodeState.freedom:
                // subNode.circle.SetActive(true);     // включить пол это лава
                // subNode.plane.SetActive(true);      //
                // subNode.resource.SetActive(false);  //
                break;
            case NodeState.captivity:
                // subNode.circle.SetActive(false);    // выключить пол это лава
                // subNode.plane.SetActive(false);     //
                // subNode.resource.SetActive(false);  //
                break;
            case NodeState.resource:
                // subNode.circle.SetActive(false);    // выключить пол это лава
                // subNode.plane.SetActive(false);     //
                // subNode.resource.SetActive(true);  //
                
                break;
        }
    }

    // private void OnMouseEnter()
    // {
    //     switch (buildManager.buildingState)
    //     {
    //         case BuildingState.viewing:
    //             switch (nodeState)
    //             {
    //                 case NodeState.resource:
    //                     
    //                     break;
    //             }
    //             break;
    //     }
    // }

    // private void OnMouseExit()
    // {
    //     
    // }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        if (buildManager.buildingState == BuildingState.building)    //    режим строительства
        {
            if (!buildManager.CanBuild)     //    проверка на пустой слот для предотвращения ощибок
            {
                return;
            }

            if (buildManager.buildingState == BuildingState.editing &&
                nodeState == NodeState.freedom)    //    тоже самое
            {
                return;
            }
        }

        switch (buildManager.buildingState)
        {
            case BuildingState.viewing:
                switch (nodeState)
                {
                    case NodeState.freedom:
                        //    ничего не делать
                        break;
                    case NodeState.captivity:
                        //    вывести UI кнопки управления
                        // entityScript.SetUIOnBuilding();
                        break;
                    case NodeState.resource:
                        //    вывести UI кнопки сбора ресурсов
                        resource.GetResourceStat();
                        nodeState = NodeState.freedom;
                        SwitchNodeState();
                        break;
                }
                break;
            
            case BuildingState.building:
                switch (nodeState)
                {
                       
                }
                break;
        }
    }
}