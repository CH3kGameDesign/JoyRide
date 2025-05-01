using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSurface : Target
{
    public float F_damage = 10;
    public float F_attackSpeed = 0.5f;

    private List<Target> T_recentlyHit = new List<Target>();
    private PlayerController PC_Player;

    private void Start()
    {
        PC_Player = PlayerController.Instance;
    }
    public override void OnHit(float _damage)
    {
        //Ignore Damage
    }

    public void Update()
    {
        switch (PC_Player.GameState)
        {
            case PlayerController.GameState_Enum.inactive:
                break;
            case PlayerController.GameState_Enum.active:
                UpdateActive();
                break;
            default:
                break;
        }
    }
    void UpdateActive()
    {
        for (int i = 0; i < T_targetList.Count; i++)
        {
            if (T_targetList[i] == null)
            {
                T_targetList.RemoveAt(i);
                i--;
                continue;
            }
            if (!T_recentlyHit.Contains(T_targetList[i]))
                HitTarget(T_targetList[i]);
        }
    }
    private void HitTarget(Target _target)
    {
        _target.OnHit(F_damage);
        T_recentlyHit.Add(_target);
        Invoke("RecentlyHitRemove", F_attackSpeed);
    }
    void RecentlyHitRemove()
    {
        T_recentlyHit.RemoveAt(0);
    }
}
