using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace SEScriptingUtilities
{
    public class ConditionalAction<BlockType> where BlockType : class,IMyTerminalBlock
    {
        public delegate bool ConditionMetHandler(BlockType block);
        public event ConditionMetHandler ConditionMet;
        Func<BlockType,bool> condition;
        Func<BlockType,bool> blockAction;
        RequiredBlock<BlockType> block;
        public ConditionalAction(Func<BlockType,bool> _blockAction,Func<BlockType,bool> _condition)
        {
            if(_blockAction == null)
            {
                throw new ArgumentNullException("Block Action");
            }
            if(_condition == null)
            {
                throw new ArgumentNullException("Condition");
            }
            condition = _condition;
            blockAction = _blockAction;
        }
        public void Update()
        {
            if(CheckCondition())
            {
                blockAction?.DynamicInvoke(block);
                ConditionMet?.DynamicInvoke(block);
            }
        }

        public bool CheckCondition()
        {
            return condition.Invoke((BlockType)block);
        }
    }
}
