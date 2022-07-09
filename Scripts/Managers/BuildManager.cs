using UnityEngine;

public enum BuildingState
{
    viewing,
    building,
    editing
}

public class BuildManager : MonoBehaviour 
{
    #region Instance

    public static BuildManager instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Многовато конечно build манагеров для сцены, найди их все и оставь только один!");
            return;
        }
        instance = this;
    }

    #endregion

    public BuildingState buildingState;

    // private BuildStatsItem buildStatsItem;

    private Camera cam;
    public LayerMask view;
    public LayerMask edit;

    void Start()
    {
        cam = Camera.main;
        buildingState = BuildingState.viewing;

        // GlobalEventManager.EnableBuildingMode.AddListener(EnableGridEditorOnClick);
        // GlobalEventManager.EnableBuildingMode.AddListener(DisableGridEditorOnClick);

    }

    void Update()
    {
        SwitchBuildingState();
    }

    public void EnableGridBuildingOnClick()
    {
        buildingState = BuildingState.building;
    }

    public void EnableGridEditorOnClick()
    {
        buildingState = BuildingState.editing;
    }

    public void DisableGridEditorOnClick()
    {
        buildingState = BuildingState.viewing;
    }

    private void SwitchBuildingState()
    {
        switch(buildingState)
        {
            case BuildingState.viewing:
                cam.cullingMask = view;
                // UIManager.instance.shopBuildsButton.SetActive(true);
                // UIManager.instance.shopRecipeButton.SetActive(true);
                // UIManager.instance.foodPanel.SetActive(true);
                break;
            case BuildingState.building:
                cam.cullingMask = edit;
                // UIManager.instance.shopBuildsButton.SetActive(false);
                // UIManager.instance.shopRecipeButton.SetActive(false);
                // UIManager.instance.foodPanel.SetActive(false);
                break;
            case BuildingState.editing:
                cam.cullingMask = edit;
                // UIManager.instance.shopBuildsButton.SetActive(false);
                // UIManager.instance.shopRecipeButton.SetActive(false);
                // UIManager.instance.foodPanel.SetActive(false);
                break;
        }
    }

    public GameObject changedThingPrefab;
    public Transform transformBuildPrefab;

    private GameObject thingToBuild;

    public bool CanBuild{ get { return thingToBuild != null; } }

    public GameObject GetThingToBuild()
    {
        return thingToBuild;
    }

    public void SetThingToBuild(GameObject thing)
    {
        thingToBuild = thing;
    }
}