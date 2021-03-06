﻿//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System.Threading.Tasks;
using OptionGaze;
using OptionGaze.Option;
using Xunit;

namespace GazerTests
{

    public class SearchSymbolsTests
    {

        public SearchSymbolsTests()
        {
            m_gazer = new GazerVM();
            m_gazer.Initialize();
            m_options = new OptionsUCVM();
            m_options.Initialize();
        }

        private readonly GazerVM m_gazer;

        private readonly OptionsUCVM m_options;

        [Fact]
        public void TestAuth()
        {
            Assert.True(m_gazer.QuestradeAccountManager.IsConnected);
        }

        [Fact]
        public async Task TestSearchString()
        {
            var res = await m_options.DataSearchService.Search("cgc");
            Assert.True(res.Count > 0);
        }

    }

}