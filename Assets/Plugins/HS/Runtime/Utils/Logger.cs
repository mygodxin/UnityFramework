using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HS
{
    /// <summary>
    ///全局日志
    /// </summary>
    public class Logger
    {
        //缓存的最大日志数量
        private static int Max_Count = 50;
        public readonly static string logger = "Logger";
        private static readonly int SaveInterval = 10;
        private static List<string> logs;
        public static void Init()
        {
#if !UNITY_EDITOR
            logs = LocalStorage.Read(logger, new List<string>());
            Timer.Inst.SetInterval(SaveInterval, OnSaveLog);
            Application.logMessageReceived += HandleLogMessage;
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
            Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);
#endif
        }
        private static void HandleLogMessage(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Exception || type == LogType.Error)
            {
                var log = "[" + DateTime.Now + "]" + logString + ":" + stackTrace;
                //UnityEngine.Debug.Log("全局Error捕获: " + log);
                if (logs.Count >= Max_Count)
                {
                    logs.RemoveAt(0);
                }
                logs.Add(log);
            }
        }

        private static void OnSaveLog()
        {
            LocalStorage.Save(logger, logs);
        }

        public static void Debug(object msg, object state = null)
        {
            //UnityEngine.Debug.Log(msg);
        }

        public static Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public static void Error(object msg, object state = null)
        {
            UnityEngine.Debug.LogError(msg);
        }


        public static void Info(object msg, object state = null)
        {
            UnityEngine.Debug.Log(msg);
        }

        public static void Warn(object msg, object state = null)
        {
            UnityEngine.Debug.LogWarning(msg);
        }
    }
}
