//  ==========================================================================
//  Copyright (C) 2020 by Genetec, Inc.
//  All rights reserved.
//  May be used only in accordance with a valid Source Code License Agreement.
//  ==========================================================================

using System;
using System.Dynamic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Questrade.BusinessObjects.Entities;
using Stockgaze.Core.Repositories;
using Stockgaze.Core.Services;

namespace StockgazePipelines.Input
{

    public interface IPipelineOperation {}
    
    public interface IQuestradeInput<T>
    {

    }

    public class EquitySymbolDataProvider : IQuestradeInput<EquitySymbol>
    {

        private readonly SymbolSearchService _searchService;

        private BufferBlock<EquitySymbol> _block;

        public EquitySymbolDataProvider(SymbolSearchService searchService)
        {
            _searchService = searchService;
            _block = new BufferBlock<EquitySymbol>();
        }
        
        public BufferBlock<EquitySymbol> Block => _block;

        public async Task Search(string searchTerm)
        {
            var symbols = await _searchService.Search(searchTerm);
            symbols.ForEach(symbol=>_block.Post(symbol));
        }

    }

}