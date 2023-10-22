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
        }

        public void Main(string argument,UpdateType updateSource)
        {   
            observableBlock = observableBlock ?? new ObservableBlock<IMyMotorStator>(utilManager,"Test Rotor");
            conditionalAction = conditionalAction ?? new ConditionalAction<IMyMotorStator>(utilManager,observableBlock
                ,(r) => StopRotor(r),(r) => Conversions.RadToDeg(r.Angle) > 50 && Conversions.RadToDeg(r.Angle) < 100);
            observableBlock.AddConditionalAction(conditionalAction);
            observableBlock.Update();
        }
        public bool StopRotor(IMyMotorStator rotor)
        {
            rotor.TargetVelocityRPM = 0;
            rotor.RotorLock = true;
            return true;
        }
    }
}
