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
    public enum ConditionPriority
    { 
        Max,
        High,
        Normal,
        Low
    }

    public class ConditionalAction<BlockType>: IEquatable<ConditionalAction<BlockType>> where BlockType : class,IMyTerminalBlock
    {
        public delegate bool ConditionMetHandler(BlockType block);

        public event ConditionMetHandler ConditionMet;
        private Func<BlockType,bool> condition;
        private Func<BlockType,bool> blockAction;
        private RequiredBlock<BlockType> block;
        private UtilityManager utilityManager;
        public ConditionalAction(UtilityManager _utilityManager,RequiredBlock<BlockType> _block,Func<BlockType,bool> _blockAction,Func<BlockType,bool> _condition)
        {
            if(_utilityManager == null)
            {
                throw new ArgumentNullException("Utility Manager");
            }
            if(_blockAction == null)
            {
                throw new ArgumentNullException("Block Action");
            }
            if(_condition == null)
            {
                throw new ArgumentNullException("Condition");
            }
            if(_block == null)
            {
                throw new ArgumentNullException("Block");
            }
            block = _block;
            utilityManager = _utilityManager;
            condition = _condition;
            blockAction = _blockAction;
        }
        public void Update()
        {
            try
            {
                if(CheckCondition())
                {
                    blockAction?.DynamicInvoke(block.GetBlock());
                    ConditionMet?.DynamicInvoke(block.GetBlock());
                }
            }
            catch (Exception ex)
            {
                utilityManager.logger.ShowException(ex,$"Error in update of ObservableBlock {block.DisplayName}!");
            }
        }

        public bool CheckCondition()
        {
            return condition.Invoke(block.GetBlock());
        }
        public override int GetHashCode()
        {
            return block.GetHashCode() + condition.GetHashCode() + blockAction.GetHashCode();
        }
        public bool Equals(ConditionalAction<BlockType> other)
        {
            return block == other.block && condition == other.condition && blockAction == other.blockAction;
        }
    }
}
