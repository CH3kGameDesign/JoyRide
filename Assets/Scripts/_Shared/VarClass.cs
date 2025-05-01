using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VarClass : MonoBehaviour
{
    [System.Serializable]
    public class blockClass
    {
        public string id = "";
        public string name = "";
        public string description = "";

        public AddressableObjectClass objAddressable = new AddressableObjectClass();
        public AddressableSpriteClass sprAddressable = new AddressableSpriteClass();

        public string type = "";
        public string spawnGroup = "";

        public float curValue = 0;
        public float curWeight = 0;

        public numberClass value = new numberClass();
        public numberClass weight = new numberClass();

    }
    [System.Serializable]
    public class numberClass
    {
        public float Base = 0;
        public float Linear = 0;
        public float Coefficient2 = 0;
        public float Coefficient3 = 0;
        public float MilestoneRate = 0;
        public float MilestoneMultiplier = 0;

        public float GetNumAtLevel(int level)
        {
            float temp =
                Base
                + (Linear * (level - 1))
                + (Coefficient2 * Mathf.Pow(level - 1, 2))
                + (Coefficient3 * Mathf.Pow(level - 1, 3))
                ;
            if (MilestoneRate > 0)
                temp *= 1 + (MilestoneMultiplier * Mathf.Floor(level / MilestoneRate));

            return temp;
        }

        public numberClass Clone()
        {
            numberClass temp = new numberClass();
            temp.Base = Base;
            temp.Linear = Linear;
            temp.Coefficient2 = Coefficient2;
            temp.Coefficient3 = Coefficient3;
            temp.MilestoneRate = MilestoneRate;
            temp.MilestoneMultiplier = MilestoneMultiplier;
            return temp;
        }
    }
    [System.Serializable]
    public class AddressableObjectClass
    {
        public string id = "";
        public GameObject obj = null;
    }

    [System.Serializable]
    public class AddressableSpriteClass
    {
        public string id = "";
        public Sprite spr = null;
    }
}

[System.Serializable]
public class numberClass
{
    public int curNumber = 0;

    public float Base = 100;
    public float Linear = 0;
    public float Coefficient2 = 20;
    public float Coefficient3 = 0;
    public float MilestoneRate = 0;
    public float MilestoneMultiplier = 0;

    public int Update(int _level)
    {
        float _result = Base;
        _result += Linear * _level;
        _result += Coefficient2 * Mathf.Pow(_level, 2);
        _result += Coefficient3 * Mathf.Pow(_level, 3);
        if (MilestoneRate > 0)
            _result *= 1 + (MilestoneMultiplier * Mathf.Floor((_level + 1) / MilestoneRate));
        curNumber = Mathf.FloorToInt(_result);
        return curNumber;
    }
}
