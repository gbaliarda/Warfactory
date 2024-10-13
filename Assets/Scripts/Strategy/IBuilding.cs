using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    GameObject BuildingPrefab { get; }
    GameObject Build(Vector3 position, int rotation);

    void Destroy();
}
