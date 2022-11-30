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
        public static class BlockFinding
        {
            static Dictionary<string,bool> groupsFound = new Dictionary<string,bool>();
            static Dictionary<string,bool> blocksFound = new Dictionary<string,bool>();
            //TODO:Caching of found blocks?
            public static MyGridProgram gridProgram = Logging.gridProgram;
            public static bool FindRequiredBlocksByName(params string[] blockNames)
            {
                List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                int requiredBlocksCnt = 0;
                gridProgram.GridTerminalSystem.GetBlocks(blocks);
                try
                {
                    foreach(IMyTerminalBlock block in blocks)
                    {
                        for(int i = 0;i < blockNames.Length;i++)
                        {
                            if(blockNames[i] == "")
                            {
                                Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                            }
                            if(!blocksFound.ContainsKey(blockNames[i]))
                            {
                                blocksFound.Add(blockNames[i],false);
                            }
                            if(block.DisplayNameText.Contains(blockNames[i]))
                            {
                                blocksFound[blockNames[i]] = true;
                                FoundBlock(true,blockNames[i]);
                                requiredBlocksCnt++;
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Logging.DebugLog($"An error has occured finding the required blocks\nError:{e.Message}");
                }
                return requiredBlocksCnt == blocks.Count ? true : false;
            }
            public static bool FindRequiredGroupsByName(params string[] groupNames)
            {
                List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
                int requiredGroupsCnt = 0;
                gridProgram.GridTerminalSystem.GetBlockGroups(groups);
                try
                {
                    foreach(IMyBlockGroup group in groups)
                    {
                        for(int i = 0;i < groupNames.Length;i++)
                        {
                            if(groupNames[i] == "")
                            {
                                Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                            }
                            if(!groupsFound.ContainsKey(groupNames[i]))
                            {
                                groupsFound.Add(groupNames[i],false);
                            }
                            if(group.Name == groupNames[i])
                            {
                                groupsFound[groupNames[i]] = true;
                                FoundGroup(true,groupNames[i]);
                                requiredGroupsCnt++;
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Logging.DebugLog($"An error has occured finding the required blocks\nError:{e.Message}");
                }
                return requiredGroupsCnt == groups.Count ? true : false;
            }
            public static bool FindRequiredBlocksByKey(params string[] blockKeys)
            {
                List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                int requiredBlocksCnt = 0;
                gridProgram.GridTerminalSystem.GetBlocks(blocks);
                try
                {
                    foreach(IMyTerminalBlock block in blocks)
                    {
                        for(int i = 0;i < blockKeys.Length;i++)
                        {
                            if(blockKeys[i] == "")
                            {
                                Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                            }
                            if(!blocksFound.ContainsKey(blockKeys[i]))
                            {
                                blocksFound.Add(blockKeys[i],false);
                            }
                            if(block.DisplayNameText.Contains(blockKeys[i]))
                            {
                                blocksFound[blockKeys[i]] = true;
                                FoundBlock(true,blockKeys[i]);
                                requiredBlocksCnt++;
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Logging.DebugLog($"An error has occured finding the required blocks\nError:{e.Message}");
                }
                return requiredBlocksCnt == blocks.Count ? true : false;
            }
            public static bool FindRequiredGroupsByKey(params string[] groupKeys)
            {
                List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
                int requiredGroupsCnt = 0;
                gridProgram.GridTerminalSystem.GetBlockGroups(groups);
                try
                {
                    foreach(IMyBlockGroup group in groups)
                    {
                        for(int i = 0;i < groupKeys.Length;i++)
                        {
                            if(groupKeys[i] == "")
                            {
                                Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                            }
                            if(!groupsFound.ContainsKey(groupKeys[i]))
                            {
                                groupsFound.Add(groupKeys[i],false);
                            }
                            if(group.Name.Contains(groupKeys[i]))
                            {
                                groupsFound[groupKeys[i]] = true;
                                FoundGroup(true,groupKeys[i]);
                                requiredGroupsCnt++;
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Logging.DebugLog($"An error has occured finding the required blocks\nError:{e.Message}");
                }
                return requiredGroupsCnt == groups.Count ? true : false;
            }
            public static T GetRequiredBlockByName<T>(string name) where T:class
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
            public static List<T> GetRequiredGroupByName<T>(string name) where T : class
            {
                return GetRequiredGroupByKey<T>(name);
            }
            public static T GetRequiredBlockByKey<T>(string key) where T:class
            {
                try
                {
                    if(key == "")
                    {
                        Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return default(T);
                    }
                    List<T> blocks = new List<T>();
                    gridProgram.GridTerminalSystem.GetBlocksOfType(blocks);
                    foreach(T block in blocks)
                    {
                        if((block as IMyTerminalBlock).DisplayNameText.Contains(key))
                        {
                            Logging.DebugLog($"Acquired block '{(block as IMyTerminalBlock).DisplayNameText}'");
                            return block;
                        }
                    }
                }
                catch(Exception e)
                {
                    Logging.ShowDebug();
                    Logging.ShowException(e);
                }
                return default(T);
            }
            public static IMyTerminalBlock GetRequiredBlockByKey(string key)
            {
                try
                {
                    if(key == "")
                    {
                        Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return null;
                    }
                    List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                    gridProgram.GridTerminalSystem.GetBlocks(blocks);
                    foreach(IMyTerminalBlock block in blocks)
                    {
                        if(block.DisplayNameText.Contains(key))
                        {
                            Logging.DebugLog($"Acquired block '{(block as IMyTerminalBlock).DisplayNameText}'");
                            return block;
                        }
                    }
                }
                catch(Exception e)
                {
                    Logging.ShowDebug();
                    Logging.ShowException(e);
                }
                return null;
            }
            public static void GetRequiredGroupByKey<T>(string key,List<T> container) where T : class
            {
                try
                {
                    if(key == "")
                    {
                        Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return;
                    }
                    List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
                    gridProgram.GridTerminalSystem.GetBlockGroups(blockGroups);
                    foreach(IMyBlockGroup group in blockGroups)
                    {
                        if(group.Name.Contains(key))
                        {
                            Logging.DebugLog($"Acquired block group '{group.Name}'");
                            group.GetBlocksOfType(container);
                            if(container == null)
                            {
                                Logging.ShowException(new Exception($"Null!Container of blocks for {group.Name} is NULL!"));
                            }
                            if(container.Count > 0)
                            {
                                Logging.DebugLog($"Acquired block group '{group.Name}'");
                            }
                            else
                            {
                                Logging.ShowException(new Exception($"Error!Container of blocks for {group.Name} is Empty!"));
                            }
                        }
                    }

                }
                catch(Exception e)
                {
                    Logging.ShowDebug();
                    Logging.ShowException(e);
                }
            }
            public static void GetRequiredGroupByKey(string key,List<IMyTerminalBlock> container)
            {
                try
                {
                    if(key == "")
                    {
                        Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return;
                    }
                    List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
                    gridProgram.GridTerminalSystem.GetBlockGroups(blockGroups);
                    foreach(IMyBlockGroup group in blockGroups)
                    {
                        if(group.Name.Contains(key))
                        {
                            Logging.DebugLog($"Acquired block group '{group.Name}'");
                            group.GetBlocksOfType(container);
                            if(container == null)
                            {
                                Logging.ShowException(new Exception($"Null!Container of blocks for {group.Name} is NULL!"));
                            }
                            if(container.Count > 0)
                            {
                                Logging.DebugLog($"Acquired block group '{group.Name}'");
                            }
                            else
                            {
                                Logging.ShowException(new Exception($"Error!Container of blocks for {group.Name} is Empty!"));
                            }
                        }
                    }

                }
                catch(Exception e)
                {
                    Logging.ShowDebug();
                    Logging.ShowException(e);
                }
            }
            public static IMyBlockGroup GetRequiredGroupByKey(string key)
            {
                try
                {
                    if(key == "")
                    {
                        Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return null;
                    }
                    List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
                    gridProgram.GridTerminalSystem.GetBlockGroups(blockGroups);
                    for(int i = 0;i < blockGroups.Count;i++)
                    {
                        if(blockGroups[i].Name.Contains(key))
                        {
                            Logging.DebugLog($"Acquired block group '{blockGroups[i].Name}'");
                            return blockGroups[i];
                        }
                    }
                }
                catch(Exception e)
                {
                    Logging.ShowDebug();
                    Logging.ShowException(e);
                }
                return null;
            }
            public static List<T> GetRequiredGroupByKey<T>(string key) where T : class
            {
                try
                {
                    if(key == "")
                    {
                        Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return null;
                    }
                    List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
                    gridProgram.GridTerminalSystem.GetBlockGroups(blockGroups);
                    for(int i = 0;i < blockGroups.Count;i++)
                    {
                        if(blockGroups[i].Name.Contains(key))
                        {
                            Logging.DebugLog($"Acquired block group '{blockGroups[i].Name}'",true);
                            List<T> retList = new List<T>();
                            blockGroups[i].GetBlocksOfType(retList);
                            return retList;
                        }
                    }
                    Logging.DebugLog($"Couldn't acquire block group with identifier:{key}",true);
                }
                catch(Exception e)
                {
                    Logging.ShowDebug();
                    Logging.ShowException(e);
                }
                return null;
            }
            public static void FoundBlock(bool found,string blockIdentifier,IMyTerminalBlock block = null)
            {
                if(!found)
                {
                    Logging.DebugLog($"Block with identifier:'{blockIdentifier}' was not found");
                }
                else
                {
                    Logging.DebugLog($"Block '{(block == null ? blockIdentifier : block.DisplayNameText)}' was found");
                }
            }
            public static void FoundGroup(bool found,string groupIdentifier,List<IMyTerminalBlock> group=null)
            {
                if(!found)
                {
                    Logging.DebugLog($"Block with identifier:'{groupIdentifier}' was not found");
                }
                else
                {
                    Logging.DebugLog($"Block '{ groupIdentifier}' was found");
                }
            }
            public static void FoundGroup(bool found,string groupIdentifier,IMyBlockGroup group)
            {
                if(!found)
                {
                    Logging.DebugLog($"Block with identifier:'{groupIdentifier}' was not found");
                }
                else
                {
                    Logging.DebugLog($"Block '{(group != null ? group.Name : groupIdentifier)}' was found");
                }
            }
            public static void FoundGroup(bool found,string groupIdentifier)
            {
                if(!found)
                {
                    Logging.DebugLog($"Block with identifier:'{groupIdentifier}' was not found");
                }
                else
                {
                    Logging.DebugLog($"Block '{ groupIdentifier}' was found");
                }
            }
        }
    }
}
