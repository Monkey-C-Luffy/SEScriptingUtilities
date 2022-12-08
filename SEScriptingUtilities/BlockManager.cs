using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public class BlockManager
        {
            public Dictionary<string,IMyBlockGroup> groupsFound = new Dictionary<string,IMyBlockGroup>();
            public Dictionary<string,IMyTerminalBlock> blocksFound = new Dictionary<string,IMyTerminalBlock>();

            public int requiredBlocksCnt = 0;
            public int requiredGroupsCnt = 0;

            private BlockFinder _blockFinderInstance;
            public BlockFinder BlockFinderInstance
            {
                get
                {
                    return _blockFinderInstance;
                }
                set
                {
                    if(_blockFinderInstance == null) _blockFinderInstance = value;
                }
            }

            private Logger _loggerInstance;
            public Logger LoggerInstance
            {
                get
                {
                    return _loggerInstance;
                }
                set
                {
                    if(_loggerInstance == null) _loggerInstance = value;
                }
            }

            private MyGridProgram _programInstance;
            public MyGridProgram ProgramInstance
            {
                get
                {
                    return _programInstance;
                }
                set
                {
                    if(_programInstance == null) _programInstance = value;
                }
            }
            public bool CheckAllBlocksAndGroupsFound()
            {
                if(blocksFound.Count != requiredBlocksCnt) return false;
                if(groupsFound.Count != requiredGroupsCnt) return false;
                return true;
            }
        }
    }
}
