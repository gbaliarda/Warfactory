using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveController : MonoBehaviour, IMovable
{
    public float MovementSpeed => GetComponent<Actor>().Stats.MovementSpeed;

    public void Move(Vector3 direction)
    {
        transform.position += MovementSpeed * Time.deltaTime * direction;
    }
}
