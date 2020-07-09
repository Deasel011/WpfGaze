using System;
using System.Linq;
using System.Threading.Tasks;
using OptionGaze;
using OptionGaze.Option;
using OptionGaze.Synchronization;

namespace ScheduledSynchronizer
{

    internal class Program
    {

        public static async Task Main(string[] args)
        {
            var synchronizationName = FindExecutingProgram(args);
            GazerVM vm = new GazerVM();
            await vm.Initialize();

            switch (synchronizationName)
            {
                case "Symbol":
                    await GazerVM.GetQuestradeSymbolIdManager().Refresh();
                    break;
                case "SymbolData":
                    await GazerVM.GetQuestradeSymbolDataManager().Refresh();
                    break;
                case "OptionData":
                    await GazerVM.GetQuestradeOptionManager().Refresh();
                    break;
                default:
                    throw new ArgumentException($"Could not understand requested synchronization name.");
            }

        }

        private static string FindExecutingProgram(string[] args)
        {
            var programArgumentIndex = args.ToList().FindIndex(arg => arg.Equals("-e", StringComparison.InvariantCultureIgnoreCase)) + 1;
            return args[programArgumentIndex];
        }

    }

}