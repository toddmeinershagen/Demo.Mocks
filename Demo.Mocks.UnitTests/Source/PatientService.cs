using System;
using System.Collections.Generic;

namespace Demo.Mocks.UnitTests.Source
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ISecurityService _securityService;
 
        public PatientService(IPatientRepository patientRepository, ISecurityService securityService)
        {
            _patientRepository = patientRepository;
            _securityService = securityService;
        }

        public List<Patient> GetPatients()
        {
            try
            {
                return _patientRepository.All;
            }
            catch (Exception)
            {
                return new List<Patient>();
            }
           
        }

        public Patient GetPatient(int id)
        {
            if (!_securityService.IsInRole("Admin"))
                return null;
            
            return _patientRepository.Get(id);
        }

        public int AddPatient(Patient patient)
        {
            return _patientRepository.Add(patient);
        }
    }
}
