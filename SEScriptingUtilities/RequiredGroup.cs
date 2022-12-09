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
        public class RequiredGroup<T> : IEquatable<RequiredGroup<T>> where T : class
        {
            private UtilityManager _utilityManager;
            private List<T> _groupBlocks = null;
            public List<T> GroupBlocks
            {
                get
                {
                    return _groupBlocks;
                }
                private set
                {
                    _groupBlocks = value;
                }
            }
            private string _name;
            private string _identifier;
            private bool _exists;
            private bool _loaded;
            public string Name
            {
                get
                {
                    return _name;
                }
                private set
                {
                    _name = value;
                }
            }
            public string Identifier
            {
                get
                {
                    return _identifier;
                }
                private set
                {
                    _identifier = value;
                }
            }
            public bool Exists
            {
                get
                {
                    return _exists;
                }
                private set
                {
                    _exists = value;
                }
            }
            public bool Loaded
            {
                get
                {
                    return _loaded;
                }
                private set
                {
                    _loaded = value;
                }
            }
            public RequiredGroup(UtilityManager utilityManager,string _groupIdentifier,bool load = true)
            {
                _utilityManager = utilityManager;
                Identifier = _groupIdentifier;
                Name = Identifier;
                Exists = false;
                Loaded = false;
                if(load) LoadGroup();
            }
            public bool LoadGroup()
            {
                if(CheckGroupExists())
                {
                    GroupBlocks = _utilityManager.blockFinder.GetGroupByName<T>(Identifier);
                    if(GroupBlocks != null)
                    { 
                        Loaded = true;
                        Name = Identifier;           
                    }
                }
                _utilityManager.logger.ShowDebug();
                return Loaded;
            }
            public bool CheckGroupExists()
            {
                Exists = _utilityManager.blockFinder.FindBlocksByName(Identifier);
                return Exists;
            }
            public List<IMyTerminalBlock> ConvertToTerminalBlockList() 
            {
                return BlockUtilities.ConvertToTerminalBlockList(GroupBlocks,_utilityManager.logger);
            }

            public List<T> GetGroup(bool load=true)
            {
                if(!Loaded && load) LoadGroup();
                return GroupBlocks;
            }
            public override int GetHashCode()
            {
                return Name.GetHashCode() + Identifier.GetHashCode() + GroupBlocks.GetHashCode();
            }
            public bool Equals(RequiredGroup<T> other)
            {
                return GroupBlocks == other.GroupBlocks && Identifier == other.Identifier;
            }

            public static implicit operator List<T>(RequiredGroup<T> requiredGroup)
            {
                return requiredGroup.GetGroup();
            }
        }
    }
}
