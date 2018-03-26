using MRP.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP.Common.IServices
{
    public interface IExportService
    {
        byte[] GetPatientsExcel(IEnumerable<PatientDTO> patients);
    }
}
