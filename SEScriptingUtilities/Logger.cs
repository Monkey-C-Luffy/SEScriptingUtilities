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
        public class Logger
        {
            private bool _debugEnable = true;
            public bool DebugEnable
            {
                get
                {
                    return _debugEnable;
                }
                set
                {
                    _debugEnable = value;
                }
            }

            public int maxDebugLines = 20;

            static List<string> debugStringsList = new List<string>();

            private readonly MyGridProgram _programInstance;
            public int InstructionsCount
            {
                get
                {
                    return _programInstance.Runtime.CurrentInstructionCount;
                }
            }
            public Logger(MyGridProgram programInstance)
            {
                _programInstance = programInstance;
            }
            public void ShowInstructionCount(string message="")
            {
                DebugLog($"Instructions count:{InstructionsCount},at {message}",true);
            }
            public void ShowDebug()
            {
                if(!DebugEnable) return;
                for(int i = 0;i < debugStringsList.Count;i++)
                {
                    _programInstance.Echo(debugStringsList[i]);
                }
            }
            public void DebugLog(string debugString,bool showDebug = true)
            {
                if(!DebugEnable) return;
                if(maxDebugLines>0 &&debugStringsList.Count > maxDebugLines)
                {
                    debugStringsList.RemoveAt(0);
                }             
                debugStringsList.Add(debugString);
                if(showDebug) ShowDebug();
            }
            public void ShowException(Exception e,string extraMessage = "")
            {
                _programInstance.Echo($"An error happened:{e.Message}\nStackTrace:{e.StackTrace}\nFurther details:{extraMessage}");
            }       
        }
    }
}
