using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour, IAttackable
{
    public void Attack(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }

    public void AttackOnMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);
        Attack(hit.point);
    }
}
