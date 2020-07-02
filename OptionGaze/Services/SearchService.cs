using System.Collections.Generic;
using System.Threading.Tasks;
using OptionGaze.Login;

namespace OptionGaze.Services
{

    public abstract class SearchService<T>
    {

        private static int RequestId;

        private List<Task> m_tasks;

        protected ulong OffsetIncrement = 20;

        protected QuestradeAccountManager AccountManager { get; }

        protected List<T> Results { get; private set; }

        private static int GetNextRequestId => ++RequestId;

        protected SearchService(QuestradeAccountManager qam)
        {
            AccountManager = qam;
        }

        public async Task<List<T>> Search(object searchParameters)
        {
            Results = new List<T>();
            if (m_tasks != null && m_tasks.Count > 0)
            {
                m_tasks.ForEach(t => t.Dispose());
            }

            m_tasks = new List<Task>();
            var requestId = GetNextRequestId;
            ulong offset = 0;
            while (await SearchPage(searchParameters, offset, requestId)) offset += OffsetIncrement;


            return Results;
        }

        protected abstract Task<bool> SearchPage(object searchParameters, ulong offset, int requestId);

    }

}