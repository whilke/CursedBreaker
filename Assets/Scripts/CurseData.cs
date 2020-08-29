using System;
using enBask;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "curse", menuName = "Data/CurseData", order = 0)]
public class CurseData : SerializedScriptableObject
{
    public string name;

    [PropertyRange(0d, 1d)]
    public float InfectionChance;

    public Color Color;

    public float IncubationTime;

    public float InfectionRate;

    public Vector2 Range;

    public int MaxInfectionCount;

    public Type Effect;
}