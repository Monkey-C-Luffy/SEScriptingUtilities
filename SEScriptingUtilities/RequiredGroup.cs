/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace SEScripting
{
    public class RequiredGroup<T> : IEquatable<RequiredGroup<T>> where T:class
    {
        private List<T> _groupBlocks = null;
        public List<T> GroupBlocks
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

        public bool Loaded { get; private set;}

        public RequiredGroup(string _groupIdentifier)
        {
            Identifier = _groupIdentifier;
            GroupType = null;
            Name = "";
            Exists = false;
        }

        public bool LoadGroup()
        {
            if(CheckGroupExists())
            {
               GroupBlocks = BlockFinding.GetRequiredGroupByKey<T>(Identifier);
                if(GroupBlocks != null)
                {
                    Name = Identifier;
                    GroupType = GroupBlocks.GetType();
                    Exists = true;
                    Loaded = true;
                }
            }
            DebugGroupFound();
            return Loaded;
        }

        public bool CheckGroupExists()
        {
            Exists = BlockFinding.FindRequiredGroupsByKey(Identifier);
            DebugGroupFound();
            return Exists;
        }
        private void DebugGroupFound()
        {
            BlockFinding.FoundGroup(Exists,Identifier,BlockUtilities.ConvertToTerminalBlockList(GroupBlocks));
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Identifier.GetHashCode() + GroupBlocks.GetHashCode() + GroupType.GetHashCode();
        }

        public bool Equals(RequiredGroup<T> other)
        {
            return GroupBlocks == other.GroupBlocks && Identifier == other.Identifier && GroupType == other.GroupType;
        }

        public static explicit operator List<T>(RequiredGroup<T> requiredGroup)
        {
            return requiredGroup.GroupBlocks;
        }
    }
}
