using MRP.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP.Common.IRepositories
{
    public interface IPatientsRepository
    {
        Task<IEnumerable<PatientDTO>> GetPatients(FindPatientModel model, int limit, int skip);
        Task<IEnumerable<PatientDTO>> GetPatients(string query, int limit, int skip);
        Task<bool> AddPatient(PatientDTO patient);
        Task<bool> UpdatePatient(PatientDTO patient);
        Task<bool> RemovePatient(string patientId);
        Task<bool> AddOrUpdateDiagnoseAsync(PatientDiagnoseDTO diagnose);
        Task<IEnumerable<PatientDTO>> GetPatients(string query);
        // Task<bool> RemoveDiagnosis(int diagnosisId, string userId);
        // Task<bool> AddDiagnosis(PatientDiagnosisDTO diagnosis);
        // Task<PatientDTO> UpdateDiagnose(PatientDiagnoseDTO diagnose);
    }
}
