using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsDirectionCommand : ICommand
{
    private Transform _transform;
    private Vector2 _direction;
    private float _distanceToMove = 15f;
    private float _speed;

    public MoveTowardsDirectionCommand(Transform transform, Vector2 direction, float speed)
    {
        _transform = transform;
        _direction = direction.normalized;
        _speed = speed;
    }

    public void Execute()
    {
        if (_transform == null) return;

        Vector2 currentPosition = _transform.position;
        Vector2 destination = currentPosition + _direction * _distanceToMove;

        _transform.position = Vector2.MoveTowards(currentPosition, destination, _speed * Time.deltaTime);
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
