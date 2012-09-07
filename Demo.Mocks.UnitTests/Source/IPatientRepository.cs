using System.Collections.Generic;

namespace Demo.Mocks.UnitTests.Source
{
    public interface IPatientRepository
    {
        List<Patient> All { get; set; }
        Patient Get(int id);
        int Add(Patient patient);
        //void Add(Patient patient, out int id);
        void Update(Patient patient);
        void Delete(int id);
        void Delete(Patient patient);
    }
}