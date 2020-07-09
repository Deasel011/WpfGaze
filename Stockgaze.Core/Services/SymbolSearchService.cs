//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System;
using System.Threading.Tasks;
using Questrade.BusinessObjects.Entities;
using QuestradeAPI;
using Stockgaze.Core.Login;

namespace Stockgaze.Core.Services
{

    public class SymbolSearchService : SearchService<EquitySymbol>
    {

        public SymbolSearchService(QuestradeAccountManager qam) : base(qam)
        {
        }

        protected override Task<bool> SearchPage(object searchString, ulong offset, int requestId)
        {
            var tcs = new TaskCompletionSource<bool>();
            SearchSymbolsResponse.BeginSearchSymbols(
                AccountManager.GetAuthInfo,
                async response =>
                {
                    SearchSymbolsResponse.EndSearchSymbols(response);
                    var res = response.AsyncState as SearchSymbolsResponse;
                    if (res.Symbols.Count == 0)
                    {
                        tcs.SetResult(false);
                    }
                    else
                    {
                        Results.AddRange(res.Symbols);

                        await Task.Delay(TimeSpan.FromMilliseconds(50));
                        tcs.SetResult(true);
                    }
                },
                requestId,
                searchString as string,
                offset);
            return tcs.Task;
        }

    }

}