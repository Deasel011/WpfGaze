//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System.Threading.Tasks;
using OptionGaze;
using OptionGaze.Option;
using Stockgaze.Core;
using Stockgaze.Core.Option;
using Xunit;

namespace GazerTests
{

    public class SearchSymbolsTests
    {

        public SearchSymbolsTests()
        {
            m_gazer = new GazerController();
            m_gazer.Initialize();
            m_options = new OptionsController();
            m_options.Initialize();
        }

        private readonly GazerController m_gazer;

        private readonly OptionsController m_options;

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