using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Serialization;

#region Classes

[Serializable]
public class EnvSeasons
{
    public enum Seasons
    {
        Winter, //    90
        Spring, //    92
        Summer, //    92
        Autumn //    91
    }

    public Seasons currentSeason;
    public bool calcSeasons = true;
    public int temperature;
}

[Serializable]
public class EnvComponents
{
    public WindZone windZone;
    public WeatherLighting LightingGenerator;

    public GameObject Sun = null;
    public GameObject Moon = null;
    public Transform DirectLight;
    public Transform AdditionalDirectLight;
}

[Serializable]
public class EnvInteriorZoneSettings
{
    [HideInInspector]
    public Color currentInteriorDirectLightMod;
    [HideInInspector]
    public Color currentInteriorAmbientLightMod;
    [HideInInspector]
    public Color currentInteriorAmbientEQLightMod;
    [HideInInspector]
    public Color currentInteriorAmbientGRLightMod;
    [HideInInspector]
    public Color currentInteriorSkyboxMod;
    [HideInInspector]
    public Color currentInteriorFogColorMod = new Color(0, 0, 0, 0);
    [HideInInspector]
    public float currentInteriorFogMod = 1f;
    [HideInInspector]
    public float currentInteriorWeatherEffectMod = 1f;
    [HideInInspector]
    public float currentInteriorZoneAudioVolume = 1f;
    [HideInInspector]
    public float currentInteriorZoneAudioFadingSpeed = 1f;
}

[Serializable]
public class EnvTime
{
    public enum TimeProgressMode
    {
        None,
        Simulated
    }

    public TimeProgressMode ProgressTime = TimeProgressMode.Simulated;
    [Range(0, 60)] public int Seconds = 0;
    [Range(0, 60)] public int Minutes = 0;
    [Range(0, 24)] public int Hours = 12;
    public int Days = 1;
    public int Years = 1;
    
    [Space(20)] 
    public int DaysInYear = 365;
    
    [Space(20)]
    public float DayLengthInMinutes = 5f;
    public float NightLengthInMinutes = 5f;

    [Range(-13, 13)]
    [Tooltip("Time offset for timezones")]
    public int utcOffset = 0;
    [Range(-90, 90)]
    [Tooltip("-90,  90   Horizontal earth lines")]
    public float Latitude = 0f;
    [Range(-180, 180)]
    [Tooltip("-180, 180  Vertical earth line")]
    public float Longitude = 0f;
    [HideInInspector]
    public float solarTime;
    [HideInInspector]
    public float lunarTime;
    [Range(0.3f, 0.7f)]
    public float dayNightSwitch = 0.45f;
    
    public AnimationCurve directLightSunIntensity = new AnimationCurve();
    public AnimationCurve directLightMoonIntensity = new AnimationCurve();
    public AnimationCurve shadowIntensity = new AnimationCurve();
}

[Serializable]
public class EnvWeather
{
    public bool updateWeather = true;
    
    public List<WeatherPreset> weatherPreset = new List<WeatherPreset>();
    public List<WeatherPrefab> weatherPrefabs = new List<WeatherPrefab>();

    public WeatherPreset startWeatherPreset;

    public WeatherPrefab currentWeatherPrefab;
    public WeatherPreset currentWeatherPreset;
    
    [HideInInspector] public WeatherPrefab lastActiveWeatherPrefab;
    [HideInInspector] public WeatherPreset lastActiveWeatherPreset;

    [HideInInspector] public GameObject VFXHolder;
    [HideInInspector] public float wetness;
    [HideInInspector] public float currentWetness;
    [HideInInspector] public float snowStrength;
    [HideInInspector] public float currentSnowStrength;
    [HideInInspector] public int thundersfx;
    // [HideInInspector] public EnvironmentAudioSource currentAudioSource;
    [HideInInspector] public bool weatherFullyChanged = false;
    [HideInInspector] public float currentTemperature;

}

[Serializable]
public class EnvFogging
{
    [HideInInspector] public float skyFogStart = 0f;
    [HideInInspector] public float skyFogHeight = 1f;
    [HideInInspector] public float skyFogIntensity = 0.1f;
    [HideInInspector] public float skyFogStrength = 0.1f;
    [HideInInspector] public float scatteringStrength = 0.5f;
    [HideInInspector] public float sunBlocking = 0.5f;
    [HideInInspector] public float moonIntensity = 1;
}

