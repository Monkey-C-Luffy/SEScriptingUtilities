/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using Sandbox.ModAPI.Ingame;

namespace SEScripting
{
    public class RequiredBlock<T> : IEquatable<RequiredBlock<T>> where T:class
    {
        private T _block = default(T);
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
        public Type BlockType { get; private set; }
        public string Name { get; private set; }
        public string Identifier { get; private set; }

        public bool Exists { get; private set; }

        public bool Loaded { get; private set;}

        public RequiredBlock(string blockIdentifier)
        {
            Identifier = blockIdentifier;
            BlockType = null;
            Name = "";
            Exists = false;
            Loaded = false;
        }

        public bool LoadBlock()
        {
            if(CheckBlockExists())
            {
                Block = BlockFinding.GetRequiredBlockByKey<T>(Identifier);
                if(Block != null)
                {
                    Name = (Block as IMyTerminalBlock).DisplayNameText;
                    BlockType = Block.GetType();
                    Exists = true;
                    Loaded = true;
                }
            }
            DebugBlockFound();
            return Loaded;
        }

        public bool CheckBlockExists()
        {
            Exists = BlockFinding.FindRequiredBlocksByKey(Identifier);
            DebugBlockFound();
            return Exists;
        }
        public T GetBlock<T>() where T : class
        {
            return Block as T;
        }

        private void DebugBlockFound()
        {
            BlockFinding.FoundBlock(Exists,Identifier,Block as IMyTerminalBlock);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Identifier.GetHashCode() + Block.GetHashCode() + BlockType.GetHashCode();
        }

        public bool Equals(RequiredBlock<T> other)
        {
            return Block as IMyTerminalBlock == ( other.Block as IMyTerminalBlock) && Identifier == other.Identifier && BlockType == other.BlockType;
        }
    }
}
