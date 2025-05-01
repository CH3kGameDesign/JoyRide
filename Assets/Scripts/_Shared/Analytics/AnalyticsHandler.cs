using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (GoogleFormExport))]
public class AnalyticsHandler : MonoBehaviour
{

    public bool sendAnalytics = true;

    public static AnalyticsHandler Instance;
    GoogleFormExport GFE;
    float gameLength = 0;
    List<float> FPS = new List<float>();

    [HideInInspector] public string S_gameType = "undefined";

    [HideInInspector] public int I_levelNum = 0;

    [HideInInspector] public int I_blocksBroken = 0;
    [HideInInspector] public int I_elixirGained = 0;
    [HideInInspector] public int I_gemsGained = 0;
    [HideInInspector] public int I_cardsGained = 0;
    [HideInInspector] public int I_powerUpsUsed = 0;

    void Start()
    {
        Instance = this;
        GFE = GetComponent<GoogleFormExport>();
    }

    private void Update()
    {
        FPSUpdate();
        gameLength += Time.unscaledDeltaTime;
    }

    void FPSUpdate()
    {
        FPS.Add(1 / Time.unscaledDeltaTime);
    }

    public void Reset(string _gameType, int _levelNum)
    {
        gameLength = 0;
        FPS.Clear();

        S_gameType = _gameType;

        I_levelNum = _levelNum;

        I_blocksBroken = 0;
        I_elixirGained = 0;
        I_gemsGained = 0;
        I_cardsGained = 0;
        I_powerUpsUsed = 0;
    }

    public void TrackData()
    {
        float averageFPS = 0;
        foreach (var item in FPS)
            averageFPS += item;
        averageFPS /= FPS.Count;

        TrackedData _trackedData = new TrackedData
            (
                _buildInfo: $"buildVersion:{Application.version}|timeDate:{System.DateTime.Now.ToString("yyyy/MM/dd h:mm:ss tt")}",
                _gameType: S_gameType,
                _deviceInfo: $"onDevice:{!Application.isEditor}|device:{SystemInfo.deviceModel}",
                _levelNum: I_levelNum,
                _gameLength: gameLength,
                _fps: averageFPS.ToString("F0"),
                _blocksBroken: I_blocksBroken,
                _elixirGained: I_elixirGained,
                _gemsGained: I_gemsGained,
                _cardsGained: I_cardsGained,
                _powerUpsUsed: I_powerUpsUsed
            );
        PerformanceTracker.TrackData(_trackedData);
        SendAnalytics();
    }

    public void SendAnalytics()
    {
        if (sendAnalytics)
            PerformanceTracker.SendToGoogle();
    }

    public void OnApplicationQuit()
    {

    }
}
