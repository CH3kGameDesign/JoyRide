using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float F_speed = 10;
    public float F_lifetime = 5;
    float f_damage = 0;

    public bool B_hitEnvironment = true;
    bool b_hitPlayer = true;
    bool b_hitEnemies = false;
    
    public void OnCreate(float _damage, bool _player)
    {
        f_damage = _damage;
        b_hitPlayer = !_player;
        b_hitEnemies = _player;
        Destroy(this.gameObject, F_lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpdate();
    }

    private void MoveUpdate()
    {
        transform.position += transform.forward * F_speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player": if (b_hitPlayer) HitObject(other); break;
            case "Enemy": if (b_hitEnemies) HitObject(other); break;
            case "Obstacle": if (B_hitEnvironment) Break(); break;
            default:  break;
        }

    }

    void HitObject(Collider other)
    {
        other.GetComponent<Target>().OnHit(f_damage);
        Break();
    }

    void Break()
    {
        Destroy(this.gameObject);
    }

}
