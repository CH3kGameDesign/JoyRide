using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotPerSecond = Vector3.zero;
    public bool B_realTime = true;

    private void Update()
    {
        if (B_realTime)
            transform.localEulerAngles += rotPerSecond * Time.unscaledDeltaTime;
        else
            transform.localEulerAngles += rotPerSecond * Time.deltaTime;
    }
}
