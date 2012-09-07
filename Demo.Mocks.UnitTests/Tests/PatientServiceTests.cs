using System;
using System.Collections.Generic;
using Demo.Mocks.UnitTests.Source;
using NSubstitute;
using NUnit.Framework;

namespace Demo.Mocks.UnitTests.Tests
{
    [TestFixture]
    public class PatientServiceTests
    {
        private IPatientRepository _repository;
        private ISecurityService _securityService;
        private  PatientService _service;

        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<IPatientRepository>();
            _securityService = Substitute.For<ISecurityService>();
            _securityService.IsInRole("Admin").Returns(true);
            _service = new PatientService(_repository, _securityService);
        }

        [Test]
        public void given_patients_exist_when_getting_patients_should_return_patients()
        {
            _repository.All.Returns(new List<Patient>{new Patient(), new Patient()});

            Assert.That(_service.GetPatients(), Is.Not.Empty);
        }

        [Test]
        public void given_patients_do_not_exist_when_getting_patients_should_return_as_empty_list()
        {
            _repository.All.Returns(new List<Patient> {});

            Assert.That(_service.GetPatients(), Is.Empty);
        }

        [Test]
        public void given_patient_exists_when_getting_patient_should_return_patient()
        {
            var patient = new Patient();
            var patient2 = new Patient();
            _repository.Get(Arg.Is<int>(x => x < 4)).Returns(patient);
            _repository.Get(Arg.Is<int>(x => x >= 4)).Returns(patient2);

            Assert.That(_service.GetPatient(2), Is.EqualTo(patient));
            Assert.That(_service.GetPatient(4), Is.EqualTo(patient2));
        }

        [Test]
        public void given_new_patient_when_adding_should_return_new_id()
        {
            _repository.Add(Arg.Any<Patient>()).Returns(1, 3, 5);

            Assert.That(_service.AddPatient(new Patient()), Is.EqualTo(1));
            Assert.That(_service.AddPatient(new Patient()), Is.EqualTo(3));
            Assert.That(_service.AddPatient(new Patient()), Is.EqualTo(5));
        }

        [Test]
        public void given_repository_fails_when_getting_patients_should_return_empty_list()
        {
            _repository.All.Returns(x => { throw new Exception(); });
            
            Assert.That(_service.GetPatients(), Is.Empty);
        }

        [Test]
        public void given_user_is_not_in_admin_role_when_getting_patient_should_not_return_patient()
        {
            _securityService.IsInRole("Admin").Returns(false);
            _repository.Get(5).Returns(new Patient());
            var patient = _service.GetPatient(5);
            Assert.That(patient, Is.Null);

            _repository.DidNotReceive().Get(Arg.Any<int>());
            _securityService.Received().IsInRole("Admin");
        }

        [Test]
        public void given_user_is_in_admin_role_when_getting_patient_should_return_patient()
        {
            _securityService.IsInRole("Admin").Returns(true);
            var expectedPatient = new Patient();
            _repository.Get(5).Returns(expectedPatient);
            var patient = _service.GetPatient(5);
            Assert.That(patient, Is.EqualTo(expectedPatient));

            _repository.Received().Get(5);
            _securityService.Received().IsInRole("Admin");
        }

        [Test]
        public void test_multiple_times_return_values()
        {
            _securityService.IsInRole("Admin").Returns(true, false, true);
            Assert.That(_securityService.IsInRole("Admin"), Is.EqualTo(true));
            Assert.That(_securityService.IsInRole("Admin"), Is.EqualTo(false));
            Assert.That(_securityService.IsInRole("Admin"), Is.EqualTo(true));
        }

        [Test]
        public void test_one_return_called_multiple_times()
        {
            _securityService.IsInRole("Admin").Returns(true);
            Assert.That(_securityService.IsInRole("Admin"), Is.EqualTo(true));
            Assert.That(_securityService.IsInRole("Admin"), Is.EqualTo(true));

            _securityService.Received(2).IsInRole("Admin");
        }
    }
}
