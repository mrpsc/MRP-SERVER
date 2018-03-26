using MRP.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP.Common.IRepositories
{
    public interface IQueryRepository
    {
        Task<bool> AddQuery(QueryObjDTO queryObj);
    }
}
