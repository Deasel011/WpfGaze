using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OptionGaze.Login;
using Questrade.BusinessObjects.Entities;
using QuestradeAPI;

namespace OptionGaze.Services
{

    public class OptionDataSearchService : SearchService<Level1OptionData>
    {

        private List<OptionIdFilter> Filters;
        private List<ulong> m_ids;

        public OptionDataSearchService(QuestradeAccountManager qam) : base(qam)
        {
            OffsetIncrement = 20;
            Filters = new List<OptionIdFilter>();
        }

        public void SetIdsList(List<ulong> ids)
        {
            m_ids = ids;
        }

        public void SetFilter(List<OptionIdFilter> filters)
        {
            Filters.Clear();
            Filters.AddRange(filters);
        }
        
        protected override Task<bool> SearchPage(object searchParameters, ulong offset, int requestId)
        {
            var tcs = new TaskCompletionSource<bool>();
            if (offset >= Convert.ToUInt64(m_ids.Count))
            {
                tcs.SetResult(false);
                return tcs.Task;
            }

            GetOptionQuotesResponse.BeginGetOptionQuotes(
                AccountManager.GetAuthInfo,
                async response =>
                {
                    
                    GetOptionQuotesResponse.EndGetOptionQuotes(response);
                    var res = response.AsyncState as GetOptionQuotesResponse;
                    Results.AddRange(res.OptionQuotes);

                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    tcs.SetResult(true);
                },
                requestId,
                Filters,
                m_ids.Skip(Convert.ToInt32(offset)).Take(20).ToList()
            );
            return tcs.Task;
        }

    }

}