#endregion
public class WeatherEnviroment : MonoBehaviour
{
    //    Profile
    [Header("Profile")] 
    public WeatherProfile profile = null;
    [HideInInspector]public bool profileLoaded = false;
    
    [HideInInspector] public EnvSeasonsSettings seasonsSettings = new EnvSeasonsSettings();
    [HideInInspector] public EnvWeatherSettings weatherSettings = new EnvWeatherSettings();
    [HideInInspector] public EnvSkySettings skySettings = new EnvSkySettings();
    [HideInInspector] public EnvLightSettings lightSettings = new EnvLightSettings();
    [HideInInspector] public EnvDistanceBlurSettings distanceBlurSettings = new EnvDistanceBlurSettings();
    [HideInInspector] public EnvFogSettings fogSettings = new EnvFogSettings();
    [HideInInspector] public EnvAudioSettings audioSettings = new EnvAudioSettings();
    [HideInInspector]public EnvInteriorZoneSettings interiorZoneSettings = new EnvInteriorZoneSettings();

    
    
    [HideInInspector] public bool isNight = true;

    //    Time
    [HideInInspector] public float InternalHour;
    [HideInInspector] public float currentHour;
    [HideInInspector] public float currentDay;
    [HideInInspector] public float currentYear;
    [HideInInspector] public double currentTimeInHours;
    [HideInInspector] public float LST;
    [HideInInspector] public float lastHourUpdate;
    [HideInInspector] public float hourTime;
    
    //Shadows
    [HideInInspector]
    public float shadowIntensityMod;
        
    // Scattering constants
    public const float pi = Mathf.PI;
    private Vector3 K = new Vector3(686.0f, 678.0f, 666.0f);
    private const float n = 1.0003f;
    private const float N = 2.545E25f;
    private const float pn = 0.035f;
    
    //weather
    [HideInInspector]public Color currentWeatherSkyMod;
    [HideInInspector]public Color currentWeatherLightMod;
    [HideInInspector]public Color currentWeatherFogMod;

    //    Classes
    public EnvSeasons Seasons = null;
    public EnvComponents Components;
    public EnvTime GameTime = null;
    public EnvWeather Weather = null;
    public EnvFogging Fog = null;
    
    //    References
    public Light MainLight;
    public Light AdditionalLight;
    public Transform MoonTransform;
    public Renderer MoonRenderer;

    private void Start()
    {
        GameTime.Days = 105;
    }

    private void Update()
    {
        UpdateTime(GameTime.DaysInYear);
        UpdateSunAndMoonPosition();
        CalculateDirectLight();
        UpdateSeasons();
    }

    #region Time and sun/moon/stars position

    public Transform CreateDirectionalLight(bool additional)
    {
        GameObject newGO = new GameObject();

        if(!additional)
            newGO.name = "Env Directional Light";
        else
            newGO.name = "Env Directional Light - Moon";

        newGO.transform.parent = transform;
        newGO.transform.parent = null;
        Light newLight = newGO.AddComponent<Light>();
        newLight.type = LightType.Directional;
        newLight.shadows = LightShadows.Soft;
        return newGO.transform;
    }
    
    // Update the GameTime
    public void UpdateTime(int daysInYear)
    {
        if (Application.isPlaying)
        {

            float t = 0f;

            if (!isNight)
                t = (24.0f / 60.0f) / GameTime.DayLengthInMinutes;
            else
                t = (24.0f / 60.0f) / GameTime.NightLengthInMinutes;

            hourTime = t * Time.deltaTime;

            switch (GameTime.ProgressTime)
            {
                case EnvTime.TimeProgressMode.None://Set Time over editor or other scripts.
                    SetTime(GameTime.Years, GameTime.Days, GameTime.Hours, GameTime.Minutes, GameTime.Seconds);
                    break;
                case EnvTime.TimeProgressMode.Simulated:
                    InternalHour += hourTime;
                    SetGameTime();
                    //customMoonPhase += Time.deltaTime / (30f * (GameTime.DayLengthInMinutes * 60f)) * 2f;
                    break;
            }
        }
        else
        {
            SetTime(GameTime.Years, GameTime.Days, GameTime.Hours, GameTime.Minutes, GameTime.Seconds);
        }

        //if (customMoonPhase < -1) customMoonPhase += 2;
        //else if (customMoonPhase > 1) customMoonPhase -= 2;

        //Fire OnHour Event
        if (InternalHour > (lastHourUpdate + 1f))
        {
            lastHourUpdate = InternalHour;
        }

        // Check Days
        if (GameTime.Days >= daysInYear)
        {
            GameTime.Years = GameTime.Years + 1;
            GameTime.Days = 0;
        }

        currentHour = InternalHour;
        currentDay = GameTime.Days;
        currentYear = GameTime.Years;

        currentTimeInHours = GetInHours(InternalHour, currentDay, currentYear, daysInYear);
    }

