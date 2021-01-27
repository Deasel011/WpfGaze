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
            GazerController controller = null;
            try
            {
                var synchronizationName = FindExecutingProgram(args);
                controller = new GazerController();
                await controller.Initialize();

                if (!Enum.TryParse(synchronizationName, out TaskType taskType))
                {
                    throw new ArgumentException($"Could not understand requested synchronization name.");
                }

                switch (taskType)
                {
                    case TaskType.SymbolIds:
                        await GazerController.GetQuestradeSymbolIdManager().Refresh();
                        break;
                    case TaskType.SymbolData:
                        await GazerController.GetQuestradeSymbolDataManager().Refresh();
                        break;
                    case TaskType.OptionData:
                        await GazerController.GetQuestradeOptionManager().Refresh();
                        break;
                }
            }
            finally
            {
                controller?.Dispose();  
            }
        }

        private static string FindExecutingProgram(string[] args)
        {
            var programArgumentIndex = args.ToList().FindIndex(arg => arg.Equals("-e", StringComparison.InvariantCultureIgnoreCase)) + 1;
            return args[programArgumentIndex];
        }

    }

}