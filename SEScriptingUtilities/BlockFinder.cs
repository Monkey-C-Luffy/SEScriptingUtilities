/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program
    {
        public class BlockFinder
        {
            private readonly Logger _loggerInstance;
            private readonly MyGridProgram _programInstance;

            private int requiredBlocksCnt = 0;
            private int requiredGroupsCnt = 0;

            public Dictionary<string,IMyBlockGroup> groupsFound = new Dictionary<string,IMyBlockGroup>();
            public Dictionary<string,IMyTerminalBlock> blocksFound = new Dictionary<string,IMyTerminalBlock>();

            public bool allowCaching = true;
            public BlockFinder(MyGridProgram programInstance,Logger logger)
            {
                _programInstance = programInstance;
                _loggerInstance = logger;
            }
            public bool FindBlocksByName(params string[] blockNames)
            {
                List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                Dictionary<string,int> indexesOfFoundBlocks = new Dictionary<string,int>();
                bool foundABlock;
                _programInstance.GridTerminalSystem.GetBlocks(blocks);
                try
                {
                    for(int i = 0;i < blockNames.Length;i++)
                    {
                        foundABlock = false;
                        foreach(IMyTerminalBlock block in blocks)
                        {
                            if(blockNames[i] == "")
                            {
                                _loggerInstance.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                            }
                            if(block.DisplayNameText == blockNames[i])
                            {
                                if(!blocksFound.ContainsKey(blockNames[i]) && allowCaching) blocksFound.Add(blockNames[i],block);
                                if(!indexesOfFoundBlocks.ContainsKey(blockNames[i])) indexesOfFoundBlocks.Add(blockNames[i],i);
                                _loggerInstance.DebugLog($"Block '{block.DisplayNameText}' was found!");
                                requiredBlocksCnt++;
                                foundABlock = true;
                            }
                        }
                        if(!foundABlock) _loggerInstance.DebugLog($"Block '{blockNames[i]}' was NOT found!"); ;
                    }
                }
                catch(Exception e)
                {
                    _loggerInstance.DebugLog($"An error has occured finding the  blocks\nError:{e.Message}");
                }       
                return requiredBlocksCnt == blockNames.Length ? true : false;
            }
            public bool FindGroupsByName(params string[] groupNames)
            {
                List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
                Dictionary<string,int> indexesOfFoundGroups = new Dictionary<string, int>();
                bool foundAGroup;
                _programInstance.GridTerminalSystem.GetBlockGroups(groups);
                try
                {
                    for(int i = 0;i < groupNames.Length;i++)
                    {
                        foundAGroup = false;
                        foreach(IMyBlockGroup group in groups)
                        {
                            if(groupNames[i] == "")
                            {
                                _loggerInstance.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                            }
                            if(group.Name == groupNames[i])
                            {
                                if(!groupsFound.ContainsKey(groupNames[i]) && allowCaching) groupsFound.Add(groupNames[i],group);
                                if(!indexesOfFoundGroups.ContainsKey(groupNames[i])) indexesOfFoundGroups.Add(groupNames[i],i);
                                _loggerInstance.DebugLog($"Group '{group.Name}' was found!");
                                requiredGroupsCnt++;
                                foundAGroup = true;
                            }                                          
                        }
                        if(!foundAGroup) _loggerInstance.DebugLog($"Group '{groupNames[i]}' was NOT found!"); ;
                    }
                }
                catch(Exception e)
                {
                    _loggerInstance.DebugLog($"An error has occured finding the  blocks\nError:{e.Message}");
                }             
                return requiredGroupsCnt == groupNames.Length ? true : false;
            }
            public T GetBlockByName<T>(string name) where T : class, IMyTerminalBlock
            {
                try
                {
                    if(name == "")
                    {
                        _loggerInstance.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return default(T);
                    }
                    if(blocksFound.ContainsKey(name))
                    {
                        T block = blocksFound[name] as T;
                        _loggerInstance.DebugLog($"Block '{(block as IMyTerminalBlock).DisplayNameText}' was acquired!");
                        return block;
                    }
                    List<T> blocks = new List<T>();
                    _programInstance.GridTerminalSystem.GetBlocksOfType(blocks);
                    foreach(T block in blocks)
                    {
                        if((block as IMyTerminalBlock).DisplayNameText == name)
                        {
                            _loggerInstance.DebugLog($"Block '{(block as IMyTerminalBlock).DisplayNameText}' was acquired!");
                            return block;
                        }
                    }
                    _loggerInstance.DebugLog($"Block '{name}' could NOT be acquired!");
                }
                catch(Exception e)
                {
                    _loggerInstance.ShowDebug();
                    _loggerInstance.ShowException(e,$"Error trying to get block with name:{name}");
                }
                return default(T);
            }
            public void GetGroupByName<T>(string name,List<T> container) where T : class,IMyTerminalBlock
            {
                try
                {
                    if(name == "")
                    {
                        _loggerInstance.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return;
                    }
                    if(groupsFound.ContainsKey(name))
                    {
                        groupsFound[name].GetBlocksOfType(container);
                        _loggerInstance.DebugLog($"Group '{groupsFound[name]}' was acquired!");
                        return;
                    }
                    List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
                    _programInstance.GridTerminalSystem.GetBlockGroups(blockGroups);
                    foreach(IMyBlockGroup group in blockGroups)
                    {
                        if(group.Name == name)
                        {
                            if(container == null)
                            {
                                _loggerInstance.ShowException(new Exception($"Null!Container of blocks for {group.Name} is NULL!"));
                            }
                            if(container.Count > 0)
                            {
                                _loggerInstance.DebugLog($"Group '{group.Name}' was acquired!");
                                group.GetBlocksOfType(container);
                                return;
                            }
                            else
                            {
                                _loggerInstance.ShowException(new Exception($"Error!Container of blocks for {group.Name} is Empty!"));
                            }
                            break;
                        }
                    }
                    _loggerInstance.DebugLog($"Group '{name}' could NOT be acquired!");
                }
                catch(Exception e)
                {
                    _loggerInstance.ShowDebug();
                    _loggerInstance.ShowException(e);
                }
            }
            public List<T> GetGroupByName<T>(string name) where T : class,IMyTerminalBlock
            {
                try
                {
                    if(name == "")
                    {
                        _loggerInstance.ShowException(new Exception("EmptyStringKeyException"),"Cannot search by empty key,try another key!");
                        return null;
                    }
                    if(groupsFound.ContainsKey(name))
                    {
                        List<T> group = ConvertToTypedList<T>(groupsFound[name],_loggerInstance);
                        _loggerInstance.DebugLog($"Group '{groupsFound[name]}' was acquired!");
                        return group;
                    }
                    List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
                    _programInstance.GridTerminalSystem.GetBlockGroups(blockGroups);
                   foreach(IMyBlockGroup blockGroup in blockGroups)
                   {
                        if(blockGroup.Name == name)
                        {
                            List<T> retList = new List<T>();
                            blockGroup.GetBlocksOfType(retList);
                            if(retList == null)
                            {
                                _loggerInstance.ShowException(new Exception($"Null!Container of blocks for {blockGroup.Name} is NULL!"));
                            }
                            if(retList.Count > 0)
                            {
                                _loggerInstance.DebugLog($"Group '{blockGroup.Name}' was acquired!");
                                return retList;
                            }
                            else
                            {
                                _loggerInstance.ShowException(new Exception($"Error!Container of blocks for {blockGroup.Name} is Empty!"));
                            }
                            break;
                        }
                    }
                    _loggerInstance.DebugLog($"Group '{name}' could NOT be acquired!");
                }
                catch(Exception e)
                {
                    _loggerInstance.ShowDebug();
                    _loggerInstance.ShowException(e);
                }
                return null;
            }
            public static List<T> ConvertToTypedList<T>(IMyBlockGroup groupBlocks,Logger logger)
         where T : class, IMyTerminalBlock
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
            public bool CheckAllBlocksAndGroupsFound()
            {
                if(blocksFound.Count != requiredBlocksCnt) return false;
                if(groupsFound.Count != requiredGroupsCnt) return false;
                return true;
            }
        }
    }
}
