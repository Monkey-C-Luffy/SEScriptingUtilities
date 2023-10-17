using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;

namespace SEScriptingUtilities
{
    public class ObservableBlock<T>: IEquatable<ObservableBlock<T>> where T : class, IMyTerminalBlock
    {
        UtilityManager _utilityManager;
        RequiredBlock<T> block;
        private List<ConditionalAction<T>> conditionalActions = new List<ConditionalAction<T>>();
        public ObservableBlock(UtilityManager utilityManager, RequiredBlock<T> requiredBlock)
        {
            if(utilityManager == null)
            {
                throw new ArgumentNullException("Utility Manager");
            }
            if(requiredBlock == null)
            {
                throw new ArgumentNullException("Required Block");
            }
            block = requiredBlock;
            _utilityManager = utilityManager;
        }
        public void Update()
        {
            foreach(var action in conditionalActions)
            { 
                action.Update();
            }
        }
        public bool AddConditionalActions(ConditionalAction<T> conditionalAction)
        {
            try
            {
                conditionalActions.Add(conditionalAction);
            }
            catch(Exception e)
            {
                _utilityManager.logger.ShowException(e,$"Error trying to add condtional action to conditional actions list in ObservableBlock:{block.DisplayName}!");
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return block.DisplayName.GetHashCode() + block.Identifier.GetHashCode() + block.GetHashCode();
        }

        public bool Equals(ObservableBlock<T> other)
        {
            return block == other.block;
        }
    }
}
