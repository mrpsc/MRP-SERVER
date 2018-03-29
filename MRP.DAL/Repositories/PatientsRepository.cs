using MRP.Common.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRP.Common.DTO;
using MRP.DAL.Models;
using MongoDB.Driver;
using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using System.Configuration;
using MRP.DAL.Services;
using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MRP.Common.DTO.Pages;

namespace MRP.DAL.Repositories
{
    public class PatientsRepository : IPatientsRepository
    {
        MongoClient _client;
        IMongoDatabase _database;
        IMongoCollection<Patient> _patients;

        public PatientsRepository()
        {
            _client = new MongoClient(ConfigurationManager.ConnectionStrings["Mongo"].ConnectionString);
            _database = _client.GetDatabase(ConfigurationManager.AppSettings.Get("MongoDbName"));
            _patients = _database.GetCollection<Patient>("Patients");
        }

        public PatientsRepository(string connectionString, string dbName)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(dbName);
            _patients = _database.GetCollection<Patient>("Patients");
        }

        public async Task<PatientPage> GetPatient(string patientId)
        {
            try
            {
                var patients = await _patients.Find(p => p.PatientId == patientId).Limit(1).ToListAsync();
                return new PatientPage { Patients = patients.ConvertToDTOExtension() };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<PatientDTO>> GetPatients()
        {
            List<Patient> collection = await _patients.Find(p => true).ToListAsync();
            return collection.ConvertToDTOExtension().ToList();
        }

        public async Task<PatientPage> GetPatients(FindPatientModel model, int limit, int skip)
        {
            try
            {
                List<Patient> collection;
                if (!String.IsNullOrWhiteSpace(model.PatientId))
                    collection = await _patients.Find(p => p.PatientId == model.PatientId)
                                                .Skip(skip)
                                                .Limit(limit)
                                                .ToListAsync();
                else
                    collection = await _patients.Find(p => p.Name == model.Name).ToListAsync();
                var patientPage = new PatientPage
                {
                    Patients = collection.ConvertToDTOExtension().ToList(),
                    Count = Convert.ToInt32(await _patients.CountAsync(new BsonDocument()))
                };
                return patientPage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<PatientPage> GetPatients(string query, int limit, int skip)
        {
            try
            {
                query = "{'$and':[{'Diagnose':{$exists:true}},{'Diagnose.Symptoms':{$exists:true}}," + query + "]}";
                
                var doc = BsonDocument.Parse(query);
                List<Patient> collection;
                if (!String.IsNullOrWhiteSpace(query))
                    collection = await _patients.Find(doc)
                                                
                                                .Skip(skip)
                                                .Limit(limit)
                                                .ToListAsync();


                else return null;
                var patientPage = new PatientPage
                {
                    Patients = collection.ConvertToDTOExtension().ToList(),
                    Count = Convert.ToInt32(await _patients.CountAsync(new BsonDocument()))
                };
                return patientPage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<PatientDTO>> GetPatients(string query)
        {
            try
            {
                query = "{'$and':[{'Diagnose':{$exists:true}},{'Diagnose.Symptoms':{$exists:true}}," + query + "]}";

                var doc = BsonDocument.Parse(query);
                List<Patient> collection;
                if (!String.IsNullOrWhiteSpace(query))
                    collection = await _patients.Find(doc).ToListAsync();
                else return null;
                return collection.ConvertToDTOExtension().ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> AddPatient(PatientDTO patient)
        {
            try
            {
                await _patients.InsertOneAsync(patient.ConvertToModel());
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public Task<bool> RemovePatient(string patientId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddOrUpdateDiagnoseAsync(PatientDiagnoseDTO diagnose)
        {
            try
            {
                var update = Builders<Patient>.Update.CurrentDate("LastModified").Set(p => p.Diagnose, diagnose.ConvertToModel());
                var res = await _patients.UpdateOneAsync(p => p.PatientId == diagnose.PatientId, update);
                return res.IsAcknowledged;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> UpdatePatient(PatientDTO patient)
        {
            try
            {
                await _patients.ReplaceOneAsync(p => p.PatientId == patient.PatientId, patient.ConvertToModel());
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


        //public async Task<bool> AddDiagnosis(PatientDiagnosisDTO diagnosis)
        //{
        //    var update = Builders<Patient>.Update.CurrentDate("LastModified").AddToSet(p => p.Diagnosis, diagnosis.ConvertToModel());
        //    var res = await _patients.UpdateOneAsync(p => p.PatientId == diagnosis.PatientId, update);
        //    return res.IsAcknowledged;
        //}

        //public async Task<PatientDTO> EditPatientInfo(PatientDTO patient)
        //{
        //    Patient clientPatient = patient.ConvertToModel();
        //    Patient dbPatient = _patients.Find(p => p.PatientId == clientPatient.PatientId).First();
        //    var updates = new List<UpdateDefinition<Patient>> { Builders<Patient>.Update.CurrentDate("LastModified") };
        //    string[] unchanged = { "Id", "PatientId", "LastModified", "Diagnosis" };
        //    IEnumerable<PropertyInfo> properties = typeof(Patient).GetProperties().Where(p => !unchanged.Contains(p.Name));
        //    foreach (PropertyInfo propertyInfo in properties)
        //    {
        //        if (propertyInfo.CanRead)
        //        {
        //            object firstValue = propertyInfo.GetValue(clientPatient);
        //            object secondValue = propertyInfo.GetValue(dbPatient);
        //            if (!Equals(firstValue, secondValue))
        //            {
        //                updates.Add(Builders<Patient>.Update.Set(propertyInfo.Name, firstValue));
        //            }
        //        }
        //    }
        //    var resPatient = await _patients.FindOneAndUpdateAsync(p => p.PatientId == clientPatient.PatientId, Builders<Patient>.Update.Combine(updates));
        //    return resPatient.ConvertToDTO();
        //}

        //public async Task<PatientDTO> UpdateDiagnosis(PatientDiagnosisDTO diagnosis)
        //{
        //    var patient = await _patients.FindOneAndUpdateAsync(p =>
        //            p.PatientId == diagnosis.PatientId && p.Diagnosis.Any(d => d.Id == diagnosis.Id),
        //            Builders<Patient>.Update.Set(p => p.Diagnosis.ElementAt(-1), diagnosis.ConvertToModel()));
        //    return patient.ConvertToDTO();
        //}

        //public async Task<bool> RemoveDiagnosis(int diagnosisId, string userId)
        //{
        //    try
        //    {
        //        var patient = await _patients.FindOneAndUpdateAsync(p =>
        //                p.PatientId == userId && p.Diagnosis.Any(d => d.Id == diagnosisId),
        //                Builders<Patient>.Update.PullFilter(p => p.Diagnosis,d=> d.Id == diagnosisId));
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return false;
        //    }
        // }
    }
}
