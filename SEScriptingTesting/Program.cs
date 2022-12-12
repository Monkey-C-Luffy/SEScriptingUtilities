using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public Program()
        {
            Logger logger = new Logger(this);
            BlockFinder blockFinder = new BlockFinder(this,logger);
            UtilityManager utilMngr = new UtilityManager(this,logger,blockFinder);
            RequiredGroup<IMyMotorStator> rotors = new RequiredGroup<IMyMotorStator>(utilMngr,"Rotors");
            RequiredBlock<IMyMotorStator> rotor = new RequiredBlock<IMyMotorStator>(utilMngr,"Rotor");
            
            rotor.AddCondition((r)=>r.Angle>=60.5&&r.Angle<=59.5);

            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }


        public void Main(string argument,UpdateType updateSource)
        {

        }
    }
}
