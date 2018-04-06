using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterSearchBot.Types;

namespace TwitterSearchBot.Contracts
{
    public interface IProfileSheet
    {
        void WriteProfile(FollowData rowData);
        void WriteProfile(List<FollowData> rows);
    }
}
