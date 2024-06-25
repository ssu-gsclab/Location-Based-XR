using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LogToUI : MonoBehaviour
{
    public Text DebugText;
    // Use this for initialization
    private static LogToUI Instance;
    public int maxLogLines = 13;
    private static List<string> logList = new List<string>();
    private static List<string> newLogList = new List<string>();

    void Start()
    {
        DebugText = DebugText == null ? GetComponent<Text>() : DebugText;
        Instance = this;
        DebugText.text = "";
    }

    public static void Log(string txt)
    {
        Debug.Log(txt);
        newLogList.Add(txt);
    }

    public static void LogWarning(string txt)
    {
        Debug.LogWarning(txt);
        newLogList.Add("<color=\"yellow\">" + txt + "</color>");
    }

    public static void LogError(string txt)
    {
        Debug.LogError(txt);
        newLogList.Add("<color=\"red\">" + txt + "</color>");
    }

    void Update()
    {
        if (newLogList != null && DebugText != null && newLogList.Count > 0)
        {
            logList.AddRange(newLogList);
            newLogList.Clear();
            if (logList.Count > maxLogLines)
            {
                logList.RemoveRange(0, logList.Count - maxLogLines);
            }

            DebugText.text = string.Join("\n", logList);
        }
    }
}