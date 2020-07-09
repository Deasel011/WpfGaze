using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Questrade.BusinessObjects.Entities;
using QuestradeAPI;
using Stockgaze.Core.Login;

namespace Stockgaze.Core.Services
{

    public class OptionsSearchService : SearchService<(ulong, List<ChainPerExpiryDate>)>
    {

        private List<ulong> m_ids;

        public OptionsSearchService(QuestradeAccountManager qam) : base(qam)
        {
            OffsetIncrement = 1;
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

            GetOptionsResponse.BeginGetOptions(
                AccountManager.GetAuthInfo,
                async response =>
                {
                    GetOptionsResponse.EndGetOptions(response);
                    var res = response.AsyncState as GetOptionsResponse;
                    Results.Add((m_ids.Skip(Convert.ToInt32(offset)).First(), res.OptionChain));

                    await Task.Delay(TimeSpan.FromMilliseconds(50));
                    tcs.SetResult(true);
                },
                requestId,
                m_ids.Skip(Convert.ToInt32(offset)).First()
            );
            return tcs.Task;
        }

    }

}