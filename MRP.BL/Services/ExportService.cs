using MRP.Common.DTO;
using MRP.Common.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Jitbit.Utils;

namespace MRP.BL.Services
{
    public class ExportService : IExportService
    {

        public byte[] GetPatientsExcel(IEnumerable<PatientDTO> patients)
        {
            HashSet<string> symptomNames = new HashSet<string>();
            foreach (var patient in patients)
            {
                if (patient.Diagnose != null && patient.Diagnose.Symptoms != null)
                {
                    symptomNames.UnionWith(new HashSet<string>(patient.Diagnose.Symptoms.Keys));
                }
            }
            var myExport = new CsvExport(",",false);
            foreach (var patient in patients)
            {
                myExport.AddRow();
                myExport["Id"] = patient.Id;
                myExport["Name"] = patient.Name;
                if (patient.Diagnose != null && patient.Diagnose.Symptoms != null)
                {
                    foreach (var symptom in symptomNames)
                    {
                        if (patient.Diagnose.Symptoms.ContainsKey(symptom))
                        {
                            myExport[symptom] = patient.Diagnose.Symptoms[symptom].ToString();
                        }
                    }
                }
            }
            return myExport.ExportToBytes();
        }
    }
}
