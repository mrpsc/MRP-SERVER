using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP.Common.DTO
{
    public class QueryObjDTO
    {
        public string Id { get; set; } = "";
        public string Title { get; set; }
        public string Description { get; set; }
        public string Query { get; set; }
    }
}
