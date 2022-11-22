/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using Sandbox.ModAPI.Ingame;

namespace SEScripting
{
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

        public bool Exists { get; private set; }

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
        public T GetBlock<T>() where T : class
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
            return Name.GetHashCode() + Identifier.GetHashCode() + Block.GetHashCode() + BlockType.GetHashCode();
        }

        public bool Equals(RequiredBlock other)
        {
            return Block == other.Block && Identifier == other.Identifier && BlockType == other.BlockType;
        }
    }
}
