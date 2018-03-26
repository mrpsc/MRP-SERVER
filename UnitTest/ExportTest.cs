using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MRP.BL.Services;
using MRP.Common.DTO;

namespace UnitTest
{
    [TestClass]
    public class ExportTest
    {
        [TestMethod]
        public void TEST_IF_EXPORT_PATIENTS_EXPORTS_CORRECTLY()
        {
            ExportService service = new ExportService();
            List<PatientDTO> patients = new List<PatientDTO>();


            for (int i = 0; i < 10; i++)
            {
                patients.Add(new PatientDTO
                {
                    Id = i.ToString() + i.ToString(),
                    Name = "Tony" + i,
                    Diagnose = new PatientDiagnoseDTO()
                });
            }
            patients[0].Diagnose.Symptoms = new Dictionary<string, dynamic>
            {
                ["Age"] = "what",
                ["What"] = 13
            };
            patients[2].Diagnose.Symptoms = new Dictionary<string, dynamic>
            {
                ["Age"] = "what",
                ["Abra"] = 13
            };
            patients[1].Diagnose.Symptoms = new Dictionary<string, dynamic>
            {
                ["Age"] = "what",
                ["kamama"] = 13
            };
            patients[3].Diagnose.Symptoms = new Dictionary<string, dynamic>
            {
                ["Loki"] = "what",
                ["What"] = 13
            };
            patients[4].Diagnose.Symptoms = new Dictionary<string, dynamic>
            {
                ["kamama"] = "what",
                ["What"] = 13
            };

            byte[] byteArr = service.GetPatientsExcel(patients);
        }
    }
}
