using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public interface IPotion
{
    PotionStats PotionStats { get; }
    IBuffable Owner { get; }
    float DurationLeft { get; }
    float CooldownLeft { get; }

    void Buff();

    void SetCooldown(float cooldown);
    void SetDuration(float duration);
}
