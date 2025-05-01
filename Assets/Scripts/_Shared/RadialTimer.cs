using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialTimer : MonoBehaviour
{
    public Image I_icon;
    public Image I_fill;

    [HideInInspector] public int i_id;

    private float f_maxTimer = 0;

    public void OnCreate(Sprite _icon, float _timerLength, int _id)
    {
        I_fill.fillAmount = 1;
        f_maxTimer = _timerLength;
        I_icon.sprite = _icon;
        i_id = _id;
    }

    public void UpdateFill(float _remainingTime)
    {
        I_fill.fillAmount = _remainingTime / f_maxTimer;
    }
}
