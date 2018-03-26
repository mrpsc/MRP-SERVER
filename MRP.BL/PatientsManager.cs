using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRP.Common.DTO;
using MRP.Common.IRepositories;
using MRP.DAL.Repositories;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MRP.Common.IServices;
using MRP.BL.Services;

namespace MRP.BL
{
    public class PatientsManager
    {
        private IPatientsRepository _pRep;
        private IExportService _exportService;
        
        public PatientsManager()
        {
            _pRep = new PatientsRepository();
            _exportService = new ExportService();
        }

        public Task<IEnumerable<PatientDTO>> GetPatients(FindPatientModel model)
        {
            return _pRep.GetPatients(model, 1, 0);
        }

        public Task<bool> AddPateint(PatientDTO patient)
        {
            return _pRep.AddPatient(patient);
        }

        public Task<IEnumerable<PatientDTO>> GetPatients(string query)
        {
            return _pRep.GetPatients(query);
        }

        public Task<IEnumerable<PatientDTO>> GetPatients(string query, int limit, int skip)
        {
            return _pRep.GetPatients(query, limit, skip);
        }

        public async Task<bool> AddDiagnosis(PatientDiagnoseDTO diagnose)
        {
            return await _pRep.AddOrUpdateDiagnoseAsync(diagnose);
        }

        public async Task<bool> EditDiagnose(PatientDiagnoseDTO diagnose)
        {
            return await _pRep.AddOrUpdateDiagnoseAsync(diagnose);
        }

        public async Task<byte[]> ExportPatients(string query)
        {
            var patients = await GetPatients(query);
            return _exportService.GetPatientsExcel(patients);
        }

        public Task<bool> EditPatient(PatientDTO patient)
        {
            return _pRep.UpdatePatient(patient);
        }
    }
}
