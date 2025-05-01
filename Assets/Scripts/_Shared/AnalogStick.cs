using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AnalogStick : Selectable
{
    public RectTransform RT_Base;
    public RectTransform RT_Pad;

    public UnityEvent<Vector2> UE_Output;

    private bool b_selected = false;
    private Vector2 v2_output = Vector2.zero;
    private float f_maxDistance = 200;

    public void Update()
    {
        if (b_selected)
        {
            Vector3 _temp = Vector2.ClampMagnitude(Input.mousePosition - RT_Base.position,f_maxDistance);
            RT_Pad.position = _temp + RT_Base.position;
            v2_output = Vector2.ClampMagnitude(_temp/f_maxDistance,1);

            UE_Output.Invoke(v2_output);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        RT_Pad.gameObject.SetActive(true);
        RT_Base.gameObject.SetActive(true);
        RT_Base.position = eventData.position;
        b_selected = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        RT_Pad.gameObject.SetActive(false);
        RT_Base.gameObject.SetActive(false);
        b_selected = false;
        v2_output = Vector2.zero;

        UE_Output.Invoke(v2_output);
    }
}
