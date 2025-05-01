using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade Manager", menuName = "JoyRide/Managers/Upgrade Manager")]
public class UpgradeScriptable : ScriptableObject
{
    public List<UpgradeClass> list = new List<UpgradeClass>();

    public UpgradeClass GetUpgradeFromID(string _id)
    {
        foreach (var item in list)
            if (item.id == _id)
                return item;
        Debug.LogError("Couldn't find upgrade from id: " + _id);
        return null;
    }


    public List<UpgradeClass> CloneList()
    {
        List<UpgradeClass> _temp = new List<UpgradeClass>();
        foreach (var item in list)
            _temp.Add(item.Clone());
        return _temp;
    }
}