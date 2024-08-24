using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    GameObject BuildingPrefab { get; }
    void Build(Vector3 position);

    void Destroy();
}
