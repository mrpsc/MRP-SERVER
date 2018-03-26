using MRP.Common.DTO;
using MRP.Common.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            StringBuilder str = new StringBuilder();
            str.Append("<table border=`" + "1px" + "`b>");
            str.Append("<tr>");
            str.Append("<td><b><font face=Arial Narrow size=3>Id</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Name</font></b></td>");
            foreach (var symptom in symptomNames)
            {
                str.Append($"<td><b><font face=Arial Narrow size=3>{symptom}</font></b></td>");
            }
            str.Append("</tr>");
            foreach (var patient in patients)
            {
                str.Append("<tr>");
                str.Append($"<td><font face=Arial Narrow size=14px>{patient.PatientId}</font></td>");
                str.Append($"<td><font face=Arial Narrow size=14px>{patient.Name}</font></td>");
                if (patient.Diagnose != null && patient.Diagnose.Symptoms != null)
                {
                    foreach (var symptom in symptomNames)
                    {
                        if (patient.Diagnose.Symptoms.ContainsKey(symptom))
                        {
                            str.Append($"<td><font face=Arial Narrow size=14px>{patient.Diagnose.Symptoms[symptom].ToString()}</font></td>");
                        }
                        else
                        {
                            str.Append("<td><font face=Arial Narrow size=14px></font></td>");
                        }
                    }
                }
                str.Append("</tr>");
            }
            str.Append("</table>");
            return Encoding.UTF8.GetBytes(str.ToString());
        }
    }
}
