using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCommand : ICommand
{
    private IBuffable _buffable;
    private IPotion _buff;

    public BuffCommand(IBuffable buffable, IPotion buff)
    {
        _buffable = buffable;
        _buff = buff;
    }

    public void Execute() => _buffable.AddBuff(_buff);

    public void Undo() => _buffable.RemoveBuff(_buff);
}