using System;
using Sandbox.ModAPI.Ingame;
using SEScriptingUtilities;

namespace Testing
{
    partial class Program : MyGridProgram
    {
        UtilityManager utilManager;
        RequiredBlock<IMyMotorStator> requiredBlock;
        ObservableBlock<IMyMotorStator> observableBlock;
        ConditionalAction<IMyMotorStator> conditionalAction;
        public Program()
        {   
            Runtime.UpdateFrequency=  UpdateFrequency.Update1;
            utilManager = new UtilityManager(this);
            requiredBlock = new RequiredBlock<IMyMotorStator>(utilManager,"Test Rotor");
            observableBlock = new ObservableBlock<IMyMotorStator>(utilManager,requiredBlock);
            conditionalAction = new ConditionalAction<IMyMotorStator>((r) => r.RotorLock,(r) => r.Angle > 50 && r.Angle < 100);
            observableBlock.AddConditionalActions(conditionalAction);
        }

        public void Save()
        { 
        }

        public void Main(string argument,UpdateType updateSource)
        {   
            observableBlock.Update();
        }
    }
}
