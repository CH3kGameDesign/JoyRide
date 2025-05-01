using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int I_value = 10;
    
    public void GiveReward()
    {
        UpgradeManager.Instance.Currency_Add(I_value);
        Destroy(this.gameObject);
    }
}
