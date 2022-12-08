using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public Program()
        {
            BlockFinder blockFinder = new BlockFinder();
            Logger logger = new Logger();
            logger.DebugEnable = true;
            BlockManager blockManager = new BlockManager();
            RequiredBlock<IMyMotorStator> rotor = new RequiredBlock<IMyMotorStator>("Rotor",false);
            rotor.LoadBlock();
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Main(string argument,UpdateType updateSource)
        {

        }
    }
}
