using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public bool realTime = true;
    [Space(10)]
    private Vector3 curPosNoise;
    private Vector3 tarPosNoise;
    public float posRadius = 1;
    public float posSpeed = 0.5f;
    public bool posX = true;
    public bool posY = true;
    public bool posZ = true;
    [Space(10)]
    private Vector3 curRotNoise;
    private Vector3 tarRotNoise;
    public float rotRadius = 10;
    public float rotSpeed = 5f;
    public bool rotX = true;
    public bool rotY = true;
    public bool rotZ = true;

    // Update is called once per frame
    void Update()
    {
        if (posRadius > 0 && posSpeed > 0)
            PosNoise_Update();
        if (rotRadius > 0 && rotSpeed > 0)
            RotNoise_Update();
    }

    void PosNoise_Update()
    {
        Vector3 _tar;
        if (realTime)
            _tar = Vector3.MoveTowards(curPosNoise, tarPosNoise, posSpeed * Time.unscaledDeltaTime);
        else
            _tar = Vector3.MoveTowards(curPosNoise, tarPosNoise, posSpeed * Time.deltaTime);
        Vector3 _dist = curPosNoise - _tar;
        transform.localPosition += _dist;
        curPosNoise = _tar;
        if (_tar == tarPosNoise)
            PosNoise_Refresh();
    }

    void PosNoise_Refresh()
    {
        Vector3 _ran = Random.insideUnitSphere * posRadius;
        if (!posX) _ran.x = 0;
        if (!posY) _ran.y = 0;
        if (!posZ) _ran.z = 0;
        tarPosNoise = _ran;
    }

    void RotNoise_Update()
    {
        Vector3 _tar;
        if (realTime)
            _tar = Vector3.MoveTowards(curRotNoise, tarRotNoise, rotSpeed * Time.unscaledDeltaTime);
        else
            _tar = Vector3.MoveTowards(curRotNoise, tarRotNoise, rotSpeed * Time.deltaTime);
        Vector3 _dist = curRotNoise - _tar;
        transform.localEulerAngles += _dist;
        curRotNoise = _tar;
        if (_tar == tarRotNoise)
            RotNoise_Refresh();
    }

    void RotNoise_Refresh()
    {
        Vector3 _ran = Random.insideUnitSphere * rotRadius;
        if (!rotX) _ran.x = 0;
        if (!rotY) _ran.y = 0;
        if (!rotZ) _ran.z = 0;
        tarRotNoise = _ran;
    }
}