    public void SetInternalTime(int year, int dayOfYear, int hour, int minute, int seconds)
    {
        GameTime.Years = year;
        GameTime.Days = dayOfYear;
        GameTime.Minutes = minute;
        GameTime.Hours = hour;
        InternalHour = hour + (minute * 0.0166667f) + (seconds * 0.000277778f);
    }

    /// <summary>
    /// Updates Game Time days and years. Used internaly only.
    /// </summary>
    public void SetGameTime()
    {
        if (InternalHour >= 24f)
        {
            InternalHour = InternalHour - 24f;
            lastHourUpdate = InternalHour;
                GameTime.Days = GameTime.Days + 1;
        }
        else if (InternalHour < 0f)
        {
            InternalHour = 24f + InternalHour;
            lastHourUpdate = InternalHour;
            GameTime.Days = GameTime.Days - 1;
        }

        float inHours = InternalHour;
        GameTime.Hours = (int)(inHours);
        inHours -= GameTime.Hours;
        GameTime.Minutes = (int)(inHours * 60f);
        inHours -= GameTime.Minutes * 0.0166667f;
        GameTime.Seconds = (int)(inHours * 3600f);
    }

    /// <summary>
    /// Set the exact date. by DateTime
    /// </summary>
    public void SetTime(DateTime date)
    {
        GameTime.Years = date.Year;
        GameTime.Days = date.DayOfYear;
        GameTime.Minutes = date.Minute;
        GameTime.Seconds = date.Second;
        GameTime.Hours = date.Hour;
        InternalHour = date.Hour + (date.Minute * 0.0166667f) + (date.Second * 0.000277778f);
    }

    /// <summary>
    /// Set the exact date.
    /// </summary>
    public void SetTime(int year, int dayOfYear, int hour, int minute, int seconds)
    {
        GameTime.Years = year;
        GameTime.Days = dayOfYear;
        GameTime.Minutes = minute;
        GameTime.Hours = hour;
        InternalHour = hour + (minute * 0.0166667f) + (seconds * 0.000277778f);
    }

    /// <summary>
    /// Set the time of day in hours. (12.5 = 12:30)
    /// </summary>
    public void SetInternalTimeOfDay(float inHours)
    {
        InternalHour = inHours;
        GameTime.Hours = (int)(inHours);
        inHours -= GameTime.Hours;
        GameTime.Minutes = (int)(inHours * 60f);
        inHours -= GameTime.Minutes * 0.0166667f;
        GameTime.Seconds = (int)(inHours * 3600f);
    }

    /// <summary>
    /// Get current time in a nicely formatted string with seconds!
    /// </summary>
    /// <returns>The time string.</returns>
    public string GetTimeStringWithSeconds()
    {
        return string.Format("{0:00}:{1:00}:{2:00}", GameTime.Hours, GameTime.Minutes, GameTime.Seconds);
    }

    /// <summary>
    /// Get current time in a nicely formatted string!
    /// </summary>
    /// <returns>The time string.</returns>
    public string GetTimeString()
    {
        return string.Format("{0:00}:{1:00}", GameTime.Hours, GameTime.Minutes);
    }

