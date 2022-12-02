using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public Program()
        {
            Logging.gridProgram = this;
            Logging.DebugEnable = true;
            BlockManager blockManager = new BlockManager();
            BlockFinding.BlockManagerInstance = blockManager;
            RequiredBlock<IMyMotorStator> rotor = new RequiredBlock<IMyMotorStator>("Rotor",false);
            rotor.LoadBlock();
            Logging.DebugLog($"Rotor Loaded:{rotor.Loaded}",true);
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Main(string argument,UpdateType updateSource)
        {

        }
    }
}
