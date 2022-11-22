﻿/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace SEScripting
{
    public static class Utils
    {
        public static MyGridProgram gridProgram;
        static List<string> debugStringsList = new List<string>();

        #region BlockFinding

        public static bool FindRequiredBlocksByName(params string[] blockNames)
        {
            return FindRequiredBlocksByKey(blockNames); 
        }
        public static bool FindRequiredGroupsByName(params string[] groupNamess)
        {
            return FindRequiredGroupsByKey(groupNamess);
      }
        public static bool FindRequiredBlocksByKey(params string[] blockKeys)
        {
            Dictionary<string,bool> blocksFound = new Dictionary<string,bool>();
            List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
            gridProgram.GridTerminalSystem.GetBlocks(blocks);
            try
            {
                foreach(var block in blocks)
                {
                    for(int i = 0;i < blockKeys.Length;i++)
                    {
                        if(!blocksFound.ContainsKey(blockKeys[i]))
                        {
                            blocksFound.Add(blockKeys[i],false);
                        }
                        if(block.DisplayNameText.Contains(blockKeys[i]))
                        {
                            blocksFound[blockKeys[i]] = true;
                        }
                    }
                }
                foreach(KeyValuePair<string,bool> blockFound in blocksFound)
                {
                    FoundBlock(blockFound.Value,blockFound.Key);
                }
            }
            catch(Exception e)
            {
                DebugLog($"An error has occured finding the required blocks\nError:{e.Message}");
            }
            int cnt = 0;
            foreach(var item in blocksFound)
            {
                if(!item.Value)
                {
                    DebugLog($"Block with key '{item.Key},was not found!");
                }
                cnt++;
            }
            return cnt == blocks.Count ? true : false;
        }
        public static bool FindRequiredGroupsByKey(params string[] groupKeys)
        {
            Dictionary<string,bool> groupsFound = new Dictionary<string,bool>();
            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            gridProgram.GridTerminalSystem.GetBlockGroups(groups);
            try
            {
                foreach(var group in groups)
                {
                    for(int i = 0;i < groupKeys.Length;i++)
                    {
                        if(!groupsFound.ContainsKey(groupKeys[i]))
                        {
                            groupsFound.Add(groupKeys[i],false);
                        }
                        if(group.Name.Contains(groupKeys[i]))
                        {
                            groupsFound[groupKeys[i]] = true;
                        }
                    }
                }
                foreach(KeyValuePair<string,bool> groupFound in groupsFound)
                {
                    FoundGroup(groupFound.Value,groupFound.Key);
                }
            }
            catch(Exception e)
            {
                DebugLog($"An error has occured finding the required blocks\nError:{e.Message}");
            }
            int cnt = 0;
            foreach(var item in groupsFound)
            {
                if(!item.Value)
                {
                    DebugLog($"Block with key '{item.Key},was not found!");
                }
                cnt++;
            }
            return cnt == groups.Count ? true : false;
        }
        public static T GetRequiredBlockByName<T>(string name)
        {
            return GetRequiredBlockByKey<T>(name);
        }
        public static IMyTerminalBlock GetRequiredBlockByName(string name)
        {
            return GetRequiredBlockByKey(name);
        }
        public static void GetRequiredGroupByName<T>(string name,List<T> container) where T : class
        {
            GetRequiredGroupByKey(name,container);       
        }
        public static void GetRequiredGroupByName(string name,List<IMyTerminalBlock> container) 
        {
            GetRequiredGroupByKey(name,container);          
        }
        public static IMyBlockGroup GetRequiredGroupByName(string name)
        {
            return GetRequiredGroupByKey(name);       
        }
        public static T GetRequiredBlockByKey<T>(string key)
        {
            try
            {
                List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                gridProgram.GridTerminalSystem.GetBlocks(blocks);
                foreach(IMyTerminalBlock block in blocks)
                {
                    if(block.DisplayNameText.Contains(key))
                    {
                        DebugLog($"Acquired block '{(block as IMyTerminalBlock).DisplayNameText}'");
                        return (T)block;
                    }
                }
            }
            catch(Exception e)
            {
                ShowDebug();
                ShowException(e);
            }
            return default(T);
        }
        public static IMyTerminalBlock GetRequiredBlockByKey(string key)
        {
            try
            {
                List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                gridProgram.GridTerminalSystem.GetBlocks(blocks);
                foreach(IMyTerminalBlock block in blocks)
                {
                    if(block.DisplayNameText.Contains(key))
                    {
                        DebugLog($"Acquired block '{(block as IMyTerminalBlock).DisplayNameText}'");
                        return block;
                    }
                }
            }
            catch(Exception e)
            {
                ShowDebug();
                ShowException(e);
            }
            return null;
        }
        public static void GetRequiredGroupByKey<T>(string key,List<T> container) where T : class
        {
            try
            {
                List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
                gridProgram.GridTerminalSystem.GetBlockGroups(blockGroups);
                foreach(IMyBlockGroup group in blockGroups)
                {
                    if(group.Name.Contains(key))
                    {
                        DebugLog($"Acquired block group '{group.Name}'");
                        group.GetBlocksOfType(container);
                        if(container == null)
                        {
                            ShowException(new Exception($"Null!Container of blocks for {group.Name} is NULL!"));
                        }
                        if(container.Count > 0)
                        {
                            DebugLog($"Acquired block group '{group.Name}'");
                        }
                        else
                        {
                            ShowException(new Exception($"Error!Container of blocks for {group.Name} is Empty!"));
                        }
                    }
                }

            }
            catch(Exception e)
            {
                ShowDebug();
                ShowException(e);
            }
        }
        public static void GetRequiredGroupByKey(string key,List<IMyTerminalBlock> container)
        {
            try
            {
                List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
                gridProgram.GridTerminalSystem.GetBlockGroups(blockGroups);
                foreach(IMyBlockGroup group in blockGroups)
                {
                    if(group.Name.Contains(key))
                    {
                        DebugLog($"Acquired block group '{group.Name}'");
                        group.GetBlocksOfType(container);
                        if(container == null)
                        {
                            ShowException(new Exception($"Null!Container of blocks for {group.Name} is NULL!"));
                        }
                        if(container.Count > 0)
                        {
                            DebugLog($"Acquired block group '{group.Name}'");
                        }
                        else
                        {
                            ShowException(new Exception($"Error!Container of blocks for {group.Name} is Empty!"));
                        }
                    }
                }

            }
            catch(Exception e)
            {
                ShowDebug();
                ShowException(e);
            }
        }
        public static IMyBlockGroup GetRequiredGroupByKey(string key)
        {
            try
            {
                List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
                gridProgram.GridTerminalSystem.GetBlockGroups(blockGroups);
                for(int i=0;i<blockGroups.Count;i++)
                {
                    if(blockGroups[i].Name.Contains(key))
                    {
                        DebugLog($"Acquired block group '{blockGroups[i].Name}'");
                        return blockGroups[i];
                    }
                }
            }
            catch(Exception e)
            {
                ShowDebug();
                ShowException(e);
            }
            return null;
        }
        public static void FoundBlock(bool found,string blockIdentifier,IMyTerminalBlock block=null)
        {
            if(!found)
            {
                DebugLog($"Block with identifier:'{blockIdentifier}' was not found");
            }
            else
            {
                DebugLog($"Block '{(block == null?blockIdentifier:block.DisplayNameText)}' was found");
            }
        }
        public static void FoundGroup(bool found,string groupIdentifier,List<IMyTerminalBlock> group)
        { 
            if(!found)
            {
                DebugLog($"Block with identifier:'{groupIdentifier}' was not found");
            }
            else
            {
                DebugLog($"Block '{ groupIdentifier}' was found");
            }
        }
        public static void FoundGroup(bool found,string groupIdentifier,IMyBlockGroup group)
        {
            if(!found)
            {
                DebugLog($"Block with identifier:'{groupIdentifier}' was not found");
            }
            else
            {
                DebugLog($"Block '{( group!=null?group.Name:groupIdentifier)}' was found");
            }
        }
        public static void FoundGroup(bool found,string groupIdentifier)
        {
            if(!found)
            {
                DebugLog($"Block with identifier:'{groupIdentifier}' was not found");
            }
            else
            {
                DebugLog($"Block '{ groupIdentifier}' was found");
            }
        }
            #endregion

            #region DebugStuff

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
        #endregion
    }
}
