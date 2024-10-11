using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private GameObject _basePortal;
    public override void Die()
    {

        base.Die();
    }
}
