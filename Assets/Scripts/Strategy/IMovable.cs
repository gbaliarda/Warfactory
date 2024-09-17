using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    float MovementSpeed { get; }
    void Move(Vector2 direction);
}
