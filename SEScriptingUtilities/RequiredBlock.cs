﻿/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program
    {
        public class RequiredBlock<T> : IEquatable<RequiredBlock<T>> where T : class
        {
            private T _block = default(T);
            private string _name;
            private string _identifier;
            private bool _exists;
            private bool _loaded;
            public T Block
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

            public RequiredBlock(string blockIdentifier,bool load = true)
            {
                Identifier = blockIdentifier;
                Name = "";
                Exists = false;
                Loaded = false;
                if(load) LoadBlock();
            }

            public bool LoadBlock()
            {
                if(CheckBlockExists())
                {
                    Block = BlockFinding.GetRequiredBlockByKey<T>(Identifier);
                    Logging.ShowDebug();
                    if(Block != default(T))
                    {
                        Name = (Block as IMyTerminalBlock).DisplayNameText;
                        Loaded = true;
                    }
                }
                return Loaded;
            }

            public bool CheckBlockExists()
            {
                Exists = BlockFinding.FindRequiredBlocksByName(Identifier);
                Logging.ShowDebug();
                return Exists;
            }
            public T GetBlock<T>() where T : class
            {
                return Block as T;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode() + Identifier.GetHashCode() + Block.GetHashCode();
            }

            public bool Equals(RequiredBlock<T> other)
            {
                return Block as IMyTerminalBlock == (other.Block as IMyTerminalBlock) && Identifier == other.Identifier;
            }

            public static explicit operator T(RequiredBlock<T> block)
            {
                return block.GetBlock<T>();
            }
        }
    }
}
