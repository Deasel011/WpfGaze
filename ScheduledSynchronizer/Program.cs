using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stockgaze.Core;
using Stockgaze.Core.Enums;
using Stockgaze.Core.Option;

namespace ScheduledSynchronizer
{

    internal class Program
    {

        public static List<string> ErrorList = new List<string>();
        
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
                    case TaskType.OptionSearch:
                        var optionController = new OptionsController();
                        var emailService = GazerController.GetEmailService();
                        optionController.Initialize();
                        var settings = ExtractOptionControllerSettings(args);
                        optionController.ApplyConfig(settings);
                        optionController.runSync = true;
                        await optionController.LoadOptions();
                        emailService.SendOptionResults("Options Report",settings.ToString() ,settings.Emails, optionController.OptionSymbolsSynchronous);
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

        private static OptionControllerConfig ExtractOptionControllerSettings(string[] args)
        {
            var optionControllerConfig = new OptionControllerConfig();
            if (TryGetArgumentValue(args, nameof(OptionControllerConfig.MinStrikePrice), out string minStrikePrice))
            {
                optionControllerConfig.MinStrikePrice = ParseDoubleArgument(minStrikePrice,optionControllerConfig.MinStrikePrice,nameof(OptionControllerConfig.MinStrikePrice));
            }
            if (TryGetArgumentValue(args, nameof(OptionControllerConfig.MaxStrikePrice), out string maxStrikePrice))
            {
                optionControllerConfig.MaxStrikePrice = ParseDoubleArgument(maxStrikePrice, optionControllerConfig.MaxStrikePrice,nameof(OptionControllerConfig.MaxStrikePrice));
            }
            if (TryGetArgumentValue(args, nameof(OptionControllerConfig.Emails), out string emails))
            {
                optionControllerConfig.Emails = ParseListArgument(emails, optionControllerConfig.Emails, nameof(OptionControllerConfig.Emails));
            }
            if (TryGetArgumentValue(args, nameof(OptionControllerConfig.ExpiryDate), out string expiryDate))
            {
                optionControllerConfig.ExpiryDate = ParseDateTimeArgument(expiryDate, optionControllerConfig.ExpiryDate, nameof(OptionControllerConfig.ExpiryDate));
            }
            if (TryGetArgumentValue(args, nameof(OptionControllerConfig.SearchNasdaq), out string searchNasdaq))
            {
                optionControllerConfig.SearchNasdaq = ParseBoolArgument(searchNasdaq, optionControllerConfig.SearchNasdaq, nameof(OptionControllerConfig.SearchNasdaq));
            }
            if (TryGetArgumentValue(args, nameof(OptionControllerConfig.SearchNyse), out string searchNyse))
            {
                optionControllerConfig.SearchNyse = ParseBoolArgument(searchNyse, optionControllerConfig.SearchNyse, nameof(OptionControllerConfig.SearchNyse));
            }
            if (TryGetArgumentValue(args, nameof(OptionControllerConfig.SearchTsx), out string searchTsx))
            {
                optionControllerConfig.SearchTsx = ParseBoolArgument(searchTsx, optionControllerConfig.SearchTsx, nameof(OptionControllerConfig.SearchTsx));
            }
            if (TryGetArgumentValue(args, nameof(OptionControllerConfig.FilterInfiniteReturn), out string filterInfiniteReturns))
            {
                optionControllerConfig.FilterInfiniteReturn = ParseBoolArgument(filterInfiniteReturns, optionControllerConfig.FilterInfiniteReturn, nameof(OptionControllerConfig.FilterInfiniteReturn));
            }
            if (TryGetArgumentValue(args, nameof(OptionControllerConfig.FilterMinVolume), out string filterMinVolume))
            {
                optionControllerConfig.FilterMinVolume = ParseIntArgument(filterMinVolume, optionControllerConfig.FilterMinVolume, nameof(OptionControllerConfig.FilterMinVolume));
            }

            return optionControllerConfig;
        }

        private static double ParseDoubleArgument(string doubleStringValue, double defaultValue, string argName)
        {
            if (double.TryParse(doubleStringValue, out double value)) return value;

            ErrorList.Add(FormatError(argName, doubleStringValue));
            return defaultValue;
        }
        
        private static int ParseIntArgument(string intStringValue, int defaultValue, string argName)
        {
            if (int.TryParse(intStringValue, out int value)) return value;

            ErrorList.Add(FormatError(argName, intStringValue));
            return defaultValue;
        }
        
        private static DateTime ParseDateTimeArgument(string dateTimeStringValue, DateTime defaultValue, string argName)
        {
            if (DateTime.TryParse(dateTimeStringValue, out DateTime value)) return value;

            ErrorList.Add(FormatError(argName, dateTimeStringValue));
            return defaultValue;
        }
        
        
        private static bool ParseBoolArgument(string boolStringValue, bool defaultValue, string argName)
        {
            if (bool.TryParse(boolStringValue, out bool value)) return value;

            ErrorList.Add(FormatError(argName, boolStringValue));
            return defaultValue;
        }
        
        private static List<string> ParseListArgument(string listStringValue, List<string> defaultValue, string argName)
        {
            var result = listStringValue.Split(';').ToList();
            if (result.Count > 0) return result;

            ErrorList.Add(FormatError(argName, listStringValue));
            return defaultValue;
        }

        private static string FormatError(string argName, string value) { return $"Error occured with argument {argName} with value <{value}>"; }

        private static bool TryGetArgumentValue(string[] args, string argName, out string value)
        {
            var argumentIndex = args.ToList().FindIndex(arg => arg.Equals($"--{argName}", StringComparison.InvariantCultureIgnoreCase)) + 1;
            if (argumentIndex > 0)
            {
                value = args[argumentIndex];
                return true;
            }
            value = null;
            return false;
        }

    }
}