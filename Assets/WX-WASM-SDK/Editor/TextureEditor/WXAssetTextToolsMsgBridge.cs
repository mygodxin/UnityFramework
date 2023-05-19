using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/**
    可执行脚本控制台接收消息解析器
     */
namespace WeChatWASM
{
    public class WXAssetTextToolsMsgBridge
    {
        private const string HEADER_LOG = "[LOG]";
        private const string HEADER_PROGRESS = "[PROGRESS]";
        private const string HEADER_DONE = "[DONE]";
        private const string HEADER_BIGMSGDONE = "[BIGMSGDONE]";
        private const string HEADER_DONEERR = "[DONEERR]";
        private const string HEADER_RECORDFILE = "[RECORDFILE]";

        private static List<string> RecordFiles = new List<string>();

        public static void ResetRecordFiles()
        {
            RecordFiles.Clear();
        }

        public static string[] GetRecordFiles()
        {
            return RecordFiles.ToArray();
        }

        public static void Parse(string msg,Action<string> callback, Action<int, int, string> progress)
        {
            if(msg.IndexOf(HEADER_LOG) == 0)
            {
                Debug.Log(msg.Substring(HEADER_LOG.Length));
            }else if (msg.IndexOf(HEADER_DONEERR) == 0)
            {
                Debug.LogError(msg.Substring(HEADER_LOG.Length));
            }else if(msg.IndexOf(HEADER_PROGRESS) == 0)
            {
                int total = 0, current = 0;
                string m = "";
                ParseProgressMsg(msg.Substring(HEADER_PROGRESS.Length),out current,out total,out m);
                progress(current, total, m);
            }else if(msg.IndexOf(HEADER_DONE) == 0)
            {
                callback(msg.Substring(HEADER_DONE.Length));
            }else if(msg.IndexOf(HEADER_BIGMSGDONE) == 0)
            {
                callback(ReadBigMsgDone(msg.Substring(HEADER_BIGMSGDONE.Length)));
            }
            else if(msg.IndexOf(HEADER_RECORDFILE) == 0)
            {
                RecordFiles.Add(msg.Substring(HEADER_RECORDFILE.Length));
            }
            else
            {
                return;
            }
        }


        private static void ParseProgressMsg(string content,out int current,out int total,out string msg)
        {
            string[] sp = content.Split(',');
            current = int.Parse(sp[0]);
            total = int.Parse(sp[1]);
            string token = $"{sp[0]},{sp[1]},";
            msg = content.Substring(content.IndexOf(token) + token.Length);
        }

        private static string ReadBigMsgDone(string path)
        {
            if (File.Exists(path))
            {
                string content = "";
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        content = reader.ReadToEnd();
                    }
                }
                File.Delete(path);
                return content;
            }
            else
            {
                return "";
            }
        }
    }
}
