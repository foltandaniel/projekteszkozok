using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERROR_HANDLER : MonoBehaviour {

    void Awake()
    {
       // Debug.Log("Error handler");
        Application.logMessageReceived += HandleException;

    }

    void HandleException(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            Console.LogError(condition);

            string firstline = stackTrace.Substring(0, stackTrace.IndexOf('\n'));
            Console.LogError("STACKTRACE TOP ELEMENT: \n" +firstline + "\nA stacktrace további elemét a logfájlban megtudod nézni!");
        }
    }
}
