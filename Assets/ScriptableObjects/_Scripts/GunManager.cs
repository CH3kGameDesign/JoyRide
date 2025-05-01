using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "JoyRide/Managers/Gun Manager", fileName ="New Gun Manager")]
public class GunManager : ScriptableObject
{
    public static GunManager Instance;
    public List<Gun> list = new List<Gun>();
    [System.Serializable]
    public class Gun
    {
        public string name;
        public string id;
        public int level = 1;
        public float timer = 0;
        
        public LevelStat[] statList = new LevelStat[0];
        public Addressable_GameObject bullet;

        public LevelStat stats()
        {
            return statList[level];
        }

        public Gun Clone()
        {
            Gun _temp = new Gun();
            _temp.name = name;
            _temp.level = 1;
            _temp.statList = Stats_Clone();
            _temp.bullet = bullet;
            return _temp;
        }

        LevelStat[] Stats_Clone()
        {
            LevelStat[] _temp = new LevelStat[statList.Length];
            for (int i = 0; i < statList.Length; i++)
            {
                LevelStat stat = new LevelStat();
                stat.damage = statList[i].damage;
                stat.dps = statList[i].dps;
                _temp[i] = stat;
            }
            return _temp;
        }
    }
    [System.Serializable]
    public class LevelStat
    {
        public float dps;
        public float damage;
    }

    [System.Serializable]
    public class Addressable_GameObject
    {
        public string id;
        public GameObject gameObject;
    }

    public Gun GetGunByID(string _id)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (_id == list[i].id)
                return list[i].Clone();
        }
        Debug.LogError("Couldn't Find Gun by ID: " + _id);
        return null;
    }
    public Gun GetGunByNum(int _num)
    {
        if (_num < list.Count)
            return list[_num].Clone();
        Debug.LogError("Requested Gun is Outside of Range: " + _num);
        return null;
    }
}