    public DateTime CreateSystemDate()
    {
        DateTime date = new DateTime();

        date = date.AddYears(GameTime.Years - 1);
        date = date.AddDays(GameTime.Days - 1);

        return date;
    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public Vector3 OrbitalToLocal(float theta, float phi)
    {
        Vector3 pos;

        float sinTheta = Mathf.Sin(theta);
        float cosTheta = Mathf.Cos(theta);
        float sinPhi = Mathf.Sin(phi);
        float cosPhi = Mathf.Cos(phi);

        pos.z = sinTheta * cosPhi;
        pos.y = cosTheta;
        pos.x = sinTheta * sinPhi;

        return pos;
    }

    public void CalculateSunPosition(float d, float ecl)
    {
        /////http://www.stjarnhimlen.se/comp/ppcomp.html#5////
        ///////////////////////// SUN ////////////////////////
        float w = 282.9404f + 4.70935E-5f * d;
        float e = 0.016709f - 1.151E-9f * d;
        float M = 356.0470f + 0.9856002585f * d;

        float E = M + e * Mathf.Rad2Deg * Mathf.Sin(Mathf.Deg2Rad * M) * (1 + e * Mathf.Cos(Mathf.Deg2Rad * M));

        float xv = Mathf.Cos(Mathf.Deg2Rad * E) - e;
        float yv = Mathf.Sin(Mathf.Deg2Rad * E) * Mathf.Sqrt(1 - e * e);

        float v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv);
        float r = Mathf.Sqrt(xv * xv + yv * yv);

        float l = v + w;

        float xs = r * Mathf.Cos(Mathf.Deg2Rad * l);
        float ys = r * Mathf.Sin(Mathf.Deg2Rad * l);

        float xe = xs;
        float ye = ys * Mathf.Cos(Mathf.Deg2Rad * ecl);
        float ze = ys * Mathf.Sin(Mathf.Deg2Rad * ecl);

        float decl_rad = Mathf.Atan2(ze, Mathf.Sqrt(xe * xe + ye * ye));
        float decl_sin = Mathf.Sin(decl_rad);
        float decl_cos = Mathf.Cos(decl_rad);

        float GMST0 = (l + 180);
        float GMST = GMST0 + GetUniversalTimeOfDay() * 15;
        LST = GMST + GameTime.Longitude;

        float HA_deg = LST - Mathf.Rad2Deg * Mathf.Atan2(ye, xe);
        float HA_rad = Mathf.Deg2Rad * HA_deg;
        float HA_sin = Mathf.Sin(HA_rad);
        float HA_cos = Mathf.Cos(HA_rad);

        float x = HA_cos * decl_cos;
        float y = HA_sin * decl_cos;
        float z = decl_sin;

        float sin_Lat = Mathf.Sin(Mathf.Deg2Rad * GameTime.Latitude);
        float cos_Lat = Mathf.Cos(Mathf.Deg2Rad * GameTime.Latitude);

        float xhor = x * sin_Lat - z * cos_Lat;
        float yhor = y;
        float zhor = x * cos_Lat + z * sin_Lat;

        float azimuth = Mathf.Atan2(yhor, xhor) + Mathf.Deg2Rad * 180;
        float altitude = Mathf.Atan2(zhor, Mathf.Sqrt(xhor * xhor + yhor * yhor));

        float sunTheta = (90 * Mathf.Deg2Rad) - altitude;
        float sunPhi = azimuth;

        //Set SolarTime: 1 = mid-day (sun directly above you), 0.5 = sunset/dawn, 0 = midnight;
        GameTime.solarTime = Mathf.Clamp01(Remap(sunTheta, -1.5f, 0f, 1.5f, 1f));

        Components.Sun.transform.localPosition = OrbitalToLocal(sunTheta, sunPhi);
        Components.Sun.transform.LookAt(transform.position);
    }

    public void CalculateMoonPosition(float d, float ecl)
    {

        float N = 125.1228f - 0.0529538083f * d;
        float i = 5.1454f;
        float w = 318.0634f + 0.1643573223f * d;
        float a = 60.2666f;
        float e = 0.054900f;
        float M = 115.3654f + 13.0649929509f * d;

        float rad_M = Mathf.Deg2Rad * M;

        float E = rad_M + e * Mathf.Sin(rad_M) * (1f + e * Mathf.Cos(rad_M));

        float xv = a * (Mathf.Cos(E) - e);
        float yv = a * (Mathf.Sqrt(1f - e * e) * Mathf.Sin(E));

        float v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv);
        float r = Mathf.Sqrt(xv * xv + yv * yv);

        float rad_N = Mathf.Deg2Rad * N;
        float sin_N = Mathf.Sin(rad_N);
        float cos_N = Mathf.Cos(rad_N);

        float l = Mathf.Deg2Rad * (v + w);
        float sin_l = Mathf.Sin(l);
        float cos_l = Mathf.Cos(l);

