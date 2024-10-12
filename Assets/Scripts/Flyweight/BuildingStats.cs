using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingStats", menuName = "Stats/Building", order = 0)]
public class BuildingStats : ScriptableObject
{
    [SerializeField] private BuildingStatValues _buildingStatValues;

    public int MaxLife => _buildingStatValues.MaxLife;
    public float Overclock => _buildingStatValues.Overclock;
}

[System.Serializable]
public struct BuildingStatValues
{
    public int MaxLife;
    public float Overclock;
}
