using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Questrade.BusinessObjects.Entities;

namespace OptionGaze.Repositories
{

    public interface IQuestradeStockIdRepository
    {

        DateTime LastUpdated { get; }

        List<EquitySymbol> QuestradeEquitySymbols { get; }

        Task Refresh();

    }

}