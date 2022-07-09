using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class EnvSeasonsSettings
{
    [Header("Spring")] 
    public int SpringStart = 60;
    public int SpringEnd = 60;
    public AnimationCurve springBaseTemperature = new AnimationCurve();
    
    [Header("Summer")] 
    public int SummerStart = 60;
    public int SummerEnd = 60;
    public AnimationCurve summerBaseTemperature = new AnimationCurve();
    
    [Header("Autumn")] 
    public int AutumnStart = 60;
    public int AutumnEnd = 60;
    public AnimationCurve autumnBaseTemperature = new AnimationCurve();
    
    [Header("Winter")] 
    public int WinterStart = 60;
    public int WinterEnd = 60;
    public AnimationCurve winterBaseTemperature = new AnimationCurve();
}

[Serializable]
public class EnvWeatherSettings
{
    public float fogTransitionSpeed = 1f;
    public float windIntensityTransitionSpeed = 1f;
    public float effectTransitionSpeed = 1f;
    public float audioTransitionSpeed = 0.1f;
    public float wetnessAccumulationSpeed = 0.05f;
    public float wetnessDryingSpeed = 0.05f;
    public float snowAccumulationSpeed = 0.05f;
    
    [Header("Lightning Effect:")]
    public GameObject lightningEffect;
    [Range(500f, 10000f)]
    public float lightningRange = 500f;
    [Range(500f, 5000f)]
    public float lightningHeight = 750f;
    
    [Header("Temperature:")]
    public float temperatureChangingSpeed = 10f;
}

[Serializable]
public class EnvSkySettings
{
    [Header("Sun")]
    public float sunIntensity = 100f;
    public float sunDiskScale = 20f;
    public float sunDiskIntensity = 3f;
    public Gradient sunDiskColor;
    public Gradient simpleSkyColor;
    public Gradient simpleHorizonColor;
    public Gradient simpleSunColor;
    public AnimationCurve simpleSunDiskSize = new AnimationCurve();

    [Header("Moon")]
    public float moonBrightness = 1f;
    public float moonSize = 10f;
    public float glowSize = 10f;
    public AnimationCurve moonGlow = new AnimationCurve();
    public Color moonGlowColor;
    public float startMoonPhase = 0.0f;

}

[Serializable]
public class EnvLightSettings
{
    public enum LightingMode
    {
        Single,
        Dual
    }

    [Header("Direct")]
    public LightingMode directionalLightMode = LightingMode.Single;
    
    public Gradient LightColor;
    public AnimationCurve directLightSunIntensity = new AnimationCurve();
    public AnimationCurve directLightMoonIntensity = new AnimationCurve();
    public float lightIntensityTransitionSpeed = 1f;
    public AnimationCurve shadowIntensity = new AnimationCurve();
    public float directLightAngleOffset = 0f;
    public UnityEngine.Rendering.AmbientMode ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
    public AnimationCurve ambientIntensity = new AnimationCurve();
    public Gradient ambientSkyColor;
    public Gradient ambientEquatorColor;
    public Gradient ambientGroundColor;
    public bool stopRotationAtHigh = false;
    public float rotationStopHigh = 0.5f;

}

[Serializable]
public class EnvDistanceBlurSettings
{
    public bool antiFlicker = true;
    public bool highQuality = true;
    [Range(1,7)]
    public float radius = 7f;
}

[Serializable]
public class EnvFogSettings
{
    [Header("Mode")]
    public FogMode Fogmode = FogMode.Exponential;
    public bool useSimpleFog = false;
    public bool useUnityFog = false;
    [Header("Distance Fog")]
    public bool distanceFog = true;
    public bool useRadialDistance = true;
    public float startDistance = 0.0f;
    [Range(0f,10f)]
    public float distanceFogIntensity = 4.0f;
    [Range(0f,1f)]
    public float maximumFogDensity = 0.9f;
    [Header("Height Fog")]
    public bool heightFog = true;
    public float height = 90.0f;
    [Range(0f,1f)]
    public float heightFogIntensity = 1f;
    [HideInInspector]
    public float heightDensity = 0.15f;
    [Header("Height Fog Noise")]
    [Range(0f, 1f)]
    public float noiseIntensity = 1f;
    [Range(0f, 1f)]
    public float noiseIntensityOffset = 0.3f;
    [Range(0f, 0.1f)]
    public float noiseScale = 0.001f;
    public Vector2 noiseVelocity = new Vector2(3f, 1.5f);

    [Header("Fog Dithering")]
    public bool dithering = true;

    public Gradient simpleFogColor;

    [HideInInspector]
    public float skyFogIntensity = 1f;

}

[Serializable]
public class EnvAudioSettings 
{
    [Tooltip("A list of all possible thunder audio effects.")]
    public List<AudioClip> ThunderSFX = new List<AudioClip> ();
}

public class WeatherProfile : ScriptableObject
{
    public EnvSeasonsSettings seasonsSettings = new EnvSeasonsSettings();
    public EnvWeatherSettings weatherSettings = new EnvWeatherSettings();
    public EnvSkySettings skySettings = new EnvSkySettings();
    public EnvLightSettings lightSettings = new EnvLightSettings();
    public EnvDistanceBlurSettings distanceBlurSettings = new EnvDistanceBlurSettings();
    public EnvFogSettings fogSettings = new EnvFogSettings();
    public EnvAudioSettings audioSettings = new EnvAudioSettings();
    
    public enum settingsMode
    {
        Seasons,
        Weather,
        Sky,
        Light,
        DistanceBlur,
        Fog,
        Audio
    }

    [HideInInspector] public settingsMode viewMode;
    [HideInInspector] public bool modified;
}

public static class EnvProfileCreation
{
    #if UNITY_EDITOR

    [MenuItem("Assets/Create/Weather/Profile")]
    public static WeatherProfile CreateNewWeatherProfile()
    {
        WeatherProfile profile = ScriptableObject.CreateInstance<WeatherProfile>();
        
        // SetupDefaults (profile);
        string path = AssetDatabase.GetAssetPath (Selection.activeObject);
        if (path == "") 
        {
            path = "Assets/Weather/Profiles";
        } 
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + "Weather Profile" + ".asset");
        AssetDatabase.CreateAsset (profile, assetPathAndName);
        AssetDatabase.SaveAssets ();
        AssetDatabase.Refresh();
        return profile;
    }
    
    #endif

    public static void SetupDefaults(WeatherProfile profile)
    {
        // WeatherProfile defaultProfile = GetDefaultProfile("")
    }
}

