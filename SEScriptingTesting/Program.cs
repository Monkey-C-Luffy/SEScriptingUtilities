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
            RequiredBlock<IMyMotorStator> rotor = new RequiredBlock<IMyMotorStator>(utilMngr,"Rotor");

            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Main(string argument,UpdateType updateSource)
        {

        }
    }
}
