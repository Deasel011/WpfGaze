using System;
using System.Linq;
using System.Threading.Tasks;
using Stockgaze.Core;
using Stockgaze.Core.Enums;

namespace ScheduledSynchronizer
{

    internal class Program
    {

        public static async Task Main(string[] args)
        {
            var synchronizationName = FindExecutingProgram(args);
            GazerVM vm = new GazerVM();
            await vm.Initialize();

            if (!Enum.TryParse(synchronizationName, out TaskType taskType))
            {
                throw new ArgumentException($"Could not understand requested synchronization name.");
            }

            switch (taskType)
            {
                case TaskType.SymbolIds:
                    await GazerVM.GetQuestradeSymbolIdManager().Refresh();
                    break;
                case TaskType.SymbolData:
                    await GazerVM.GetQuestradeSymbolDataManager().Refresh();
                    break;
                case TaskType.OptionData:
                    await GazerVM.GetQuestradeOptionManager().Refresh();
                    break;
            }
        }

        private static string FindExecutingProgram(string[] args)
        {
            var programArgumentIndex = args.ToList().FindIndex(arg => arg.Equals("-e", StringComparison.InvariantCultureIgnoreCase)) + 1;
            return args[programArgumentIndex];
        }

    }

}