        float rad_i = Mathf.Deg2Rad * i;
        float cos_i = Mathf.Cos(rad_i);

        float xh = r * (cos_N * cos_l - sin_N * sin_l * cos_i);
        float yh = r * (sin_N * cos_l + cos_N * sin_l * cos_i);
        float zh = r * (sin_l * Mathf.Sin(rad_i));

        float cos_ecl = Mathf.Cos(Mathf.Deg2Rad * ecl);
        float sin_ecl = Mathf.Sin(Mathf.Deg2Rad * ecl);

        float xe = xh;
        float ye = yh * cos_ecl - zh * sin_ecl;
        float ze = yh * sin_ecl + zh * cos_ecl;

        float ra = Mathf.Atan2(ye, xe);
        float decl = Mathf.Atan2(ze, Mathf.Sqrt(xe * xe + ye * ye));

        float HA = Mathf.Deg2Rad * LST - ra;

        float x = Mathf.Cos(HA) * Mathf.Cos(decl);
        float y = Mathf.Sin(HA) * Mathf.Cos(decl);
        float z = Mathf.Sin(decl);

        float latitude = Mathf.Deg2Rad * GameTime.Latitude;
        float sin_latitude = Mathf.Sin(latitude);
        float cos_latitude = Mathf.Cos(latitude);

        float xhor = x * sin_latitude - z * cos_latitude;
        float yhor = y;
        float zhor = x * cos_latitude + z * sin_latitude;

        float azimuth = Mathf.Atan2(yhor, xhor) + Mathf.Deg2Rad * 180f;
        float altitude = Mathf.Atan2(zhor, Mathf.Sqrt(xhor * xhor + yhor * yhor));

        float MoonTheta = (90f * Mathf.Deg2Rad) - altitude;
        float MoonPhi = azimuth;

