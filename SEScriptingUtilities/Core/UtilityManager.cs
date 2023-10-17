using System;
using Sandbox.ModAPI.Ingame;

namespace SEScriptingUtilities
{
    public class UtilityManager
    {
        public readonly BlockFinder blockFinder;

        public readonly Logger logger;
        public readonly MyGridProgram gridProgram;
        public UtilityManager(MyGridProgram _gridProgram,Logger _logger = null,BlockFinder _blockFinder = null)
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
