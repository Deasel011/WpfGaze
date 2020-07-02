using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Questrade.BusinessObjects.Entities;

namespace OptionGaze.Repositories
{

    public interface IQuestradeRepository<T>
    {

        DateTime LastUpdated { get; }

        List<T> Data { get; }

        Task Refresh();

    }

    public interface IQuestradeOptionRepository : IQuestradeRepository<(ulong SymbolId, List<ChainPerExpiryDate>)>
    {

    }

    public interface IQuestradeStockIdRepository : IQuestradeRepository<EquitySymbol>
    {

    }

    public interface IQuestradeStockDataRepository : IQuestradeRepository<SymbolData>
    {

    }

}