/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program
    {
        public static class BlockFinding
        {
            static Dictionary<string,IMyBlockGroup> groupsFound = new Dictionary<string,IMyBlockGroup>();
            static Dictionary<string,IMyTerminalBlock> blocksFound = new Dictionary<string,IMyTerminalBlock>();
            private static BlockManager _blockManagerInstance;
            public static BlockManager BlockManagerInstance
            {
                get
                {
                    return _blockManagerInstance;
                }
                set
                {
                    if(_blockManagerInstance == null)
                    {
                        _blockManagerInstance = value;
                    }
                }
            }
            //TODO:Try reimplement the Find and Get by key
            public static MyGridProgram gridProgram = Logging.gridProgram;
            public static bool FindBlocksByName(params string[] blockNames)
            {
                List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                Dictionary<string,int> indexesOfFoundBlocks = new Dictionary<string,int>();
                int BlocksCnt = 0;
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
                                blocksFound.Add(blockNames[i],block);
                            }
                            if(block.DisplayNameText == blockNames[i])
                            {
                                if(!indexesOfFoundBlocks.ContainsKey(blockNames[i])) indexesOfFoundBlocks.Add(blockNames[i],i);
                                FoundBlock(true,blockNames[i]);
                                BlocksCnt++;
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Logging.DebugLog($"An error has occured finding the  blocks\nError:{e.Message}");
                }
                if(BlocksCnt < blockNames.Length)
                {
                    for(int i = 0;i < blockNames.Length;i++)
                    {
                        if(!indexesOfFoundBlocks.ContainsKey(blockNames[i]))
                        {
                            AcquiredBlock(false,blockNames[i]);
                        }
                    }
                }          
                return BlocksCnt == blockNames.Length ? true : false;
            }
            public static bool FindGroupsByName(params string[] groupNames)
            {
                List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
                Dictionary<string,int> indexesOfFoundGroups = new Dictionary<string, int>();
                gridProgram.GridTerminalSystem.GetBlockGroups(groups);
                int GroupsCnt = 0;
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
                                groupsFound.Add(groupNames[i],group);
                            }
                            if(group.Name == groupNames[i])
                            {
                                if(!indexesOfFoundGroups.ContainsKey(groupNames[i])) indexesOfFoundGroups.Add(groupNames[i],i);
                                AcquiredGroup(true,groupNames[i]);
                                GroupsCnt++;
                            }                                          
                        }
                    }
                }
                catch(Exception e)
                {
                    Logging.DebugLog($"An error has occured finding the  blocks\nError:{e.Message}");
                }
                if(GroupsCnt < groupNames.Length)
                {
                    for(int i = 0;i < groupNames.Length;i++)
                    {
                        if(!indexesOfFoundGroups.ContainsKey(groupNames[i]))
                        {
                            AcquiredGroup(false,groupNames[i]);
                        }
                    }
                }             
                return GroupsCnt == groupNames.Length ? true : false;
            }
            public static T GetBlockByName<T>(string name) where T : class
            {
                try
                {
                    if(name == "")
                    {
                        Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return default(T);
                    }
                    if(blocksFound.ContainsKey(name)) return blocksFound[name] as T;
                    List<T> blocks = new List<T>();
                    gridProgram.GridTerminalSystem.GetBlocksOfType(blocks);
                    foreach(T block in blocks)
                    {
                        if((block as IMyTerminalBlock).DisplayNameText == name)
                        {
                            AcquiredBlock(true,name,block);
                            return block;
                        }
                    }
                    AcquiredBlock(false,name);
                }
                catch(Exception e)
                {
                    Logging.ShowDebug();
                    Logging.ShowException(e);
                }
                return default(T);
            }
            public static IMyTerminalBlock GetBlockByName(string name)
            {
                try
                {
                    if(name == "")
                    {
                        Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return null;
                    }
                    if(blocksFound.ContainsKey(name)) return blocksFound[name];
                    List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                    gridProgram.GridTerminalSystem.GetBlocks(blocks);
                    foreach(IMyTerminalBlock block in blocks)
                    {
                        if(block.DisplayNameText == name)
                        {
                            AcquiredBlock(true,name,block);
                            return block;
                        }
                    }
                    AcquiredBlock(false,name);
                }
                catch(Exception e)
                {
                    Logging.ShowDebug();
                    Logging.ShowException(e);
                }
                return null;
            }
            public static void GetGroupByName<T>(string name,List<T> container) where T : class
            {
                try
                {
                    if(name == "")
                    {
                        Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return;
                    }
                    if(groupsFound.ContainsKey(name))
                    {
                        container = BlockUtilities.ConvertToTypedList<T>(groupsFound[name]);
                        return;
                    }
                    List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
                    gridProgram.GridTerminalSystem.GetBlockGroups(blockGroups);
                    foreach(IMyBlockGroup group in blockGroups)
                    {
                        if(group.Name == name)
                        {
                            group.GetBlocksOfType(container);
                            if(container == null)
                            {
                                Logging.ShowException(new Exception($"Null!Container of blocks for {group.Name} is NULL!"));
                            }
                            if(container.Count > 0)
                            {
                                AcquiredGroup(true,group.Name,container);
                                Logging.DebugLog($"Acquired block group '{group.Name}'");
                            }
                            else
                            {
                                Logging.ShowException(new Exception($"Error!Container of blocks for {group.Name} is Empty!"));
                            }
                        }
                    }
                    AcquiredGroup(false,name);
                }
                catch(Exception e)
                {
                    Logging.ShowDebug();
                    Logging.ShowException(e);
                }
            }
            public static void GetGroupByName(string name,List<IMyTerminalBlock> container)
            {
                try
                {
                    if(name == "")
                    {
                        Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return;
                    }
                    if(groupsFound.ContainsKey(name))
                    {
                        container = BlockUtilities.ConvertToTerminalBlockList(groupsFound[name]);
                        return;
                    }
                    List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
                    gridProgram.GridTerminalSystem.GetBlockGroups(blockGroups);
                    foreach(IMyBlockGroup group in blockGroups)
                    {
                        if(group.Name == name)
                        {
                            group.GetBlocksOfType(container);
                            if(container == null)
                            {
                                Logging.ShowException(new Exception($"Null!Container of blocks for {group.Name} is NULL!"));
                            }
                            if(container.Count > 0)
                            {
                                AcquiredGroup(true,group.Name,container);
                                Logging.DebugLog($"Acquired block group '{group.Name}'");
                            }
                            else
                            {
                                Logging.ShowException(new Exception($"Error!Container of blocks for {group.Name} is Empty!"));
                            }
                        }
                    }
                    AcquiredGroup(false,name);
                }
                catch(Exception e)
                {
                    Logging.ShowDebug();
                    Logging.ShowException(e);
                }
            }
            public static IMyBlockGroup GetGroupByName(string name)
            {
                try
                {
                    if(name == "")
                    {
                        Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return null;
                    }
                    if(groupsFound.ContainsKey(name))
                    {
                        return groupsFound[name];
                    }
                    List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
                    gridProgram.GridTerminalSystem.GetBlockGroups(blockGroups);
                    for(int i = 0;i < blockGroups.Count;i++)
                    {
                        if(blockGroups[i].Name == name)
                        {
                            AcquiredGroup(true,blockGroups[i].Name,blockGroups[i]);
                            Logging.DebugLog($"Acquired block group '{blockGroups[i].Name}'");
                            return blockGroups[i];
                        }
                    }
                    AcquiredGroup(false,name);
                }
                catch(Exception e)
                {
                    Logging.ShowDebug();
                    Logging.ShowException(e);
                }
                return null;
            }
            public static List<T> GetGroupByName<T>(string name) where T : class
            {
                try
                {
                    if(name == "")
                    {
                        Logging.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return null;
                    }
                    if(groupsFound.ContainsKey(name))
                    {
                        return BlockUtilities.ConvertToTypedList<T>(groupsFound[name]);
                    }
                    List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
                    gridProgram.GridTerminalSystem.GetBlockGroups(blockGroups);
                    for(int i = 0;i < blockGroups.Count;i++)
                    {
                        if(blockGroups[i].Name == name)
                        {
                            AcquiredGroup(true,blockGroups[i].Name,blockGroups[i]);
                            List<T> retList = new List<T>();
                            blockGroups[i].GetBlocksOfType(retList);
                            return retList;
                        }
                    }
                    AcquiredGroup(false,name);
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
            public static void FoundBlock<T>(bool found,string blockIdentifier,T block = default(T)) where T : class
            {
                if(!found)
                {
                    Logging.DebugLog($"Block with identifier:'{blockIdentifier}' was not found!");
                }
                else
                {
                    Logging.DebugLog($"Block '{(block == null ? blockIdentifier : (block as IMyTerminalBlock).DisplayNameText)}' was found!");
                }
            }
            //TODO:Make a RequiredBlock,RequiredGroup FoundBlock so that the identifier required is given by user to RequiredBlock,RequiredGroup
            public static void FoundGroup(bool found,string groupIdentifier,List<IMyTerminalBlock> group = null)
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
            public static void FoundGroup<T>(bool found,string groupIdentifier,List<T> group = null)
            {
                if(!found)
                {
                    Logging.DebugLog($"Block with identifier:'{groupIdentifier}' was not found!");
                }
                else
                {
                    Logging.DebugLog($"Block '{ groupIdentifier}' was found!");
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
            public static void AcquiredBlock(bool found,string blockIdentifier,IMyTerminalBlock block = null)
            {
                if(!found)
                {
                    Logging.DebugLog($"Block with identifier:'{blockIdentifier}' couldn't be acquired!");
                }
                else
                {
                    Logging.DebugLog($"Block '{(block == null ? blockIdentifier : block.DisplayNameText)}' was acquired!");
                }
            }
            public static void AcquiredBlock<T>(bool found,string blockIdentifier,T block = default(T)) where T : class
            {
                if(!found)
                {
                    Logging.DebugLog($"Block with identifier:'{blockIdentifier}' couldn't be acquired!");
                }
                else
                {
                    Logging.DebugLog($"Block '{(block == null ? blockIdentifier : (block as IMyTerminalBlock).DisplayNameText)}' was acquired!");
                }
            }
            public static void AcquiredGroup(bool found,string groupIdentifier,List<IMyTerminalBlock> group = null)
            {
                if(!found)
                {
                    Logging.DebugLog($"Block with identifier:'{groupIdentifier}' couldn't be acquired!");
                }
                else
                {
                    Logging.DebugLog($"Block '{ groupIdentifier}' was acquired!");
                }
            }
            public static void AcquiredGroup<T>(bool found,string groupIdentifier,List<T> group = null)
            {
                if(!found)
                {
                    Logging.DebugLog($"Block with identifier:'{groupIdentifier}' couldn't be acquired!");
                }
                else
                {
                    Logging.DebugLog($"Block '{ groupIdentifier}' was acquired!");
                }
            }
            public static void AcquiredGroup(bool found,string groupIdentifier,IMyBlockGroup group)
            {
                if(!found)
                {
                    Logging.DebugLog($"Block with identifier:'{groupIdentifier}' couldn't be acquired!");
                }
                else
                {
                    Logging.DebugLog($"Block '{(group != null ? group.Name : groupIdentifier)}' was acquired!");
                }
            }
            public static void AcquiredGroup(bool found,string groupIdentifier)
            {
                if(!found)
                {
                    Logging.DebugLog($"Block with identifier:'{groupIdentifier}' couldn't be acquired!");
                }
                else
                {
                    Logging.DebugLog($"Block '{ groupIdentifier}' was acquired!");
                }
            }
        }
    }
}
