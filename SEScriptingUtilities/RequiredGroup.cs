/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace SEScripting
{
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
                GroupBlocks = BlockFinding.GetRequiredGroupByKey(Identifier);
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
            Exists = BlockFinding.FindRequiredGroupsByKey(Identifier);
            DebugGroupFound();
            return Exists;
        }

        public List<T> GetBlocks<T>() where T : class
        {
            List<T> temp = null;
            GroupBlocks.GetBlocksOfType(temp);
            if(temp == null)
            {
                Logging.DebugLog($"Couldn't get group blocks from RequiredGroup:{Identifier}!");
            }
            return temp;
        }
        public void GetBlocks<T>(List<T> blocksContainer) where T : class
        {
            List<T> blocks = null;
            GroupBlocks.GetBlocksOfType(blocks);
            if(blocks == null)
            {
                Logging.DebugLog($"Couldn't get group blocks from RequiredGroup:{Identifier}!");
            }
            blocksContainer = blocks;
        }
        public List<T> ConvertToList<T>() where T:class
        {
            List<T> blocks = new List<T>();
            GroupBlocks.GetBlocksOfType(blocks);
            return blocks;
        }
        private void DebugGroupFound()
        {
            BlockFinding.FoundGroup(Exists,Identifier,GroupBlocks);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Identifier.GetHashCode() + GroupBlocks.GetHashCode() + GroupType.GetHashCode();
        }

        public bool Equals(RequiredGroup other)
        {
            return GroupBlocks == other.GroupBlocks && Identifier == other.Identifier && GroupType == other.GroupType;
        }

        public static explicit operator List<IMyTerminalBlock>(RequiredGroup requiredGroup)
        {
            return requiredGroup.GetBlocks<IMyTerminalBlock>();
        }
    }
}
