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
            public bool _debugEnable = true;
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

            public const int maxDebugLines = 20;

            static List<string> debugStringsList = new List<string>();

            private BlockManager _blockManagerInstance;
            public BlockManager BlockManager
            {
                get
                {
                    return _blockManagerInstance;
                }
                set
                {
                    if(_blockManagerInstance == null) _blockManagerInstance = value;
                }
            }

            public MyGridProgram ProgramInstance
            {
                get
                {
                    if(_blockManagerInstance != null) return _blockManagerInstance.ProgramInstance;
                    return null;
                }
            }
            public int InstructionsCount
            {
                get
                {
                    return ProgramInstance.Runtime.CurrentInstructionCount;
                }
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
                    ProgramInstance.Echo(debugStringsList[i]);
                }
            }

            public void DebugLog(string debugString,bool showDebug = false)
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
                ProgramInstance.Echo($"An error happened: {extraMessage}:\n{e.Message}{e.StackTrace}");
            }
        }
    }
}
