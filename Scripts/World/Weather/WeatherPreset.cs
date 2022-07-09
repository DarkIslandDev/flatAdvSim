using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

[Serializable]
public class WeatherEffects
{
    public GameObject prefab;
    public Vector3 localPositionOffset;
    public Vector3 localRotationOffset;
}

public class WeatherPreset : ScriptableObject
{
    public string Name;

    [Header("Season Settings")] public bool Spring = true;
    [Range(1, 100)] public float possibiltyInSpring = 50f;
    public bool Summer = true;
    [Range(1, 100)] public float possibiltyInSummer = 50f;
    public bool Autumn = true;
    [Range(1, 100)] public float possibiltyInAutumn = 50f;
    public bool winter = true;
    [Range(1, 100)] public float possibiltyInWinter = 50f;

    [Header("Linear Fog")] public float fogStartDistance = 0f;
    public float fogDistance = 1000f;
    [Header("Exp Fog")] public float fogDensity = 0.0001f;

    [Header("Advanced Fog Settings")]
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
    [Range(0f, 2.0f)] public float volumeLightIntensity = 1.0f;
    [Range(-1.0f, 1.0f)] public float shadowIntensityMod = 0.0f;

    [Range(0f, 100f)] public float heightFogDensity = 1f;
    [Range(0, 2)] public float SkyFogHeight = 0.5f;

    [Range(0f, 1f)] public float skyFogStart = 0.0f;

    [Header("Weather Settings")] public List<WeatherEffects> effectSystems = new List<WeatherEffects>();

    [Range(0, 1)] [Tooltip("Wind intensity that will applied to wind zone.")]
    public float WindStrenght = 0.5f;

    [Range(0, 1)] [Tooltip("The maximum wetness level that can be reached.")]
    public float wetnessLevel = 0f;

    [Range(0, 1)] [Tooltip("The maximum snow level that can be reached.")]
    public float snowLevel = 0f;

    [Range(-50f, 50f)] [Tooltip("The temperature modifcation for this weather type. (Will be added or substracted)")]
    public float temperatureLevel = 0f;

    [Tooltip("Activate this to enable thunder and lightning.")]
    public bool isLightningStorm;

    [Range(0, 2)] [Tooltip("The Intervall of lightning in seconds. Random(lightningInterval,lightningInterval * 2). ")]
    public float lightningInterval = 10f;

    [Header("Audio Settings - SFX")] public AudioClip weatherSFX;
    [Header("Audio Settings - Ambient")] public AudioClip SpringDayAmbient;
    public AudioClip SpringNightAmbient;
    public AudioClip SummerDayAmbient;
    public AudioClip SummerNightAmbient;
    public AudioClip AutumnDayAmbient;
    public AudioClip AutumnNightAmbient;
    public AudioClip WinterDayAmbient;
    public AudioClip WinterNightAmbient;
}

public class WeatherPresetCreation
{
#if UNITY_EDITOR

    [MenuItem("Assets/Create/Enviro/WeatherPreset")]
    public static void CreateMyAsset()
    {
        WeatherPreset wpreset = ScriptableObject.CreateInstance<WeatherPreset>();
        wpreset.Name = "New Weather Preset " + UnityEngine.Random.Range(0, 999999).ToString();
        wpreset.AmbientColor = CreateGradient();
        wpreset.DirectionalColor = CreateGradient();
        wpreset.FogColor = CreateGradient();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + "Weather Preset" + ".asset");
        AssetDatabase.CreateAsset(wpreset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = wpreset;
    }

#endif

    public static GameObject GetAssetPrefab(string name)
    {
#if UNITY_EDITOR

        string[] assets = AssetDatabase.FindAssets(name, null);
        for (int idx = 0; idx < assets.Length; idx++)
        {
            string path = AssetDatabase.GUIDToAssetPath(assets[idx]);
            if (path.Contains(".prefab"))
            {
                return AssetDatabase.LoadAssetAtPath<GameObject>(path);
            }
        }
#endif
        return null;
    }

    public static Gradient CreateGradient()
    {
        Gradient nG = new Gradient();
        GradientColorKey[] gClr = new GradientColorKey[2];
        GradientAlphaKey[] gAlpha = new GradientAlphaKey[2];

        gClr[0].color = Color.white;
        gClr[0].time = 0f;
        gClr[1].color = Color.white;
        gClr[1].time = 0f;

        gAlpha[0].alpha = 0f;
        gAlpha[0].time = 0f;
        gAlpha[1].alpha = 0f;
        gAlpha[1].time = 1f;

        nG.SetKeys(gClr, gAlpha);

        return nG;
    }
}