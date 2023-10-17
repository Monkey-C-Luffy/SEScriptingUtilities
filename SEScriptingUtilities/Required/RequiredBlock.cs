/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace SEScriptingUtilities
{
    public class RequiredBlock<T> : RequiredBase, IEquatable<RequiredBlock<T>> where T : class, IMyTerminalBlock
    {
        private T _block = default(T);
        public T Block
        {
            get
            {
                return _block;
            }
            private set
            {
                _block = value;
            }
        }
        public RequiredBlock(UtilityManager utilityManager,string blockIdentifier,bool autoLoad = true)
        {
            if(utilityManager == null)
            {
                throw new ArgumentNullException("Utility Manager");
            }
            _utilityManager = utilityManager;
            Identifier = blockIdentifier;
            DisplayName = "";
            Exists = false;
            Loaded = false;
            if(autoLoad) Load();
        }
        public override bool Load()
        {
            if(CheckExists())
            {
                Block = _utilityManager.blockFinder.GetBlockByName<T>(Identifier);
                if(Block != default(T))
                {
                    Loaded = true;
                    DisplayName = Block.DisplayNameText;
                }
            }
            _utilityManager.logger.ShowDebug();
            return Loaded;
        }
        public override bool CheckExists()
        {
            Exists = _utilityManager.blockFinder.FindBlocksByName(Identifier);
            return Exists;
        }
        public T GetBlock(bool load = true)
        {
            if(!Loaded && load) Load();
            return Block;
        }
        public override int GetHashCode()
        {
            return DisplayName.GetHashCode() + Identifier.GetHashCode() + Block.GetHashCode();
        }
        public bool Equals(RequiredBlock<T> other)
        {
            return Block == other.Block && Identifier == other.Identifier;
        }
        public static explicit operator T(RequiredBlock<T> block)
        {
            return block.GetBlock();
        }
    }
}
