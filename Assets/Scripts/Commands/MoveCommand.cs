using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    private IMovable _movable;
    private Vector3 _direction;

    public MoveCommand(IMovable movable, Vector3 direction)
    {
        _movable = movable;
        _direction = direction;
    }
    public void Execute() => _movable.Move(_direction);

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
