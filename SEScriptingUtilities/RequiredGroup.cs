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

            public RequiredGroup(string _groupIdentifier,bool load = true)
            {
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
                    GroupBlocks = BlockFinding.GetRequiredGroupByKey<T>(Identifier);
                    Logging.ShowDebug();
                    if(GroupBlocks != null)
                    {
                        Name = Identifier;
                        Loaded = true;
                    }
                }
                return Loaded;
            }

            public bool CheckGroupExists()
            {
                Exists = BlockFinding.FindRequiredBlocksByName(Identifier);
                Logging.ShowDebug();
                return Exists;
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
                return requiredGroup.GroupBlocks;
            }
        }
    }
}
