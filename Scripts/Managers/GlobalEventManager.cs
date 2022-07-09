using System;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventManager
{
    public static UnityEvent EnableBuildingMode = new UnityEvent();
    public static UnityEvent DisableBuildingMode = new UnityEvent();
    public static UnityEvent UIRecipeShopEvent = new UnityEvent();
    public static UnityEvent UIBuildShopEvent = new UnityEvent();
    public static UnityEvent UICreateNotEnoughMoneyMessege = new UnityEvent();
    public static UnityEvent UIActivatePanelForCloseWorldUI = new UnityEvent();

    public static void SendEnableBuildingState()
    {
        EnableBuildingMode.Invoke();
    }

    public static void SendDisableBuildingState()
    {
        DisableBuildingMode.Invoke();
    }
    public static void SendUIRecipeShopEvent()
    {
        UIRecipeShopEvent.Invoke();
    }
    public static void SendUIBuildShopEvent()
    {
        UIBuildShopEvent.Invoke();
    }

    public static void SendUICreateNotEnoughMoneyMessege()
    {
        UICreateNotEnoughMoneyMessege.Invoke();
    }

    public void SendUIActivatePanelForCloseWorldUI()
    {
        UIActivatePanelForCloseWorldUI.Invoke();
    }
}
