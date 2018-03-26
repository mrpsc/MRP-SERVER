using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP.Common.DTO.Pages
{
    public class UsersPage
    {
        public IEnumerable<UserDTO> Users{ get; set; }
        public int Count { get; set; }
    }
}
