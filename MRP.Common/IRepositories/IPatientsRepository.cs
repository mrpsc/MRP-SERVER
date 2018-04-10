using MRP.Common.DTO;
using MRP.Common.DTO.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP.Common.IRepositories
{
    public interface IPatientsRepository
    {
        Task<PatientPage> GetPatients(FindPatientModel model, int limit, int skip);
        Task<PatientPage> GetPatients(string query, int limit, int skip);
        Task<PatientDTO> AddPatient(PatientDTO patient);
        Task<bool> UpdatePatient(PatientDTO patient);
        Task<bool> RemovePatient(string patientId);
        Task<bool> AddOrUpdateDiagnoseAsync(PatientDiagnoseDTO diagnose);
        Task<IEnumerable<PatientDTO>> GetPatients(string query);
        Task<IEnumerable<PatientDTO>> GetPatients();
        Task<PatientPage> GetPatient(string model);
        // Task<bool> RemoveDiagnosis(int diagnosisId, string userId);
        // Task<bool> AddDiagnosis(PatientDiagnosisDTO diagnosis);
        // Task<PatientDTO> UpdateDiagnose(PatientDiagnoseDTO diagnose);
    }
}
