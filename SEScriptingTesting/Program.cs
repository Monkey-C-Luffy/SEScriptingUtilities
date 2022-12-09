using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public Program()
        {
            UtilityManager utilMngr = new UtilityManager(this);
            RequiredBlock<IMyMotorStator> rotor = new RequiredBlock<IMyMotorStator>(utilMngr,"Rotor",false);
            RequiredGroup<IMyMotorStator> group = new RequiredGroup<IMyMotorStator>(utilMngr,"Group",false);
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Main(string argument,UpdateType updateSource)
        {

        }
    }
}
