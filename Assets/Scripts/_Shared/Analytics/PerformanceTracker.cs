using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class PerformanceTracker
{
    private const string fileName = "CC_PerformanceOutput.csv";

    private const bool sendToGoogle = true;
    private const string BASE_URL = "https://docs.google.com/forms/d/e/1FAIpQLSer_u3QsYC90txNyXAxTDwx3-la4w-VaLikkF4jNa3AidaSgQ/formResponse";

    static List<TrackedData> trackedData = new List<TrackedData>();
    
    public static void TrackData(TrackedData _data)
    {
        trackedData.Add(_data);
    }
    public static void ClearData()
    {
        trackedData.Clear();
    }
    
    private static void SaveToDownloads(string directoryPath, string fileName)
    {
        string filePath = Path.Combine(directoryPath, fileName);
        var csvGenerator = new CsvGenerator<TrackedData>();
        List<string> _csv = csvGenerator.GenerateCsv(trackedData, fileName);
        File.WriteAllLines(filePath, _csv);
    }

    public static void SendToGoogle()
    {
        if (sendToGoogle && trackedData.Count>=1)
        {
            WWWForm form = new WWWForm();
            string temp = "";
            var csvGenerator = new CsvGenerator<TrackedData>();
            List<string> _csv = csvGenerator.GenerateCsv(trackedData, fileName);
            foreach (var item in _csv)
            {
                temp += item + "\n";
            }
            form.AddField("entry.1355281758", temp);
            byte[] rawData = form.data;
            GoogleFormExport.Export(BASE_URL, rawData);
        }
        ClearData();
    }
}

public struct TrackedData
{
    public string BuildInfo { get; private set; }
    public string GameType { get; private set; }
    public string DeviceInfo { get; private set; }
    public int LevelNum { get; private set; }
    public float GameLength { get; private set; }
    public string AverageFPS { get; private set; }
    public int BlocksBroken { get; private set; }
    public int ElixirGained { get; private set; }
    public int GemsGained { get; private set; }
    public int CardsGained { get; private set; }
    public int PowerUpsUsed { get; private set; }

    public TrackedData(string _buildInfo, string _gameType, string _deviceInfo, int _levelNum, float _gameLength, string _fps, int _blocksBroken, int _elixirGained, int _gemsGained, int _cardsGained, int _powerUpsUsed)
    {
        BuildInfo = _buildInfo;
        GameType = _gameType;
        DeviceInfo = _deviceInfo;
        LevelNum = _levelNum;
        GameLength = _gameLength;
        AverageFPS = _fps;
        BlocksBroken = _blocksBroken;
        ElixirGained = _elixirGained;
        GemsGained = _gemsGained;
        CardsGained = _cardsGained;
        PowerUpsUsed = _powerUpsUsed;
    }
}