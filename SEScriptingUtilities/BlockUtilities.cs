/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace SEScripting
{
    public static class BlockUtilities
    {
 
        public static List<IMyTerminalBlock> ConvertToTerminalBlockList<T>(RequiredGroup<T> requiredGroup) where T : class
        {
            return ConvertToTerminalBlockList(requiredGroup.GroupBlocks);
        }
        public static List<IMyTerminalBlock> ConvertToTerminalBlockList<T>(List<T> groupBlocks) where T : class
        {
            List<IMyTerminalBlock> terminalBlocksList = new List<IMyTerminalBlock>();
            try
            {
                for(int i = 0;i < groupBlocks.Count;i++)
                {
                    terminalBlocksList.Add(groupBlocks[i] as IMyTerminalBlock);
                }
            }
            catch(Exception e)
            {
                Logging.ShowException(e,$"Something went wrong trying to convert to {typeof(T)}!");
            }
            return terminalBlocksList;
        }
    }
}
