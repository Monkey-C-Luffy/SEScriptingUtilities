using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;

namespace SEScriptingUtilities
{
    public class ObservableBlock<T>: IEquatable<ObservableBlock<T>> where T : class, IMyTerminalBlock
    {
        private UtilityManager _utilityManager;
        private RequiredBlock<T> _block;
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
            _block = requiredBlock;
            _utilityManager = utilityManager;
        }
        public void Update()
        {
            try
            {
                foreach(var action in conditionalActions)
                {
                    action.Update();
                }
            }
            catch(Exception ex)
            {
                _utilityManager.logger.ShowException(ex,$"Error in update of ObservableBlock {_block.DisplayName}!");
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
                _utilityManager.logger.ShowException(e,$"Error trying to add condtional action to conditional actions list in ObservableBlock:{_block.DisplayName}!");
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return _block.GetHashCode();
        }

        public bool Equals(ObservableBlock<T> other)
        {
            return _block == other._block;
        }
    }
}
