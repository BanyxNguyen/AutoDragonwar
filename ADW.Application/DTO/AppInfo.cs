using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADW.Application.DTO
{
    public class AppInfo
    {
        public MyPageDragonDTO DragonPageInfo { get; set; }
        public AdventureDTO AdventureInfo { get; set; }
        public IList<PayloadDragonPage> PayloadDragonPages { get; set; }
    }
}
