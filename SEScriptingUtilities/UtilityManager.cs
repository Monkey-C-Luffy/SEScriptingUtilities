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
        public class UtilityManager
        {
            public readonly BlockFinder blockFinder;

            public readonly Logger logger;
            public readonly MyGridProgram gridProgram;
            public UtilityManager(MyGridProgram _gridProgram,Logger _logger=null,BlockFinder _blockFinder=null)
            {
                if(_gridProgram == null)
                {
                    throw new ArgumentNullException("Grid Program");
                }
                gridProgram = _gridProgram;
                logger = _logger ?? new Logger(gridProgram);
                blockFinder = _blockFinder ?? new BlockFinder(gridProgram,logger);
            }
        }
    }
}
