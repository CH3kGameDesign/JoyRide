using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private PlayerController PC_player;
    private float f_spawnTimer = 0;
    public Vector2 V2_spawnDelay = new Vector2(0.25f, 1f);
    public Vector2Int V2_spawnAmount = new Vector2Int(2, 10);
    public Vector2 V2_spawnRadius = new Vector2(10, 20);

    public BoxCollider BC_bounds;

    public List<spawnClass> SpawnList = new List<spawnClass>();
    private float f_totalSpawnWeight = 0;

    [System.Serializable]
    public class spawnClass
    {
        public GameObject _gameObject;
        public float _weight;
    }

    private void Start()
    {
        PC_player = PlayerController.Instance;
        TotalSpawnWeight_Update();
    }

    void TotalSpawnWeight_Update()
    {
        f_totalSpawnWeight = 0;
        foreach (var item in SpawnList)
            f_totalSpawnWeight += item._weight;
    }

    private void Update()
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
        if (f_spawnTimer <= 0)
        {
            Spawn();
            f_spawnTimer = Random.Range(V2_spawnDelay.x, V2_spawnDelay.y);
        }
        else
            f_spawnTimer -= Time.deltaTime;
    }
    void Spawn()
    {
        int _amt = Random.Range(V2_spawnAmount.x, V2_spawnAmount.y);
        GameObject _tarEnemy = GetRanEnemy();
        for (int i = 0; i < _amt; i++)
        {
            Vector2 _ranPos = Vector3.Normalize(Random.insideUnitCircle) * Random.Range(V2_spawnRadius.x, V2_spawnRadius.y);
            Vector3 _spawnPos = PC_player.transform.position + new Vector3(_ranPos.x,0, _ranPos.y);
            _spawnPos = BC_bounds.ClosestPoint(_spawnPos);
            Enemy _enemy = Instantiate(_tarEnemy, transform).transform.GetChild(0).GetComponent<Enemy>();
            _enemy.transform.position = _spawnPos;
            _enemy.OnCreate(50);
        }
    }

    GameObject GetRanEnemy()
    {
        float _ran = Random.Range(0, f_totalSpawnWeight);
        foreach (var item in SpawnList)
        {
            _ran -= item._weight;
            if (_ran <= 0)
                return item._gameObject;
        }
        return SpawnList[0]._gameObject;
    }
}
