using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace SEScripting
{
    public static class BlocksManager
    {
        private static int _requiredBlockCount = 0;
        private static int _requiredGroupCount = 0;

        private static bool _foundRequiredBlocks = false;
        private static bool _foundRequiredGroups= false;

        private static bool _loadedRequiredBlocks = false;
        private static bool _loadedRequiredGroups = false;

        private static bool _indexedRequiredBlocks = false;
        private static bool _indexedRequiredGroups = false;

        public static bool IndexedRequiredBlocks
        {
            get
            {
                return _indexedRequiredBlocks;
            }
            private set
            {
                _indexedRequiredBlocks = value;
            }
        }
        public static bool IndexedRequiredGroups
        {
            get
            {
                return _indexedRequiredGroups;
            }
            private set
            {
                _indexedRequiredGroups = value;
            }
        }
        public static bool LoadedRequiredBlocks
        {
            get
            {
                if(!_loadedRequiredBlocks)
                {
                    _loadedRequiredBlocks = LoadRequiredBlocks();
                }
                return _loadedRequiredBlocks;
            }
        }
        public static bool LoadedRequiredGroups
        {
            get
            {
                if(!_loadedRequiredGroups)
                {
                    _loadedRequiredGroups = LoadRequiredGroups();
                }
                return _loadedRequiredGroups;
            }
        }
        public static bool FoundRequiredBlocks
        {
            get
            {
                if(!_foundRequiredBlocks)
                {
                    _foundRequiredBlocks = FindRequiredBlocks();
                }
                return _foundRequiredBlocks;
            }
        }
        public static bool FoundRequiredGroups
        {
            get
            {
                if(!_foundRequiredGroups)
                {
                    _foundRequiredGroups = FindRequiredGroups();
                }
                return _foundRequiredGroups;
            }
        }
        public static bool DebugEnable { get; set; }

        public static Dictionary<string,RequiredBlock> requiredBlocksByTypeDict = new Dictionary<string,RequiredBlock>();
        public static Dictionary<string,RequiredGroup> requiredGroupsByTypeDict = new Dictionary<string,RequiredGroup>();

        public static bool AddRequiredBlocks(bool load,params string[] requiredBlockIdentifiers)
        {
            int localRequiredBlockCount = requiredBlockIdentifiers.Length;
            for(int i = 0;i < requiredBlockIdentifiers.Length;i++)
            {
                try
                {
                    RequiredBlock tempBlock = new RequiredBlock(requiredBlockIdentifiers[i]);
                    if(load)
                    {
                        tempBlock.LoadBlock();
                    }
                    if(requiredBlocksByTypeDict.ContainsKey(tempBlock.Identifier))
                    {
                        Utils.DebugLog($"Warning!Duplicate Block Identifier found:{tempBlock.Identifier}");
                    }
                    else
                    {
                        requiredBlocksByTypeDict.Add(tempBlock.Identifier,tempBlock);
                    }
                    _requiredBlockCount++;
                    localRequiredBlockCount++;
                    Utils.DebugLog($"Succesfully added block {tempBlock.Name} to required blocks queue.",true);
                }
                catch(Exception e)
                {
                    Utils.ShowException(e);
                }
                IndexedRequiredBlocks = true;
            }
            if(localRequiredBlockCount == requiredBlockIdentifiers.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool AddRequiredGroups(bool load,params string[] requiredGroupIdentifiers)
        {
            int localRequiredGroupCount = requiredGroupIdentifiers.Length; 
            for(int i = 0;i < requiredGroupIdentifiers.Length;i++)
            {
                try
                {
                    RequiredGroup tempGroup = new RequiredGroup(requiredGroupIdentifiers[i]);
                    if(load)
                    {
                        tempGroup.LoadGroup();
                    }
                    if(requiredGroupsByTypeDict.ContainsKey(tempGroup.Identifier))
                    {
                        Utils.DebugLog($"Warning!Duplicate Group Identifier found:{tempGroup.Identifier}");
                    }
                    else
                    {
                        requiredGroupsByTypeDict.Add(tempGroup.Identifier,tempGroup);
                    }
                    _requiredBlockCount++;
                    localRequiredGroupCount++;
                    Utils.DebugLog($"Succesfully added group {tempGroup.Identifier} to required blocks queue.",true);
                }
                catch(Exception e)
                {
                    Utils.ShowException(e);
                }
                IndexedRequiredGroups = true;
            }
            if(localRequiredGroupCount == requiredGroupIdentifiers.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool LoadRequiredBlocks()
        {
            if(!FoundRequiredBlocks) return false;
            foreach(RequiredBlock requiredBlock in requiredBlocksByTypeDict.Values)
            {
                try
                {
                    requiredBlock.LoadBlock();
                }
                catch(Exception e)
                {
                    Utils.ShowException(e,$"Failed to load required block:{requiredBlock.Identifier}!");
                    return false;
                }
            }
            return true;
        }
        public static bool FindRequiredBlocks()
        {
            if(!IndexedRequiredBlocks)
            {
                Utils.DebugLog($"Required Blocks haven't been added to dictionary!Cannot find them!",true);
                return false;
            }
            foreach(RequiredBlock requiredBlock in requiredBlocksByTypeDict.Values)
            {
                if(requiredBlock.Exists)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool LoadRequiredGroups()
        { 
            if(!FoundRequiredGroups)  return false;
            foreach(RequiredGroup requiredGroup in requiredGroupsByTypeDict.Values)
            {
                try
                {
                    requiredGroup.LoadGroup();
                }
                catch(Exception e)
                {
                    Utils.ShowException(e,$"Failed to load required group:{requiredGroup.Identifier}!");
                    return false;
                }
            }
            return true;
        }
        public static bool FindRequiredGroups()
        {
            if(!IndexedRequiredGroups)
            {
                Utils.DebugLog($"Required Groups haven't been added to dictionary!Cannot find them!",true);
                return false;
            }
            foreach(RequiredGroup requiredGroup in requiredGroupsByTypeDict.Values)
            {
                if(requiredGroup.Exists)
                {
                    return false;
                }
            }
            return true;
        }
        public static RequiredBlock GetRequiredBlockByName(string name)
        {
            return GetRequiredBlockByKey(name);
        }
        public static RequiredBlock GetRequiredBlockByKey(string key)
        {
            if(!LoadedRequiredBlocks)
            {
                Utils.DebugLog($"Cannot get required group with identifier:{key}, required groups weren't loaded!",true);
                return null;
            }
            RequiredBlock temp = null;
            if(!requiredBlocksByTypeDict.TryGetValue(key,out temp))
            {
                Utils.DebugLog($"Couldn't get value from key:{key} in dictionary!",true);
            }
            return temp;
        }
        public static RequiredGroup GetRequiredGroupByName(string name) 
        {
            return GetRequiredGroupByKey(name);
        }
        public static RequiredGroup GetRequiredGroupByKey(string key)
        {
            if(!LoadedRequiredGroups)
            {
                Utils.DebugLog($"Cannot get required group with identifier:{key}, required groups weren't loaded!",true);
                return null;
            }
            RequiredGroup temp = null;
            if(!requiredGroupsByTypeDict.TryGetValue(key,out temp))
            {
                Utils.DebugLog($"Couldn't get value from key:{key} in dictionary!",true);
            }
            return temp;
        }
    }
        /// <summary>
        /// A Space Engineers Block that is required for your script to function.Provides book-keeping values and methods to Verify it exists and Get the block
        /// </summary>
        public class RequiredBlock : IEquatable<RequiredBlock>
    {
        private IMyTerminalBlock _block = null;
        public IMyTerminalBlock Block
        {
            get
            {
                if(_block == null)
                {
                    LoadBlock();
                }
                return _block;
            }
            private set
            {
                _block = value;
            }
        }
        public Type BlockType { get; private set; }
        public string Name { get; private set; }
        public string Identifier { get; private set; }

        public bool Exists { get; private set;}

        public RequiredBlock(string blockIdentifier)
        {
            Identifier = blockIdentifier;
            BlockType = null;
            Name = "";
            Exists = false;
        }

        public IMyTerminalBlock LoadBlock()
        {
            if(CheckBlockExists())
            {
                Block = Utils.GetRequiredBlockByKey(Identifier);
                if(Block != null)
                {
                    Name = Block.DisplayNameText;
                    BlockType = Block.GetType();
                    Exists = true;
                    return Block;
                }
            }
            DebugBlockFound();
            return null;
        }

        public bool CheckBlockExists()
        {
            Exists = Utils.FindRequiredBlocksByKey(Identifier);
            DebugBlockFound();
            return Exists;
        }
        public T GetBlock<T>() where T:class
        {
            return Block as T;
        }

        private void DebugBlockFound()
        {
            if(!BlocksManager.DebugEnable) return;
            Utils.FoundBlock(Exists,Identifier,Block);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode()+Identifier.GetHashCode()+Block.GetHashCode() +BlockType.GetHashCode();
        }

        public bool Equals(RequiredBlock other)
        {
            return Block == other.Block && Identifier == other.Identifier && BlockType == other.BlockType;
        }
    }
    public class RequiredGroup : IEquatable<RequiredGroup>
    {
        private IMyBlockGroup _groupBlocks = null;
        public IMyBlockGroup GroupBlocks
        {
            get
            {
                if(_groupBlocks == null)
                {
                    LoadGroup();
                }
                return _groupBlocks;
            }
            private set
            {
                _groupBlocks = value;
            }
        }
        public Type GroupType { get; private set; }
        public string Name { get; private set; }
        public string Identifier { get; private set; }

        public bool Exists { get; private set; }

        public RequiredGroup(string _groupIdentifier)
        {
            Identifier = _groupIdentifier;
            GroupType = null;
            Name = "";
            Exists = false;
        }

        public IMyBlockGroup LoadGroup()
        {
            if(CheckGroupExists())
            {
               GroupBlocks =  Utils.GetRequiredGroupByKey(Identifier);
                if(GroupBlocks != null)
                {
                    Name = GroupBlocks.Name;
                    GroupType = GroupBlocks.GetType();
                    Exists = true;
                    return GroupBlocks;
                }
            }
            DebugGroupFound();
            return null;
        }

        public bool CheckGroupExists()
        {
            Exists = Utils.FindRequiredGroupsByKey(Identifier);
            DebugGroupFound();
            return Exists;
        }

        public List<T> GetBlocks<T>() where T : class
        {
            List<T> temp = null;
            GroupBlocks.GetBlocksOfType(temp);
            if(temp == null)
            {
                Utils.DebugLog($"Couldn't get group blocks from RequiredGroup:{Identifier}!");
            }
            return temp;
        }
        public void GetBlocks<T>(List<T> blocksContainer) where T : class
        {
            List<T> blocks = null;
            GroupBlocks.GetBlocksOfType(blocks);
            if(blocks == null)
            {
                Utils.DebugLog($"Couldn't get group blocks from RequiredGroup:{Identifier}!");
            }
            blocksContainer = blocks;
        }
        private void DebugGroupFound()
        {
            if(!BlocksManager.DebugEnable) return;
            Utils.FoundGroup(Exists,Identifier,GroupBlocks);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Identifier.GetHashCode() + GroupBlocks.GetHashCode() + GroupType.GetHashCode();
        }

        public bool Equals(RequiredGroup other)
        {
            return GroupBlocks == other.GroupBlocks && Identifier == other.Identifier && GroupType == other.GroupType;
        }

        public static implicit operator List<IMyTerminalBlock>(RequiredGroup requiredGroup)
        {
            return requiredGroup.GetBlocks<IMyTerminalBlock>();
        }
    }
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
