using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterSearchBot.Contracts
{
    public interface ITweetSearch
    {
        Task<List<Types.Status>> SearchByTagsAsync(string[] tags);
    }
}
