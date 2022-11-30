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
            //RequiredGroup<IMyMotorStator> rotorsGroup = new RequiredGroup<IMyMotorStator>("Rotor Group",false);
            RequiredBlock<IMyMotorStator> rotor = new RequiredBlock<IMyMotorStator>("Rotor",false);
            //rotorsGroup.LoadGroup();
            rotor.LoadBlock();
            //List<IMyMotorStator> rotorsGroup = BlockFinding.GetRequiredGroupByKey<IMyMotorStator>("Rotor Group");
            //CheckGroupExists("Rotor Group",rotorsGroup);
        }

        public void Main(string argument,UpdateType updateSource)
        {

        }
        public bool CheckGroupExists<T>(string Identifier,List<T> GroupBlocks) where T : class
        {
            bool Exists = BlockFinding.FindRequiredGroupsByKey(Identifier);
            DebugGroupFound(Exists,Identifier,GroupBlocks);
            return Exists;
        }
        private void DebugGroupFound<T>(bool Exists,string Identifier,List<T> GroupBlocks) where T:class
        {
            BlockFinding.FoundGroup(Exists,Identifier,BlockUtilities.ConvertToTerminalBlockList(GroupBlocks));
        }
    }
}
