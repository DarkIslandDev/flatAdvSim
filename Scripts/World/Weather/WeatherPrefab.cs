using UnityEngine;
using System.Collections.Generic;



public class WeatherPrefab : MonoBehaviour
{
    public WeatherPreset weatherPreset;
    
    [HideInInspector] public List<ParticleSystem> effectSystems = new List<ParticleSystem>();
    [HideInInspector] public List<float> effectEmmisionRates = new List<float>();
}