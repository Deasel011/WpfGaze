using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OptionGaze.Login;
using Questrade.BusinessObjects.Entities;
using QuestradeAPI;

namespace OptionGaze.Services
{

    /// <summary>
    ///     DataSearchService takes in a string[] of IDS to query
    /// </summary>
    public class DataSearchService : SearchService<SymbolData>
    {

        private List<ulong> m_ids;

        public DataSearchService(QuestradeAccountManager qam) : base(qam)
        {
        }

        public void SetIdsList(List<ulong> ids)
        {
            m_ids = ids;
        }

        protected override Task<bool> SearchPage(object searchParameters, ulong offset, int requestId)
        {
            var tcs = new TaskCompletionSource<bool>();
            if (offset >= Convert.ToUInt64(m_ids.Count))
            {
                tcs.SetResult(false);
                return tcs.Task;
            }

            GetSymbolsResponse.BeginGetSymbols(
                AccountManager.GetAuthInfo,
                async response =>
                {
                    GetSymbolsResponse.EndGetSymbols(response);
                    var res = response.AsyncState as GetSymbolsResponse;
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
                m_ids.Skip(Convert.ToInt32(offset)).Take(20).ToList(),
                new List<string>()
            );
            return tcs.Task;
        }

    }

}