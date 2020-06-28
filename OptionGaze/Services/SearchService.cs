using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OptionGaze.Login;

namespace OptionGaze.Services
{

    public abstract class SearchService<T>
    {

        private static int RequestId;
        
        private QuestradeAccountManager m_qam;

        protected QuestradeAccountManager AccountManager => m_qam;

        protected List<T> Results => m_results;

        private List<T> m_results;
        
        private List<Task> m_tasks;
        
        private static int GetNextRequestId => ++RequestId;

        protected SearchService(QuestradeAccountManager qam)
        {
            m_qam = qam;
        }

        public async Task<List<T>> Search(object searchParameters)
        {
            m_results = new List<T>();
            if (m_tasks != null && m_tasks.Count > 0)
            {
                m_tasks.ForEach(t => t.Dispose());
            }

            m_tasks = new List<Task>();
            var requestId = GetNextRequestId;
            ulong offset = 0;
            while (await SearchPage(searchParameters, offset, requestId))
            {
                offset += Convert.ToUInt64(20);
            }
            
            
            return m_results;
        }

        protected abstract Task<bool> SearchPage(object searchParameters, ulong offset, int requestId);

    }

}