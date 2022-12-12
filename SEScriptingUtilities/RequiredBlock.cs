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
        public class RequiredBlock<T> : RequiredBase, IEquatable<RequiredBlock<T>> where T : class,IMyTerminalBlock
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

            private List<Func<T,bool>> conditions = new List<Func<T,bool>>();

            public delegate bool ConditionMetHandler();
            //private delegate bool ConditionMetHandler<In>(Func<In,bool> predicate);
            //private delegate bool ConditionMetHandler<In1,In2>(Func<In1,In2,bool> predicate);

            public event ConditionMetHandler ConditionMet;
            public RequiredBlock(UtilityManager utilityManager,string blockIdentifier,bool load = true)
            {
                _utilityManager = utilityManager;
                Identifier = blockIdentifier;
                Name = "";
                Exists = false;
                Loaded = false;
                if(load) Load();
            }

            public override bool Load()
            {
                if(CheckExists())
                {
                    Block = _utilityManager.blockFinder.GetBlockByName<T>(Identifier);
                    if(Block != default(T))
                    {
                        Loaded = true;
                        Name = Block.DisplayNameText;
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
            public T GetBlock(bool load=true)
            {
                if(!Loaded && load) Load();
                return Block;
            }

            public bool AddCondition(Func<T,bool> predicate)
            {
                try
                {
                    conditions.Add(predicate);
                }
                catch(Exception e)
                {
                    _utilityManager.logger.ShowException(e,$"Error trying to add predicate to conditions list in RequiredBlock:{Name}!");
                    return false;
                }
                return true;
            }
            public override int GetHashCode()
            {
                return Name.GetHashCode() + Identifier.GetHashCode() + Block.GetHashCode();
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
}
