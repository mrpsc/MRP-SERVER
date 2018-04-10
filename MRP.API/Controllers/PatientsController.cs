using MRP.BL;
using MRP.Common.DTO;
using MRP.Common.DTO.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MRP.API.Controllers
{
    [RoutePrefix("api/Patients"), Authorize]
    public class PatientsController : ApiController
    {
        private PatientsManager _manager;

        public PatientsController()
        {
            _manager = new PatientsManager();
        }

        [Route("GetPatients"), HttpGet]
        public async Task<IHttpActionResult> GetPatients([FromUri]string patientId)
        {
            try
            {
                if (patientId != null)
                {
                    return Json(await _manager.GetPatient(patientId));
                }
                PatientPage patientPage = await _manager.GetPatients(new FindPatientModel { PatientId = patientId });
                return patientPage.Patients.Count() > 0 ? Json(patientPage) : (IHttpActionResult)BadRequest("no patients found!");
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [Route("GetPatients"), HttpPost]
        public async Task<IHttpActionResult> GetPatients([FromBody]object body, [FromUri]int limit, [FromUri]int skip)
        {
            try
            {
                string query = body.ToString();
                PatientPage patientPage = await _manager.GetPatients(query, limit, skip);

                return patientPage.Patients.Count() > 0 ? Json(patientPage) : (IHttpActionResult)BadRequest("no patients found!");
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [Route("AddPatient"), HttpPost]
        public async Task<IHttpActionResult> AddPatient([FromBody]PatientDTO patient)
        {
            try
            {
                PatientDTO patientDTO = await _manager.AddPateint(patient);
                if (patientDTO == patient)
                {
                    return BadRequest("Patient already exists with the given ID.");
                }
                else if (patientDTO != null)
                {
                    return Created<PatientDTO>("Patient created successfuly", patientDTO);
                }
                else
                {
                    return (IHttpActionResult)BadRequest("changes not excepted!");
                }
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        //[Route("AddDiagnosis"),HttpPost]
        //public async Task<IHttpActionResult> AddDiagnosis([FromBody]PatientDiagnoseDTO diagnosis)
        //{
        //    try
        //    {
        //        // return await _manager.AddDiagnosis(diagnosis) ? Created<PatientDiagnosisDTO>("",null) : (IHttpActionResult)BadRequest("changes not excepted!");
        //        return Ok();
        //    }
        //    catch (Exception ex) { return InternalServerError(ex); }

        //}

        [Route("EditPatient"), HttpPut]
        public async Task<IHttpActionResult> EditPatient([FromBody]PatientDTO patient)
        {
            try
            {
                var isUpdated = await _manager.EditPatient(patient);

                return isUpdated ? Ok() : (IHttpActionResult)BadRequest("changes not excepted!");
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [Route("EditDiagnosis"), HttpPut]
        public async Task<IHttpActionResult> EditDiagnose([FromBody]PatientDiagnoseDTO diagnosis)
        {
            try
            {
                var isUpdated = await _manager.EditDiagnose(diagnosis);
                return isUpdated ? Ok(isUpdated) : (IHttpActionResult)BadRequest("changes not excepted!");
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [Route("ExportPatients"), HttpPost]
        public async Task<IHttpActionResult> ExportPatients([FromBody]object body)
        {
            try
            {
                string query = body.ToString();
                // IEnumerable<PatientDTO> patients = await _manager.GetPatients(query);
                byte[] file = await _manager.ExportPatients(query);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(file)
                };
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Export" + DateTime.Now.Year.ToString() + ".csv"
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                var response = ResponseMessage(result);
                return response;
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }
    }
}
