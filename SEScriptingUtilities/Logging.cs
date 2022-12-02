/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;


namespace IngameScript
{
    partial class Program
    {
        public static class Logging
        {
            public static bool DebugEnable { get; set; }

            public const int maxDebugLines = 20;

            static List<string> debugStringsList = new List<string>();

            public static MyGridProgram gridProgram;

            public static int InstructionsCount
            {
                get
                {
                    return gridProgram.Runtime.CurrentInstructionCount;
                }
            }
            public static void ShowInstructionCount(string message="")
            {
                DebugLog($"Instructions count:{InstructionsCount},at {message}",true);
            }
            public static void ShowDebug()
            {
                if(!DebugEnable) return;
                for(int i = 0;i < debugStringsList.Count;i++)
                {
                    gridProgram.Echo(debugStringsList[i]);
                }
            }

            public static void DebugLog(string debugString,bool showDebug = false)
            {
                if(!DebugEnable) return;
                if(maxDebugLines>0 &&debugStringsList.Count > maxDebugLines)
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
}
