using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public TextMeshProUGUI TM_name;
    public TextMeshProUGUI TM_description;

    public GameObject[] G_upgradeMarkers;
    public Image[] I_upgradeBGs;

    [HideInInspector] public UpgradeClass Upgrade;

    public void OnCreate(UpgradeClass _upgrade)
    {
        Upgrade = _upgrade;
        TM_name.text = Upgrade.name;
        TM_description.text = Upgrade.description;

        for (int i = 0; i < G_upgradeMarkers.Length; i++)
        {
            G_upgradeMarkers[i].SetActive(i < Upgrade.level);
            if (i <= Upgrade.level)
                I_upgradeBGs[i].color = new Color(0.7764706f, 0.4431373f, 0.2470588f);
            else
                I_upgradeBGs[i].color = new Color(0.4705882f, 0.227451f, 0.1764706f);
        }
    }

    public void OnClick()
    {
        UpgradeManager.Instance.LevelUp_Upgrade(Upgrade);
    }
}
