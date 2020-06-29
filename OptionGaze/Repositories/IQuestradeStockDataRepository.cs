using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Questrade.BusinessObjects.Entities;

namespace OptionGaze.Repositories
{

    public interface IQuestradeStockDataRepository
    {

        DateTime LastUpdated { get; }

        List<SymbolData> QuestradeSymbolData { get; }

        Task Refresh();

    }

}