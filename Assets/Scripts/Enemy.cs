using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : Target
{
    public NavMeshAgent NMA_agent;
    private PlayerController PC_player;
    public float F_destinationUpdateDelay = 0.5f;
    private float f_destTimer = 0;
    public Transform T_canvasHolder;
    public Slider S_healthBar;
    public GameObject G_reward;

    private List<GunManager.Gun> guns = new List<GunManager.Gun>();

    [Header("Debugging Stuf")]
    public int DEBUG_gunNum = 0;

    public void Start()
    {
        PC_player = PlayerController.Instance;
        DEBUG_AddGun();
    }
    void DEBUG_AddGun()
    {
        guns.Add(GunManager.Instance.GetGunByNum(DEBUG_gunNum));
    }

    public void Update()
    {
        switch (PC_player.GameState)
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
        ShootUpdate();
        if (f_destTimer <= 0)
        {
            DestinationUpdate();
            f_destTimer = F_destinationUpdateDelay;
        }
        else
            f_destTimer -= Time.deltaTime;
        T_canvasHolder.position = NMA_agent.transform.position;
    }
    public void DestinationUpdate()
    {
        NMA_agent.SetDestination(PC_player.transform.position);
    }

    void ShootUpdate()
    {
        foreach (var item in guns)
        {
            if (item.timer <= 0)
            {
                for (int i = 0; i < T_targetList.Count; i++)
                {
                    if (T_targetList[i] == null)
                    {
                        T_targetList.RemoveAt(i);
                        i--;
                        continue;
                    }
                    Projectile _bullet = Instantiate(item.bullet.gameObject).GetComponent<Projectile>();
                    _bullet.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                    Vector3 _target = T_targetList[0].GetPos();
                    _target.y = 0.5f;
                    _bullet.transform.LookAt(_target);
                    _bullet.OnCreate(item.stats().damage, TE_targetType == targetEnum.player);
                    item.timer = 1;
                    break;
                }
            }
            else
                item.timer -= Time.deltaTime * item.stats().dps;
        }
    }


    public override void OnCreate(float _health)
    {
        base.OnCreate(_health);
    }

    public override void OnHit(float _damage)
    {
        S_healthBar.gameObject.SetActive(true);
        S_healthBar.value = F_health / F_maxHealth;
        base.OnHit(_damage);
    }

    public override void OnDeath()
    {
        if (G_reward != null)
            Instantiate(G_reward, transform.position - Vector3.up, G_reward.transform.rotation);
        base.OnDeath();
    }

    public override Vector3 GetPos()
    {
        return NMA_agent.transform.position;
    }
}
