using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MRP.Common.DTO;
using MRP.DAL.Repositories;

namespace UnitTest
{
    [TestClass]
    public class DalTest
    {
        const string CONNECTION_STRING = "mongodb://karina:12345@ds046047.mlab.com:46047/mrpdb";
        const string DB_NAME = "mrpdb";

        [TestMethod]
        public void TEST_IF_ADDING_AND_REMOVING_DIAGNOS()
        {
            //PatientsRepository repo = new PatientsRepository(CONNECTION_STRING, DB_NAME);

            ////var added = repo.AddDiagnosis(new PatientDiagnoseDTO
            ////{
            ////    Id = 1010101,
            ////    InclusionDate = DateTime.Now,
            ////    PatientId = "222222222",
            ////    Symptoms = { },
            ////});
            ////Console.WriteLine(added.Result);
            ////var isRemoved = repo.RemoveDiagnosis(1010101, "222222222");
            //Console.WriteLine(isRemoved.Result);
            //var patient = repo.GetPatients(new FindPatientModel { Name = "Dimaa", PatientId = "222222222" }, 100, 0);

            //Assert.AreEqual(added.Result, true);
            //Assert.AreEqual(isRemoved.Result, true);
        }

        [TestMethod]
        public void TEST_IF_UPDATE_AND_CREATE()
        {
            PatientsRepository repo = new PatientsRepository(CONNECTION_STRING, DB_NAME);

            var patient = repo.GetPatients(new FindPatientModel { PatientId = "333333333" }, 1, 0);

            Console.WriteLine(patient.Result);

            var added = repo.AddOrUpdateDiagnoseAsync(new PatientDiagnoseDTO
            {
                PatientId = "333333333",
                General = "Short, Too short.",
                Symptoms = new System.Collections.Generic.Dictionary<string, dynamic> { ["Age"] = 13 },
                DoctorId = "5e",
                InOutPatient = true
            });

            Console.WriteLine(added.Result);
        }

        [TestMethod]
        public void TEST_IF_CAN_QUERY()
        {
            PatientsRepository repo = new PatientsRepository(CONNECTION_STRING, DB_NAME);
            //string query = "{'Name':'g','Diagnose.Symptoms':{'DoseDay':'21'}}";
            string query = "{'$and':[{'Diagnose':{$exists:true}},{'Diagnose.Symptoms':{$exists:true}},{'Diagnose.Symptoms.DoseDay':{'$eq':'21'}}]}";

            var patientsTask = repo.GetPatients(query, 100, 0);


            Console.WriteLine(patientsTask.Result);
        }
    }
}
