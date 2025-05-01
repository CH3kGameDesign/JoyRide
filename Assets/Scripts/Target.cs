using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Transform T_holder;
    [HideInInspector]
    public float F_health = 50;
    public float F_maxHealth = 50;
    public List<Target> T_targetList = new List<Target>();
    public targetEnum TE_targetType = targetEnum.enemy;
    public enum targetEnum { player, enemy, obstacle, playerIgnore, enemyIgnore};

    private void Start()
    {
        F_health = F_maxHealth;
    }

    public virtual void OnCreate(float _health)
    {
        F_maxHealth = _health;
        F_health = _health;
    }

    public virtual void OnHit(float _damage)
    {
        F_health -= _damage;
        if (F_health <= 0)
            OnDeath();
    }

    public virtual void OnDeath()
    {
        if (T_holder != null)
            Destroy(T_holder.gameObject);
        else
            Destroy(gameObject);
    }

    public virtual Vector3 GetPos()
    {
        return transform.position;
    }

    public virtual void AddTarget(Target _target, bool _add = true)
    {
        switch (TE_targetType)
        {
            case targetEnum.player:
                if (_target.TE_targetType == targetEnum.enemy || _target.TE_targetType == targetEnum.obstacle)
                    TargetList_Edit(_target, _add);
                break;
            case targetEnum.enemy:
                if (_target.TE_targetType == targetEnum.player)
                    TargetList_Edit(_target, _add);
                break;
            case targetEnum.obstacle:
                break;
            case targetEnum.playerIgnore:
                if (_target.TE_targetType == targetEnum.enemy || _target.TE_targetType == targetEnum.obstacle)
                    TargetList_Edit(_target, _add);
                break;
            default:
                break;
        }
    }
    void TargetList_Edit(Target _target, bool _add)
    {
        if (_add)
        {
            if (!T_targetList.Contains(_target))
                T_targetList.Add(_target);
        }
        else
        {
            if (T_targetList.Contains(_target))
                T_targetList.Remove(_target);
        }

    }

    public virtual void RemoveTarget(Target _target)
    {
        AddTarget(_target, false);
    }
}
