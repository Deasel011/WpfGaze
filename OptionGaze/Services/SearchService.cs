//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OptionGaze.Login;
using Questrade.BusinessObjects.Entities;
using QuestradeAPI;

namespace OptionGaze.Services
{

    public class SearchService
    {

        private static int RequestId;

        private static List<EquitySymbol> m_results;

        private readonly QuestradeAccountManager m_qam;

        private List<Task> tasks;

        private static int GetNextRequestId => ++RequestId;

        public SearchService(QuestradeAccountManager qam)
        {
            m_qam = qam;
        }

        public async Task<List<EquitySymbol>> Search(string searchString)
        {
            m_results = new List<EquitySymbol>();
            if (tasks != null && tasks.Count > 0)
            {
                tasks.ForEach(t => t.Dispose());
            }

            tasks = new List<Task>();
            var requestId = GetNextRequestId;
            ulong offset = 0;
            while (await SearchPage(searchString, offset, requestId))
            {
                offset += Convert.ToUInt64(20);
            }
            
            
            return m_results;
        }

        private Task<bool> SearchPage(string searchString, ulong offset, int requestId)
        {
            var tcs = new TaskCompletionSource<bool>();
            SearchSymbolsResponse.BeginSearchSymbols(
                m_qam.GetAuthInfo,
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
                        m_results.AddRange(res.Symbols);
                    
                        await Task.Delay(TimeSpan.FromMilliseconds(50));
                        tcs.SetResult(true);
                    }
                },
                requestId,
                searchString,
                offset);
            return tcs.Task;
        }

    }

}