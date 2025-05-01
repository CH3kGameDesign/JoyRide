using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCircle : MonoBehaviour
{
    public Target T_targetHandler;

    public void OnTriggerEnter(Collider other)
    {
        Target _target;
        if (other.TryGetComponent<Target>(out _target))
            T_targetHandler.AddTarget(_target);
    }
    public void OnTriggerExit(Collider other)
    {
        Target _target;
        if (other.TryGetComponent<Target>(out _target))
            T_targetHandler.RemoveTarget(_target);
    }
}
