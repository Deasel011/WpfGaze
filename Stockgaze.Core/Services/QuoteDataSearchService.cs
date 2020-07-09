using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Questrade.BusinessObjects.Entities;
using QuestradeAPI;
using Stockgaze.Core.Login;

namespace Stockgaze.Core.Services
{

    public class QuoteDataSearchService : SearchService<Level1DataItem>
    {

        private List<ulong> m_ids;

        public QuoteDataSearchService(QuestradeAccountManager qam) : base(qam)
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

            GetQuoteResponse.BeginGetQuote(
                AccountManager.GetAuthInfo,
                async response =>
                {
                    GetQuoteResponse.EndGetQuote(response);
                    var res = response.AsyncState as GetQuoteResponse;
                    Results.AddRange(res.Quotes);

                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    tcs.SetResult(true);
                },
                requestId,
                m_ids.Skip(Convert.ToInt32(offset)).Take(Convert.ToInt32(OffsetIncrement)).ToList()
            );
            return tcs.Task;
        }

    }

}