﻿/*
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
        public static class BlockUtilities
        {
            public static List<IMyTerminalBlock> ConvertToTerminalBlockList<T>(List<T> groupBlocks,Logger logger) where T : class
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
                    logger.ShowException(e,$"Something went wrong trying to convert to {typeof(T)}!");
                }
                return terminalBlocksList;
            }
            public static List<IMyTerminalBlock> ConvertToTerminalBlockList(IMyBlockGroup groupBlocks,Logger logger) 
            {
                List<IMyTerminalBlock> terminalBlocksList = new List<IMyTerminalBlock>();
                try
                {
                    groupBlocks.GetBlocks(terminalBlocksList);
                }
                catch(Exception e)
                {
                    logger.ShowException(e,$"Something went wrong trying to convert to TerminalBlockList!");
                }
                return terminalBlocksList;
            }
            public static List<T> ConvertToTypedList<T>(List<IMyTerminalBlock> groupBlocks,Logger logger)
         where T : class
            {
                List<T> typedList = new List<T>();
                try
                {
                    for(int i = 0;i < groupBlocks.Count;i++)
                    {
                        typedList.Add(groupBlocks[i] as T);
                    }
                }
                catch(Exception e)
                {
                    logger.ShowException(e,$"Something went wrong trying to convert to List of {typeof(T)}!");
                }
                return typedList;
            }
            public static List<T> ConvertToTypedList<T>(IMyBlockGroup groupBlocks,Logger logger)
         where T : class
            {
                List<T> typedList = new List<T>();
              
                try
                {
                    groupBlocks.GetBlocksOfType(typedList);
                }
                catch(Exception e)
                {
                    logger.ShowException(e,$"Something went wrong trying to convert to List of {typeof(T)}!");
                }
                return typedList;
            }
        }   
    }
}
