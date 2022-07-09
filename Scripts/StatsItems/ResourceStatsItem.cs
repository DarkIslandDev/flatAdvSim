using UnityEngine;
using UnityEditor;

public enum ResourceType
{
	Wood,
	Rock,
}

[CreateAssetMenu(fileName = "New Resource", menuName = "Resources/Resource")]
public class ResourceStatsItem : ScriptableObject
{
    [SerializeField] string id;
    public string ID{get { return id;}}
    new public string name = "New Resource Stats";
    public ResourceType resourceType;
    public int minCount;
    public int maxCount;
    public GameObject resourcePrefab;

    #if UNITY_EDITOR
	protected virtual void OnValidate()
	{
		string path = AssetDatabase.GetAssetPath(this);
		id = AssetDatabase.AssetPathToGUID(path);
	}
	#endif
}