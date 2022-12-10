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

            rotors.ApplyActionToBlocks((rotor)=>rotor.Enabled=false);

            foreach(var rotor in rotors)
            {
                logger.DebugLine(rotor.Enabled.ToString());
            }

            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Main(string argument,UpdateType updateSource)
        {

        }
    }
}
