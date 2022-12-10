using IngameScript.Mockups.Blocks;
using IngameScript.Mockups;
using System.Threading;
using Malware.MDKUtilities;
using System.Collections.Generic;

namespace IngameScript.MDK
{
    public class TestBootstrapper
    {
        // All the files in this folder, as well as all files containing the file ".debug.", will be excluded
        // from the build process. You can use this to create utilites for testing your scripts directly in 
        // Visual Studio.

        static TestBootstrapper()
        {
            // Initialize the MDK utility framework
            MDKUtilityFramework.Load();
        }
        public static void Main()
        {
            // Create a run instance, this one is running in the console.
            var run = new ConsoleMockedRun
            {
                // We need a terminal system for the script.
                GridTerminalSystem = new MockGridTerminalSystem
                {
                    // This is the programmable block mockup which
                    // will pretend to be the running block for our
                    // script, the `Me` property
                    new MockProgrammableBlock
                    {
                        // Name it for convenience
                        CustomName = "Our PB",

                        // Rather than actually instantiating the script
                        // we just tell it the type of the script we want
                        // it to run
                        ProgramType = typeof(Program)
                    },
                    new MockGroup("Rotors",
                    new List<Sandbox.ModAPI.Ingame.IMyTerminalBlock>(){ new MockMotorStator(), new MockMotorStator(), new MockMotorStator()})
                    // We can add more blocks here, separated by a comma.
                    // We can even add multiple mocked programmable
                    // blocks if we are so inclined.
                }
              
            };

            // If our script doesn't start itself in its constructor, we'll need
            // to run it manually.
            run.Trigger("Our PB","An optional argument");

            // If our script doesn't utilize the `Runtime.UpdateFrequency` at all,
            // the following isn't needed. It's not harmful either, though.
            MockedRunFrame frame;
            while(run.NextTick(out frame))
            {
                // Just insert a little delay between ticks. You can change this
                // to your specifications, it's not important.
                Thread.Sleep(16);
            }
        }
        //public static void Main()
        //{
        //    // In order for your program to actually run, you will need to provide a mockup of all the facilities 
        //    // your script uses from the game, since they're not available outside of the game.

        //    // Create and configure the desired program.
        //    var program = MDKFactory.CreateProgram<Program>();
        //    MDKFactory.Run(program);
        //}
    }
}