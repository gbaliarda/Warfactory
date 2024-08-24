using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffable
{
    void AddBuff(IPotion potion);

    void RemoveBuff(IPotion potion);
}
