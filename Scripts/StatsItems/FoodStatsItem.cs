using UnityEngine;
using UnityEditor;

public enum FoodType
{
    Mushroom,
    Apple
}

[CreateAssetMenu(fileName = "New Food", menuName = "Resources/Food")]
public class FoodStatsItem : ScriptableObject
{
    [SerializeField] string id;
    public string ID{get { return id;}}
    new public string name = "New Food Stats";
    public FoodType FoodType;
    public GameObject foodPrefab;

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        string path = AssetDatabase.GetAssetPath(this);
        id = AssetDatabase.AssetPathToGUID(path);
    }
#endif
}