        Components.Moon.transform.localPosition = OrbitalToLocal(MoonTheta, MoonPhi);
        GameTime.lunarTime = Mathf.Clamp01(Remap(MoonTheta, -1.5f, 0f, 1.5f, 1f));
        Components.Moon.transform.LookAt(transform.position);
    }

    public void UpdateSunAndMoonPosition()
    {
        DateTime date = CreateSystemDate();
        float d = 367 * date.Year - 7 * (date.Year + (date.Month / 12 + 9) / 12) / 4 + 275 * date.Month / 9 + date.Day - 730530;

        d += (GetUniversalTimeOfDay() / 24f);

        float ecl = 23.4393f - 3.563E-7f * d;

            CalculateSunPosition(d, ecl);
            CalculateMoonPosition(d, ecl);
        }
    /// <summary>
    /// Get current time in hours. UTC0 (12.5 = 12:30)
    /// </summary>
    /// <returns>The the current time of day in hours.</returns>
    public float GetUniversalTimeOfDay()
    {
        return InternalHour - GameTime.utcOffset;
    }

    /// <summary>
    /// Calculate total time in hours.
    /// </summary>
    /// <returns>The the current date in hours.</returns>
    public double GetInHours(float hours, float days, float years, int daysInYear)
    {
        double inHours = hours + (days * 24f) + ((years * daysInYear) * 24f);
        return inHours;
    }
    
    public void CalculateDirectLight()
    {
        if (MainLight == null)
            MainLight = Components.DirectLight.GetComponent<Light>();

        if (lightSettings.directionalLightMode == EnvLightSettings.LightingMode.Single || Components.AdditionalDirectLight == null)
        {
            Color lightClr = Color.Lerp(lightSettings.LightColor.Evaluate(GameTime.solarTime), currentWeatherLightMod, currentWeatherLightMod.a);
            MainLight.color = Color.Lerp(lightClr, interiorZoneSettings.currentInteriorDirectLightMod, interiorZoneSettings.currentInteriorDirectLightMod.a);

            float lightIntensity;
            // Set sun and moon intensity
            if (!isNight)
            {
                lightIntensity = lightSettings.directLightSunIntensity.Evaluate(GameTime.solarTime);

                Components.Sun.transform.LookAt(new Vector3(transform.position.x, transform.position.y - lightSettings.directLightAngleOffset, transform.position.z));
                if (lightSettings.stopRotationAtHigh == false || lightSettings.stopRotationAtHigh && GameTime.solarTime >= lightSettings.rotationStopHigh)
                {
                    Components.DirectLight.rotation = Components.Sun.transform.rotation;
                }
            }
            else
            {
                lightIntensity = lightSettings.directLightMoonIntensity.Evaluate(GameTime.lunarTime);

                Components.Moon.transform.LookAt(new Vector3(transform.position.x, transform.position.y - lightSettings.directLightAngleOffset, transform.position.z));
                if (lightSettings.stopRotationAtHigh == false || lightSettings.stopRotationAtHigh && GameTime.lunarTime >= lightSettings.rotationStopHigh)
                {
                    Components.DirectLight.rotation = Components.Moon.transform.rotation;
                }
            }
            // Set the light and shadow intensity
            MainLight.intensity = Mathf.Lerp(MainLight.intensity, lightIntensity, Time.deltaTime * lightSettings.lightIntensityTransitionSpeed);
            MainLight.shadowStrength = Mathf.Clamp01(lightSettings.shadowIntensity.Evaluate(GameTime.solarTime) + shadowIntensityMod);
        }
        else if (lightSettings.directionalLightMode == EnvLightSettings.LightingMode.Dual && Components.AdditionalDirectLight != null)
        {
            if(AdditionalLight == null)
               AdditionalLight = Components.AdditionalDirectLight.GetComponent<Light>();

            Color lightClr = Color.Lerp(lightSettings.LightColor.Evaluate(GameTime.solarTime), currentWeatherLightMod, currentWeatherLightMod.a);
            MainLight.color = Color.Lerp(lightClr, interiorZoneSettings.currentInteriorDirectLightMod, interiorZoneSettings.currentInteriorDirectLightMod.a);
            AdditionalLight.color = MainLight.color;

            float lightIntensitySun, lightIntensityMoon;
            lightIntensitySun = lightSettings.directLightSunIntensity.Evaluate(GameTime.solarTime);
            lightIntensityMoon = lightSettings.directLightMoonIntensity.Evaluate(GameTime.lunarTime) * (1 - GameTime.solarTime);

            Components.Sun.transform.LookAt(new Vector3(transform.position.x, transform.position.y - lightSettings.directLightAngleOffset, transform.position.z));

            if (lightSettings.stopRotationAtHigh == false || lightSettings.stopRotationAtHigh && GameTime.solarTime >= lightSettings.rotationStopHigh)
            {
                Components.DirectLight.rotation = Components.Sun.transform.rotation;
            }

            Components.Moon.transform.LookAt(new Vector3(transform.position.x, transform.position.y - lightSettings.directLightAngleOffset, transform.position.z));
            if (lightSettings.stopRotationAtHigh == false || lightSettings.stopRotationAtHigh && GameTime.lunarTime >= lightSettings.rotationStopHigh)
            {
                Components.AdditionalDirectLight.rotation = Components.Moon.transform.rotation;
            }

            MainLight.intensity = Mathf.Lerp(MainLight.intensity, lightIntensitySun, Time.deltaTime * lightSettings.lightIntensityTransitionSpeed);
            MainLight.shadowStrength = Mathf.Clamp01(lightSettings.shadowIntensity.Evaluate(GameTime.solarTime) + shadowIntensityMod);

            AdditionalLight.intensity = Mathf.Lerp(AdditionalLight.intensity, lightIntensityMoon, Time.deltaTime * lightSettings.lightIntensityTransitionSpeed);
            AdditionalLight.shadowStrength = Mathf.Clamp01(lightSettings.shadowIntensity.Evaluate(GameTime.solarTime) + shadowIntensityMod);
        }
    }

    #endregion


    #region Seasons

    public void UpdateSeasons()
    {
        if (Seasons.calcSeasons)
        {
            int dayData = GameTime.Days;
            switch (dayData)
            {
                case 0:
                    Seasons.currentSeason = EnvSeasons.Seasons.Winter;
                    break;
                case 91:
                    Seasons.currentSeason = EnvSeasons.Seasons.Spring;
                    break;
                case 183:
                    Seasons.currentSeason = EnvSeasons.Seasons.Summer;
                    break;
                case 276:
                    Seasons.currentSeason = EnvSeasons.Seasons.Autumn;
                    break;
            }
        }
    }

    #endregion

    #region Effect Setup

    

    #endregion
}