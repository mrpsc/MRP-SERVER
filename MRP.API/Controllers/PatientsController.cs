using MRP.BL;
using MRP.Common.DTO;
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
    [RoutePrefix("api/Patients"),Authorize]
    public class PatientsController : ApiController
    {
        private PatientsManager _manager;

        public PatientsController()
        {
            _manager = new PatientsManager();
        }

        [Route("GetPatients"),HttpPost]
        public async Task<IHttpActionResult> GetPatients([FromBody]FindPatientModel model)
        {
            try
            {
                IEnumerable<PatientDTO> patients = await _manager.GetPatients(model);
                return patients.Count() > 0 ? Json(patients) : (IHttpActionResult)BadRequest("no patients found!");
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [Route("GetPatients"), HttpPost]
        public async Task<IHttpActionResult> GetPatients([FromBody]string query, [FromUri]int limit, [FromUri]int skip)
        {
            try
            {
                IEnumerable<PatientDTO> patients = await _manager.GetPatients(query, limit, skip);
                return patients.Count() > 0 ? Json(patients) : (IHttpActionResult)BadRequest("no patients found!");
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [Route("AddPatient"),HttpPost]
        public async Task<IHttpActionResult> AddPatient([FromBody]PatientDTO patient)
        {
            try
            {
                return await _manager.AddPateint(patient) ? Created<PatientDiagnoseDTO>("", null) : (IHttpActionResult)BadRequest("changes not excepted!");
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

        [Route("EditPatient"),HttpPut]
        public async Task<IHttpActionResult> EditPatient([FromBody]PatientDTO patient)
        {
            try
            {
                var isUpdated = await _manager.EditPatient(patient);

                return isUpdated ? Ok() : (IHttpActionResult)BadRequest("changes not excepted!");
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [Route("EditDiagnosis"),HttpPut]
        public async Task<IHttpActionResult> EditDiagnose([FromBody]PatientDiagnoseDTO diagnosis)
        {
            try
            {
                var isUpdated = await _manager.EditDiagnose(diagnosis);
                return isUpdated ? Ok(isUpdated) : (IHttpActionResult)BadRequest("changes not excepted!");
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [AllowAnonymous]
        [Route("ExportPatients"), HttpPost]
        public async Task<IHttpActionResult> ExportPatients([FromBody]string query)
        {
            try
            {
                // IEnumerable<PatientDTO> patients = await _manager.GetPatients(query);
                byte[] file = await _manager.ExportPatients(query);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(file)
                };
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Information" + DateTime.Now.Year.ToString() + ".xlsx"
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                var response = ResponseMessage(result);
                return response;
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }
    }
}
