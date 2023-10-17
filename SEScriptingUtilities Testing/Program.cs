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
    partial class Program : MyGridProgram
    {

        public Program()
        { 
        }

        public void Save()
        { 
        }

        public void Main(string argument,UpdateType updateSource)
        {
            UtilityManager utilManager = new UtilityManager(this);
            RequiredBlock<IMyMotorStator> requiredBlock = new RequiredBlock<IMyMotorStator>(utilManager,"Test Rotor");
        }
    }
}
