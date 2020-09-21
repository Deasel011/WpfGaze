using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Questrade.BusinessObjects.Entities;
using Stockgaze.Core;
using Stockgaze.Core.Login;
using Stockgaze.Core.Services;
using StockgazePipelines.Input;
using Xunit;

namespace StockgazePipelinesTest
{

    public class Tests
    {

        private GazerVM m_gazer;

        private QuestradeAccountManager m_accountManager;

        private SymbolSearchService m_symbolSearchService;

        public Tests()
        {
            m_accountManager = new QuestradeAccountManager();
            m_symbolSearchService = new SymbolSearchService(m_accountManager);
            m_gazer = new GazerVM();
            m_gazer.Initialize();
        }
        
        [Fact]
        public async Task Test1()
        {
            // Arrange
            var dataService = new EquitySymbolDataProvider(m_symbolSearchService);
            var input = dataService.Block;
            var filter = new TransformBlock<EquitySymbol,EquitySymbol>(
                s => s.m_isTradable ? s : null);
            var ignore = DataflowBlock.NullTarget<EquitySymbol>();
            var output = new ActionBlock<EquitySymbol>(
                s => {
                    Console.WriteLine($"{s.m_symbol} {s.m_listingExchange} {s.m_description}");
                });
            input.LinkTo(filter);
            filter.LinkTo(output, s => s != null);
            filter.LinkTo(ignore);
            
            // Act
            if (m_accountManager.TryRefreshAuth())
            {
                await dataService.Search("CGC");
            }
            else
            // Assert
            Assert.True(false);
        }

    }

}