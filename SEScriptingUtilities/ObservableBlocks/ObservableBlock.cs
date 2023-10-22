using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;

namespace SEScriptingUtilities
{
    public class ObservableBlock<T>: RequiredBlock<T>,IEquatable<ObservableBlock<T>> where T : class, IMyTerminalBlock
    {
        private Dictionary<ConditionalAction<T>,ConditionPriority> conditionalActions = new Dictionary<ConditionalAction<T>, ConditionPriority>();
        public ObservableBlock(UtilityManager utilityManager,string blockIdentifier,bool autoLoad = true)
            :base(utilityManager,blockIdentifier,autoLoad){ }
        public void Update()
        {
            try
            {
                foreach(var action in conditionalActions.Keys)
                {
                    action.Update();
                }
            }
            catch(Exception ex)
            {
                _utilityManager.logger.ShowException(ex,$"Error in update of ObservableBlock {Block.DisplayName}!");
            }
        }
        public bool AddConditionalAction(ConditionalAction<T> conditionalAction,ConditionPriority conditionPriority = ConditionPriority.Normal)
        {
            try
            {
                if(!conditionalActions.ContainsKey(conditionalAction)) conditionalActions.Add(conditionalAction,conditionPriority);
            }
            catch(Exception e)
            {
                _utilityManager.logger.ShowException(e,$"Error trying to add condtional action to conditional actions list in ObservableBlock:{Block.DisplayName}!");
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return Block.GetHashCode();
        }

        public bool Equals(ObservableBlock<T> other)
        {
            return Block == other.Block;
        }
    }
}
