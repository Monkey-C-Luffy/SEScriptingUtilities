/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;


namespace SEScripting
{
    public static class Logging
    {
        public static bool DebugEnable { get; set; }
        static List<string> debugStringsList = new List<string>();

        public static MyGridProgram gridProgram;

        public static void ShowDebug()
        {
            for(int i = 0;i < debugStringsList.Count;i++)
            {
                gridProgram.Echo(debugStringsList[i]);
            }
        }

        public static void DebugLog(string debugString,bool showDebug = false)
        {
            if(debugStringsList.Count > 20)
            {
                debugStringsList.RemoveAt(0);
            }
            debugStringsList.Add(debugString);
            if(showDebug) ShowDebug();
        }
        public static void ShowException(Exception e,string extraMessage = "")
        {
            gridProgram.Echo($"An error happened: {extraMessage}:\n{e.Message}{e.StackTrace}");
        }
    }
}
