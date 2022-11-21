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
            if(!FoundRequiredGroups) return false;
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
}
