/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace SEScripting
{
    public static class BlocksManager
    {
        private static int _requiredBlockCount = 0;
        private static int _requiredGroupCount = 0;

        private static bool _foundRequiredBlocks = false;
        private static bool _foundRequiredGroups = false;

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
                return _loadedRequiredBlocks;
            }
            private set
            {
                _loadedRequiredBlocks = value;
            }
        }
        public static bool LoadedRequiredGroups
        {
            get
            {
                return _loadedRequiredGroups;
            }
            private set
            {
                _loadedRequiredGroups = value;
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

        public static Dictionary<string,RequiredBlock> requiredBlocksByIdentifierDict = new Dictionary<string,RequiredBlock>();
        public static Dictionary<string,RequiredGroup> requiredGroupsByIdentifierDict = new Dictionary<string,RequiredGroup>();

        public static bool AddRequiredBlocks(bool load,params string[] requiredBlockIdentifiers)
        {
            int localRequiredBlockCount = requiredBlockIdentifiers.Length;
            for(int i = 0;i < requiredBlockIdentifiers.Length;i++)
            {
                try
                {
                    RequiredBlock tempBlock = new RequiredBlock(requiredBlockIdentifiers[i]);
                    if(requiredBlocksByIdentifierDict.ContainsKey(tempBlock.Identifier))
                    {
                        Logging.DebugLog($"Warning!Duplicate Block Identifier found:{tempBlock.Identifier}");
                    }
                    else
                    {
                        requiredBlocksByIdentifierDict.Add(tempBlock.Identifier,tempBlock);
                    }
                    _requiredBlockCount++;
                    localRequiredBlockCount++;
                    Logging.DebugLog($"Succesfully added block {tempBlock.Name} to required blocks queue.",true);
                }
                catch(Exception e)
                {
                    Logging.ShowException(e);
                }
                IndexedRequiredBlocks = true;
            }
            if(load)
            {
                LoadedRequiredBlocks =  LoadRequiredBlocks();
            }
            if(localRequiredBlockCount == requiredBlockIdentifiers.Length && FoundRequiredBlocks)
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
                    if(requiredGroupsByIdentifierDict.ContainsKey(tempGroup.Identifier))
                    {
                        Logging.DebugLog($"Warning!Duplicate Group Identifier found:{tempGroup.Identifier}");
                    }
                    else
                    {
                        requiredGroupsByIdentifierDict.Add(tempGroup.Identifier,tempGroup);
                    }
                    _requiredBlockCount++;
                    localRequiredGroupCount++;
                    Logging.DebugLog($"Succesfully added group {tempGroup.Identifier} to required blocks queue.",true);
                }
                catch(Exception e)
                {
                    Logging.ShowException(e);
                }
                IndexedRequiredGroups = true;
            }
            if(load)
            {
                LoadRequiredGroups();
            }
            if(localRequiredGroupCount == requiredGroupIdentifiers.Length && FoundRequiredGroups)
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
            foreach(RequiredBlock requiredBlock in requiredBlocksByIdentifierDict.Values)
            {
                try
                {
                    requiredBlock.LoadBlock();
                }
                catch(Exception e)
                {
                    Logging.ShowException(e,$"Failed to load required block:{requiredBlock.Identifier}!");
                    return false;
                }
            }
            return true;
        }
        public static bool FindRequiredBlocks()
        {
            if(!IndexedRequiredBlocks)
            {
                Logging.DebugLog($"Required Blocks haven't been added to dictionary!Cannot find them!",true);
                return false;
            }
            foreach(RequiredBlock requiredBlock in requiredBlocksByIdentifierDict.Values)
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
            if(!FoundRequiredGroups) return false;
            foreach(RequiredGroup requiredGroup in requiredGroupsByIdentifierDict.Values)
            {
                try
                {
                    requiredGroup.LoadGroup();
                }
                catch(Exception e)
                {
                    Logging.ShowException(e,$"Failed to load required group:{requiredGroup.Identifier}!");
                    return false;
                }
            }
            return true;
        }
        public static bool FindRequiredGroups()
        {
            if(!IndexedRequiredGroups)
            {
                Logging.DebugLog($"Required Groups haven't been added to dictionary!Cannot find them!",true);
                return false;
            }
            foreach(RequiredGroup requiredGroup in requiredGroupsByIdentifierDict.Values)
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
            if(!requiredBlocksByIdentifierDict.ContainsKey(key)) AddRequiredBlocks(true,new string[]{key });
            RequiredBlock temp = null;
            if(!requiredBlocksByIdentifierDict.TryGetValue(key,out temp))
            {
                Logging.DebugLog($"Couldn't get value from key:{key} in dictionary!",true);
            }
            return temp;
        }
        public static RequiredGroup GetRequiredGroupByName(string name)
        {
            return GetRequiredGroupByKey(name);
        }
        public static RequiredGroup GetRequiredGroupByKey(string key)
        {
            if(!requiredGroupsByIdentifierDict.ContainsKey(key)) AddRequiredBlocks(true,new string[] { key });
            RequiredGroup temp = null;
            if(!requiredGroupsByIdentifierDict.TryGetValue(key,out temp))
            {
                Logging.DebugLog($"Couldn't get value from key:{key} in dictionary!",true);
            }
            return temp;
        }
    }
}
