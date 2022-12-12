/*
  MIT License

Copyright (c) 2022 Monkey C Luffy
 */
using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using System.Linq;
using Sandbox.ModAPI.Interfaces;
using static IngameScript.Program;
using System.Collections;

namespace IngameScript
{
    partial class Program
    {
        public class RequiredGroup<T> : RequiredBase,IEquatable<RequiredGroup<T>>,IEnumerable<T> where T : class,IMyTerminalBlock
        {
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

            public RequiredGroup(UtilityManager utilityManager,string _groupIdentifier,bool load = true)
            {
                _utilityManager = utilityManager;
                Identifier = _groupIdentifier;
                Name = Identifier;
                Exists = false;
                Loaded = false;
                if(load) Load();
            }

            public T this[int i]
            {
                get { return GroupBlocks[i]; }
                set { GroupBlocks[i] = value; }
            }
            public bool ApplyActionToBlocks(Action<T> action)
            {
                try
                {
                    foreach(T block in GroupBlocks)
                    {
                        action.Invoke(block);
                    }
                    _utilityManager.logger.DebugLine($"Succesfully applied action to block group of RequiredGroup '{Name}'!");
                }
                catch(Exception e)
                {
                    _utilityManager.logger.ShowException(e,$"Tried applying action:{action.Method} to blocks of type:{typeof(T)}\nin RequiredBlock with name {_name}");
                    return false;
                }
                return true;
            }
            public override bool Load()
            {
                if(CheckExists())
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
            public override bool CheckExists()
            {
                Exists = _utilityManager.blockFinder.FindGroupsByName(Identifier);
                return Exists;
            }
            public List<IMyTerminalBlock> ConvertToTerminalBlockList() 
            {
                List<IMyTerminalBlock> terminalBlocksList = new List<IMyTerminalBlock>();
                try
                {
                    for(int i = 0;i < _groupBlocks.Count;i++)
                    {
                        terminalBlocksList.Add(_groupBlocks[i]);
                    }
                }
                catch(Exception e)
{
                    _utilityManager.logger.ShowException(e,$"Something went wrong trying to convert to {typeof(T)}!");
                }
                return terminalBlocksList;
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

            public IEnumerator<T> GetEnumerator()
            {
                foreach(T block in GroupBlocks)
                {
                    if(block == default(T)) break;
                    yield return block;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                foreach(T block in GroupBlocks)
                {
                    if(block == default(T)) break;
                    yield return block;
                }
            }

            public static implicit operator List<T>(RequiredGroup<T> requiredGroup)
            {
                return requiredGroup.GetGroup();
            }
        }
    }
}
